using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp3
{
    public partial class FrmProductoModal : Form
    {
        private readonly int? _productId;
        private readonly TextBox _code = UiTheme.TextBox("Ej. ACC-004");
        private readonly TextBox _name = UiTheme.TextBox("Nombre del producto");
        private readonly TextBox _description = UiTheme.TextBox("Descripción opcional");
        private readonly ComboBox _category = new() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
        private readonly NumericUpDown _price = new() { DecimalPlaces = 2, Maximum = 10000000, Minimum = 0, ThousandsSeparator = true, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
        private readonly NumericUpDown _stock = new() { Maximum = 1000000, Minimum = 0, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
        private readonly NumericUpDown _minimum = new() { Maximum = 1000000, Minimum = 0, Value = 5, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };

        public FrmProductoModal() : this(null) { }

        public FrmProductoModal(int? productId)
        {
            _productId = productId;
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
                LoadCategories();
                if (_productId.HasValue) LoadProduct();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface()
        {
            Text = _productId.HasValue ? "Editar producto | ElectroStore" : "Nuevo producto | ElectroStore";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(570, 600);
            BackColor = Color.White;
            Padding = new Padding(24);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 10 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            for (var i = 1; i < 8; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.Controls.Add(UiTheme.Label(_productId.HasValue ? "Editar producto" : "Registrar producto", 19, true), 0, 0);
            layout.SetColumnSpan(layout.GetControlFromPosition(0, 0)!, 2);
            AddField(layout, "Código", _code, 1);
            AddField(layout, "Nombre", _name, 2);
            AddField(layout, "Descripción", _description, 3);
            AddField(layout, "Categoría", _category, 4);
            AddField(layout, "Precio", _price, 5);
            AddField(layout, "Existencias", _stock, 6);
            AddField(layout, "Stock mínimo", _minimum, 7);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var save = UiTheme.Button("Guardar producto", true);
            var cancel = UiTheme.Button("Cancelar");
            save.Click += SaveClicked;
            cancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
            actions.Controls.Add(save);
            actions.Controls.Add(cancel);
            layout.Controls.Add(actions, 0, 9);
            layout.SetColumnSpan(actions, 2);
            Controls.Add(layout);
            AcceptButton = save;
            CancelButton = cancel;
        }

        private static void AddField(TableLayoutPanel layout, string caption, Control input, int row)
        {
            var label = UiTheme.Label(caption, 9, true, UiTheme.Muted);
            label.Anchor = AnchorStyles.Left;
            input.Dock = DockStyle.Fill;
            input.Margin = new Padding(0, 6, 0, 6);
            layout.Controls.Add(label, 0, row);
            layout.Controls.Add(input, 1, row);
        }

        private void LoadCategories()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT IdCategoria, Nombre FROM dbo.Categorias WHERE Activa = 1 OR IdCategoria = (SELECT IdCategoria FROM dbo.Productos WHERE IdProducto = @id) ORDER BY Nombre", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = (object?)_productId ?? DBNull.Value;
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                _category.DisplayMember = "Nombre";
                _category.ValueMember = "IdCategoria";
                _category.DataSource = table;
                if (table.Rows.Count == 0)
                    MessageBox.Show(this, "Crea primero una categoría activa en la base de datos.", "Sin categorías", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudieron cargar las categorías.\n\n" + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProduct()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT IdCategoria, Codigo, Nombre, Descripcion, Precio, Stock, StockMinimo FROM dbo.Productos WHERE IdProducto = @id AND Activo = 1", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = _productId!.Value;
                connection.Open();
                using var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show(this, "El producto ya no existe o está inactivo.", "Producto no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                _category.SelectedValue = reader.GetInt32(0);
                _code.Text = reader.GetString(1);
                _name.Text = reader.GetString(2);
                _description.Text = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                _price.Value = reader.GetDecimal(4);
                _stock.Value = reader.GetInt32(5);
                _minimum.Value = reader.GetInt32(6);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo cargar el producto para editar.\\n\\n" + ex.Message, "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void SaveClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_code.Text) || string.IsNullOrWhiteSpace(_name.Text) || _category.SelectedValue is null)
            {
                MessageBox.Show(this, "Completa el código, el nombre y la categoría.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                var sql = _productId.HasValue
                    ? "UPDATE dbo.Productos SET IdCategoria = @categoria, Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, Stock = @stock, StockMinimo = @minimo WHERE IdProducto = @id AND Activo = 1"
                    : "INSERT INTO dbo.Productos (IdCategoria, Codigo, Nombre, Descripcion, Precio, Stock, StockMinimo) VALUES (@categoria, @codigo, @nombre, @descripcion, @precio, @stock, @minimo)";
                using var command = new SqlCommand(sql, connection);
                if (_productId.HasValue) command.Parameters.Add("@id", SqlDbType.Int).Value = _productId.Value;
                command.Parameters.Add("@categoria", SqlDbType.Int).Value = Convert.ToInt32(_category.SelectedValue);
                command.Parameters.Add("@codigo", SqlDbType.NVarChar, 30).Value = _code.Text.Trim();
                command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = _name.Text.Trim();
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(_description.Text) ? DBNull.Value : _description.Text.Trim();
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = _price.Value;
                command.Parameters.Add("@stock", SqlDbType.Int).Value = (int)_stock.Value;
                command.Parameters.Add("@minimo", SqlDbType.Int).Value = (int)_minimum.Value;
                connection.Open();
                command.ExecuteNonQuery();
                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                MessageBox.Show(this, "Ya existe un producto con ese código.", "Código duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo guardar el producto.\n\n" + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
