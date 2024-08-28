namespace PDEVerifyGUI {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            DirSelectButton = new Button();
            DirTextBox = new TextBox();
            FolderBrowserDialog = new FolderBrowserDialog();
            DirLabel = new Label();
            DirFixButton = new Button();
            FileFixButton = new Button();
            FileTextBox = new TextBox();
            FileSelectButton = new Button();
            DirLabel2 = new Label();
            FileLabel = new Label();
            SuspendLayout();
            // 
            // DirSelectButton
            // 
            DirSelectButton.Font = new Font("Microsoft YaHei UI", 10F);
            DirSelectButton.Location = new Point(332, 128);
            DirSelectButton.Name = "DirSelectButton";
            DirSelectButton.Size = new Size(126, 41);
            DirSelectButton.TabIndex = 0;
            DirSelectButton.Text = "选根目录";
            DirSelectButton.UseVisualStyleBackColor = true;
            DirSelectButton.Click += BtnSelectDirectory_Click;
            // 
            // DirTextBox
            // 
            DirTextBox.AllowDrop = true;
            DirTextBox.Font = new Font("Microsoft YaHei UI", 10F);
            DirTextBox.Location = new Point(12, 83);
            DirTextBox.Name = "DirTextBox";
            DirTextBox.ReadOnly = true;
            DirTextBox.Size = new Size(446, 29);
            DirTextBox.TabIndex = 1;
            // 
            // DirLabel
            // 
            DirLabel.AutoSize = true;
            DirLabel.Font = new Font("Microsoft YaHei UI", 10F);
            DirLabel.ForeColor = Color.OrangeRed;
            DirLabel.Location = new Point(15, 48);
            DirLabel.Name = "DirLabel";
            DirLabel.Size = new Size(290, 23);
            DirLabel.TabIndex = 2;
            DirLabel.Text = "*必选 - 修复根目录下所有.cache文件";
            // 
            // DirFixButton
            // 
            DirFixButton.Font = new Font("Microsoft YaHei UI", 10F);
            DirFixButton.Location = new Point(179, 128);
            DirFixButton.Name = "DirFixButton";
            DirFixButton.Size = new Size(126, 41);
            DirFixButton.TabIndex = 3;
            DirFixButton.Text = "修复全部";
            DirFixButton.UseVisualStyleBackColor = true;
            DirFixButton.Click += DirFixButton_Click;
            // 
            // FileFixButton
            // 
            FileFixButton.Font = new Font("Microsoft YaHei UI", 10F);
            FileFixButton.Location = new Point(179, 310);
            FileFixButton.Name = "FileFixButton";
            FileFixButton.Size = new Size(126, 40);
            FileFixButton.TabIndex = 7;
            FileFixButton.Text = "修复";
            FileFixButton.UseVisualStyleBackColor = true;
            FileFixButton.Click += FileFixButton_Click;
            // 
            // FileTextBox
            // 
            FileTextBox.AllowDrop = true;
            FileTextBox.Font = new Font("Microsoft YaHei UI", 10F);
            FileTextBox.Location = new Point(15, 261);
            FileTextBox.Name = "FileTextBox";
            FileTextBox.ReadOnly = true;
            FileTextBox.Size = new Size(446, 29);
            FileTextBox.TabIndex = 5;
            FileTextBox.DragDrop += FileTextBox_DragDrop;
            FileTextBox.DragEnter += FileTextBox_DragEnter;
            // 
            // FileSelectButton
            // 
            FileSelectButton.Font = new Font("Microsoft YaHei UI", 10F);
            FileSelectButton.Location = new Point(332, 310);
            FileSelectButton.Name = "FileSelectButton";
            FileSelectButton.Size = new Size(126, 40);
            FileSelectButton.TabIndex = 4;
            FileSelectButton.Text = "选择文件";
            FileSelectButton.UseVisualStyleBackColor = true;
            FileSelectButton.Click += FileSelectButton_Click;
            // 
            // DirLabel2
            // 
            DirLabel2.AutoSize = true;
            DirLabel2.Font = new Font("Microsoft YaHei UI", 15F);
            DirLabel2.Location = new Point(12, 9);
            DirLabel2.Name = "DirLabel2";
            DirLabel2.Size = new Size(114, 32);
            DirLabel2.TabIndex = 10;
            DirLabel2.Text = "全部修复";
            // 
            // FileLabel
            // 
            FileLabel.AutoSize = true;
            FileLabel.Font = new Font("Microsoft YaHei UI", 15F);
            FileLabel.Location = new Point(15, 215);
            FileLabel.Name = "FileLabel";
            FileLabel.Size = new Size(114, 32);
            FileLabel.TabIndex = 11;
            FileLabel.Text = "单个修复";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(467, 370);
            Controls.Add(FileTextBox);
            Controls.Add(FileSelectButton);
            Controls.Add(FileLabel);
            Controls.Add(FileFixButton);
            Controls.Add(DirLabel2);
            Controls.Add(DirTextBox);
            Controls.Add(DirLabel);
            Controls.Add(DirFixButton);
            Controls.Add(DirSelectButton);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PDEVerifyGUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label DirLabel;
        private Button DirSelectButton;
        private TextBox DirTextBox;
        private Button DirFixButton;
        private Button FileFixButton;
        private TextBox FileTextBox;
        private Button FileSelectButton;
        private FolderBrowserDialog FolderBrowserDialog;
        private Label DirLabel2;
        private Label FileLabel;
    }
}