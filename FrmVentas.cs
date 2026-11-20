using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using WinFormsApp3.Models;
using WinFormsApp3.Repositories;

namespace WinFormsApp3
{
    public partial class FrmVentas : Form
    {
        private readonly int _usuarioId;
        private readonly SalesRepository _salesRepository = new();
        private readonly DataGridView _productsGrid = UiTheme.Grid();
        private readonly DataGridView _cartGrid = UiTheme.Grid();
        private readonly NumericUpDown _quantity = new() { Minimum = 1, Maximum = 100000, Value = 1, Width = 100, Font = new Font("Segoe UI", 10) };
        private readonly Label _totalLabel = UiTheme.Label("Total: $0.00", 16, true, UiTheme.Navy);
        private readonly Label _status = UiTheme.Label("Selecciona un producto para agregarlo a la venta.", 9, color: UiTheme.Muted);
        private readonly DataTable _cart = CreateCartTable();
        private readonly Button _completeSale = UiTheme.Button("Registrar venta", true);

        public FrmVentas() : this(0) { }

        public FrmVentas(int usuarioId)
        {
            _usuarioId = usuarioId;
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
                LoadProducts();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface()
        {
            Text = "Nueva venta | ElectroStore";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(920, 680);
            ClientSize = new Size(1100, 760);
            BackColor = UiTheme.Background;
            Padding = new Padding(22);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 52));

            var heading = new Panel { Dock = DockStyle.Fill };
            var title = UiTheme.Label("Nueva venta", 21, true);
            title.Location = new Point(0, 0);
            heading.Controls.Add(title);
            var subtitle = UiTheme.Label("Selecciona productos y cantidades para agregarlos al carrito.", 9, color: UiTheme.Muted);
            subtitle.Location = new Point(0, 34);
            heading.Controls.Add(subtitle);
            layout.Controls.Add(heading, 0, 0);
            layout.Controls.Add(UiTheme.Label("CATÁLOGO DE PRODUCTOS", 8, true, UiTheme.Muted), 0, 1);

