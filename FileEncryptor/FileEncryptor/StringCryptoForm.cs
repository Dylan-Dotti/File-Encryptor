using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FileEncryptor
{
    public partial class StringCryptoForm : Form
    {
        private readonly CryptographyService encryptor;

        public StringCryptoForm()
        {
            InitializeComponent();
            encryptor = new CryptographyService();
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            string input = inputField.Text;
            string password = passwordField.Text;
            byte[] encrypted = encryptor.Encrypt(input, password);
            outputField.Text = BitConverter.ToString(encrypted);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            string[] strings = inputField.Text.Split('-');
            byte[] inputBytes = new byte[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                inputBytes[i] = Convert.ToByte(strings[i], 16);
            }
            string password = passwordField.Text;
            try
            {
                outputField.Text = encryptor.DecryptToString(inputBytes, password);
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Invalid password", "Invalid Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
