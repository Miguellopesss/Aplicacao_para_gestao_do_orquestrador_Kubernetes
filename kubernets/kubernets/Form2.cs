using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace kubernets
{
    public partial class Form2 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string Endpoint = string.Empty;
        private string Token = string.Empty;

        public Form2(string ipAddress, string token)
        {
            InitializeComponent();

            this.Text = $"Kubernetes - IP: {ipAddress}";
            Endpoint = $"https://{ipAddress}:6443/api";
            Token = token;

            // Ensures correct column header style
            listView1.OwnerDraw = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            listView1.ColumnWidthChanging += listView1_ColumnWidthChanging;
            listView1.DrawColumnHeader += DrawHeader;
            listView1.DrawItem += (s, e) => e.DrawDefault = true;
            listView1.DrawSubItem += (s, e) => e.DrawDefault = true;



            Load += async (s, e) =>
            {
                await LoadNodeNames();
                await LoadNamespacesToListBox();
                await LoadNamespacesToComboBox();
                await LoadAllPods();
            };

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true
            };

            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
        }

        private void DrawHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            using var headerBg = new SolidBrush(Color.LightGray);
            e.Graphics.FillRectangle(headerBg, e.Bounds);

            if (e.Header != null && !string.IsNullOrEmpty(e.Header.Text)) // Ensure e.Header is not null
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Header.Text,
                    new Font("Segoe UI", 9, FontStyle.Bold),
                    e.Bounds,
                    Color.Black,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                );
            }
        }



        private async Task LoadNodeNames()
        {
            try
            {
                string url = $"{Endpoint}/v1/nodes";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                var nodes = doc.RootElement.GetProperty("items");
                listView1.Items.Clear();

                foreach (var node in nodes.EnumerateArray())
                {
                    string name = node.GetProperty("metadata").GetProperty("name").GetString() ?? string.Empty;

                    string ip = node
                        .GetProperty("status")
                        .GetProperty("addresses")
                        .EnumerateArray()
                        .FirstOrDefault(a => a.GetProperty("type").GetString() == "InternalIP")
                        .GetProperty("address")
                        .GetString() ?? "Unknown";

                    string kubelet = node.GetProperty("status")
                        .GetProperty("nodeInfo")
                        .GetProperty("kubeletVersion")
                        .GetString() ?? string.Empty;

                    var item = new ListViewItem(new[] { name, ip, kubelet });
                    listView1.Items.Add(item);
                }


                // Remove last phantom column space
                listView1.Columns[listView1.Columns.Count - 1].Width = -2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading nodes: " + ex.Message);
            }
        }

        private async Task LoadNamespacesToListBox()
        {
            try
            {
                string url = $"{Endpoint}/v1/namespaces";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                var items = doc.RootElement.GetProperty("items");
                listBox1.Items.Clear();

                foreach (var ns in items.EnumerateArray())
                {
                    string? name = ns.GetProperty("metadata").GetProperty("name").GetString();
                    if (name != null)
                    {
                        listBox1.Items.Add($"Name: {name}");
                    }
                    string? phase = ns.GetProperty("status").GetProperty("phase").GetString() ?? "Unknown";
                    string? creationTimestamp = ns.GetProperty("metadata").GetProperty("creationTimestamp").GetString();
                    if (!string.IsNullOrEmpty(creationTimestamp))
                    {
                        string created = DateTime
                            .Parse(creationTimestamp)
                            .ToString("dd/MM/yyyy HH:mm");

                        listBox1.Items.Add($"Created at: {created}");
                    }
                    else
                    {
                        listBox1.Items.Add("Created at: Unknown");
                    }

                    listBox1.Items.Add($"Status: {phase}");
                    listBox1.Items.Add("________________________________________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading namespaces: " + ex.Message);
            }
        }


        private async void button1_Click_1(object sender, EventArgs e)
        {
            string namespaceName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                MessageBox.Show("Please enter a valid namespace name.");
                return;
            }

            try
            {
                var payload = new
                {
                    metadata = new { name = namespaceName }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync($"{Endpoint}/v1/namespaces", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Namespace created successfully!");
                    await LoadNamespacesToListBox();
                    await LoadNamespacesToComboBox();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show($"Error while creating namespace: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a namespace to delete.");
                return;
            }

            string? selected = listBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected) || !selected.StartsWith("Name:"))
            {
                MessageBox.Show("Please select the 'Name:' line of the namespace to delete.");
                return;
            }

            string namespaceName = selected.Substring("Name:".Length).Trim();

            try
            {
                var response = await httpClient.DeleteAsync($"{Endpoint}/v1/namespaces/{namespaceName}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Namespace deleted successfully!");
                    await LoadNamespacesToListBox();
                    await LoadNamespacesToComboBox();
                }
                else
                {
                    MessageBox.Show($"Error while deleting namespace: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void listView1_ColumnWidthChanging(object? sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await LoadNamespacesToListBox();
            await LoadNamespacesToComboBox();
        }


        private async Task LoadNamespacesToComboBox()
        {
            try
            {
                string url = $"{Endpoint}/v1/namespaces";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                comboBox1.Items.Clear();

                foreach (var ns in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    string? name = ns.GetProperty("metadata").GetProperty("name").GetString();
                    if (!string.IsNullOrEmpty(name)) // Ensure name is not null or empty
                    {
                        comboBox1.Items.Add(name);
                    }
                }

                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading namespaces: " + ex.Message);
            }
        }



        private async Task LoadAllPods()
{
            try
            {
                string url = $"{Endpoint}/v1/pods";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                listBox2.Items.Clear();

                foreach (var pod in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    string? name = pod.GetProperty("metadata").GetProperty("name").GetString();
                    string? ns = pod.GetProperty("metadata").GetProperty("namespace").GetString();
                    string? status = pod.GetProperty("status").GetProperty("phase").GetString();
                    string? creationTimestamp = pod.GetProperty("metadata").GetProperty("creationTimestamp").GetString();

                    string created = !string.IsNullOrEmpty(creationTimestamp)
                        ? DateTime.Parse(creationTimestamp).ToString("dd/MM/yyyy HH:mm")
                        : "Unknown";

                    var containers = pod.GetProperty("spec").GetProperty("containers");
                    string? image = containers[0].GetProperty("image").GetString();
                    if (string.IsNullOrEmpty(image))
                    {
                        image = "Unknown";
                    }

                    string ports = "";

                    if (containers[0].TryGetProperty("ports", out var portList))
                    {
                        ports = string.Join(", ",
                            portList.EnumerateArray().Select(p => p.GetProperty("containerPort").GetInt32().ToString()));
                    }

                    listBox2.Items.Add($"Name: {name}");
                    listBox2.Items.Add($"Namespace: {ns}");
                    listBox2.Items.Add($"Status: {status}");
                    listBox2.Items.Add($"Image: {image}");
                    if (!string.IsNullOrEmpty(ports))
                        listBox2.Items.Add($"Ports: {ports}");
                    listBox2.Items.Add($"Created at: {created}");
                    listBox2.Items.Add("________________________________________________________________________________________________________");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pods: " + ex.Message);
            }
        }


        private async void button6_Click(object sender, EventArgs e)
        {
            string podName = textBox2.Text.Trim();
            string? ns = comboBox1.SelectedItem?.ToString();
            string image = textBoxImage.Text.Trim();
            string portText = textBoxPort.Text.Trim();

            if (string.IsNullOrWhiteSpace(podName) || string.IsNullOrWhiteSpace(ns) || string.IsNullOrWhiteSpace(image))
            {
                MessageBox.Show("Please fill in pod name, image, and select a namespace.");
                return;
            }

            try
            {
                var container = new Dictionary<string, object>
                {
                    { "name", podName.ToLower() },
                    { "image", image }
                };

                        if (int.TryParse(portText, out int port))
                        {
                            container["ports"] = new[]
                            {
                        new { containerPort = port }
                    };
                        }

                var payload = new
                {
                    apiVersion = "v1",
                    kind = "Pod",
                    metadata = new { name = podName },
                    spec = new
                    {
                        containers = new[] { container }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync($"{Endpoint}/v1/namespaces/{ns}/pods", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pod created successfully.");
                    await LoadAllPods();
                    textBox2.Clear();
                    textBoxImage.Clear();
                    textBoxPort.Clear();
                }
                else
                {
                    MessageBox.Show($"Error creating pod: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private async void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null ||
                string.IsNullOrEmpty(listBox2.SelectedItem.ToString()) ||
                !listBox2.SelectedItem.ToString()!.StartsWith("Name:"))
            {
                MessageBox.Show("Select the 'Name:' line of a pod to delete.");
                return;
            }

            string podName = listBox2.SelectedItem.ToString()!.Substring("Name:".Length).Trim();
            int selectedIndex = listBox2.SelectedIndex;

            // Verifica se a linha seguinte existe e contém o namespace
            if (selectedIndex + 1 >= listBox2.Items.Count)
            {
                MessageBox.Show("Unable to read namespace line. Please ensure the full pod info is listed.");
                return;
            }

            string? nsLine = listBox2.Items[selectedIndex + 1]?.ToString();
            if (nsLine == null || !nsLine.StartsWith("Namespace:"))
            {
                MessageBox.Show("Invalid format. Namespace line not found after pod name.");
                return;
            }

            string ns = nsLine.Substring("Namespace:".Length).Trim();

            try
            {
                var response = await httpClient.DeleteAsync($"{Endpoint}/v1/namespaces/{ns}/pods/{podName}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pod deleted successfully.");
                    await LoadAllPods();
                }
                else
                {
                    MessageBox.Show($"Error deleting pod: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private async void button4_Click_1(object sender, EventArgs e)
        {
            await LoadAllPods();
            await LoadNamespacesToComboBox();
        }
    }
}
