namespace EncryptionTool
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_browse = new System.Windows.Forms.Button();
            this.btn_encrypt = new System.Windows.Forms.Button();
            this.tb_encryptionKey = new System.Windows.Forms.TextBox();
            this.btn_decrypt = new System.Windows.Forms.Button();
            this.tb_file = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_decryptionKey = new System.Windows.Forms.TextBox();
            this.encryptionWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timeWatchLbl = new System.Windows.Forms.Label();
            this.cb_noDelete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_browse
            // 
            this.btn_browse.Location = new System.Drawing.Point(234, 33);
            this.btn_browse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(130, 29);
            this.btn_browse.TabIndex = 0;
            this.btn_browse.Text = "Browse File";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // btn_encrypt
            // 
            this.btn_encrypt.Location = new System.Drawing.Point(178, 83);
            this.btn_encrypt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_encrypt.Name = "btn_encrypt";
            this.btn_encrypt.Size = new System.Drawing.Size(130, 32);
            this.btn_encrypt.TabIndex = 2;
            this.btn_encrypt.Text = "Encrypt";
            this.btn_encrypt.UseVisualStyleBackColor = true;
            this.btn_encrypt.Click += new System.EventHandler(this.btn_encrypt_Click);
            // 
            // tb_encryptionKey
            // 
            this.tb_encryptionKey.Location = new System.Drawing.Point(29, 90);
            this.tb_encryptionKey.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_encryptionKey.Name = "tb_encryptionKey";
            this.tb_encryptionKey.Size = new System.Drawing.Size(135, 20);
            this.tb_encryptionKey.TabIndex = 3;
            // 
            // btn_decrypt
            // 
            this.btn_decrypt.Location = new System.Drawing.Point(178, 133);
            this.btn_decrypt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_decrypt.Name = "btn_decrypt";
            this.btn_decrypt.Size = new System.Drawing.Size(130, 35);
            this.btn_decrypt.TabIndex = 4;
            this.btn_decrypt.Text = "Decrypt";
            this.btn_decrypt.UseVisualStyleBackColor = true;
            this.btn_decrypt.Click += new System.EventHandler(this.btn_decrypt_Click);
            // 
            // tb_file
            // 
            this.tb_file.Location = new System.Drawing.Point(29, 38);
            this.tb_file.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_file.Name = "tb_file";
            this.tb_file.Size = new System.Drawing.Size(201, 20);
            this.tb_file.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Choose file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Encryption Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Decription Key";
            // 
            // tb_decryptionKey
            // 
            this.tb_decryptionKey.Location = new System.Drawing.Point(29, 141);
            this.tb_decryptionKey.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_decryptionKey.Name = "tb_decryptionKey";
            this.tb_decryptionKey.Size = new System.Drawing.Size(135, 20);
            this.tb_decryptionKey.TabIndex = 8;
            // 
            // encryptionWorker
            // 
            this.encryptionWorker.WorkerReportsProgress = true;
            this.encryptionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.encryptionWorker_DoWork);
            this.encryptionWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.encryptionWorker_ProgressChanged);
            this.encryptionWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.encryptionWorker_RunWorkerCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(29, 185);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(335, 23);
            this.progressBar.TabIndex = 10;
            // 
            // timeWatchLbl
            // 
            this.timeWatchLbl.AutoSize = true;
            this.timeWatchLbl.Location = new System.Drawing.Point(234, 220);
            this.timeWatchLbl.Name = "timeWatchLbl";
            this.timeWatchLbl.Size = new System.Drawing.Size(74, 13);
            this.timeWatchLbl.TabIndex = 11;
            this.timeWatchLbl.Text = "Time Elapsed:";
            // 
            // cb_noDelete
            // 
            this.cb_noDelete.AutoSize = true;
            this.cb_noDelete.Location = new System.Drawing.Point(234, 64);
            this.cb_noDelete.Name = "cb_noDelete";
            this.cb_noDelete.Size = new System.Drawing.Size(135, 17);
            this.cb_noDelete.TabIndex = 12;
            this.cb_noDelete.Text = "Don\'t delete original file";
            this.cb_noDelete.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(407, 271);
            this.Controls.Add(this.cb_noDelete);
            this.Controls.Add(this.timeWatchLbl);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_decryptionKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_file);
            this.Controls.Add(this.btn_decrypt);
            this.Controls.Add(this.tb_encryptionKey);
            this.Controls.Add(this.btn_encrypt);
            this.Controls.Add(this.btn_browse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Encryption Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Button btn_encrypt;
        private System.Windows.Forms.TextBox tb_encryptionKey;
        private System.Windows.Forms.Button btn_decrypt;
        private System.Windows.Forms.TextBox tb_file;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_decryptionKey;
        private System.ComponentModel.BackgroundWorker encryptionWorker;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label timeWatchLbl;
        private System.Windows.Forms.CheckBox cb_noDelete;
    }
}

