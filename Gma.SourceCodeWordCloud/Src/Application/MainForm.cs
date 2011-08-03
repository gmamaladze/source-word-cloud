using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Gma.CodeCloud.Base;
using Gma.CodeCloud.Base.FileIO;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.Languages;
using Gma.CodeCloud.Base.TextAnalyses.Blacklist;
using Gma.CodeCloud.Base.TextAnalyses.Processing;
using Gma.CodeCloud.Base.TextAnalyses.Stemmers;
using Gma.CodeCloud.Controls;

namespace Gma.CodeCloud
{
    public partial class MainForm : Form
    {
        private readonly CloudControl m_CloudControl = new CloudControl();
        private decimal m_TotalWordCount;
        private CancellationTokenSource m_CancellationTokenSourcec;

        public MainForm(string initialPath) : this()
        {
            this.FolderTree.SelectedPath = initialPath;
            this.ToolStripButtonGoClick(null, null);
        }


        public MainForm()
        {
            InitializeComponent();

            this.FolderTree.SelectedPath = Path.GetPathRoot(Directory.GetCurrentDirectory());

            m_CloudControl.Dock = DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(m_CloudControl);
            m_CloudControl.Click += CloudControlClick;
            m_CloudControl.MouseMove += CloudControlMouseMove;
            m_CloudControl.Paint += CloudControlPaint;
           
            foreach (var layoutType in Enum.GetValues(typeof(LayoutType)))
            {
                this.toolStripComboBoxLayout.Items.Add(layoutType);
            }
            
            this.toolStripComboBoxLayout.SelectedItem = LayoutType.Spiral;

            foreach (var fontFamily in FontFamily.Families)
            {
                if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                {
                    toolStripComboBoxFont.Items.Add(fontFamily.Name);
                }
            }

            toolStripComboBoxMinFontSize.SelectedItem = "8";
            toolStripComboBoxMaxFontSize.SelectedItem = "72";
            toolStripComboBoxFont.SelectedItem = "Tahoma";
            toolStripComboBoxLanguage.SelectedItem = "c#";
        }

        private void CloudControlPaint(object sender, PaintEventArgs e)
        {
            SetCaptionText(string.Format("Showing {0} words of {1}", m_CloudControl.ItemsCount, m_TotalWordCount));
        }

        private void CloudControlMouseMove(object sender, MouseEventArgs e)
        {
            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            if (itemUderMouse == null)
            {
                toolTip.SetToolTip(m_CloudControl, null);
                return;
            }

            toolTip.SetToolTip(
                m_CloudControl,
                GetItemCaption(itemUderMouse));

            toolTip.ToolTipTitle = string.Format("Statistics for word [{0}]", itemUderMouse.Word.Text);
        }

        private string GetItemCaption(LayoutItem itemUderMouse)
        {
            if (m_TotalWordCount==0 || itemUderMouse==null)
            {
                return null;
            }

            return string.Format(
                "\n\r{0} - occurances\n\r{1}% - of total words\n\r-------------------------------------\n\r{2}",
                itemUderMouse.Word.Occurrences,
                Math.Round(itemUderMouse.Word.Occurrences * 100 / m_TotalWordCount, 2),
                itemUderMouse.Word.GetCaption());
        }

        private void CloudControlClick(object sender, EventArgs e)
        {
            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            if (itemUderMouse==null)
            {
                return;
            }

            MessageBox.Show(
                GetItemCaption(itemUderMouse),
               string.Format("Statistics for word [{0}]", itemUderMouse.Word.Text),
               MessageBoxButtons.OK,
               MessageBoxIcon.Information);
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            Language language = ByLanguageFactory.GetLanguageFromString(toolStripComboBoxLanguage.Text);
            FileIterator fileIterator = ByLanguageFactory.GetFileIterator(language);

            IBlacklist blacklist = ByLanguageFactory.GetBlacklist(language);
            IWordStemmer stemmer = ByLanguageFactory.GetStemmer(language);

            IsRunning = true;
            m_CloudControl.WeightedWords = new IWord[0];
            using (m_CancellationTokenSourcec = new CancellationTokenSource())
            try
            {
                var result = fileIterator
                    .GetFiles(path)
                    .AsParallel()
                    .WithCancellation(m_CancellationTokenSourcec.Token)
                    //.WithCallback(DoProgress)
                    .SelectMany(file => ByLanguageFactory.GetWordExtractor(language, file))
                    .Filter(blacklist)
                    .CountOccurences()
                    .GroupByStem(stemmer)
                    .SortByOccurences()
                    .AsEnumerable()
                    .Cast<IWord>()
                    .ToArray();

                m_CloudControl.WeightedWords = result;
                m_TotalWordCount = result.Sum(word => word.Occurrences);
            } 
            catch (OperationCanceledException)
            {
                
            }
            IsRunning = false;
        }


        public void SetCaptionText(string text)
        {
            this.Text = string.Concat("Source Code Word Colud Generator :: " + text);
        }

        private void ToolStripButtonCancelClick(object sender, EventArgs e)
        {
            m_CancellationTokenSourcec.Cancel(false);
            IsRunning = false;
        }

        private bool IsRunning
        {
            set
            {
                ToolStripButtonCancel.Enabled = value;
                ToolStripButtonGo.Enabled = !value;
                ToolStripProgressBar.Value = 0;
                ToolStripProgressBar.Style = value ? ProgressBarStyle.Continuous : ProgressBarStyle.Blocks;
                SetCaptionText(value ? "Working ..." : string.Empty);
                Application.DoEvents();
            }
        }

        private void ToolStripButtonEditBlacklistClick(object sender, EventArgs e)
        {
            Language language = ByLanguageFactory.GetLanguageFromString(toolStripComboBoxLanguage.Text);
            string fileName = ByLanguageFactory.GetBlacklistFileName(language);
            Process.Start("notepad.exe", fileName);
        }

        private void ToolStripComboBoxFontSelectedIndexChanged(object sender, EventArgs e)
        {
            m_CloudControl.Font = new Font(toolStripComboBoxFont.Text, 12, FontStyle.Regular);
            int value;
            if (int.TryParse(toolStripComboBoxMinFontSize.Text, out value))
            {
                m_CloudControl.MinFontSize = value;
            }

            if (int.TryParse(toolStripComboBoxMaxFontSize.Text, out value))
            {
                m_CloudControl.MaxFontSize = value;
            }

            m_CloudControl.LayoutType = (LayoutType) toolStripComboBoxLayout.SelectedItem;
        }

        private static string ShortenFileName(string fullFileName, int maxLength)
        {
            if (fullFileName.Length <= maxLength)
            {
                return fullFileName;
            }

            int partLength = maxLength / 2 - 2;

            return string.Concat(
                fullFileName.Remove(partLength),
                "...",
                fullFileName.Substring(fullFileName.Length - partLength));
        }
    }
}
