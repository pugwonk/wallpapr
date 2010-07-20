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
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.numFrequency = new System.Windows.Forms.NumericUpDown();
            this.ddInterval = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddOrderBy = new System.Windows.Forms.ComboBox();
            this.rbAnyTags = new System.Windows.Forms.RadioButton();
            this.rbAllTags = new System.Windows.Forms.RadioButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getNewWallpaperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getNewWallpaperToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.ddPosition = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.thisPhotoOnFlickrcomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numFrequency)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(185, 118);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(136, 20);
            this.txtUserId.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(182, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "User Names (comma delimited):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tags:";
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(9, 41);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(324, 20);
            this.txtTags.TabIndex = 3;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 313);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Flickr API Key:";
            this.label2.Visible = false;
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(86, 310);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(260, 20);
            this.txtApiKey.TabIndex = 10;
            this.txtApiKey.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ddOrderBy);
            this.groupBox1.Controls.Add(this.rbAnyTags);
            this.groupBox1.Controls.Add(this.rbAllTags);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTags);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtUserId);
            this.groupBox1.Location = new System.Drawing.Point(7, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 160);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Options";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(38, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "(comma delimited)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Pick from:";
            // 
            // ddOrderBy
            // 
            this.ddOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddOrderBy.FormattingEnabled = true;
            this.ddOrderBy.Location = new System.Drawing.Point(9, 118);
            this.ddOrderBy.Name = "ddOrderBy";
            this.ddOrderBy.Size = new System.Drawing.Size(154, 21);
            this.ddOrderBy.TabIndex = 9;
            // 
            // rbAnyTags
            // 
            this.rbAnyTags.AutoSize = true;
            this.rbAnyTags.Location = new System.Drawing.Point(106, 67);
            this.rbAnyTags.Name = "rbAnyTags";
            this.rbAnyTags.Size = new System.Drawing.Size(93, 17);
            this.rbAnyTags.TabIndex = 8;
            this.rbAnyTags.TabStop = true;
            this.rbAnyTags.Text = "Match any tag";
            this.rbAnyTags.UseVisualStyleBackColor = true;
            // 
            // rbAllTags
            // 
            this.rbAllTags.AutoSize = true;
            this.rbAllTags.Location = new System.Drawing.Point(9, 67);
            this.rbAllTags.Name = "rbAllTags";
            this.rbAllTags.Size = new System.Drawing.Size(91, 17);
            this.rbAllTags.TabIndex = 7;
            this.rbAllTags.TabStop = true;
            this.rbAllTags.Text = "Match all tags";
            this.rbAllTags.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Wallpaper Flickr";
            this.notifyIcon1.Visible = true;
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(205, 92);
            // 
            // getNewWallpaperToolStripMenuItem
            // 
            this.getNewWallpaperToolStripMenuItem.Name = "getNewWallpaperToolStripMenuItem";
            this.getNewWallpaperToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.getNewWallpaperToolStripMenuItem.Text = "Show window";
            this.getNewWallpaperToolStripMenuItem.Click += new System.EventHandler(this.getNewWallpaperToolStripMenuItem_Click);
            // 
            // getNewWallpaperToolStripMenuItem1
            // 
            this.getNewWallpaperToolStripMenuItem1.Name = "getNewWallpaperToolStripMenuItem1";
            this.getNewWallpaperToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.getNewWallpaperToolStripMenuItem1.Text = "Get new wallpaper";
            this.getNewWallpaperToolStripMenuItem1.Click += new System.EventHandler(this.getNewWallpaperToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
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
            this.btnOK.Location = new System.Drawing.Point(124, 239);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 37);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // thisPhotoOnFlickrcomToolStripMenuItem
            // 
            this.thisPhotoOnFlickrcomToolStripMenuItem.Name = "thisPhotoOnFlickrcomToolStripMenuItem";
            this.thisPhotoOnFlickrcomToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.thisPhotoOnFlickrcomToolStripMenuItem.Text = "This photo on flickr.com";
            this.thisPhotoOnFlickrcomToolStripMenuItem.Click += new System.EventHandler(this.thisPhotoOnFlickrcomToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 288);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ddPosition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ddInterval);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.numFrequency);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WallpaperFlickr";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numFrequency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.NumericUpDown numFrequency;
        private System.Windows.Forms.ComboBox ddInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddOrderBy;
        private System.Windows.Forms.RadioButton rbAnyTags;
        private System.Windows.Forms.RadioButton rbAllTags;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem getNewWallpaperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getNewWallpaperToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddPosition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolStripMenuItem thisPhotoOnFlickrcomToolStripMenuItem;

    }
}

