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

namespace K8sDashboard
{
    public class Form1 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private FlowLayoutPanel panelContainer;
        private System.Windows.Forms.Timer refreshTimer;

        private Dictionary<string, List<(string Name, string Phase)>> podsPerNode = new();

        private Dictionary<string, (int CpuMilli, int MemMi)> capacityPerNode = new();

        public Form1()
        {
            this.Text = "Kubernetes";
            this.Width = 1350;
            this.Height = 800;
            AutoScroll = true;

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
                using var f2 = new Form2();
                f2.ShowDialog();
            };
            Controls.Add(btnConfig);

            // --- 2) FlowLayoutPanel abaixo do botão ---
            panelContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, btnConfig.Bottom + 10, 0, 0),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            Controls.Add(panelContainer);

            refreshTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            refreshTimer.Tick += async (s, e) => await LoadKubernetesData();
            refreshTimer.Start();

            Load += async (s, e) => await LoadKubernetesData();
        }

        private async Task LoadKubernetesData()
        {
            try
            {
                string nodesUrl = "http://192.168.48.137:8001/api/v1/nodes";
                string nodeMetricsUrl = "http://192.168.48.137:8001/apis/metrics.k8s.io/v1beta1/nodes";
                string podsUrl = "http://192.168.48.137:8001/api/v1/pods";

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
            panelContainer.Controls.Clear();

            foreach (var metricNode in metricsDoc.RootElement.GetProperty("items").EnumerateArray())
            {
                string? nodeName = metricNode.GetProperty("metadata").GetProperty("name").GetString();
                var usage = metricNode.GetProperty("usage");



                string? cpuU = usage.GetProperty("cpu").GetString();
                if (cpuU == null)
                {
                    throw new InvalidOperationException("O valor de 'cpu' não pode ser nulo.");
                }

                if (cpuU == null)
                {
                    throw new InvalidOperationException("O valor de 'cpu' não pode ser nulo.");
                }
                long cpuUsageNano = 0;
                if (cpuU.EndsWith("n"))
                {
                    cpuUsageNano = long.Parse(cpuU[..^1], CultureInfo.InvariantCulture);
                }
                else if (cpuU.EndsWith("u"))
                {
                    cpuUsageNano = long.Parse(cpuU[..^1], CultureInfo.InvariantCulture) * 1_000L;
                }
                else if (cpuU.EndsWith("m"))
                {
                    cpuUsageNano = long.Parse(cpuU[..^1], CultureInfo.InvariantCulture) * 1_000_000L;
                }
                else
                {
                    cpuUsageNano = (long)(double.Parse(cpuU, CultureInfo.InvariantCulture) * 1_000_000_000L);
                }


                string memU = usage.GetProperty("memory").GetString() ?? string.Empty;
                int memUsageMi = 0;
                if (memU.EndsWith("Ki"))
                {
                    memUsageMi = (int)(long.Parse(memU[..^2], CultureInfo.InvariantCulture) / 1024);
                }
                else if (memU.EndsWith("Mi"))
                {
                    memUsageMi = int.Parse(memU[..^2], CultureInfo.InvariantCulture);
                }
                else if (memU.EndsWith("Gi"))
                {
                    memUsageMi = int.Parse(memU[..^2], CultureInfo.InvariantCulture) * 1024;
                }
                else
                {
                    memUsageMi = (int)(long.Parse(memU, CultureInfo.InvariantCulture) / (1024 * 1024));
                }

                if (!string.IsNullOrEmpty(nodeName) && capacityPerNode.TryGetValue(nodeName, out var cap))
                {
                    long cpuCapNano = cap.CpuMilli * 1_000_000L;
                    int memCapMi = cap.MemMi;

                    int cpuPct = cpuCapNano > 0
                        ? (int)Math.Round((double)cpuUsageNano / cpuCapNano * 100)
                        : 0;
                    int memPct = memCapMi > 0
                        ? (int)Math.Round((double)memUsageMi / memCapMi * 100)
                        : 0;

                    var pods = podsPerNode.ContainsKey(nodeName) ? podsPerNode[nodeName] : new List<(string, string)>();
                    int runningCount = pods.Count(p => p.Item2 == "Running");
                    int succeededCount = pods.Count(p => p.Item2 == "Succeeded");

                    var nodeElem = nodesDoc.RootElement.GetProperty("items")
                        .EnumerateArray()
                        .First(n => n.GetProperty("metadata").GetProperty("name").GetString() == nodeName);

                    var conds = nodeElem.GetProperty("status").GetProperty("conditions").EnumerateArray();
                    var readyCond = conds.First(c => c.GetProperty("type").GetString() == "Ready");
                    string status = readyCond.GetProperty("status").GetString() == "True" ? "Ready" : "NotReady";
                    string? internalIP = nodeElem.GetProperty("status").GetProperty("addresses")
                         .EnumerateArray()
                         .FirstOrDefault(a => a.GetProperty("type").GetString() == "InternalIP")
                         .GetProperty("address").GetString();

                    if (internalIP == null)
                    {
                        throw new InvalidOperationException("O endereço IP interno não pode ser nulo.");
                    }

                    string? kubeletVer = nodeElem.GetProperty("status").GetProperty("nodeInfo").GetProperty("kubeletVersion").GetString();
                    if (kubeletVer == null)
                    {
                        kubeletVer = "Desconhecido";
                    }
                    string? creationTs = nodeElem.GetProperty("metadata").GetProperty("creationTimestamp").GetString();
                    if (creationTs == null)
                    {
                        creationTs = "Desconhecido";
                    }

                    var panel = CreateNodePanel(
                        nodeName,
                        cpuPct, memPct,
                        runningCount, succeededCount,
                        status, string.Empty,
                        internalIP, kubeletVer,
                        creationTs
                    );

                    panelContainer.Controls.Add(panel);
                }
                else
                {

                    throw new InvalidOperationException("O nome do node é nulo ou não encontrado no dicionário.");
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
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = System.Drawing.Color.WhiteSmoke
            };


            var labelTitle = new Label
            {
                Text = $"NODE: {nodeName}",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Left = 10,
                Top = 10,
                Width = 800
            };
            panel.Controls.Add(labelTitle);


            var infoGroup = new GroupBox
            {
                Text = "Info do Node",
                Left = 10,
                Top = 50,
                Width = 250,
                Height = 180,
                Font = new System.Drawing.Font("Segoe UI", 10)
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

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Form1
            // 
            BackColor = SystemColors.Control;
            ClientSize = new Size(282, 253);
            Name = "Form1";
            ResumeLayout(false);

        }
    }
}
