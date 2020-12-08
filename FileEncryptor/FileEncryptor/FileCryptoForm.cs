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

namespace FileEncryptor
{
    public partial class FileCryptoForm : Form
    {
        private readonly FileCryptographyService cryptoService;

        public FileCryptoForm()
        {
            InitializeComponent();
            cryptoService = new FileCryptographyService();
        }

        private void fileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (fileRadioButton.Checked)
            {
                selectButton.Text = "Select File";
                UpdateSelectionLabel(fileBrowser.FileName);
                UpdateButtonsEnabled();
            }
        }

        private void folderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (folderRadioButton.Checked)
            {
                selectButton.Text = "Select Folder";
                UpdateSelectionLabel(folderBrowser.SelectedPath);
                UpdateButtonsEnabled();
            }
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            string path = selectionLabel.Text;
            string key = passwordInput.Text;
            if (fileRadioButton.Checked)
            {
                cryptoService.EncryptFile(path, key);
            }
            else if (folderRadioButton.Checked)
            {
                cryptoService.EncryptFolder(path, key);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            string path = selectionLabel.Text;
            string key = passwordInput.Text;
            try
            {
                if (fileRadioButton.Checked)
                {
                    cryptoService.DecryptFile(path, key);
                }
                else if (folderRadioButton.Checked)
                {
                    cryptoService.DecryptFolder(path, key);
                }
            }
            catch (CryptographicException)
            {
                MessageBox.Show(
                    "Decryption failed. Either the password is incorrect or an entity has not been encrypted",
                    "Decryption error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (fileRadioButton.Checked)
            {
                fileBrowser.ShowDialog();
                UpdateSelectionLabel(fileBrowser.FileName);
            }
            else
            {
                folderBrowser.ShowDialog();
                UpdateSelectionLabel(folderBrowser.SelectedPath);
            }
            UpdateButtonsEnabled();
        }

        private void passwordInput_TextChanged(object sender, EventArgs e)
        {
            passwordInput.Text = passwordInput.Text.TrimStart();
            UpdateButtonsEnabled();
        }

        private void UpdateSelectionLabel(string selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                selectionLabel.Text = $"No {(fileRadioButton.Checked ? "file" : "folder")} selected";
            }
            else
            {
                selectionLabel.Text = selectedPath;
            }
        }

        private void UpdateButtonsEnabled()
        {
            bool isValidInput = !string.IsNullOrWhiteSpace(passwordInput.Text) &&
                (fileRadioButton.Checked && !string.IsNullOrWhiteSpace(fileBrowser.FileName) || 
                 folderRadioButton.Checked && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath));
            encryptButton.Enabled = isValidInput;
            decryptButton.Enabled = isValidInput;
        }
    }
}
