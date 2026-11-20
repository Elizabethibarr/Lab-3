using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace WinFormsApp3
{
    public partial class FrmPrincipal : Form
    {
        private readonly int _usuarioId;
        private readonly string _nombreUsuario;
        private readonly string _rolUsuario;
        private readonly DataGridView _grid = UiTheme.Grid();
        private readonly Label _status = UiTheme.Label("Cargando inventario...", 9, color: UiTheme.Muted);
        private readonly Label _productCount = UiTheme.Label("—", 20, true, UiTheme.Text);
        private readonly Label _criticalCount = UiTheme.Label("—", 20, true, Color.FromArgb(202, 102, 30));
        private readonly TextBox _search = UiTheme.TextBox("Buscar por nombre o código");

        public FrmPrincipal() : this(0, "Usuario", "Sin rol") { }

        public FrmPrincipal(string nombreUsuario) : this(0, nombreUsuario, "Sin rol") { }

        public FrmPrincipal(int usuarioId, string nombreUsuario) : this(usuarioId, nombreUsuario, "Sin rol") { }

        public FrmPrincipal(int usuarioId, string nombreUsuario, string rolUsuario)
        {
            _usuarioId = usuarioId;
            _nombreUsuario = nombreUsuario;
            _rolUsuario = rolUsuario;
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
            Text = "ElectroStore | Panel principal";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(980, 650);
            ClientSize = new Size(1200, 780);
            BackColor = UiTheme.Background;
            Font = new Font("Segoe UI", 9.5f);

            var shell = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 218));
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(24, 0, 24, 0) };
            var headerTitle = UiTheme.Label("ElectroStore", 16, true, UiTheme.Navy);
            headerTitle.Location = new Point(20, 20);
            var user = UiTheme.Label("●  " + _nombreUsuario, 9, true, UiTheme.Muted);
            user.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            header.Resize += (_, _) => user.Location = new Point(header.ClientSize.Width - user.Width - 24, 25);
            header.Controls.Add(headerTitle);
            header.Controls.Add(user);

            var sidebar = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Navy, Padding = new Padding(14, 25, 14, 14) };
            var sidebarLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = UiTheme.Navy };
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
            var menu = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true, BackColor = UiTheme.Navy };
            menu.Controls.Add(UiTheme.Label("MENÚ", 8, true, Color.FromArgb(160, 181, 212)));
            var inventoryButton = MenuButton("▦   Inventario");
            inventoryButton.Click += (_, _) => LoadProducts();
            var reportsButton = MenuButton("▤   Reportes");
            reportsButton.Click += (_, _) => OpenReports();
            var salesButton = MenuButton("＄   Nueva venta");
            salesButton.Click += (_, _) => OpenSales();
            var addButton = MenuButton("＋   Nuevo producto");
            addButton.Click += (_, _) => AddProduct();
            menu.Controls.Add(inventoryButton);
            menu.Controls.Add(salesButton);
            menu.Controls.Add(reportsButton);
            menu.Controls.Add(addButton);
            if (string.Equals(_rolUsuario, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                var usersButton = MenuButton("♙   Usuarios");
                usersButton.Click += (_, _) => OpenUsers();
                menu.Controls.Add(usersButton);
            }
            sidebarLayout.Controls.Add(menu, 0, 0);

            var accountPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 49, 80), Padding = new Padding(10), Margin = new Padding(0, 8, 0, 0) };
            var accountTitle = UiTheme.Label("SESIÓN ACTIVA", 8, true, Color.FromArgb(160, 181, 212));
            accountTitle.Location = new Point(10, 10);
            var accountName = UiTheme.Label(_nombreUsuario, 9, true, Color.White);
            accountName.AutoSize = false;
            accountName.Size = new Size(165, 22);
            accountName.Location = new Point(10, 34);
            accountName.AutoEllipsis = true;
            var roleLabel = UiTheme.Label("Rol: " + _rolUsuario, 8, color: Color.FromArgb(205, 217, 237));
            roleLabel.AutoSize = false;
            roleLabel.Size = new Size(165, 20);
            roleLabel.Location = new Point(10, 57);
            roleLabel.AutoEllipsis = true;
            var logoutButton = MenuButton("↪   Cerrar sesión");
            logoutButton.Width = 165;
            logoutButton.Height = 36;
            logoutButton.Margin = new Padding(0);
            logoutButton.Location = new Point(0, 84);
            logoutButton.Click += (_, _) => Logout();
            accountPanel.Controls.Add(accountTitle);
            accountPanel.Controls.Add(accountName);
            accountPanel.Controls.Add(roleLabel);
            accountPanel.Controls.Add(logoutButton);
            sidebarLayout.Controls.Add(accountPanel, 0, 1);
            sidebar.Controls.Add(sidebarLayout);

            var content = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(26, 22, 26, 14), ColumnCount = 1, RowCount = 5 };
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));

            var titlePanel = new Panel { Dock = DockStyle.Fill };
            var title = UiTheme.Label("Inventario", 21, true);
            title.Location = new Point(0, 2);
            titlePanel.Controls.Add(title);
            titlePanel.Controls.Add(UiTheme.Label("Controla los productos y sus existencias.", 9, color: UiTheme.Muted) is Label sub ? SetLocation(sub, new Point(0, 34)) : new Label());
            content.Controls.Add(titlePanel, 0, 0);

            var stats = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, BackColor = UiTheme.Background, Padding = new Padding(0, 4, 0, 4) };
            stats.Controls.Add(StatCard("PRODUCTOS ACTIVOS", _productCount, UiTheme.Blue));
            stats.Controls.Add(StatCard("STOCK CRÍTICO", _criticalCount, Color.FromArgb(202, 102, 30)));
            content.Controls.Add(stats, 0, 1);

            var toolbar = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 1, Padding = new Padding(0, 4, 0, 4) };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            _search.Dock = DockStyle.Fill;
            _search.Margin = new Padding(0, 0, 10, 0);
            _search.TextChanged += (_, _) => LoadProducts();
            var refresh = UiTheme.Button("Actualizar");
            refresh.AutoSize = false;
            refresh.MinimumSize = Size.Empty;
            refresh.Size = new Size(108, 36);
            refresh.Anchor = AnchorStyles.None;
            refresh.Margin = new Padding(2);
            refresh.Padding = new Padding(6, 2, 6, 2);
            refresh.Click += (_, _) => LoadProducts();
            var create = UiTheme.Button("＋  Agregar", true);
            create.AutoSize = false;
            create.MinimumSize = Size.Empty;
            create.Size = new Size(122, 36);
            create.Anchor = AnchorStyles.None;
            create.Margin = new Padding(2);
            create.Padding = new Padding(6, 2, 6, 2);
            create.Click += (_, _) => AddProduct();
            var edit = ToolbarButton("Editar", false);
            edit.Click += (_, _) => EditProduct();
            var delete = ToolbarButton("Eliminar", false);
            delete.ForeColor = Color.Firebrick;
            delete.Click += (_, _) => DeleteProduct();
            toolbar.Controls.Add(_search, 0, 0);
            toolbar.Controls.Add(refresh, 1, 0);
            toolbar.Controls.Add(create, 2, 0);
            toolbar.Controls.Add(edit, 3, 0);
            toolbar.Controls.Add(delete, 4, 0);
            content.Controls.Add(toolbar, 0, 2);

            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), BorderStyle = BorderStyle.FixedSingle };
            _grid.CellFormatting += FormatCriticalStock;
            gridCard.Controls.Add(_grid);
            content.Controls.Add(gridCard, 0, 3);
            content.Controls.Add(_status, 0, 4);

            shell.Controls.Add(header, 0, 0);
            shell.SetColumnSpan(header, 2);
            shell.Controls.Add(sidebar, 0, 1);
            shell.Controls.Add(content, 1, 1);
            Controls.Add(shell);
        }

        private static Label SetLocation(Label label, Point location)
        {
            label.Location = location;
            return label;
        }

        private static Panel StatCard(string caption, Label value, Color accent)
        {
            var card = UiTheme.Card();
            card.Size = new Size(245, 90);
            var heading = UiTheme.Label(caption, 8, true, UiTheme.Muted);
            heading.Location = new Point(16, 12);
            value.Location = new Point(16, 34);
            value.ForeColor = accent;
            card.Controls.Add(heading);
            card.Controls.Add(value);
            return card;
        }

        private static Button MenuButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Width = 185,
                Height = 44,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                BackColor = UiTheme.Navy,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                Cursor = Cursors.Hand,
                Padding = new Padding(10, 0, 0, 0),
                Margin = new Padding(0, 12, 0, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 58, 94);
            return button;
        }

        private void LoadProducts()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT p.IdProducto, p.Codigo, p.Nombre, c.Nombre AS Categoria, p.Precio, p.Stock, p.StockMinimo FROM dbo.Productos p INNER JOIN dbo.Categorias c ON c.IdCategoria = p.IdCategoria WHERE p.Activo = 1 AND (@filtro = N'' OR p.Nombre LIKE @buscar OR p.Codigo LIKE @buscar) ORDER BY p.Nombre", connection);
                var filter = _search.Text.Trim();
                command.Parameters.Add("@filtro", SqlDbType.NVarChar, 120).Value = filter;
                command.Parameters.Add("@buscar", SqlDbType.NVarChar, 122).Value = "%" + filter + "%";
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                _grid.DataSource = table;
                if (_grid.Columns.Contains("IdProducto")) _grid.Columns["IdProducto"].Visible = false;
                if (_grid.Columns.Contains("StockMinimo")) _grid.Columns["StockMinimo"].HeaderText = "Mínimo";
                _grid.Columns["Codigo"].HeaderText = "Código";
                _grid.Columns["Nombre"].HeaderText = "Producto";
                _grid.Columns["Categoria"].HeaderText = "Categoría";
                _grid.Columns["Precio"].DefaultCellStyle.Format = "C2";
                _productCount.Text = table.Rows.Count.ToString();
                _criticalCount.Text = table.AsEnumerable().Count(row => row.Field<int>("Stock") <= row.Field<int>("StockMinimo")).ToString();
                _status.Text = "Inventario actualizado  •  " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                _status.Text = "No se pudo cargar el inventario.";
                MessageBox.Show(this, "No se pudo cargar el inventario. Verifica que la base de datos esté creada y accesible.\n\n" + ex.Message,
                    "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatCriticalStock(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !_grid.Columns.Contains("Stock") || !_grid.Columns.Contains("StockMinimo")) return;
            if (_grid.Rows[e.RowIndex].Cells["Stock"].Value is int stock &&
                _grid.Rows[e.RowIndex].Cells["StockMinimo"].Value is int minimum && stock <= minimum)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 239, 230);
                e.CellStyle.ForeColor = Color.FromArgb(158, 69, 28);
            }
        }

        private static Button ToolbarButton(string text, bool primary)
        {
            var button = UiTheme.Button(text, primary);
            button.AutoSize = false;
            button.MinimumSize = Size.Empty;
            button.Size = new Size(102, 36);
            button.Anchor = AnchorStyles.None;
            button.Margin = new Padding(2);
            button.Padding = new Padding(5, 2, 5, 2);
            return button;
        }

        private int? SelectedProductId()
        {
            if (_grid.CurrentRow is null || !_grid.Columns.Contains("IdProducto"))
            {
                MessageBox.Show(this, "Selecciona un producto de la lista.", "Producto no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return Convert.ToInt32(_grid.CurrentRow.Cells["IdProducto"].Value);
        }

        private void AddProduct()
        {
            using var dialog = new FrmProductoModal();
            if (dialog.ShowDialog(this) == DialogResult.OK) LoadProducts();
        }

        private void EditProduct()
        {
            var id = SelectedProductId();
            if (!id.HasValue) return;
            using var dialog = new FrmProductoModal(id.Value);
            if (dialog.ShowDialog(this) == DialogResult.OK) LoadProducts();
        }

        private void DeleteProduct()
        {
            var id = SelectedProductId();
            if (!id.HasValue) return;
            var name = Convert.ToString(_grid.CurrentRow!.Cells["Nombre"].Value) ?? "el producto seleccionado";
            if (MessageBox.Show(this, $"¿Desactivar {name}? Se conservará en el historial de ventas, pero dejará de aparecer en el inventario.", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("UPDATE dbo.Productos SET Activo = 0 WHERE IdProducto = @id AND Activo = 1", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                connection.Open();
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("El producto ya no existe o ya fue desactivado.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo eliminar el producto.\\n\\n" + ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenReports()
        {
            using var reports = new FrmReportes();
            reports.ShowDialog(this);
        }

        private void OpenUsers()
        {
            if (!string.Equals(_rolUsuario, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Solo un administrador puede gestionar usuarios.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var users = new FrmUsuarios(_usuarioId);
            users.ShowDialog(this);
        }

        private void Logout()
        {
            var result = MessageBox.Show(this, "¿Quieres cerrar la sesión actual?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OpenSales()
        {
            if (_usuarioId <= 0)
            {
                MessageBox.Show(this, "Cierra sesión y vuelve a entrar para registrar una venta con tu usuario.", "Usuario no identificado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sales = new FrmVentas(_usuarioId);
            if (sales.ShowDialog(this) == DialogResult.OK)
                LoadProducts();
        }
    }
}
