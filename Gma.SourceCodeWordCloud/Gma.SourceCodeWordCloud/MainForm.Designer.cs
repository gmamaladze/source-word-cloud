namespace CodeWordCloud
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TreeMap = new Microsoft.Research.CommunityTechnologies.Treemap.TreemapControl();
            this.FolderTree = new WindowsExplorer.ExplorerTree();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonGo = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonEditBlacklist = new System.Windows.Forms.ToolStripButton();
            Splitter = new System.Windows.Forms.Splitter();
            this.MainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Splitter
            // 
            Splitter.Dock = System.Windows.Forms.DockStyle.Right;
            Splitter.Location = new System.Drawing.Point(561, 28);
            Splitter.Name = "Splitter";
            Splitter.Size = new System.Drawing.Size(3, 491);
            Splitter.TabIndex = 3;
            Splitter.TabStop = false;
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
            this.TreeMap.Location = new System.Drawing.Point(0, 28);
            this.TreeMap.MaxColor = System.Drawing.Color.Yellow;
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
            this.TreeMap.Size = new System.Drawing.Size(561, 491);
            this.TreeMap.TabIndex = 0;
            this.TreeMap.TextLocation = Microsoft.Research.CommunityTechnologies.Treemap.TextLocation.CenterCenter;
            // 
            // FolderTree
            // 
            this.FolderTree.BackColor = System.Drawing.Color.White;
            this.FolderTree.Dock = System.Windows.Forms.DockStyle.Right;
            this.FolderTree.Location = new System.Drawing.Point(564, 28);
            this.FolderTree.Name = "FolderTree";
            this.FolderTree.SelectedPath = "";
            this.FolderTree.ShowAddressbar = true;
            this.FolderTree.ShowMyDocuments = false;
            this.FolderTree.ShowMyFavorites = false;
            this.FolderTree.ShowMyNetwork = false;
            this.FolderTree.ShowToolbar = false;
            this.FolderTree.Size = new System.Drawing.Size(253, 491);
            this.FolderTree.TabIndex = 2;
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonGo,
            this.ToolStripButtonCancel,
            this.ToolStripProgressBar,
            this.toolStripSeparator1,
            this.ToolStripButtonEditBlacklist});
            this.MainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(817, 28);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 519);
            this.Controls.Add(this.TreeMap);
            this.Controls.Add(Splitter);
            this.Controls.Add(this.FolderTree);
            this.Controls.Add(this.MainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Source Code Word Colud Generator";
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
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
    }
}

