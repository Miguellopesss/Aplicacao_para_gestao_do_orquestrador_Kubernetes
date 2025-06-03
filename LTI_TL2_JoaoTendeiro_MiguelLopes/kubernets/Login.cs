using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kubernets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace kubernets
{
    public partial class Login : Form
    {
        private List<Table> tables = new List<Table>();
        private bool tokenVisible = true;

        public Login()
        {
            InitializeComponent();
            LoadData();
            this.AcceptButton = buttonLogin;
            this.Load += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    textBoxUsername.Focus();
                }));
            };
            textBoxUsername.TabIndex = 0;
            textBoxIpAddress.TabIndex = 1;
            textBoxToken.TabIndex = 2;
            buttonLogin.TabIndex = 3;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string ipAddress = textBoxIpAddress.Text.Trim();
            string token = textBoxToken.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Please enter the username, IP address, and token.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kubernetesApiUrl = $"https://{ipAddress}:6443/api/v1/pods";

            bool isAuthenticated = await AuthenticateWithKubernetesApiAsync(kubernetesApiUrl, token,username,ipAddress);

            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SaveLoginDetails(username, ipAddress, token);

                this.Hide(); 
                K8sDashboard.DashboardForm mainForm = new K8sDashboard.DashboardForm(ipAddress,token);
                mainForm.FormClosed += (s, args) =>
                {
                    textBoxUsername.Text = "";
                    textBoxIpAddress.Text = "";
                    textBoxToken.Text = "";
                    this.Show();
                };
                mainForm.Show();
            }
        }

        private async Task<bool> AuthenticateWithKubernetesApiAsync(string apiUrl, string token, string username, string ipAddress)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                    HttpResponseMessage response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        using (var context = new AppDbContext())
                        {
                            var tableToRemove = await context.Table
                                .FirstOrDefaultAsync(d => d.Name == username && d.IpAddress == ipAddress && d.Token == token)
                                .ConfigureAwait(false);

                            if (tableToRemove != null)
                            {
                                context.Table.Remove(tableToRemove);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                this.Invoke(new Action(() =>
                                {
                                    LoadData();
                                    MessageBox.Show("Expired token! Data has been removed from the database.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Invalid or expired token.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }));
                            }
                        }

                        return false;
                    }

                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Token does not have permission to access this resource.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));

                        return false;
                    }

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Unexpected error: {response.StatusCode}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));

                    return false;
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));

                return false;
            }
        }


        private void SaveLoginDetails(string username, string ipAddress, string token)
        {
            string encryptionKey = "kubernetsLti";

            bool tokenExists = tables.Any(d =>
                d.Name.StartsWith(username) &&
                d.IpAddress == ipAddress &&
                d.Token == token);

            if (tokenExists)
            {
                return;
            }

            using (var context = new AppDbContext())
            {
                var existingTables = context.Table
                    .Where(d => d.Name.StartsWith(username) && d.IpAddress == ipAddress)
                    .ToList();

                string finalName = username;
                int suffix = 1;

                while (existingTables.Any(d => d.Name == finalName))
                {
                    finalName = $"{username}_{suffix}";
                    suffix++;
                }

                string encryptedToken = EncryptToken(token, encryptionKey);

                var table = new Table
                {
                    Name = finalName,
                    IpAddress = ipAddress,
                    Token = encryptedToken
                };

                context.Table.Add(table);
                context.SaveChanges();
            }

            LoadData(); 
        }
        private async void LoadData()
        {
            string encryptionKey = "kubernetsLti";

            using (var context = new AppDbContext())
            {
                var allTables = await context.Table.ToListAsync();

                tables = allTables.Select(d =>
                {
                    string decryptedToken;
                    try
                    {
                        decryptedToken = DecryptToken(d.Token, encryptionKey);
                    }
                    catch
                    {
                        decryptedToken = ""; 
                    }

                    return new Table
                    {
                        Name = d.Name,
                        IpAddress = d.IpAddress,
                        Token = decryptedToken 
                    };
                }).ToList();
            }

            listBox1.Items.Clear();
            foreach (var table in tables)
            {
                listBox1.Items.Add($"Username: {table.Name} -> {table.IpAddress}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < tables.Count)
            {
                var selecionado = tables[listBox1.SelectedIndex];

                textBoxUsername.Text = selecionado.Name;
                textBoxIpAddress.Text = selecionado.IpAddress;
                textBoxToken.Text = selecionado.Token;
            }
        }

        private string EncryptToken(string plainText, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptToken(string cipherText, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));

                var cipherBytes = Convert.FromBase64String(cipherText);
                using (var ms = new MemoryStream(cipherBytes))
                {
                    var iv = new byte[16];
                    ms.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        private void buttonToggleToken_Click(object sender, EventArgs e)
        {
            tokenVisible = !tokenVisible;
            textBoxToken.UseSystemPasswordChar = tokenVisible;

            buttonToggleToken.BackgroundImage = tokenVisible
                                    ? Properties.Resources.eye_slash_Icon
    :                                   Properties.Resources.eyeIcon;
        }
    }
}
