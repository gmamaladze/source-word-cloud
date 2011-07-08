﻿namespace Gma.CodeCloud
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Splitter Splitter;
            System.Windows.Forms.ToolStripLabel toolStripLabel1;
            System.Windows.Forms.ToolStripLabel toolStripLabel2;
            System.Windows.Forms.ToolStripLabel toolStripLabel3;
            System.Windows.Forms.ToolStripLabel toolStripLabel5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TreeMap = new Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl();
            this.FolderTree = new WindowsExplorer.ExplorerTree();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonGo = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonEditBlacklist = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxFont = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxMinFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxMaxFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox3 = new System.Windows.Forms.ToolStripComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            Splitter = new System.Windows.Forms.Splitter();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.MainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Splitter
            // 
            Splitter.Dock = System.Windows.Forms.DockStyle.Right;
            Splitter.Location = new System.Drawing.Point(1164, 28);
            Splitter.Name = "Splitter";
            Splitter.Size = new System.Drawing.Size(3, 491);
            Splitter.TabIndex = 3;
            Splitter.TabStop = false;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(54, 25);
            toolStripLabel1.Text = "size from:";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(21, 25);
            toolStripLabel2.Text = "to:";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new System.Drawing.Size(52, 25);
            toolStripLabel3.Text = "Show top";
            toolStripLabel3.Visible = false;
            // 
            // toolStripLabel5
            // 
            toolStripLabel5.Name = "toolStripLabel5";
            toolStripLabel5.Size = new System.Drawing.Size(33, 25);
            toolStripLabel5.Text = "Font:";
            // 
            // TreeMap
            // 
            this.TreeMap.AllowDrag = false;
            this.TreeMap.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.TreeMap.DiscreteNegativeColors = 20;
            this.TreeMap.DiscretePositiveColors = 20;
            this.TreeMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeMap.EmptySpaceLocation = Microsoft.Research.CommunityTechnologies.Treemap.EmptySpaceLocation.DeterminedByLayoutAlgorithm;
            this.TreeMap.FontFamily = "Consolas";
            this.TreeMap.FontSolidColor = System.Drawing.SystemColors.WindowText;
            this.TreeMap.IsZoomable = false;
            this.TreeMap.LayoutAlgorithm = Microsoft.Research.CommunityTechnologies.Treemap.LayoutAlgorithm.TopWeightedSquarified;
            this.TreeMap.Location = new System.Drawing.Point(0, 0);
            this.TreeMap.MaxColor = System.Drawing.Color.White;
            this.TreeMap.MaxColorMetric = 100F;
            this.TreeMap.MinColor = System.Drawing.Color.Red;
            this.TreeMap.MinColorMetric = -100F;
            this.TreeMap.Name = "TreeMap";
            this.TreeMap.NodeColorAlgorithm = Microsoft.Research.CommunityTechnologies.Treemap.NodeColorAlgorithm.UseColorMetric;
            this.TreeMap.NodeLevelsWithText = Microsoft.Research.CommunityTechnologies.Treemap.NodeLevelsWithText.All;
            this.TreeMap.PaddingDecrementPerLevelPx = 1;
            this.TreeMap.PaddingPx = 2;
            this.TreeMap.PenWidthDecrementPerLevelPx = 1;
            this.TreeMap.PenWidthPx = 1;
            this.TreeMap.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.TreeMap.SelectedFontColor = System.Drawing.SystemColors.HighlightText;
            this.TreeMap.ShowToolTips = true;
            this.TreeMap.Size = new System.Drawing.Size(746, 491);
            this.TreeMap.TabIndex = 0;
            this.TreeMap.TextLocation = Microsoft.Research.CommunityTechnologies.Treemap.TextLocation.CenterCenter;
            // 
            // FolderTree
            // 
            this.FolderTree.BackColor = System.Drawing.Color.White;
            this.FolderTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderTree.Location = new System.Drawing.Point(0, 0);
            this.FolderTree.Name = "FolderTree";
            this.FolderTree.SelectedPath = "C:\\Program Files\\Microsoft Visual Studio 10.0\\Common7\\IDE";
            this.FolderTree.ShowAddressbar = true;
            this.FolderTree.ShowMyDocuments = false;
            this.FolderTree.ShowMyFavorites = false;
            this.FolderTree.ShowMyNetwork = false;
            this.FolderTree.ShowToolbar = false;
            this.FolderTree.Size = new System.Drawing.Size(414, 491);
            this.FolderTree.TabIndex = 2;
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonGo,
            this.ToolStripButtonCancel,
            this.ToolStripProgressBar,
            this.toolStripSeparator1,
            this.ToolStripButtonEditBlacklist,
            this.toolStripSeparator2,
            toolStripLabel5,
            this.toolStripComboBoxFont,
            toolStripLabel1,
            this.toolStripComboBoxMinFontSize,
            toolStripLabel2,
            this.toolStripComboBoxMaxFontSize,
            this.toolStripLabel4,
            this.toolStripSeparator3,
            toolStripLabel3,
            this.toolStripComboBox3});
            this.MainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(1167, 28);
            this.MainToolStrip.TabIndex = 5;
            this.MainToolStrip.Text = "toolStrip1";
            // 
            // ToolStripButtonGo
            // 
            this.ToolStripButtonGo.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonGo.Image")));
            this.ToolStripButtonGo.ImageTransparentColor = System.Drawing.Color.White;
            this.ToolStripButtonGo.Name = "ToolStripButtonGo";
            this.ToolStripButtonGo.Size = new System.Drawing.Size(72, 25);
            this.ToolStripButtonGo.Text = "Generate";
            this.ToolStripButtonGo.Click += new System.EventHandler(this.ToolStripButtonGoClick);
            // 
            // ToolStripButtonCancel
            // 
            this.ToolStripButtonCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButtonCancel.Enabled = false;
            this.ToolStripButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonCancel.Image")));
            this.ToolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonCancel.Name = "ToolStripButtonCancel";
            this.ToolStripButtonCancel.Size = new System.Drawing.Size(43, 25);
            this.ToolStripButtonCancel.Text = "Cancel";
            this.ToolStripButtonCancel.Click += new System.EventHandler(this.ToolStripButtonCancelClick);
            // 
            // ToolStripProgressBar
            // 
            this.ToolStripProgressBar.Margin = new System.Windows.Forms.Padding(3);
            this.ToolStripProgressBar.Name = "ToolStripProgressBar";
            this.ToolStripProgressBar.Size = new System.Drawing.Size(200, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // ToolStripButtonEditBlacklist
            // 
            this.ToolStripButtonEditBlacklist.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButtonEditBlacklist.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonEditBlacklist.Image")));
            this.ToolStripButtonEditBlacklist.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonEditBlacklist.Name = "ToolStripButtonEditBlacklist";
            this.ToolStripButtonEditBlacklist.Size = new System.Drawing.Size(69, 25);
            this.ToolStripButtonEditBlacklist.Text = "Edit Blacklist";
            this.ToolStripButtonEditBlacklist.Click += new System.EventHandler(this.ToolStripButtonEditBlacklist_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripComboBoxFont
            // 
            this.toolStripComboBoxFont.Name = "toolStripComboBoxFont";
            this.toolStripComboBoxFont.Size = new System.Drawing.Size(150, 28);
            this.toolStripComboBoxFont.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxFont_SelectedIndexChanged);
            // 
            // toolStripComboBoxMinFontSize
            // 
            this.toolStripComboBoxMinFontSize.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "20",
            "24",
            "28",
            "36",
            "48",
            "72"});
            this.toolStripComboBoxMinFontSize.Name = "toolStripComboBoxMinFontSize";
            this.toolStripComboBoxMinFontSize.Size = new System.Drawing.Size(75, 28);
            this.toolStripComboBoxMinFontSize.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxFont_SelectedIndexChanged);
            // 
            // toolStripComboBoxMaxFontSize
            // 
            this.toolStripComboBoxMaxFontSize.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "20",
            "24",
            "28",
            "36",
            "48",
            "60",
            "72",
            "80",
            "86"});
            this.toolStripComboBoxMaxFontSize.Name = "toolStripComboBoxMaxFontSize";
            this.toolStripComboBoxMaxFontSize.Size = new System.Drawing.Size(75, 28);
            this.toolStripComboBoxMaxFontSize.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxFont_SelectedIndexChanged);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(0, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripComboBox3
            // 
            this.toolStripComboBox3.Items.AddRange(new object[] {
            "10",
            "20",
            "50",
            "100",
            "300",
            "500",
            "1000"});
            this.toolStripComboBox3.Name = "toolStripComboBox3";
            this.toolStripComboBox3.Size = new System.Drawing.Size(75, 28);
            this.toolStripComboBox3.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeMap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FolderTree);
            this.splitContainer1.Size = new System.Drawing.Size(1164, 491);
            this.splitContainer1.SplitterDistance = 746;
            this.splitContainer1.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 519);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(Splitter);
            this.Controls.Add(this.MainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Source Code Word Colud Generator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl TreeMap;
        private WindowsExplorer.ExplorerTree FolderTree;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton ToolStripButtonGo;
        private System.Windows.Forms.ToolStripProgressBar ToolStripProgressBar;
        private System.Windows.Forms.ToolStripButton ToolStripButtonCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripButtonEditBlacklist;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFont;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxMinFontSize;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxMaxFontSize;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox3;
    }
}