            ConfigureProductColumns();
            var productCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10), BorderStyle = BorderStyle.FixedSingle };
            _productsGrid.SelectionChanged += (_, _) => UpdateAddAvailability();
            productCard.Controls.Add(_productsGrid);
            layout.Controls.Add(productCard, 0, 2);

            var productActions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 4, 0, 4) };
            productActions.Controls.Add(UiTheme.Label("Cantidad", 9, true, UiTheme.Muted));
            _quantity.Margin = new Padding(8, 2, 14, 0);
            productActions.Controls.Add(_quantity);
            var add = UiTheme.Button("Agregar al carrito", true);
            add.Margin = new Padding(0, 0, 8, 0);
            add.Click += AddToCart;
            productActions.Controls.Add(add);
            var remove = UiTheme.Button("Quitar seleccionado");
            remove.Margin = new Padding(0);
            remove.Click += RemoveFromCart;
            productActions.Controls.Add(remove);
            layout.Controls.Add(productActions, 0, 3);

            var cartCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10), BorderStyle = BorderStyle.FixedSingle };
            ConfigureCartColumns();
            _cartGrid.DataSource = _cart;
            cartCard.Controls.Add(_cartGrid);
            layout.Controls.Add(cartCard, 0, 4);

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White, Padding = new Padding(12, 8, 12, 8) };
            _status.Location = new Point(12, 22);
            footer.Controls.Add(_status);
            _totalLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            footer.Controls.Add(_totalLabel);
            _completeSale.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _completeSale.Text = "Registrar venta";
            _completeSale.Click += CompleteSale;
            footer.Controls.Add(_completeSale);
            footer.Resize += (_, _) =>
            {
                _completeSale.Location = new Point(footer.ClientSize.Width - _completeSale.Width - 10, 8);
                _totalLabel.Location = new Point(footer.ClientSize.Width - _completeSale.Width - _totalLabel.Width - 38, 17);
            };

            Controls.Add(layout);
            Controls.Add(footer);
            footer.BringToFront();
            UpdateAddAvailability();
            _completeSale.Enabled = false;
        }

        private static DataTable CreateCartTable()
        {
            var table = new DataTable();
            table.Columns.Add("IdProducto", typeof(int));
            table.Columns.Add("Codigo", typeof(string));
            table.Columns.Add("Producto", typeof(string));
            table.Columns.Add("Precio", typeof(decimal));
            table.Columns.Add("Cantidad", typeof(int));
            table.Columns.Add("Importe", typeof(decimal));
            return table;
        }

        private void ConfigureCartColumns()
        {
            _cartGrid.AutoGenerateColumns = false;
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", DataPropertyName = "IdProducto", Visible = false });
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", DataPropertyName = "Codigo", HeaderText = "Código" });
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Producto", DataPropertyName = "Producto", HeaderText = "Producto" });
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Precio", DataPropertyName = "Precio", HeaderText = "Precio", DefaultCellStyle = { Format = "C2" } });
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cantidad", DataPropertyName = "Cantidad", HeaderText = "Cantidad" });
            _cartGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Importe", DataPropertyName = "Importe", HeaderText = "Importe", DefaultCellStyle = { Format = "C2" } });
        }

        private void ConfigureProductColumns()
        {
            _productsGrid.AutoGenerateColumns = false;
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", DataPropertyName = "IdProducto", Visible = false });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", DataPropertyName = "Codigo", HeaderText = "Código" });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", DataPropertyName = "Nombre", HeaderText = "Producto" });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Precio", DataPropertyName = "Precio", HeaderText = "Precio", DefaultCellStyle = { Format = "C2" } });
            _productsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stock", DataPropertyName = "Stock", HeaderText = "Disponible" });
        }

        private void LoadProducts()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT IdProducto, Codigo, Nombre, Precio, Stock FROM dbo.Productos WHERE Activo = 1 AND Stock > 0 ORDER BY Nombre", connection);
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                _productsGrid.DataSource = table;
                UpdateAddAvailability();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo cargar el catálogo de productos.\n\n" + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _status.Text = "Error al cargar productos.";
            }
        }

        private void UpdateAddAvailability()
        {
            var hasSelection = _productsGrid.SelectedRows.Count > 0;
            foreach (Control control in Controls)
            {
                if (control is TableLayoutPanel layout && layout.GetControlFromPosition(0, 3) is FlowLayoutPanel actions)
                {
                    var add = actions.Controls.OfType<Button>().FirstOrDefault(button => button.Text == "Agregar al carrito");
                    if (add is not null) add.Enabled = hasSelection;
                }
            }
        }

        private void AddToCart(object? sender, EventArgs e)
        {
            if (_productsGrid.SelectedRows.Count == 0) return;
            var selected = _productsGrid.SelectedRows[0];
            var productId = Convert.ToInt32(selected.Cells["IdProducto"].Value);
            var stock = Convert.ToInt32(selected.Cells["Stock"].Value);
            var quantityToAdd = (int)_quantity.Value;
            var existing = _cart.AsEnumerable().FirstOrDefault(row => row.Field<int>("IdProducto") == productId);
            var newQuantity = quantityToAdd + (existing?.Field<int>("Cantidad") ?? 0);

            if (newQuantity > stock)
            {
                MessageBox.Show(this, $"Solo hay {stock} unidades disponibles.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var price = Convert.ToDecimal(selected.Cells["Precio"].Value);
            if (existing is null)
                _cart.Rows.Add(productId, Convert.ToString(selected.Cells["Codigo"].Value), Convert.ToString(selected.Cells["Nombre"].Value), price, quantityToAdd, price * quantityToAdd);
            else
            {
                existing["Cantidad"] = newQuantity;
                existing["Importe"] = price * newQuantity;
            }
            UpdateTotal();
            _status.Text = "Producto agregado al carrito.";
        }

        private void RemoveFromCart(object? sender, EventArgs e)
        {
            if (_cartGrid.SelectedRows.Count == 0) return;
            var id = Convert.ToInt32(_cartGrid.SelectedRows[0].Cells["IdProducto"].Value);
            var row = _cart.AsEnumerable().FirstOrDefault(item => item.Field<int>("IdProducto") == id);
            if (row is not null) _cart.Rows.Remove(row);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var total = _cart.AsEnumerable().Sum(row => row.Field<decimal>("Importe"));
            _totalLabel.Text = "Total: " + total.ToString("C2");
            _completeSale.Enabled = _cart.Rows.Count > 0;
        }

        private void CompleteSale(object? sender, EventArgs e)
        {
            if (_cart.Rows.Count == 0) return;

            var details = _cart.AsEnumerable()
                .Select(item => new DetalleVenta
                {
                    IdProducto = item.Field<int>("IdProducto"),
                    Cantidad = item.Field<int>("Cantidad")
                })
                .ToArray();

            try
            {
                var saleId = _salesRepository.RegisterSale(_usuarioId, details);
                MessageBox.Show(this, $"Venta #{saleId} registrada correctamente.\nTotal: {_cart.AsEnumerable().Sum(row => row.Field<decimal>("Importe")).ToString("C2")}", "Venta completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex) when (ex.Number == 50004)
            {
                MessageBox.Show(this, "El stock cambió mientras se preparaba la venta. Actualiza el catálogo y vuelve a intentarlo.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadProducts();
            }
            catch (SqlException ex) when (ex.Number is 50001 or 50002 or 50003)
            {
                MessageBox.Show(this, ex.Message, "No se pudo registrar la venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo registrar la venta.\n\n" + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
