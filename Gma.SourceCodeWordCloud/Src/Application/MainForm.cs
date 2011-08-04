using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.CodeCloud.Base;
using Gma.CodeCloud.Base.FileIO;
using Gma.CodeCloud.Base.Geometry;
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
        private CancellationTokenSource m_CancelSource;
        private long m_LastTicks;
        private int m_ProgressValue;

        public MainForm(string initialPath)
            : this()
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
            m_CloudControl.MouseClick += CloudControlClick;
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
            if (m_TotalWordCount == 0 || itemUderMouse == null)
            {
                return null;
            }

            return string.Format(
                "\n\r{0} - occurances\n\r{1}% - of total words\n\r-------------------------------------\n\r{2}",
                itemUderMouse.Word.Occurrences,
                Math.Round(itemUderMouse.Word.Occurrences * 100 / m_TotalWordCount, 2),
                itemUderMouse.Word.GetCaption());
        }

        private void CloudControlClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenu.Show(e.Location);
                }
                return;
            }


            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            if (itemUderMouse == null)
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

            SetCaptionText("Estimating ...");

            string[] files = fileIterator
                .GetFiles(path)
                .ToArray();

            ToolStripProgressBar.Maximum = files.Length;

            m_CloudControl.WeightedWords = new List<IWord>();

            //Note do not dispose m_CancelSource it will be disposed by task 
            //TODO need to find correct way to work with CancelationToken
            m_CancelSource = new CancellationTokenSource();
                Task.Factory
                    .StartNew(
                        () => GetWordsParallely(files, language, blacklist, stemmer), m_CancelSource.Token)
                    .ContinueWith(
                        ApplyResults);
        }

        private List<IWord> GetWordsParallely(IEnumerable<string> files, Language language, IBlacklist blacklist, IWordStemmer stemmer)
        {
            return files
                .AsParallel()
                //.WithDegreeOfParallelism(0x8)
                .WithCancellation(m_CancelSource.Token)
                .WithCallback(DoProgress)
                .SelectMany(file => ByLanguageFactory.GetWordExtractor(language, file))
                .Filter(blacklist)
                .CountOccurences()
                .GroupByStem(stemmer)
                .SortByOccurences()
                .AsEnumerable()
                .Cast<IWord>()
                .ToList();
        }

        private void ApplyResults(Task<List<IWord>> task)
        {
            this.Invoke(
                new Action(
                    () =>
                        {
                            if (task.IsCanceled)
                            {
                                SetCaptionThreadsafe("Canceled");
                                m_CloudControl.WeightedWords = new List<IWord>();
                            }
                            else
                            {
                                if (task.IsFaulted && task.Exception != null)
                                {
                                    throw task.Exception;
                                }

                                SetCaptionThreadsafe("Finished");
                                m_CloudControl.WeightedWords = task.Result;    
                            }

                            m_TotalWordCount = m_CloudControl.WeightedWords.Sum(word => word.Occurrences);
                            this.IsRunning = false;
                        }));
        }

        private void DoProgress(string fileName)
        {
            m_ProgressValue++;
            if (DateTime.UtcNow.Ticks - m_LastTicks < TimeSpan.TicksPerSecond / 30 ) //30 frames per second
            {
                return;
            }
            SetCaptionThreadsafe(ShortenFileName(fileName, 60));
            m_LastTicks = DateTime.UtcNow.Ticks;
        }

        public void SetCaptionThreadsafe(string fileName)
        {
            Action<string> setCaption = SetCaptionText;
            this.Invoke(setCaption, fileName);
        }

        public void SetCaptionText(string text)
        {
            ToolStripProgressBar.Value = m_ProgressValue;
            this.Text = string.Concat("Source Code Word Colud Generator :: " + text);
        }

        private void ToolStripButtonCancelClick(object sender, EventArgs e)
        {
            if (m_CancelSource != null)
            {
                m_CancelSource.Cancel(true);
            }
        }

        private bool IsRunning
        {
            set
            {
                ToolStripButtonCancel.Enabled = value;
                ToolStripButtonGo.Enabled = !value;
                if (!value)
                {
                    ToolStripProgressBar.Value = 0;
                    m_ProgressValue = 0;
                }
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

            m_CloudControl.LayoutType = (LayoutType)toolStripComboBoxLayout.SelectedItem;
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


        private void HideMenuItemClick(object sender, EventArgs e)
        {
            IWord word = GetWordUderMouse();
            RemoveFromControl(word);
        }

        private void HideAndBlackListMenuItemClick(object sender, EventArgs e)
        {
            IWord word = GetWordUderMouse();
            RemoveFromControl(word);
            AddToBlacklist(word.Text);
        }

        private void RemoveFromControl(IWord word)
        {
            m_CloudControl.WeightedWords.Remove(word);
            m_CloudControl.BuildLayout();
            m_CloudControl.Invalidate();
        }

        private IWord GetWordUderMouse()
        {
            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            return itemUderMouse == null
                ? new Word(string.Empty, 0)
                : itemUderMouse.Word;
        }

        private void AddToBlacklist(string term)
        {
            Language language = ByLanguageFactory.GetLanguageFromString(toolStripComboBoxLanguage.Text);
            string fileName = ByLanguageFactory.GetBlacklistFileName(language);
            using (StreamWriter writer = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                writer.WriteLine();
                writer.Write(term);
            }
        }

        private void ToolStripButtonSaveClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp";
            saveFileDialog.FileName = Path.GetFileName(FolderTree.SelectedPath);
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog()!=DialogResult.OK)
            {
                return;
            }

            using(Bitmap bitmap = new Bitmap(m_CloudControl.Size.Width, m_CloudControl.Size.Height))
            {
                m_CloudControl.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), bitmap.Size));
                bitmap.Save(
                    saveFileDialog.FileName, 
                    saveFileDialog.FileName.EndsWith("png") 
                        ? ImageFormat.Png 
                        : ImageFormat.Bmp);
            }
        }
    }
}