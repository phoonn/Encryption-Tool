using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
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
            if (result == DialogResult.OK)
            {
                tb_file.Text = openFileDialog1.FileName;
            }
        }

        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            key = tb_encryptionKey.Text.Trim();
            filePath = tb_file.Text;
            if (String.IsNullOrEmpty(key))
            {
                MessageBox.Show("Key is empty!");
                return;

            }
            else if (String.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Invalid path!");
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
            // Encryption process
            encryptionWorker.RunWorkerAsync(argument: "encrypt");

        }

        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            key = tb_decryptionKey.Text.Trim();
            filePath = tb_file.Text;
            if (String.IsNullOrEmpty(key))
            {
                MessageBox.Show("Key is empty!");
                return;

            }
            else if (String.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Invalid path!");
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
            string tempFile = filePath.Substring(0, filePath.LastIndexOf(".")) + "encrypted" + filePath.Substring(filePath.LastIndexOf("."));

            try
            {
                byte[] salt = GenerateSalt();

                Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, salt, 5000);

                RijndaelManaged rijndaelCSP = new RijndaelManaged();
                rijndaelCSP.BlockSize = 128;
                rijndaelCSP.KeySize = 256;
                rijndaelCSP.Padding = PaddingMode.ANSIX923;


                rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

                ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

                FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                long fileSizeBytes = inputFileStream.Length;


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
            }
            catch (Exception ex)
            {
                File.Delete(tempFile);
                MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            stop.Stop();
            playSimpleSound();
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
            string tempFile = filePath.Substring(0, filePath.LastIndexOf(".")) + "decrypted" + filePath.Substring(filePath.LastIndexOf("."));
            try
            {
                using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] salt = new byte[32];
                    inputFileStream.Read(salt, 0, salt.Length);

                    Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, salt, 5000);

                    ICryptoTransform encryptor = null;
                    using (RijndaelManaged rijndaelCSP = new RijndaelManaged())
                    {
                        rijndaelCSP.BlockSize = 128;
                        rijndaelCSP.KeySize = 256;
                        rijndaelCSP.Padding = PaddingMode.ANSIX923; // fills out the encrypted block with zeros in the beginning of the file

                        rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                        rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

                        encryptor = rijndaelCSP.CreateDecryptor();
                    }

                    long fileSizeBytes = inputFileStream.Length;


                    using (FileStream outputFileStream = new FileStream(tempFile, FileMode.Append, FileAccess.Write))
                    {
                        using (var encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                        {
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

                            encryptStream.FlushFinalBlock();
                            outputFileStream.Flush();
                        }
                    }
                }

                File.Replace(tempFile, filePath, null);
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                stop.Stop();
                MessageBox.Show("Wrong decryption key!", "Key Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(tempFile);
                return;
            }
            catch (Exception ex)
            {
                stop.Stop();
                MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(tempFile);
                return;
            }
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
            timeWatchLbl.Text = "Elapsed Time: " + elapsedTime.Substring(0, elapsedTime.LastIndexOf(".") + 3);
            stop.Reset();

            btn_browse.Enabled = true;
            btn_decrypt.Enabled = true;
            btn_encrypt.Enabled = true;
            tb_encryptionKey.ReadOnly = false;
            tb_decryptionKey.ReadOnly = false;
            tb_file.ReadOnly = false;
        }
        private void playSimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(Directory.GetCurrentDirectory()+"\\wow_work_complete.wav");
            simpleSound.Play();
        }
    }
}
