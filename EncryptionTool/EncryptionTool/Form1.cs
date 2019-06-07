using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptionTool
{
    public partial class Form1 : Form
    {
        static string filePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                label1.Text = openFileDialog1.FileName;
                filePath = openFileDialog1.FileName;
            }

            Console.WriteLine(result);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string key = tbKey.Text;
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(filePath))
            {
                return;
            }

            // Encryption process
            EncryptFile(filePath, filePath.Replace("test","test1"), key);




        }

        private void EncryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] keyBytes;
                keyBytes = Encoding.Unicode.GetBytes(key);

                Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

                RijndaelManaged rijndaelCSP = new RijndaelManaged();
                rijndaelCSP.BlockSize = 256;
                rijndaelCSP.KeySize = 256;
                rijndaelCSP.Padding = PaddingMode.ANSIX923; // fills out the encrypted block with zeros in the beginning of the file
                

                rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

                ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

                FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

                byte[] inputFileData = new byte[(int)inputFileStream.Length];
                inputFileStream.Read(inputFileData, 0, (int)inputFileStream.Length);

                FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

                CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);
                encryptStream.Write(inputFileData, 0, (int)inputFileStream.Length);
                encryptStream.FlushFinalBlock();

                rijndaelCSP.Clear();
                encryptStream.Close();
                inputFileStream.Close();
                outputFileStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Encryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("File Encryption Complete!");
        }

        private void DecryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] keyBytes = Encoding.Unicode.GetBytes(key);

                Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

                RijndaelManaged rijndaelCSP = new RijndaelManaged();
                rijndaelCSP.BlockSize = 256;
                rijndaelCSP.KeySize = 256;
                rijndaelCSP.Padding = PaddingMode.Zeros; // fills out the encrypted block with zeros in the beginning of the file

                rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
                rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);
                      
                ICryptoTransform decryptor = rijndaelCSP.CreateDecryptor();

                FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

                CryptoStream decryptStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read);

                byte[] inputFileData = new byte[(int)inputFileStream.Length];
                decryptStream.Read(inputFileData, 0, (int)inputFileStream.Length);

                FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                outputFileStream.Write(inputFileData, 0, inputFileData.Length);
                outputFileStream.Flush();

                rijndaelCSP.Clear();

                decryptStream.Close();
                inputFileStream.Close();
                outputFileStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Decryption Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("File Decryption Complete!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string key = tbKey.Text;
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(filePath))
            {
                return;
            }

            // Encryption process
            DecryptFile(filePath, filePath.Replace("test1","test2"), key);
        }
    }
}
