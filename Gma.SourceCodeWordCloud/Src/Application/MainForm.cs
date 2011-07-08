﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Gma.CodeCloud.CSharp;
using Microsoft.Research.CommunityTechnologies.Treemap;

namespace Gma.CodeCloud
{
    public partial class MainForm : Form
    {
        private const string s_CSharpBlacklistFileName = "CSharpBlacklist.txt";

        private readonly CloudControl m_CloudControl = new CloudControl();

        public MainForm(string initialPath) : this()
        {
            this.FolderTree.SelectedPath = initialPath;
            this.ToolStripButtonGoClick(null, null);
        }


        public MainForm()
        {
            InitializeComponent();
            this.splitContainer1.Panel1.Controls.Remove(TreeMap);
            m_CloudControl.Dock = DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(m_CloudControl);
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            IProgressIndicator progressBarWrapper = new ProgressBarWrapper(ToolStripProgressBar);
            IWordRegistry wordRegistry = CountWords(path, progressBarWrapper);
            FillMap(wordRegistry, m_CloudControl);

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

        private class TreeMapWrapper : ICloudControl
        {
            private readonly TreemapControl m_TreemapControl;

            public TreeMapWrapper(TreemapControl treemapControl)
            {
                m_TreemapControl = treemapControl;
            }

            public void BeginUpdate()
            {
                m_TreemapControl.BeginUpdate();
            }

            public void EndUpdate()
            {
                m_TreemapControl.EndUpdate();
            }

            public void Clear()
            {
                m_TreemapControl.Clear();
            }

            public void Show(KeyValuePair<string, int>[] words)
            {

                m_TreemapControl.Clear();
                m_TreemapControl.BeginUpdate();
               
                double sum = 0;
                foreach (KeyValuePair<string, int> pair in words)
                {
                    sum += pair.Value;
                }

                foreach (KeyValuePair<string, int> pair in words)
                {
                    m_TreemapControl.Nodes.Add(pair.Key, pair.Value, pair.Value, null, string.Format("{0} - {1}% - {2} occurances", pair.Key, Math.Round(pair.Value * 100 / sum, 2), pair.Value));

                }
                m_TreemapControl.EndUpdate();

            }
}

        private void FillMap(IWordRegistry wordCounter, ICloudControl cloudControl)
        {

            KeyValuePair<string, int>[] pairs = wordCounter.GetSortedByOccurances();
            //Array.Resize(ref pairs, Math.Min(100, pairs.Length));

            cloudControl.Show(pairs);
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var fontFamily in FontFamily.Families)
            {
                if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                {
                    toolStripComboBoxFont.Items.Add(fontFamily.Name);
                }
            }

            m_CloudControl.SuspendLayout();

            toolStripComboBoxMinFontSize.SelectedItem = toolStripComboBoxMinFontSize.Items[0];
            toolStripComboBoxMaxFontSize.SelectedItem = toolStripComboBoxMinFontSize.Items[toolStripComboBoxMinFontSize.Items.Count - 1];

            toolStripComboBoxFont.Text = m_CloudControl.Font.FontFamily.Name;
            m_CloudControl.ResumeLayout();
        }

        private void toolStripComboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
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
        }
    }
}