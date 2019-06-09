using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptionTool
{
    public partial class MainForm : Form
    {
        string key;
        const int mbSize = 1048576;
        string filePath;
        Stopwatch stop = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                tb_file.Text = openFileDialog1.FileName;
            }
            Console.WriteLine(result);
        }

        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            key = tb_encryptionKey.Text;
            filePath = tb_file.Text;
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found!");
                return;
            }

            stop.Start();

            btn_browse.Enabled = false;
            btn_decrypt.Enabled = false;
            btn_encrypt.Enabled = false;
            tb_encryptionKey.Enabled = false;
            tb_decryptionKey.Enabled = false;
            tb_file.Enabled = false;
            // Encryption process
            encryptionWorker.RunWorkerAsync(argument: "encrypt");


        }

        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            key = tb_decryptionKey.Text;
            filePath = tb_file.Text;
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found!");
                return;
            }
            stop.Start();

            btn_browse.Enabled = false;
            btn_decrypt.Enabled = false;
            btn_encrypt.Enabled = false;
            tb_encryptionKey.ReadOnly = true;
            tb_decryptionKey.ReadOnly = true;
            tb_file.ReadOnly = true;

            timeWatchLbl.Text = "Elapsed Time:";
            encryptionWorker.RunWorkerAsync(argument: "decrypt");
        }

        private void EncryptFileStream(string filePath, string key)
        {
            //try
            //{
            byte[] salt = GenerateSalt();

            Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, salt, 5000);

            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.BlockSize = 128;
            rijndaelCSP.KeySize = 256;
            rijndaelCSP.Padding = PaddingMode.ANSIX923; // fills out the encrypted block with zeros in the beginning of the file


            rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
            rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

            ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

            FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            long fileSizeBytes = inputFileStream.Length;

            string tempFile = filePath.Substring(0, filePath.LastIndexOf(".")) + "encrypted1" + filePath.Substring(filePath.LastIndexOf("."));

            FileStream outputFileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
            
            outputFileStream.Write(salt, 0, salt.Length);

            CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);


            byte[] buffer = new byte[mbSize];
            long i = 0;
            int num;
            while ((num = inputFileStream.Read(buffer, 0, mbSize)) > 0)
            {
                encryptStream.Write(buffer, 0, num);
                outputFileStream.Flush();

                i++;
                long progress = 100 * (mbSize * i);
                progress = progress / fileSizeBytes;
                if (progress > 100)
                    progress = 100;
                encryptionWorker.ReportProgress((int)progress);
            }
            inputFileStream.Close();


            encryptStream.FlushFinalBlock();
            outputFileStream.Flush();
            encryptStream.Close();
            outputFileStream.Close();
            inputFileStream.Close();

            File.Replace(tempFile, filePath, null);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            stop.Stop();
            MessageBox.Show("File Encryption Complete!");
        }

        private byte[] GenerateSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            return data;
        }

        private void DecryptFileStream(string filePath, string key)
        {
            //try
            //{
            //byte[] keyBytes;
            //keyBytes = Encoding.Unicode.GetBytes(key);

            FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            byte[] salt = new byte[32];
            inputFileStream.Read(salt, 0, salt.Length);

            Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, salt, 5000);

            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.BlockSize = 128;
            rijndaelCSP.KeySize = 256;
            rijndaelCSP.Padding = PaddingMode.ANSIX923; // fills out the encrypted block with zeros in the beginning of the file


            rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
            rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

            ICryptoTransform encryptor = rijndaelCSP.CreateDecryptor();


            long fileSizeBytes = inputFileStream.Length;

            string tempFile = filePath.Substring(0, filePath.LastIndexOf(".")) + "decrypted1" + filePath.Substring(filePath.LastIndexOf("."));

            FileStream outputFileStream = new FileStream(tempFile, FileMode.Append, FileAccess.Write);

            CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);

            byte[] buffer = new byte[mbSize];
            long i = 0;
            int num;
            while ((num = inputFileStream.Read(buffer, 0, mbSize)) > 0)
            {
                encryptStream.Write(buffer, 0, num);
                outputFileStream.Flush();
                
                i++;
                long progress = 100 * (mbSize * i);
                progress = progress / fileSizeBytes;
                if (progress > 100)
                    progress = 100;
                encryptionWorker.ReportProgress((int)progress);
            }
            inputFileStream.Close();


            encryptStream.FlushFinalBlock();
            outputFileStream.Flush();
            encryptStream.Close();
            outputFileStream.Close();
            inputFileStream.Close();

            File.Replace(tempFile, filePath, null);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            stop.Stop();
            MessageBox.Show("File Decryption Complete!");
        }
        

        private void encryptionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument.ToString() == "encrypt")
                EncryptFileStream(filePath, key);
            else
                DecryptFileStream(filePath, key);
        }

        private void encryptionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void encryptionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            key = String.Empty;
            progressBar.Value = 100;
            string elapsedTime = stop.Elapsed.ToString();
            timeWatchLbl.Text = "Elapsed Time: " + elapsedTime.Substring(0,elapsedTime.LastIndexOf(".")+3);
            stop.Reset();
            
            btn_browse.Enabled = true;
            btn_decrypt.Enabled = true;
            btn_encrypt.Enabled = true;
            tb_encryptionKey.ReadOnly = false;
            tb_decryptionKey.ReadOnly = false;
            tb_file.ReadOnly = false;
        }
    }
}
