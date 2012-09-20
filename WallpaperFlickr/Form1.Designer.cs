namespace WallpaperFlickr {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.numFrequency = new System.Windows.Forms.NumericUpDown();
            this.ddInterval = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbExplore = new System.Windows.Forms.RadioButton();
            this.txtFaveUserId = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddOrderBy = new System.Windows.Forms.ComboBox();
            this.rbAnyTags = new System.Windows.Forms.RadioButton();
            this.rbAllTags = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.rbFaves = new System.Windows.Forms.RadioButton();
            this.rbSearch = new System.Windows.Forms.RadioButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getNewWallpaperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getNewWallpaperToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.thisPhotoOnFlickrcomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.ddPosition = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbVersion = new System.Windows.Forms.Label();
            this.llWebsite = new System.Windows.Forms.LinkLabel();
            this.cbStartWithWindows = new System.Windows.Forms.CheckBox();
            this.cbCache = new System.Windows.Forms.CheckBox();
            this.cbBubbles = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numFrequency)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numFrequency
            // 
            this.numFrequency.Location = new System.Drawing.Point(86, 7);
            this.numFrequency.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numFrequency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrequency.Name = "numFrequency";
            this.numFrequency.Size = new System.Drawing.Size(62, 20);
            this.numFrequency.TabIndex = 6;
            this.numFrequency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ddInterval
            // 
            this.ddInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddInterval.FormattingEnabled = true;
            this.ddInterval.Location = new System.Drawing.Point(154, 7);
            this.ddInterval.Name = "ddInterval";
            this.ddInterval.Size = new System.Drawing.Size(113, 21);
            this.ddInterval.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Rotate every:";
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbExplore);
            this.groupBox1.Controls.Add(this.txtFaveUserId);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.rbFaves);
            this.groupBox1.Controls.Add(this.rbSearch);
            this.groupBox1.Location = new System.Drawing.Point(7, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 247);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Photo Selection";
            // 
            // rbExplore
            // 
            this.rbExplore.AutoSize = true;
            this.rbExplore.Location = new System.Drawing.Point(5, 214);
            this.rbExplore.Name = "rbExplore";
            this.rbExplore.Size = new System.Drawing.Size(196, 17);
            this.rbExplore.TabIndex = 16;
            this.rbExplore.TabStop = true;
            this.rbExplore.Text = "Interesting photos uploaded recently";
            this.rbExplore.UseVisualStyleBackColor = true;
            this.rbExplore.CheckedChanged += new System.EventHandler(this.rbExplore_CheckedChanged);
            // 
            // txtFaveUserId
            // 
            this.txtFaveUserId.Location = new System.Drawing.Point(148, 178);
            this.txtFaveUserId.Name = "txtFaveUserId";
            this.txtFaveUserId.Size = new System.Drawing.Size(136, 20);
            this.txtFaveUserId.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ddOrderBy);
            this.panel1.Controls.Add(this.rbAnyTags);
            this.panel1.Controls.Add(this.rbAllTags);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtTags);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtUserId);
            this.panel1.Location = new System.Drawing.Point(77, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 140);
            this.panel1.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(45, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "(comma delimited)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Pick from:";
            // 
            // ddOrderBy
            // 
            this.ddOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddOrderBy.FormattingEnabled = true;
            this.ddOrderBy.Location = new System.Drawing.Point(16, 103);
            this.ddOrderBy.Name = "ddOrderBy";
            this.ddOrderBy.Size = new System.Drawing.Size(154, 21);
            this.ddOrderBy.TabIndex = 18;
            // 
            // rbAnyTags
            // 
            this.rbAnyTags.AutoSize = true;
            this.rbAnyTags.Location = new System.Drawing.Point(113, 52);
            this.rbAnyTags.Name = "rbAnyTags";
            this.rbAnyTags.Size = new System.Drawing.Size(93, 17);
            this.rbAnyTags.TabIndex = 17;
            this.rbAnyTags.TabStop = true;
            this.rbAnyTags.Text = "Match any tag";
            this.rbAnyTags.UseVisualStyleBackColor = true;
            // 
            // rbAllTags
            // 
            this.rbAllTags.AutoSize = true;
            this.rbAllTags.Location = new System.Drawing.Point(16, 52);
            this.rbAllTags.Name = "rbAllTags";
            this.rbAllTags.Size = new System.Drawing.Size(91, 17);
            this.rbAllTags.TabIndex = 16;
            this.rbAllTags.TabStop = true;
            this.rbAllTags.Text = "Match all tags";
            this.rbAllTags.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tags:";
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(16, 26);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(324, 20);
            this.txtTags.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(189, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "User Names (comma delimited):";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(192, 103);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(136, 20);
            this.txtUserId.TabIndex = 15;
            // 
            // rbFaves
            // 
            this.rbFaves.AutoSize = true;
            this.rbFaves.Location = new System.Drawing.Point(6, 178);
            this.rbFaves.Name = "rbFaves";
            this.rbFaves.Size = new System.Drawing.Size(136, 17);
            this.rbFaves.TabIndex = 13;
            this.rbFaves.TabStop = true;
            this.rbFaves.Text = "Show favorites for user:";
            this.rbFaves.UseVisualStyleBackColor = true;
            // 
            // rbSearch
            // 
            this.rbSearch.AutoSize = true;
            this.rbSearch.Location = new System.Drawing.Point(12, 19);
            this.rbSearch.Name = "rbSearch";
            this.rbSearch.Size = new System.Drawing.Size(59, 17);
            this.rbSearch.TabIndex = 12;
            this.rbSearch.TabStop = true;
            this.rbSearch.Text = "Search";
            this.rbSearch.UseVisualStyleBackColor = true;
            this.rbSearch.CheckedChanged += new System.EventHandler(this.rbSearch_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Wallpaper Flickr";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getNewWallpaperToolStripMenuItem,
            this.getNewWallpaperToolStripMenuItem1,
            this.thisPhotoOnFlickrcomToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 92);
            // 
            // getNewWallpaperToolStripMenuItem
            // 
            this.getNewWallpaperToolStripMenuItem.Name = "getNewWallpaperToolStripMenuItem";
            this.getNewWallpaperToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.getNewWallpaperToolStripMenuItem.Text = "Settings...";
            this.getNewWallpaperToolStripMenuItem.Click += new System.EventHandler(this.getNewWallpaperToolStripMenuItem_Click);
            // 
            // getNewWallpaperToolStripMenuItem1
            // 
            this.getNewWallpaperToolStripMenuItem1.Name = "getNewWallpaperToolStripMenuItem1";
            this.getNewWallpaperToolStripMenuItem1.Size = new System.Drawing.Size(213, 22);
            this.getNewWallpaperToolStripMenuItem1.Text = "Get new wallpaper";
            this.getNewWallpaperToolStripMenuItem1.Click += new System.EventHandler(this.getNewWallpaperToolStripMenuItem1_Click);
            // 
            // thisPhotoOnFlickrcomToolStripMenuItem
            // 
            this.thisPhotoOnFlickrcomToolStripMenuItem.Name = "thisPhotoOnFlickrcomToolStripMenuItem";
            this.thisPhotoOnFlickrcomToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.thisPhotoOnFlickrcomToolStripMenuItem.Text = "This photo on flickr.com...";
            this.thisPhotoOnFlickrcomToolStripMenuItem.Click += new System.EventHandler(this.thisPhotoOnFlickrcomToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(282, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(64, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Rotate Now";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Position:";
            // 
            // ddPosition
            // 
            this.ddPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddPosition.FormattingEnabled = true;
            this.ddPosition.Location = new System.Drawing.Point(86, 32);
            this.ddPosition.Name = "ddPosition";
            this.ddPosition.Size = new System.Drawing.Size(113, 21);
            this.ddPosition.TabIndex = 14;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(197, 343);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 37);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbVersion
            // 
            this.lbVersion.Location = new System.Drawing.Point(4, 351);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(108, 13);
            this.lbVersion.TabIndex = 16;
            this.lbVersion.Text = "label8";
            // 
            // llWebsite
            // 
            this.llWebsite.AutoSize = true;
            this.llWebsite.Location = new System.Drawing.Point(4, 367);
            this.llWebsite.Name = "llWebsite";
            this.llWebsite.Size = new System.Drawing.Size(146, 13);
            this.llWebsite.TabIndex = 17;
            this.llWebsite.TabStop = true;
            this.llWebsite.Text = "http://wallpapr.codeplex.com";
            this.llWebsite.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWebsite_LinkClicked);
            // 
            // cbStartWithWindows
            // 
            this.cbStartWithWindows.AutoSize = true;
            this.cbStartWithWindows.Location = new System.Drawing.Point(331, 328);
            this.cbStartWithWindows.Name = "cbStartWithWindows";
            this.cbStartWithWindows.Size = new System.Drawing.Size(117, 17);
            this.cbStartWithWindows.TabIndex = 18;
            this.cbStartWithWindows.Text = "Start with Windows";
            this.cbStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // cbCache
            // 
            this.cbCache.AutoSize = true;
            this.cbCache.Location = new System.Drawing.Point(331, 347);
            this.cbCache.Name = "cbCache";
            this.cbCache.Size = new System.Drawing.Size(97, 17);
            this.cbCache.TabIndex = 19;
            this.cbCache.Text = "Cache pictures";
            this.cbCache.UseVisualStyleBackColor = true;
            // 
            // cbBubbles
            // 
            this.cbBubbles.AutoSize = true;
            this.cbBubbles.Location = new System.Drawing.Point(331, 366);
            this.cbBubbles.Name = "cbBubbles";
            this.cbBubbles.Size = new System.Drawing.Size(113, 17);
            this.cbBubbles.TabIndex = 20;
            this.cbBubbles.Text = "Show info-bubbles";
            this.cbBubbles.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 390);
            this.Controls.Add(this.cbBubbles);
            this.Controls.Add(this.cbCache);
            this.Controls.Add(this.ddPosition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbStartWithWindows);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddInterval);
            this.Controls.Add(this.llWebsite);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numFrequency);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wallpapr";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numFrequency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numFrequency;
        private System.Windows.Forms.ComboBox ddInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem getNewWallpaperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getNewWallpaperToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddPosition;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolStripMenuItem thisPhotoOnFlickrcomToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbSearch;
        private System.Windows.Forms.RadioButton rbFaves;
        private System.Windows.Forms.TextBox txtFaveUserId;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddOrderBy;
        private System.Windows.Forms.RadioButton rbAnyTags;
        private System.Windows.Forms.RadioButton rbAllTags;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.LinkLabel llWebsite;
        private System.Windows.Forms.CheckBox cbStartWithWindows;
        private System.Windows.Forms.RadioButton rbExplore;
        private System.Windows.Forms.CheckBox cbCache;
        private System.Windows.Forms.CheckBox cbBubbles;

    }
}

