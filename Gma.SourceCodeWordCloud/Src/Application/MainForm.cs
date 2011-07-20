using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Gma.CodeCloud.Base;
using Gma.CodeCloud.Base.FileIO;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.Languages;
using Gma.CodeCloud.Controls;

namespace Gma.CodeCloud
{
    public partial class MainForm : Form
    {
        private readonly CloudControl m_CloudControl = new CloudControl();
        private decimal m_TotalWordCount;

        public MainForm(string initialPath) : this()
        {
            this.FolderTree.SelectedPath = initialPath;
            this.ToolStripButtonGoClick(null, null);
        }


        public MainForm()
        {
            InitializeComponent();

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

            toolTip.ToolTipTitle = string.Format("Statistics for word [{0}]", itemUderMouse.Word);
        }

        private string GetItemCaption(LayoutItem itemUderMouse)
        {
            if (m_TotalWordCount==0 || itemUderMouse==null)
            {
                return null;
            }

            return string.Format(
                "\n\r{0} - occurances\n\r{1}% - of total words",
                itemUderMouse.Weight,
                Math.Round(itemUderMouse.Weight * 100 / m_TotalWordCount, 2));
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
               string.Format("Statistics for word [{0}]", itemUderMouse.Word));
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            IProgressIndicator progressBarWrapper = new ProgressBarWrapper(ToolStripProgressBar, SetCaptionText);
            Language language = ByLanguageFactory.GetLanguageFromString(toolStripComboBoxLanguage.Text);
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            ToolStripProgressBar.Style = ProgressBarStyle.Marquee;
            FileIterator fileIterator = ByLanguageFactory.GetFileIterator(language, progressBarWrapper);
            int count = 0;
            IEnumerable<FileInfo> fileInfos = fileIterator.GetFiles(rootDirectoryInfo, ref count);
            ToolStripProgressBar.Style = ProgressBarStyle.Blocks;
            progressBarWrapper.Maximum = count;
            IWordExtractor extractor = ByLanguageFactory.GetWordExtractor(language, fileInfos, progressBarWrapper);
            IBlacklist blacklist = ByLanguageFactory.GetBlacklist(language);
            WordCounter counter = new WordCounter(blacklist);
            IWordRegistry wordRegistry = counter.Count(extractor);
            KeyValuePair<string, int>[] pairs = wordRegistry.GetSortedByOccurances();
            m_TotalWordCount = wordRegistry.TotalWords;

            m_CloudControl.WeightedWords = pairs;

            IsRunning = false;
        }

        private void SetCaptionText(string text)
        {
            this.Text = string.Concat("Source Code Word Colud Generator :: " + text);
        }

        private void ToolStripButtonCancelClick(object sender, EventArgs e)
        {
            IsRunning = false;
        }

        private bool IsRunning
        {
            set
            {
                ToolStripButtonCancel.Enabled = value;
                ToolStripButtonGo.Enabled = !value;
                ToolStripProgressBar.Value = 0;
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

        private sealed class ProgressBarWrapper : IProgressIndicator
        {
            private readonly ToolStripProgressBar m_ToolStripProgressBar;
            private readonly Action<string> m_SetMessage;

            public ProgressBarWrapper(ToolStripProgressBar toolStripProgressBar, Action<string> setMessage )
            {
                m_ToolStripProgressBar = toolStripProgressBar;
                m_SetMessage = setMessage;
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }

            public int Maximum
            {
                get { return m_ToolStripProgressBar.Maximum; }
                set { m_ToolStripProgressBar.Maximum = value; }
            }

            public int Value
            {
                get { return m_ToolStripProgressBar.Value; }
                set { m_ToolStripProgressBar.Value = value; }
            }

            public void SetMessage(string text)
            {
                m_SetMessage.Invoke(text);
                Application.DoEvents();
            }

            public void Increment(int value)
            {
                if (m_ToolStripProgressBar.Style == ProgressBarStyle.Blocks)
                {
                    m_ToolStripProgressBar.Increment(value);
                }
                Application.DoEvents();
            }
        }
    }
}
