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

            foreach (var layoutType in Enum.GetValues(typeof(LayoutType)))
            {
                this.toolStripComboBoxLayout.Items.Add(layoutType);
            }
            
            this.toolStripComboBoxLayout.SelectedItem = LayoutType.Spiral;
        }

        private void ToolStripButtonGoClick(object sender, EventArgs e)
        {
            IsRunning = true;

            string path = FolderTree.SelectedPath;
            IProgressIndicator progressBarWrapper = new ProgressBarWrapper(ToolStripProgressBar);
            IWordRegistry wordRegistry = CountWords(path, progressBarWrapper);
            KeyValuePair<string, int>[] pairs = wordRegistry.GetSortedByOccurances();
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

            toolStripComboBoxMinFontSize.SelectedItem = "6"; // toolStripComboBoxMinFontSize.Items[0];
            toolStripComboBoxMaxFontSize.SelectedItem = "72"; //toolStripComboBoxMaxFontSize.Items[toolStripComboBoxMaxFontSize.Items.Count - 1];

            toolStripComboBoxFont.SelectedItem = "Tahoma";
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



        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            CloudControl cloudControl = e.AssociatedControl as CloudControl;
            if (cloudControl==null)
            {
                return;
            }

            LayoutItem itemUderMouse;
            Point mousePositionRelativeToControl = new Point(MousePosition.X-cloudControl.Top, MousePosition.Y - cloudControl.Left);
            if (!cloudControl.TryGetItemAtLocation(mousePositionRelativeToControl, out itemUderMouse))
            {
                return;
            }

            toolTip.SetToolTip(cloudControl, itemUderMouse.ToString());
        }
    }
}
