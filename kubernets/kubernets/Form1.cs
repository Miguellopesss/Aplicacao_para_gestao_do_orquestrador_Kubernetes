using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.Linq;
using kubernets;
using System.Net;
using Windows.Services.Maps;

namespace K8sDashboard
{
    public class Form1 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private FlowLayoutPanel panelContainer;
        private System.Windows.Forms.Timer refreshTimer;

        private Dictionary<string, List<(string Name, string Phase)>> podsPerNode = new();

        private Dictionary<string, (int CpuMilli, int MemMi)> capacityPerNode = new();
        private string Endpoint = string.Empty;
        private string Token = string.Empty;
        private Dictionary<string, Panel> nodePanels = new();

        public Form1(string ipAddress, string token)
        {

            this.Text = $"Dashboard - IP: {ipAddress}";
            this.Width = 1350;
            this.Height = 800;
            AutoScroll = true;
            Endpoint = $"https://{ipAddress}:6443/api";
            Token = token;
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- 1) Botão Configurações no topo ---
            var btnConfig = new Button
            {
                Text = "Configurações",
                AutoSize = true,
                Left = 10,
                Top = 10
            };
            btnConfig.Click += (s, e) =>
            {
                this.Hide(); // Esconde o Form1

                using var f2 = new Form2(ipAddress,token);
                f2.ShowDialog(); // Abre Form2 como modal

                this.Show(); // Reexibe Form1 depois que Form2 for fechado
            };
            Controls.Add(btnConfig);

