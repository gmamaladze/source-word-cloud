using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Gma.CodeCloud.CSharp;

namespace Gma.CodeCloud
{
    public partial class MainForm : Form
    {
        private const string s_CSharpBlacklistFileName = "CSharpBlacklist.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            IProgressIndicator progressBarWrapper = new ProgressBarWrapper(ToolStripProgressBar);
            IWordRegistry wordRegistry = CountWords(path, progressBarWrapper);
            FillMap(wordRegistry);

            IsRunning = false;
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

        private static IWordRegistry CountWords(string path, IProgressIndicator progress)
        {
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = rootDirectoryInfo.GetFiles("*.cs", SearchOption.AllDirectories);
            IWordExtractor extractor = new WordExtractor(fileInfos, progress);
            IBlacklist blacklist = new TextFileBlacklist(s_CSharpBlacklistFileName);
            WordCounter counter = new WordCounter(blacklist);
            return counter.Count(extractor);
        }

        private void FillMap(IWordRegistry wordCounter)
        {
            TreeMap.Clear();
            TreeMap.BeginUpdate();
            KeyValuePair<string, int>[] pairs = wordCounter.GetSortedByOccurances();
            Array.Resize(ref pairs, Math.Min(100, pairs.Length));

            double sum = 0;
            foreach (KeyValuePair<string, int> pair in pairs)
            {
                sum += pair.Value;
            }

            foreach (KeyValuePair<string, int> pair in pairs)
            {
                TreeMap.Nodes.Add(pair.Key, pair.Value, pair.Value, null, string.Format("{0} - {1}% - {2} occurances", pair.Key, Math.Round(pair.Value * 100 / sum, 2), pair.Value));
            }
            TreeMap.EndUpdate();
        }

        private sealed class ProgressBarWrapper : IProgressIndicator
        {
            private readonly ToolStripProgressBar m_ToolStripProgressBar;

            public ProgressBarWrapper(ToolStripProgressBar toolStripProgressBar)
            {
                m_ToolStripProgressBar = toolStripProgressBar;
            }

            public int Maximum
            {
                get { return m_ToolStripProgressBar.Maximum; }
                set { m_ToolStripProgressBar.Maximum = value; }
            }

            public void Increment(int value)
            {
                m_ToolStripProgressBar.Increment(value);
                Application.DoEvents();
            }
        }

        private void ToolStripButtonEditBlacklist_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", s_CSharpBlacklistFileName);
        }
    }
}
