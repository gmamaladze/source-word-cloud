using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Gma.CodeCloud.Base;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.Languages.CSharp;
using Gma.CodeCloud.Controls;

namespace Gma.CodeCloud
{
    public partial class MainForm : Form
    {
        private const string s_CSharpBlacklistFileName = "CSharpBlacklist.txt";

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
            m_CloudControl.MouseMove += CloudControl_MouseMove;
           
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
        }

        private void CloudControl_MouseMove(object sender, MouseEventArgs e)
        {
            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            if (itemUderMouse == null)
            {
                toolTip.SetToolTip(m_CloudControl, null);
                return;
            }

            toolTip.SetToolTip(
                m_CloudControl,
                string.Format(
                    "\n\r{0} - occurances\n\r{1}% - of total words",
                    itemUderMouse.Weight,
                    Math.Round(itemUderMouse.Weight * 100 / m_TotalWordCount, 2)));

            toolTip.ToolTipTitle = string.Format("Statistics for word [{0}]", itemUderMouse.Word);
        }

        private void CloudControlClick(object sender, EventArgs e)
        {
            LayoutItem itemUderMouse = this.m_CloudControl.ItemUnderMouse;
            if (itemUderMouse==null)
            {
                return;
            }

            MessageBox.Show(
                string.Format(
                    "\n\r{0} - occurances\n\r{1}% - of total words", 
                    itemUderMouse.Weight,
                    Math.Round(itemUderMouse.Weight*100/m_TotalWordCount, 2)),
               string.Format("Statistics for word [{0}]", itemUderMouse.Word));
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            IProgressIndicator progressBarWrapper = new ProgressBarWrapper(ToolStripProgressBar);
            IWordRegistry wordRegistry = CountWords(path, progressBarWrapper);
            KeyValuePair<string, int>[] pairs = wordRegistry.GetSortedByOccurances();
            m_TotalWordCount = wordRegistry.TotalWords;

            m_CloudControl.WeightedWords = pairs;

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

        private void ToolStripButtonEditBlacklistClick(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", s_CSharpBlacklistFileName);
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

            m_CloudControl.LayoutType = (LayoutType) toolStripComboBoxLayout.SelectedItem;
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
    }
}
