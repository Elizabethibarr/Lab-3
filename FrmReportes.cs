using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using WinFormsApp3.Services;

namespace WinFormsApp3
{
    public partial class FrmReportes : Form
    {
        private readonly DataGridView _stockGrid = UiTheme.Grid();
        private readonly DataGridView _salesGrid = UiTheme.Grid();
        private readonly ReportExportService _exportService = new();
        private readonly TextBox _filter = UiTheme.TextBox("Filtrar por nombre o código");
        private readonly DateTimePicker _startDate = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Width = 125, Font = new Font("Segoe UI", 9.5f) };
        private readonly DateTimePicker _endDate = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Width = 125, Font = new Font("Segoe UI", 9.5f) };
        private readonly Label _status = UiTheme.Label("", 9, color: UiTheme.Muted);
        private readonly TabControl _tabs = new() { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f) };
        private bool _loading;

        public FrmReportes()
        {
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
                LoadReports();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface()
        {
            Text = "Reportes | ElectroStore";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(940, 650);
            ClientSize = new Size(1120, 760);
            BackColor = UiTheme.Background;
            Padding = new Padding(22);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            var titlePanel = new Panel { Dock = DockStyle.Fill };
            var title = UiTheme.Label("Reportes", 21, true);
            title.Margin = Padding.Empty;
            title.Location = new Point(0, 2);
            titlePanel.Controls.Add(title);
            var subtitle = UiTheme.Label("Consulta y exporta información del inventario y las ventas.", 9, color: UiTheme.Muted);
            subtitle.Margin = Padding.Empty;
            subtitle.Location = new Point(1, 40);
            titlePanel.Controls.Add(subtitle);
            layout.Controls.Add(titlePanel, 0, 0);

            var searchBar = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(0, 6, 0, 6) };
            searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 142));
            _filter.Dock = DockStyle.Fill;
            _filter.Margin = new Padding(0, 3, 10, 3);
            _filter.TextChanged += (_, _) => { if (!_loading) LoadReports(); };
            searchBar.Controls.Add(_filter, 0, 0);
            var refresh = UiTheme.Button("Actualizar", true);
            refresh.AutoSize = false;
            refresh.Size = new Size(132, 42);
            refresh.Dock = DockStyle.None;
            refresh.Anchor = AnchorStyles.Right;
            refresh.TextAlign = ContentAlignment.MiddleCenter;
            refresh.Margin = new Padding(0);
            refresh.Click += (_, _) => LoadReports();
            searchBar.Controls.Add(refresh, 1, 0);
            layout.Controls.Add(searchBar, 0, 1);

            var periodBar = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.White, Padding = new Padding(12, 8, 12, 8), Margin = new Padding(0, 2, 0, 2) };
            periodBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            periodBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
            periodBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
            var periodLabel = UiTheme.Label("PERIODO DE VENTAS", 8, true, UiTheme.Muted);
            periodLabel.Anchor = AnchorStyles.Left;
            periodBar.Controls.Add(periodLabel, 0, 0);
            periodBar.Controls.Add(DatePickerCard("Desde", _startDate), 1, 0);
            periodBar.Controls.Add(DatePickerCard("Hasta", _endDate), 2, 0);
            _startDate.ValueChanged += (_, _) => DateChanged();
            _endDate.ValueChanged += (_, _) => DateChanged();
            layout.Controls.Add(periodBar, 0, 2);

            var exportBar = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 5, 0, 5) };
            exportBar.Controls.Add(UiTheme.Label("Exportar reporte visible:", 9, true, UiTheme.Muted));
            exportBar.Controls.Add(ExportButton("CSV", ReportExportFormat.Csv));
            exportBar.Controls.Add(ExportButton("Excel", ReportExportFormat.Excel));
            exportBar.Controls.Add(ExportButton("PDF", ReportExportFormat.Pdf));
            layout.Controls.Add(exportBar, 0, 3);

            var stockTab = new TabPage("Existencias") { BackColor = Color.White, Padding = new Padding(10) };
            var salesTab = new TabPage("Productos vendidos") { BackColor = Color.White, Padding = new Padding(10) };
            _stockGrid.CellFormatting += FormatCriticalStock;
            stockTab.Controls.Add(_stockGrid);
            salesTab.Controls.Add(_salesGrid);
            _tabs.TabPages.Add(stockTab);
            _tabs.TabPages.Add(salesTab);
            _tabs.SelectedIndexChanged += (_, _) => UpdateFilterState();
            layout.Controls.Add(_tabs, 0, 4);
            layout.Controls.Add(_status, 0, 5);
            Controls.Add(layout);
            UpdateFilterState();
        }

        private static Panel DatePickerCard(string caption, DateTimePicker picker)
        {
            var card = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0),
                Margin = new Padding(4, 0, 4, 0)
            };
            card.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            card.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var label = UiTheme.Label(caption, 8, true, UiTheme.Muted);
            label.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            label.Margin = new Padding(0, 0, 0, 1);
            picker.Dock = DockStyle.Fill;
            picker.Margin = new Padding(0);
            card.Controls.Add(label, 0, 0);
            card.Controls.Add(picker, 0, 1);
            return card;
        }

        private Button ExportButton(string text, ReportExportFormat format)
        {
            var button = UiTheme.Button(text);
            button.MinimumSize = new Size(92, 40);
            button.Height = 40;
            button.Margin = new Padding(8, 0, 0, 0);
            button.Click += (_, _) => ExportCurrentReport(format);
            return button;
        }

        private void DateChanged()
        {
            if (!_loading) LoadSales();
        }

        private void UpdateFilterState()
        {
            var isSales = _tabs.SelectedIndex == 1;
            _startDate.Enabled = isSales;
            _endDate.Enabled = isSales;
            _status.Text = isSales
                ? "El periodo aplica al reporte de productos vendidos."
                : "El filtro busca por código o nombre del producto.";
        }

        private void LoadReports()
        {
            if (_startDate.Value.Date > _endDate.Value.Date)
            {
                MessageBox.Show(this, "La fecha inicial no puede ser posterior a la fecha final.", "Periodo inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _loading = true;
            try
            {
                LoadStock();
                LoadSales();
                _status.Text = "Reportes actualizados  •  " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            finally
            {
                _loading = false;
            }
        }

        private void LoadSales()
        {
            if (_startDate.Value.Date > _endDate.Value.Date) return;
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT p.Codigo, p.Nombre AS NombreProducto, SUM(d.Cantidad) AS UnidadesVendidas, SUM(d.Importe) AS MontoAcumulado FROM dbo.DetalleVenta AS d INNER JOIN dbo.Productos AS p ON p.IdProducto = d.IdProducto INNER JOIN dbo.Ventas AS v ON v.IdVenta = d.IdVenta WHERE v.FechaVenta >= @desde AND v.FechaVenta < @hasta AND (@filtro = N'' OR p.Codigo LIKE @buscar OR p.Nombre LIKE @buscar) GROUP BY p.Codigo, p.Nombre ORDER BY MontoAcumulado DESC", connection);
                command.Parameters.Add("@desde", SqlDbType.DateTime2).Value = _startDate.Value.Date;
                command.Parameters.Add("@hasta", SqlDbType.DateTime2).Value = _endDate.Value.Date.AddDays(1);
                var filter = _filter.Text.Trim();
                command.Parameters.Add("@filtro", SqlDbType.NVarChar, 120).Value = filter;
                command.Parameters.Add("@buscar", SqlDbType.NVarChar, 122).Value = "%" + filter + "%";
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                _salesGrid.DataSource = table;
                _salesGrid.Columns["Codigo"].HeaderText = "Código";
                _salesGrid.Columns["NombreProducto"].HeaderText = "Producto";
                _salesGrid.Columns["UnidadesVendidas"].HeaderText = "Unidades vendidas";
                _salesGrid.Columns["MontoAcumulado"].HeaderText = "Monto acumulado";
                _salesGrid.Columns["MontoAcumulado"].DefaultCellStyle.Format = "C2";
            }
            catch (Exception ex)
            {
                _status.Text = "No se pudo cargar el reporte de ventas.";
                MessageBox.Show(this, "No se pudo cargar el reporte de ventas.\n\n" + ex.Message, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStock()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT Codigo, NombreProducto, Categoria, Precio, Existencias, StockMinimo, StockCritico FROM dbo.vw_ReporteStock WHERE (@filtro = N'' OR Codigo LIKE @buscar OR NombreProducto LIKE @buscar) ORDER BY NombreProducto", connection);
                var filter = _filter.Text.Trim();
                command.Parameters.Add("@filtro", SqlDbType.NVarChar, 120).Value = filter;
                command.Parameters.Add("@buscar", SqlDbType.NVarChar, 122).Value = "%" + filter + "%";
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                _stockGrid.DataSource = table;
                _stockGrid.Columns["Codigo"].HeaderText = "Código";
                _stockGrid.Columns["NombreProducto"].HeaderText = "Producto";
                _stockGrid.Columns["Categoria"].HeaderText = "Categoría";
                _stockGrid.Columns["Precio"].DefaultCellStyle.Format = "C2";
                _stockGrid.Columns["Existencias"].HeaderText = "Stock";
                _stockGrid.Columns["StockMinimo"].HeaderText = "Mínimo";
                _stockGrid.Columns["StockCritico"].HeaderText = "Alerta";
            }
            catch (Exception ex)
            {
                _status.Text = "No se pudo cargar el reporte de existencias.";
                MessageBox.Show(this, "No se pudo cargar el reporte de existencias.\n\n" + ex.Message, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CurrentReportName => _tabs.SelectedIndex == 1 ? "ProductosVendidos" : "Existencias";

        private void ExportCurrentReport(ReportExportFormat format)
        {
            if (_tabs.SelectedIndex < 0) return;
            var grid = _tabs.SelectedIndex == 1 ? _salesGrid : _stockGrid;
            if (grid.DataSource is not DataTable table)
            {
                MessageBox.Show(this, "No hay datos cargados para exportar.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                FileName = $"{CurrentReportName}_{DateTime.Now:yyyyMMdd_HHmm}",
                Filter = format switch
                {
                    ReportExportFormat.Csv => "CSV (*.csv)|*.csv",
                    ReportExportFormat.Excel => "Libro de Excel (*.xlsx)|*.xlsx",
                    _ => "Documento PDF (*.pdf)|*.pdf"
                },
                DefaultExt = format switch { ReportExportFormat.Csv => "csv", ReportExportFormat.Excel => "xlsx", _ => "pdf" },
                AddExtension = true,
                OverwritePrompt = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                var headers = grid.Columns.Cast<DataGridViewColumn>()
                    .Where(column => column.Visible && !string.IsNullOrWhiteSpace(column.DataPropertyName))
                    .ToDictionary(column => column.DataPropertyName, column => column.HeaderText);
                var title = CurrentReportName == "ProductosVendidos" ? "Reporte de productos vendidos" : "Reporte de existencias";
                var subtitle = CurrentReportName == "ProductosVendidos"
                    ? $"Periodo: {_startDate.Value:dd/MM/yyyy} - {_endDate.Value:dd/MM/yyyy}"
                    : "Inventario actual";
                _exportService.Export(table, dialog.FileName, format, title, subtitle, headers);
                _status.Text = "Reporte exportado: " + Path.GetFileName(dialog.FileName);
                MessageBox.Show(this, "El reporte se exportó correctamente.", "Exportación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo exportar el reporte.\n\n" + ex.Message, "Error de exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatCriticalStock(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !_stockGrid.Columns.Contains("StockCritico")) return;
            if (_stockGrid.Rows[e.RowIndex].Cells["StockCritico"].Value is bool critical && critical)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 239, 230);
                e.CellStyle.ForeColor = Color.FromArgb(158, 69, 28);
                e.CellStyle.SelectionBackColor = Color.FromArgb(255, 220, 199);
            }
        }

    }
}
