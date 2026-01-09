using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FPSAnalizer
{
    public partial class MainForm : Form
    {
        private CapFrameXReader reader;
        private List<CapFrameXData> loadedData;
        private Button btnSelecFolder;
        private Button btnLoadFiles;
        private Label lblSelectedPath;
        private ProgressBar progressBar;
        private Label lblStatus;
        private DataGridView dgvFiles;
        private SplitContainer splitContainer;
        private TabControl tabControl;
        private TabPage tabSystem;
        private TabPage tabStats;
        private TabPage tabFrameTimes;
        private Label lblStats;
        private Label lblSystemInfo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFrameTimes;
        private Button btnReportPerformance;
        private BindingList<FramerateCapture> framerateCaptures;

        public MainForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Inicializar componentes
            reader = new CapFrameXReader();
            loadedData = new List<CapFrameXData>();
            framerateCaptures = new BindingList<FramerateCapture>();

            SetupControls();
        }

        private void SetupControls()
        {
            dgvFiles.Columns.Add("CPU", "CPU");
            dgvFiles.Columns.Add("GPU", "GPU");
            dgvFiles.Columns.Add("GameName", "Game");
            dgvFiles.Columns.Add("GameSettings", "Game Settings");
            dgvFiles.Columns.Add("AverageFps", "Avg FPS");
            dgvFiles.Columns.Add("TenPercentLowFramerate", "10% Low Avg FPS");
            dgvFiles.Columns.Add("OnePercentLowFramerate", "1% Low Avg FPS");
            dgvFiles.Columns.Add("ZeroPointOnePercentLowFramerate", "0.1% Low Avg FPS");

            dgvFiles.Columns["CPU"].FillWeight = 160;
            dgvFiles.Columns["GPU"].FillWeight = 180;
            dgvFiles.Columns["GameName"].FillWeight = 200;
            dgvFiles.Columns["GameSettings"].FillWeight = 160;
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            btnSelecFolder = new Button();
            btnLoadFiles = new Button();
            lblSelectedPath = new Label();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            dgvFiles = new DataGridView();
            splitContainer = new SplitContainer();
            btnReportPerformance = new Button();
            tabControl = new TabControl();
            tabSystem = new TabPage();
            lblSystemInfo = new Label();
            tabStats = new TabPage();
            lblStats = new Label();
            tabFrameTimes = new TabPage();
            chartFrameTimes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((ISupportInitialize)dgvFiles).BeginInit();
            ((ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            tabControl.SuspendLayout();
            tabSystem.SuspendLayout();
            tabStats.SuspendLayout();
            tabFrameTimes.SuspendLayout();
            ((ISupportInitialize)chartFrameTimes).BeginInit();
            SuspendLayout();
            // 
            // btnSelecFolder
            // 
            btnSelecFolder.Location = new Point(3, 3);
            btnSelecFolder.Name = "btnSelecFolder";
            btnSelecFolder.Size = new Size(120, 29);
            btnSelecFolder.TabIndex = 0;
            btnSelecFolder.Text = "Select Folder";
            btnSelecFolder.UseVisualStyleBackColor = true;
            btnSelecFolder.Click += BtnSelectFolder_Click;
            // 
            // btnLoadFiles
            // 
            btnLoadFiles.Location = new Point(3, 38);
            btnLoadFiles.Name = "btnLoadFiles";
            btnLoadFiles.Size = new Size(120, 29);
            btnLoadFiles.TabIndex = 1;
            btnLoadFiles.Text = "Load Files";
            btnLoadFiles.UseVisualStyleBackColor = true;
            btnLoadFiles.Click += BtnLoadFiles_Click;
            // 
            // lblSelectedPath
            // 
            lblSelectedPath.AutoSize = true;
            lblSelectedPath.Location = new Point(129, 10);
            lblSelectedPath.Name = "lblSelectedPath";
            lblSelectedPath.Size = new Size(0, 15);
            lblSelectedPath.TabIndex = 2;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(129, 38);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(80, 29);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 3;
            progressBar.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(215, 45);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 4;
            // 
            // dgvFiles
            // 
            dgvFiles.AllowUserToAddRows = false;
            dgvFiles.AllowUserToDeleteRows = false;
            dgvFiles.AllowUserToResizeRows = false;
            dgvFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFiles.Location = new Point(0, 73);
            dgvFiles.Name = "dgvFiles";
            dgvFiles.ReadOnly = true;
            dgvFiles.RowHeadersVisible = false;
            dgvFiles.RowHeadersWidth = 51;
            dgvFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFiles.Size = new Size(1424, 544);
            dgvFiles.TabIndex = 5;
            dgvFiles.SelectionChanged += DgvFiles_SelectionChanged;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(btnReportPerformance);
            splitContainer.Panel1.Controls.Add(dgvFiles);
            splitContainer.Panel1.Controls.Add(lblStatus);
            splitContainer.Panel1.Controls.Add(btnSelecFolder);
            splitContainer.Panel1.Controls.Add(progressBar);
            splitContainer.Panel1.Controls.Add(btnLoadFiles);
            splitContainer.Panel1.Controls.Add(lblSelectedPath);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(tabControl);
            splitContainer.Size = new Size(1424, 921);
            splitContainer.SplitterDistance = 617;
            splitContainer.TabIndex = 6;
            // 
            // btnReportPerformance
            // 
            btnReportPerformance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReportPerformance.Location = new Point(1284, 3);
            btnReportPerformance.Name = "btnReportPerformance";
            btnReportPerformance.Size = new Size(137, 29);
            btnReportPerformance.TabIndex = 6;
            btnReportPerformance.Text = "Report Performance";
            btnReportPerformance.UseVisualStyleBackColor = true;
            btnReportPerformance.Click += button2_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabSystem);
            tabControl.Controls.Add(tabStats);
            tabControl.Controls.Add(tabFrameTimes);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1424, 300);
            tabControl.TabIndex = 0;
            // 
            // tabSystem
            // 
            tabSystem.Controls.Add(lblSystemInfo);
            tabSystem.Location = new Point(4, 24);
            tabSystem.Name = "tabSystem";
            tabSystem.Padding = new Padding(3);
            tabSystem.Size = new Size(1416, 272);
            tabSystem.TabIndex = 0;
            tabSystem.Text = "System";
            tabSystem.UseVisualStyleBackColor = true;
            // 
            // lblSystemInfo
            // 
            lblSystemInfo.AutoSize = true;
            lblSystemInfo.Dock = DockStyle.Fill;
            lblSystemInfo.Location = new Point(3, 3);
            lblSystemInfo.Name = "lblSystemInfo";
            lblSystemInfo.Size = new Size(235, 15);
            lblSystemInfo.TabIndex = 1;
            lblSystemInfo.Text = "Informações do sistema serão exibidas aqui";
            // 
            // tabStats
            // 
            tabStats.Controls.Add(lblStats);
            tabStats.Location = new Point(4, 24);
            tabStats.Name = "tabStats";
            tabStats.Padding = new Padding(3);
            tabStats.Size = new Size(1416, 272);
            tabStats.TabIndex = 1;
            tabStats.Text = "Stats";
            tabStats.UseVisualStyleBackColor = true;
            // 
            // lblStats
            // 
            lblStats.AutoSize = true;
            lblStats.Dock = DockStyle.Fill;
            lblStats.Location = new Point(3, 3);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(240, 15);
            lblStats.TabIndex = 0;
            lblStats.Text = "Selecione um arquivo para ver as estatísticas";
            // 
            // tabFrameTimes
            // 
            tabFrameTimes.Controls.Add(chartFrameTimes);
            tabFrameTimes.Location = new Point(4, 24);
            tabFrameTimes.Name = "tabFrameTimes";
            tabFrameTimes.Padding = new Padding(3);
            tabFrameTimes.Size = new Size(1416, 272);
            tabFrameTimes.TabIndex = 2;
            tabFrameTimes.Text = "Frame Times";
            tabFrameTimes.UseVisualStyleBackColor = true;
            // 
            // chartFrameTimes
            // 
            chartArea1.Name = "ChartArea1";
            chartFrameTimes.ChartAreas.Add(chartArea1);
            chartFrameTimes.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chartFrameTimes.Legends.Add(legend1);
            chartFrameTimes.Location = new Point(3, 3);
            chartFrameTimes.Name = "chartFrameTimes";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = Color.RoyalBlue;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartFrameTimes.Series.Add(series1);
            chartFrameTimes.Size = new Size(1410, 266);
            chartFrameTimes.TabIndex = 0;
            chartFrameTimes.Click += chartFrameTimes_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(1424, 921);
            Controls.Add(splitContainer);
            Name = "MainForm";
            Text = "FPS Stats Analizer";
            ((ISupportInitialize)dgvFiles).EndInit();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabSystem.ResumeLayout(false);
            tabSystem.PerformLayout();
            tabStats.ResumeLayout(false);
            tabStats.PerformLayout();
            tabFrameTimes.ResumeLayout(false);
            ((ISupportInitialize)chartFrameTimes).EndInit();
            ResumeLayout(false);

        }

        private async void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Selecione a pasta com arquivos CapFrameX";
                folderDialog.ShowNewFolderButton = false;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    lblSelectedPath.Text = folderDialog.SelectedPath;
                    btnLoadFiles.Enabled = true;
                    lblStatus.Text = "Pasta selecionada. Clique em 'Carregar Arquivos' para iniciar.";
                }
            }
        }

        private async void BtnLoadFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblSelectedPath.Text) ||
                lblSelectedPath.Text == "Nenhuma pasta selecionada")
            {
                MessageBox.Show("Por favor, selecione uma pasta primeiro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpar dados anteriores
            loadedData.Clear();
            framerateCaptures.Clear();
            dgvFiles.Rows.Clear();

            // Configurar interface para carregamento
            btnLoadFiles.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Procurando e lendo arquivos...";

            try
            {
                var progress = new Progress<string>(message =>
                {
                    lblStatus.Text = message;
                });

                // Carregar arquivos de forma assíncrona
                //loadedData = await reader.ReadDirectoryAsync("sdasa", progress);

                var loadedData = new List<CapFrameXData>();

                if (loadedData.Count == 0)
                {
                    lblStatus.Text = "Nenhum arquivo CapFrameX encontrado.";
                    MessageBox.Show("Nenhum arquivo CapFrameX (.json) encontrado na pasta selecionada.",
                        "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Preparar dados para exibição
                    List<FramerateCapture> captures = reader.GetFileInfoDisplays(loadedData);
                    framerateCaptures = new BindingList<FramerateCapture>(captures);

                    // Atualizar DataGridView
                    dgvFiles.Rows.Clear();
                    foreach (var capture in captures)
                    {
                        dgvFiles.Rows.Add(
                            capture.Cpu,
                            capture.Gpu,
                            capture.GameName,
                            capture.GameSettings,
                            capture.AverageFramerate.ToString("N1"),
                            capture.TenPercentLowFramerate.ToString("N1"),
                            capture.OnePercentLowFramerate.ToString("N1"),
                            capture.ZeroPointOnePercentLowFramerate.ToString("N1")
                        );
                    }

                    lblStatus.Text = $"Carregados {loadedData.Count} arquivos com sucesso.";

                    // Selecionar primeiro item
                    if (dgvFiles.Rows.Count > 0)
                    {
                        dgvFiles.Rows[0].Selected = true;
                        UpdateDetails(loadedData[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro: {ex.Message}";
                MessageBox.Show($"Erro ao carregar arquivos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoadFiles.Enabled = true;
                progressBar.Visible = false;
            }
        }

        private void DgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFiles.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvFiles.SelectedRows[0].Index;
                if (selectedIndex < loadedData.Count)
                {
                    UpdateDetails(loadedData[selectedIndex]);
                }
            }
        }

        private void UpdateDetails(CapFrameXData data)
        {
            if (data == null)
                return;

            UpdateSystemInfo(data);
            UpdateStatistics(data);
            UpdateFrameTimesChart(data);
        }

        private void UpdateStatistics(CapFrameXData data)
        {
            var statsText = new System.Text.StringBuilder();
            statsText.AppendLine($"=== ESTATÍSTICAS DO ARQUIVO ===\n");
            statsText.AppendLine($"Arquivo: {data.FileName}");
            statsText.AppendLine($"Jogo: {data.Info?.GameName ?? "Desconhecido"}");
            statsText.AppendLine($"Data da Captura: {data.Info?.CreationDate:dd/MM/yyyy HH:mm:ss}");
            statsText.AppendLine($"Número de Runs: {data.Runs?.Count ?? 0}\n");

            if (data.Runs != null && data.Runs.Count > 0)
            {
                for (int i = 0; i < data.Runs.Count; i++)
                {
                    var run = data.Runs[i];
                    if (run.CaptureData != null && run.CaptureData.TimeInSeconds?.Count > 0)
                    {
                        var stat = run.Statistics;
                        statsText.AppendLine($"--- Run {i + 1} ---");
                        statsText.AppendLine($"Hash: {stat.Hash.Substring(0, Math.Min(16, stat.Hash.Length))}...");
                        statsText.AppendLine($"Frames: {stat.FrameCount}");
                        statsText.AppendLine($"Duração: {stat.Duration:F2} segundos");
                        statsText.AppendLine($"FPS Médio: {stat.AverageFps:F2}");
                        statsText.AppendLine($"10% Low: {stat.TenPercentLowFramerate:F2} FPS");
                        statsText.AppendLine($"1% Low: {stat.OnePercentLowFramerate:F2} FPS");
                        statsText.AppendLine($"0.1% Low: {stat.ZeroPointOnePercentLowFramerate:F2} FPS");
                        statsText.AppendLine();
                    }
                }
            }

            lblStats.Text = statsText.ToString();
        }

        private void UpdateSystemInfo(CapFrameXData data)
        {
            var sysText = new System.Text.StringBuilder();
            sysText.AppendLine($"=== INFORMAÇÕES DO SISTEMA ===\n");

            if (data.Info != null)
            {
                sysText.AppendLine($"Processador: {data.Info.Processor}");
                sysText.AppendLine($"GPU: {data.Info.GPU}");
                sysText.AppendLine($"RAM: {data.Info.SystemRam}");
                sysText.AppendLine($"Placa-mãe: {data.Info.Motherboard}");
                sysText.AppendLine($"Sistema: {data.Info.OS}");
                sysText.AppendLine($"Driver GPU: {data.Info.GPUDriverVersion}");
                sysText.AppendLine($"API: {data.Info.ApiInfo}");
                sysText.AppendLine($"ReBAR: {data.Info.ResizableBar}");
                sysText.AppendLine($"HAGS: {data.Info.HAGS}");
                sysText.AppendLine($"Game Mode: {data.Info.WinGameMode}");
                sysText.AppendLine($"Modo de Apresentação: {data.Info.PresentationMode}");
                sysText.AppendLine($"Comentário: {data.Info.Comment}");
                sysText.AppendLine($"Resolução: {data.Info.ResolutionInfo ?? "Não especificada"}");
            }

            lblSystemInfo.Text = sysText.ToString();
        }

        private void UpdateFrameTimesChart(CapFrameXData data)
        {
            if (data.Runs == null || data.Runs.Count == 0 ||
                data.Runs[0].CaptureData?.FrameTimes == null)
            {
                chartFrameTimes.Series[0].Points.Clear();
                return;
            }

            var run = data.Runs[0];
            var frameTimes = run.CaptureData.FrameTimes;

            chartFrameTimes.Series[0].Points.Clear();
            chartFrameTimes.ChartAreas[0].AxisX.Title = "Frame";
            chartFrameTimes.ChartAreas[0].AxisY.Title = "Frame Time (ms)";
            chartFrameTimes.ChartAreas[0].AxisX.Interval = frameTimes.Count / 10;

            for (int i = 0; i < frameTimes.Count; i++)
            {
                chartFrameTimes.Series[0].Points.AddXY(i + 1, frameTimes[i]);
            }

            chartFrameTimes.Titles.Clear();
            chartFrameTimes.Titles.Add($"Frame Times - {data.FileName}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Limpeza se necessário
            base.OnFormClosing(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IList<FramerateAggregatedPerformance> aggregatedPerformances = framerateCaptures
                .GroupBy(c => $"{c.Cpu}|{c.Gpu}|{c.Memory}")
                .Select(c => new FramerateAggregatedPerformance(c.ToList()))
                .ToList();

            string report = new AggregatePerformanceReporter().ReportePerformance(aggregatedPerformances, "\t");

            if (MessageBox.Show("Copy to Clipboard?", "Success", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Clipboard.SetText(report);
        }

        private void chartFrameTimes_Click(object sender, EventArgs e)
        {

        }

        public Stream GeneratePerformanceCsv()
        {
            IList<FramerateAggregatedPerformance> aggregatedPerformances = framerateCaptures
                .GroupBy(c => $"{c.Cpu}|{c.Gpu}|{c.Memory}")
                .Select(c => new FramerateAggregatedPerformance(c.ToList()))
                .ToList();

            string report = new AggregatePerformanceReporter().ReportePerformance(aggregatedPerformances, ",");

            var file = "performance_report.csv";

            using (var stream = File.CreateText(file))
            {
                stream.Write(report);

                return stream.BaseStream;
            }
        }
    }
}