            // --- 2) FlowLayoutPanel abaixo do botão ---
            panelContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, btnConfig.Bottom + 10, 0, 0),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackgroundImage = kubernets.Properties.Resources.Login,
                BackgroundImageLayout = ImageLayout.Stretch
            };


            panelContainer.ControlAdded += (s, e) =>
            {
                if (e.Control != null && panelContainer != null)
                {
                    e.Control.Margin = new Padding((panelContainer.ClientSize.Width - e.Control.Width) / 2, 10, 0, 10);
                }
            };

            panelContainer.Resize += (s, e) =>
            {
                if (panelContainer != null)
                {
                    foreach (Control ctrl in panelContainer.Controls)
                    {
                        if (ctrl != null)
                        {
                            ctrl.Margin = new Padding((panelContainer.ClientSize.Width - ctrl.Width) / 2, 10, 0, 10);
                        }
                    }
                }
            };
           
            Controls.Add(panelContainer);

            refreshTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            refreshTimer.Tick += async (s, e) => await LoadKubernetesData();
            refreshTimer.Start();

            Load += async (s, e) => await LoadKubernetesData();
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true
            };

            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

           
        }

        private async Task LoadKubernetesData()
        {
            try
            {
                string nodesUrl = $"{Endpoint}/v1/nodes";
                string nodeMetricsUrl = $"{Endpoint}s/metrics.k8s.io/v1beta1/nodes";
                string podsUrl = $"{Endpoint}/v1/pods";

                // Requisições HTTP com o httpClient configurado
                var nodesJson = await httpClient.GetStringAsync(nodesUrl);
                var metricsJson = await httpClient.GetStringAsync(nodeMetricsUrl);
                var podListJson = await httpClient.GetStringAsync(podsUrl);

                var nodesDoc = JsonDocument.Parse(nodesJson);
                var metricsDoc = JsonDocument.Parse(metricsJson);
                var podListDoc = JsonDocument.Parse(podListJson);

                IndexCapacityPerNode(nodesDoc);
                IndexPodsPerNode(podListDoc);
                CreatePanelsPerNode(metricsDoc, nodesDoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void IndexCapacityPerNode(JsonDocument nodesDoc)
        {
            capacityPerNode.Clear();
            foreach (var node in nodesDoc.RootElement.GetProperty("items").EnumerateArray())
            {
                string nodeName = node.GetProperty("metadata").GetProperty("name").GetString() ?? string.Empty;
                var cap = node.GetProperty("status").GetProperty("capacity");
                string cpuStr = cap.GetProperty("cpu").GetString() ?? "0"; // ex "4"
                string memStr = cap.GetProperty("memory").GetString() ?? string.Empty; // ex "8194300Ki"

                int cpuMilli = (int)(double.Parse(cpuStr) * 1000);
                int memMi = int.Parse(memStr.Replace("Ki", "")) / 1024;

                capacityPerNode[nodeName] = (cpuMilli, memMi);
            }
        }

        private void IndexPodsPerNode(JsonDocument podList)
        {
            podsPerNode.Clear();
            foreach (var pod in podList.RootElement.GetProperty("items").EnumerateArray())
            {
                if (pod.GetProperty("spec").TryGetProperty("nodeName", out var nodeEl))
                {
                    string? nodeName = nodeEl.GetString();
                    if (nodeName != null)
                    {
                        string phase = pod.GetProperty("status").GetProperty("phase").GetString() ?? string.Empty;
                        if (!podsPerNode.ContainsKey(nodeName))
                            podsPerNode[nodeName] = new List<(string, string)>();
                        podsPerNode[nodeName].Add((pod.GetProperty("metadata").GetProperty("name").GetString() ?? string.Empty, phase));
                    }
                }
            }
        }

        private void CreatePanelsPerNode(JsonDocument metricsDoc, JsonDocument nodesDoc)
        {
            foreach (var metricNode in metricsDoc.RootElement.GetProperty("items").EnumerateArray())
            {
                string? nodeName = metricNode.GetProperty("metadata").GetProperty("name").GetString();
                if (string.IsNullOrEmpty(nodeName)) continue;

                var usage = metricNode.GetProperty("usage");
                string? cpuU = usage.GetProperty("cpu").GetString();
                if (string.IsNullOrEmpty(cpuU)) continue;

                long cpuUsageNano = cpuU switch
                {
                    string s when s.EndsWith("n") => long.Parse(s[..^1], CultureInfo.InvariantCulture),
                    string s when s.EndsWith("u") => long.Parse(s[..^1], CultureInfo.InvariantCulture) * 1_000L,
                    string s when s.EndsWith("m") => long.Parse(s[..^1], CultureInfo.InvariantCulture) * 1_000_000L,
                    _ => (long)(double.Parse(cpuU, CultureInfo.InvariantCulture) * 1_000_000_000L)
                };

                string memU = usage.GetProperty("memory").GetString() ?? string.Empty;
                int memUsageMi = memU switch
                {
                    string s when s.EndsWith("Ki") => (int)(long.Parse(s[..^2], CultureInfo.InvariantCulture) / 1024),
                    string s when s.EndsWith("Mi") => int.Parse(s[..^2], CultureInfo.InvariantCulture),
                    string s when s.EndsWith("Gi") => int.Parse(s[..^2], CultureInfo.InvariantCulture) * 1024,
                    _ => (int)(long.Parse(memU, CultureInfo.InvariantCulture) / (1024 * 1024))
                };

                if (!capacityPerNode.TryGetValue(nodeName, out var cap)) continue;

                long cpuCapNano = cap.CpuMilli * 1_000_000L;
                int memCapMi = cap.MemMi;

                int cpuPct = cpuCapNano > 0 ? (int)Math.Round((double)cpuUsageNano / cpuCapNano * 100) : 0;
                int memPct = memCapMi > 0 ? (int)Math.Round((double)memUsageMi / memCapMi * 100) : 0;

                var pods = podsPerNode.ContainsKey(nodeName) ? podsPerNode[nodeName] : new List<(string, string)>();
                int runningCount = pods.Count(p => p.Item2 == "Running");
                int succeededCount = pods.Count(p => p.Item2 == "Succeeded");

                if (!nodePanels.ContainsKey(nodeName))
                {
                    // Encontra o node no JSON dos nodes
                    var nodeElem = nodesDoc.RootElement.GetProperty("items")
                        .EnumerateArray()
                        .FirstOrDefault(n => n.GetProperty("metadata").GetProperty("name").GetString() == nodeName);

                    if (nodeElem.ValueKind == JsonValueKind.Undefined) continue;

                    var conds = nodeElem.GetProperty("status").GetProperty("conditions").EnumerateArray();
                    var readyCond = conds.First(c => c.GetProperty("type").GetString() == "Ready");
                    string status = readyCond.GetProperty("status").GetString() == "True" ? "Ready" : "NotReady";
                    string? internalIP = nodeElem.GetProperty("status").GetProperty("addresses")
                         .EnumerateArray()
                         .FirstOrDefault(a => a.GetProperty("type").GetString() == "InternalIP")
                         .GetProperty("address").GetString() ?? "N/A";

                    string? kubeletVer = nodeElem.GetProperty("status").GetProperty("nodeInfo").GetProperty("kubeletVersion").GetString() ?? "Desconhecido";
                    string? creationTs = nodeElem.GetProperty("metadata").GetProperty("creationTimestamp").GetString() ?? "Desconhecido";

                    var panel = CreateNodePanel(nodeName, cpuPct, memPct, runningCount, succeededCount,
                                                status, "", internalIP, kubeletVer, creationTs);

                    nodePanels[nodeName] = panel;
                    panelContainer.Controls.Add(panel);
                }
                else
                {
                    var panel = nodePanels[nodeName];

                    // Atualizar gráficos
                    var charts = panel.Controls.OfType<Chart>().ToList();
                    if (charts.Count >= 3)
                    {
                        SetupPercentChart(charts[0], "CPU (%)", cpuPct);
                        SetupPercentChart(charts[1], "Memória (%)", memPct);

                        var pieSeries = charts[2].Series[0];
                        pieSeries.Points.Clear();
                        pieSeries.Points.AddXY("Running", runningCount);
                        if (succeededCount > 0)
                            pieSeries.Points.AddXY("Succeeded", succeededCount);
                    }
                }
            }
        }



        private Panel CreateNodePanel(
            string nodeName,
            int cpuPct, int memPct,
            int runningCount, int succeededCount,
            string status, string heartbeat,
            string internalIP, string kubeletVersion,
            string creationTs)
        {
            var panel = new Panel
            {
                Width = 1300,
                Height = 330,
                Margin = new Padding(10),
                BackColor = System.Drawing.Color.Transparent
            };


            var labelTitle = new Label
            {
                Text = $"NODE: {nodeName}",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Left = 10,
                Top = 10,
                Width = 800,
                ForeColor = System.Drawing.Color.White
            };
            panel.Controls.Add(labelTitle);


            var infoGroup = new GroupBox
            {
                Text = "Info do Node",
                Left = 10,
                Top = 50,
                Width = 250,
                Height = 180,
                Font = new System.Drawing.Font("Segoe UI", 10),
                ForeColor = System.Drawing.Color.White
            };
            var lbInfo = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };


            lbInfo.Items.Add($"IP Interno: {internalIP}");


            var cap = capacityPerNode[nodeName];
            double cpuCores = cap.CpuMilli / 1000.0;
            lbInfo.Items.Add($"CPU Total: {cpuCores} cores");


            double memGb = cap.MemMi / 1024.0;
            lbInfo.Items.Add($"Memória Total: {memGb:F2} GB");

            lbInfo.Items.Add($"Kubelet: {kubeletVersion}");
            lbInfo.Items.Add($"Criado em: {creationTs}");

            infoGroup.Controls.Add(lbInfo);
            panel.Controls.Add(infoGroup);


            var cpuChart = new Chart { Left = 270, Top = 50, Width = 300, Height = 200 };
            SetupPercentChart(cpuChart, "CPU (%)", cpuPct);
            panel.Controls.Add(cpuChart);


            var memChart = new Chart { Left = 590, Top = 50, Width = 300, Height = 200 };
            SetupPercentChart(memChart, "Memória (%)", memPct);
            panel.Controls.Add(memChart);


            var pieChart = new Chart { Left = 910, Top = 50, Width = 300, Height = 200 };
            pieChart.ChartAreas.Add(new ChartArea());
            pieChart.Titles.Add(new Title
            {
                Text = "Distribuição de Pods",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Docking = Docking.Top,
                Alignment = System.Drawing.ContentAlignment.MiddleCenter
            });
            pieChart.Legends.Add(new Legend
            {
                LegendStyle = LegendStyle.Table,
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new System.Drawing.Font("Segoe UI", 10)
            });
            var pieSeries = new Series
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = false,
                LegendText = "#VALX: #VALY"
            };
            pieSeries["PieLabelStyle"] = "Disabled";
            pieSeries.Points.AddXY("Running", runningCount);
            if (succeededCount > 0)
                pieSeries.Points.AddXY("Succeeded", succeededCount);
            pieChart.Series.Add(pieSeries);
            panel.Controls.Add(pieChart);

            return panel;
        }


        private void SetupPercentChart(Chart chart, string title, int pct)
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Titles.Clear();

            var area = new ChartArea("A")
            {
                BackColor = System.Drawing.Color.FromArgb(250, 250, 250),
                AxisY = { Minimum = 0, Maximum = 100, MajorGrid = { LineDashStyle = ChartDashStyle.Dot } },
                AxisX = { MajorGrid = { Enabled = false } }
            };
            chart.ChartAreas.Add(area);

            var series = new Series
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            series.Points.AddXY(title, pct);
            chart.Series.Add(series);

            chart.Titles.Add(new Title
            {
                Text = title,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            });

            chart.BackColor = System.Drawing.Color.White;
        }
    }
}
