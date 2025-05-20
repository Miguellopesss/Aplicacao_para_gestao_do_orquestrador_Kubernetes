using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace kubernets
{
    public partial class Form2 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string Endpoint = string.Empty;
        private string Token = string.Empty;
        private List<DeploymentInfo> deployments = new();



        public Form2(string ipAddress, string token)
        {
            InitializeComponent();
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;

            this.Text = $"Kubernetes - IP: {ipAddress}";
            Endpoint = $"https://{ipAddress}:6443/api";
            Token = token;

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
                await LoadAllDeployments();
                await LoadAllIngresses();
                await LoadDeploymentsToComboBox();
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

            if (e.Header != null && !string.IsNullOrEmpty(e.Header.Text))
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
                comboBox2.Items.Clear();

                foreach (var ns in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    string? name = ns.GetProperty("metadata").GetProperty("name").GetString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        comboBox1.Items.Add(name);
                        comboBox2.Items.Add(name);
                    }
                }

                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
                if (comboBox2.Items.Count > 0)
                    comboBox2.SelectedIndex = 0;
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

        private async Task LoadAllDeployments()
        {
            try
            {
                string url = $"{Endpoint}s/apps/v1/deployments";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                listBox3.Items.Clear();

                foreach (var deployment in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    string? name = deployment.GetProperty("metadata").GetProperty("name").GetString();
                    string? namespaceName = deployment.GetProperty("metadata").GetProperty("namespace").GetString();
                    string? status = deployment.GetProperty("status")
                                                .GetProperty("conditions")
                                                .EnumerateArray()
                                                .FirstOrDefault(c => c.GetProperty("type").GetString() == "Available")
                                                .TryGetProperty("status", out var statusElement) ? statusElement.GetString() : "Unknown";

                    string? replicas = deployment.GetProperty("spec").GetProperty("replicas").GetInt32().ToString();
                    string? creationTimestamp = deployment.GetProperty("metadata").GetProperty("creationTimestamp").GetString();

                    string created = !string.IsNullOrEmpty(creationTimestamp)
                        ? DateTime.Parse(creationTimestamp).ToString("dd/MM/yyyy HH:mm")
                        : "Unknown";

                    var containers = deployment.GetProperty("spec").GetProperty("template").GetProperty("spec").GetProperty("containers");
                    string? image = containers[0].GetProperty("image").GetString();
                    if (string.IsNullOrEmpty(image))
                    {
                        image = "Unknown";
                    }

                    listBox3.Items.Add($"Name: {name}");
                    listBox3.Items.Add($"Namespace: {namespaceName}");
                    listBox3.Items.Add($"Status: {status}");
                    listBox3.Items.Add($"Replicas: {replicas}");
                    listBox3.Items.Add($"Image: {image}");
                    listBox3.Items.Add($"Created at: {created}");
                    listBox3.Items.Add("________________________________________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading deployments: " + ex.Message);
            }
        }

        private async void button9_Click_1(object sender, EventArgs e)
        {
            string deploymentName = textBox5.Text.Trim();
            string? namespaceName = comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(namespaceName))
            {
                MessageBox.Show("Please select a valid namespace.");
                return;
            }

            int replicas = (int)numericUpDown1.Value;
            string containerImage = textBox4.Text.Trim();
            string portText = textBox3.Text.Trim();
            int containerPort = string.IsNullOrEmpty(portText) ? 8080 : int.Parse(portText);

            if (string.IsNullOrWhiteSpace(deploymentName) || string.IsNullOrWhiteSpace(namespaceName) || string.IsNullOrWhiteSpace(containerImage))
            {
                MessageBox.Show("Please fill in the deployment name, image, and select a namespace.");
                return;
            }

            try
            {
                var payload = new
                {
                    apiVersion = "apps/v1",
                    kind = "Deployment",
                    metadata = new
                    {
                        name = deploymentName
                    },
                    spec = new
                    {
                        replicas = replicas,
                        selector = new
                        {
                            matchLabels = new
                            {
                                app = deploymentName
                            }
                        },
                        template = new
                        {
                            metadata = new
                            {
                                labels = new
                                {
                                    app = deploymentName
                                }
                            },
                            spec = new
                            {
                                containers = new[]
                                {
                            new
                            {
                                name = deploymentName,
                                image = containerImage,
                                ports = new[]
                                {
                                    new
                                    {
                                        containerPort = containerPort
                                    }
                                }
                            }
                        }
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync($"{Endpoint}s/apps/v1/namespaces/{namespaceName}/deployments", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Deployment created successfully!");
                    await LoadAllDeployments();
                    await LoadDeploymentsToComboBox();
                }
                else
                {
                    MessageBox.Show($"Error creating the Deployment: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private async void button7_Click(object sender, EventArgs e)
        {
            await LoadAllDeployments();
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem == null ||
                string.IsNullOrEmpty(listBox3.SelectedItem.ToString()) ||
                !listBox3.SelectedItem.ToString()!.StartsWith("Name:"))
            {
                MessageBox.Show("Please select the 'Name:' line of the Deployment to delete.");
                return;
            }

            string deploymentName = listBox3.SelectedItem.ToString()!.Substring("Name:".Length).Trim();
            int selectedIndex = listBox3.SelectedIndex;

            if (selectedIndex + 1 >= listBox3.Items.Count)
            {
                MessageBox.Show("Unable to read the namespace line. Please ensure the full deployment info is listed.");
                return;
            }

            string? nsLine = listBox3.Items[selectedIndex + 1]?.ToString();
            if (nsLine == null || !nsLine.StartsWith("Namespace:"))
            {
                MessageBox.Show("Invalid format. Namespace line not found after deployment name.");
                return;
            }

            string ns = nsLine.Substring("Namespace:".Length).Trim();

            try
            {
                var response = await httpClient.DeleteAsync($"{Endpoint}s/apps/v1/namespaces/{ns}/deployments/{deploymentName}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Deployment deleted successfully!");
                    await LoadAllDeployments();
                }
                else
                {
                    MessageBox.Show($"Error deleting the Deployment: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private async Task LoadAllIngresses()
        {
            try
            {
                string url = $"{Endpoint}s/networking.k8s.io/v1/ingresses";
                var json = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(json);

                listBox4.Items.Clear();

                foreach (var ingress in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    string? name = ingress.GetProperty("metadata").GetProperty("name").GetString();
                    string? namespaceName = ingress.GetProperty("metadata").GetProperty("namespace").GetString();
                    string? creationTimestamp = ingress.GetProperty("metadata").GetProperty("creationTimestamp").GetString();

                    string created = !string.IsNullOrEmpty(creationTimestamp)
                        ? DateTime.Parse(creationTimestamp).ToString("dd/MM/yyyy HH:mm")
                        : "Unknown";

                    listBox4.Items.Add($"Name: {name}");
                    listBox4.Items.Add($"Namespace: {namespaceName}");
                    listBox4.Items.Add($"Created at: {created}");

                    var rules = ingress.GetProperty("spec").GetProperty("rules");
                    foreach (var rule in rules.EnumerateArray())
                    {
                        string? host = rule.GetProperty("host").GetString();
                        listBox4.Items.Add($"Host: {host}");

                        var httpPaths = rule.GetProperty("http").GetProperty("paths");
                        foreach (var path in httpPaths.EnumerateArray())
                        {
                            string? serviceName = path.GetProperty("backend").GetProperty("service").GetProperty("name").GetString();
                            int? servicePort = path.GetProperty("backend").GetProperty("service").GetProperty("port").GetProperty("number").GetInt32();

                            listBox4.Items.Add($"Service: {serviceName}");
                            listBox4.Items.Add($"Port: {servicePort}");
                        }
                    }

                    listBox4.Items.Add("________________________________________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ingresses: " + ex.Message);
            }
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            await LoadAllIngresses();
        }


        private async Task LoadDeploymentsToComboBox()
        {
            try
            {
                string url = $"{Endpoint}s/apps/v1/deployments";
                var json = await httpClient.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                deployments = doc.RootElement
                    .GetProperty("items")
                    .EnumerateArray()
                    .Select(dep =>
                    {
                        var info = new DeploymentInfo
                        {
                            Name = dep.GetProperty("metadata")
                                              .GetProperty("name")
                                              .GetString() ?? "",
                            NamespaceName = dep.GetProperty("metadata")
                                              .GetProperty("namespace")
                                              .GetString() ?? ""
                        };

                        var containers = dep
                            .GetProperty("spec")
                            .GetProperty("template")
                            .GetProperty("spec")
                            .GetProperty("containers")
                            .EnumerateArray();

                        foreach (var c in containers)
                            if (c.TryGetProperty("ports", out var pList))
                                foreach (var p in pList.EnumerateArray())
                                    if (p.TryGetProperty("containerPort", out var pNum) &&
                                        pNum.TryGetInt32(out int port))
                                        info.Ports.Add(port);

                        return info;
                    })
                    .ToList();

                comboBox3.Items.Clear();
                foreach (var d in deployments)
                    comboBox3.Items.Add(d.Name);

                if (comboBox3.Items.Count > 0)
                    comboBox3.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading deployments: " + ex.Message);
            }
        }




        private void ComboBox3_SelectedIndexChanged(object? sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            comboBox4.SelectedIndex = -1;      
            comboBox4.Text = string.Empty;     

            int idx = comboBox3.SelectedIndex;
            if (idx < 0 || idx >= deployments.Count)
                return;

            var ports = deployments[idx].Ports;
            if (ports.Count > 0)
            {
                foreach (var port in ports)
                    comboBox4.Items.Add(port);

                comboBox4.SelectedIndex = 0;    
            }

        }



        public class DeploymentInfo
        {
            public string Name { get; set; } = "";
            public string NamespaceName { get; set; } = "";
            public List<int> Ports { get; set; } = new();
        }


        private string GetNamespaceForDeployment(string deploymentName)
        {
            var info = deployments.FirstOrDefault(d => d.Name == deploymentName);
            return info?.NamespaceName ?? "";
        }

        private void SendYamlToApi(string url, string yaml)
        {
            try
            {
                using var content = new StringContent(yaml, Encoding.UTF8, "application/yaml");
                var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    MessageBox.Show(
                        $"Failed to create resource:\n{response.StatusCode}\n{body}",
                        "API Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"API call failed:\n{ex.Message}",
                    "API Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }





        private void button12_Click(object sender, EventArgs e)
        {

            string deploymentName = comboBox3.Text.Trim();
            string domain = textBox8.Text.Trim();
            string containerPortStr = comboBox4.Text.Trim();
            string servicePortStr = textBox7.Text.Trim();


            if (string.IsNullOrWhiteSpace(deploymentName) ||
                string.IsNullOrWhiteSpace(domain) ||
                string.IsNullOrWhiteSpace(containerPortStr) ||
                string.IsNullOrWhiteSpace(servicePortStr))
            {
                MessageBox.Show("All fields are required.","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(containerPortStr, out int containerPort) ||
                !int.TryParse(servicePortStr, out int servicePort))
            {
                MessageBox.Show("Invalid port numbers.","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            string namespaceName = GetNamespaceForDeployment(deploymentName);
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                MessageBox.Show("Could not determine the namespace for the selected deployment.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            string serviceName = deploymentName.ToLower() + "-svc";
            string ingressName = deploymentName.ToLower() + "-ingress";

            string appLabel = deploymentName.EndsWith("-dp")
                ? deploymentName[..^3]
                : deploymentName;

            var serviceYaml = new StringBuilder();
            serviceYaml.AppendLine("apiVersion: v1");
            serviceYaml.AppendLine("kind: Service");
            serviceYaml.AppendLine("metadata:");
            serviceYaml.AppendLine($"  name:      {serviceName}");
            serviceYaml.AppendLine($"  namespace: {namespaceName}");
            serviceYaml.AppendLine("spec:");
            serviceYaml.AppendLine("  selector:");
            serviceYaml.AppendLine($"    app: {appLabel}");
            serviceYaml.AppendLine("  ports:");
            serviceYaml.AppendLine("  - port:       " + servicePort);
            serviceYaml.AppendLine("    targetPort: " + containerPort);

            var ingressYaml = new StringBuilder();
            ingressYaml.AppendLine("apiVersion: networking.k8s.io/v1");
            ingressYaml.AppendLine("kind: Ingress");
            ingressYaml.AppendLine("metadata:");
            ingressYaml.AppendLine($"  name:      {ingressName}");
            ingressYaml.AppendLine($"  namespace: {namespaceName}");
            ingressYaml.AppendLine("  annotations:");
            ingressYaml.AppendLine("    traefik.ingress.kubernetes.io/router.entrypoints: web");
            ingressYaml.AppendLine("spec:");
            ingressYaml.AppendLine("  rules:");
            ingressYaml.AppendLine("  - host: " + domain);
            ingressYaml.AppendLine("    http:");
            ingressYaml.AppendLine("      paths:");
            ingressYaml.AppendLine("      - path:     /");
            ingressYaml.AppendLine("        pathType: Prefix");
            ingressYaml.AppendLine("        backend:");
            ingressYaml.AppendLine("          service:");
            ingressYaml.AppendLine($"            name: {serviceName}");
            ingressYaml.AppendLine("            port:");
            ingressYaml.AppendLine($"              number: {servicePort}");

            MessageBox.Show(serviceYaml.ToString(), "YAML - Service");
            MessageBox.Show(ingressYaml.ToString(), "YAML - Ingress");


            var urlService = $"{Endpoint}/v1/namespaces/{namespaceName}/services";
            SendYamlToApi(urlService, serviceYaml.ToString());

            var urlIngress = $"{Endpoint}s/networking.k8s.io/v1/namespaces/{namespaceName}/ingresses";
            SendYamlToApi(urlIngress, ingressYaml.ToString());

            MessageBox.Show("Service and Ingress created successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        private async void button11_Click(object sender, EventArgs e)
        {

            if (listBox4.SelectedItem == null ||
                !listBox4.SelectedItem.ToString()!.StartsWith("Name:"))
            {
                MessageBox.Show(
                    "Please select the \"Name:\" line of the Ingress to delete.",
                    "Selection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            string ingressName = listBox4.SelectedItem.ToString()!
                                      .Substring("Name:".Length)
                                      .Trim();
            int idx = listBox4.SelectedIndex;

            if (idx + 1 >= listBox4.Items.Count ||
                !listBox4.Items[idx + 1].ToString()!.StartsWith("Namespace:"))
            {
                MessageBox.Show(
                    "Unable to read the \"Namespace:\" line following the Name: entry.",
                    "Parse Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            string namespaceName = listBox4.Items[idx + 1].ToString()!
                                        .Substring("Namespace:".Length)
                                        .Trim();

            
            string serviceName = "";
            for (int i = idx; i < listBox4.Items.Count; i++)
            {
                var line = listBox4.Items[i].ToString()!;
                if (line.StartsWith("Service:"))
                {
                    serviceName = line.Substring("Service:".Length).Trim();
                    break;
                }
            }
            if (string.IsNullOrEmpty(serviceName))
            {
                MessageBox.Show(
                    "No \"Service:\" entry found for the selected Ingress.",
                    "Parse Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            var confirm = MessageBox.Show(
                $"This will delete the Ingress '{ingressName}' and its Service '{serviceName}' in namespace '{namespaceName}'.\n\nProceed?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var urlIngress = $"{Endpoint}s/networking.k8s.io/v1/namespaces/{namespaceName}/ingresses/{ingressName}";
                var resIng = await httpClient.DeleteAsync(urlIngress);
                if (!resIng.IsSuccessStatusCode)
                {
                    var body = await resIng.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Failed to delete Ingress:\n{resIng.StatusCode}\n{body}",
                        "API Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                var urlSvc = $"{Endpoint}/v1/namespaces/{namespaceName}/services/{serviceName}";
                var resSvc = await httpClient.DeleteAsync(urlSvc);
                if (!resSvc.IsSuccessStatusCode)
                {
                    var body = await resSvc.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Ingress deleted, but failed to delete Service:\n{resSvc.StatusCode}\n{body}",
                        "Partial Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Ingress and Service were successfully deleted.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                await LoadAllIngresses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while deleting resources:\n{ex.Message}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
