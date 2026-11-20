using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using WinFormsApp3.Models;

namespace WinFormsApp3
{
    public partial class FrmUsuarios : Form
    {
        private readonly int _currentUserId;
        private readonly DataGridView _grid = UiTheme.Grid();
        private readonly TextBox _search = UiTheme.TextBox("Buscar por usuario o nombre");
        private readonly Label _status = UiTheme.Label("", 9, color: UiTheme.Muted);

        public FrmUsuarios() : this(0) { }

        public FrmUsuarios(int currentUserId)
        {
            _currentUserId = currentUserId;
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
                LoadUsers();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface()
        {
            Text = "Usuarios | ElectroStore";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(900, 600);
            ClientSize = new Size(1050, 700);
            BackColor = UiTheme.Background;
            Padding = new Padding(24);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            var heading = new Panel { Dock = DockStyle.Fill };
            var title = UiTheme.Label("Administración de usuarios", 21, true);
            title.Location = new Point(0, 0);
            heading.Controls.Add(title);
            var subtitle = UiTheme.Label("Gestiona las cuentas, roles y acceso al sistema.", 9, color: UiTheme.Muted);
            subtitle.Location = new Point(1, 34);
            heading.Controls.Add(subtitle);
            layout.Controls.Add(heading, 0, 0);

            var toolbar = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 1, Padding = new Padding(0, 6, 0, 6) };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
            _search.Dock = DockStyle.Fill;
            _search.Margin = new Padding(0, 3, 10, 3);
            _search.TextChanged += (_, _) => LoadUsers();
            var refresh = ToolbarButton("Actualizar");
            refresh.Click += (_, _) => LoadUsers();
            var add = ToolbarButton("＋  Nuevo usuario", true);
            add.Click += (_, _) => OpenUserEditor(null);
            var edit = ToolbarButton("Editar");
            edit.Click += (_, _) => EditSelected();
            var delete = ToolbarButton("Eliminar");
            delete.ForeColor = Color.Firebrick;
            delete.Click += (_, _) => DeleteSelected();
            toolbar.Controls.Add(_search, 0, 0);
            toolbar.Controls.Add(refresh, 1, 0);
            toolbar.Controls.Add(add, 2, 0);
            toolbar.Controls.Add(edit, 3, 0);
            toolbar.Controls.Add(delete, 4, 0);
            layout.Controls.Add(toolbar, 0, 1);

            var gridCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), BorderStyle = BorderStyle.FixedSingle };
            _grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditSelected(); };
            gridCard.Controls.Add(_grid);
            layout.Controls.Add(gridCard, 0, 2);
            layout.Controls.Add(_status, 0, 3);
            Controls.Add(layout);

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Editar usuario", null, (_, _) => EditSelected());
            contextMenu.Items.Add("Activar / desactivar", null, (_, _) => ToggleSelected());
            _grid.ContextMenuStrip = contextMenu;
        }

        private static Button ToolbarButton(string text, bool primary = false)
        {
            var button = UiTheme.Button(text, primary);
            button.AutoSize = false;
            button.MinimumSize = Size.Empty;
            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(3);
            button.Padding = new Padding(5, 2, 5, 2);
            return button;
        }

        private void LoadUsers()
        {
            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("SELECT IdUsuario, NombreUsuario, NombreCompleto, Rol, Activo, FechaCreacion FROM dbo.Usuarios WHERE (@filtro = N'' OR NombreUsuario LIKE @buscar OR NombreCompleto LIKE @buscar) ORDER BY NombreCompleto", connection);
                var filter = _search.Text.Trim();
                command.Parameters.Add("@filtro", SqlDbType.NVarChar, 120).Value = filter;
                command.Parameters.Add("@buscar", SqlDbType.NVarChar, 122).Value = "%" + filter + "%";
                var table = new DataTable();
                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                var users = table.AsEnumerable().Select(row => new UsuarioListRow
                {
                    IdUsuario = row.Field<int>("IdUsuario"),
                    NombreUsuario = row.Field<string>("NombreUsuario") ?? string.Empty,
                    NombreCompleto = row.Field<string>("NombreCompleto") ?? string.Empty,
                    Rol = row.Field<string>("Rol") ?? string.Empty,
                    Activo = row.Field<bool>("Activo"),
                    FechaCreacion = row.Field<DateTime>("FechaCreacion")
                }).ToList();
                _grid.DataSource = users;
                _grid.Columns["IdUsuario"].Visible = false;
                _grid.Columns["NombreUsuario"].HeaderText = "Usuario";
                _grid.Columns["NombreCompleto"].HeaderText = "Nombre completo";
                _grid.Columns["Rol"].HeaderText = "Rol";
                _grid.Columns["Activo"].HeaderText = "Activo";
                _grid.Columns["FechaCreacion"].HeaderText = "Fecha de alta";
                _grid.Columns["FechaCreacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
                _status.Text = $"Usuarios encontrados: {table.Rows.Count}  •  Doble clic o clic derecho para editar.";
            }
            catch (Exception ex)
            {
                _status.Text = "No se pudo cargar la lista de usuarios.";
                MessageBox.Show(this, "No se pudo cargar la lista. Verifica que dbo.Usuarios tenga la columna Rol; ejecuta Database\\ModuloUsuarios.sql.\n\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSelected()
        {
            if (_grid.CurrentRow is null) return;
            var row = _grid.CurrentRow;
            var user = new UserRecord(
                Convert.ToInt32(row.Cells["IdUsuario"].Value),
                Convert.ToString(row.Cells["NombreUsuario"].Value) ?? string.Empty,
                Convert.ToString(row.Cells["NombreCompleto"].Value) ?? string.Empty,
                Convert.ToString(row.Cells["Rol"].Value) ?? "Vendedor",
                Convert.ToBoolean(row.Cells["Activo"].Value));
            OpenUserEditor(user);
        }

        private void OpenUserEditor(UserRecord? user)
        {
            using var editor = user is null
                ? new FrmUsuarioModal()
                : new FrmUsuarioModal(user.Id, user.Username, user.FullName, user.Role, user.Active);
            if (editor.ShowDialog(this) == DialogResult.OK)
                LoadUsers();
        }

        private void DeleteSelected()
        {
            if (_grid.CurrentRow is null)
            {
                MessageBox.Show(this, "Selecciona un usuario de la lista.", "Usuario no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = Convert.ToInt32(_grid.CurrentRow.Cells["IdUsuario"].Value);
            if (_currentUserId > 0 && id == _currentUserId)
            {
                MessageBox.Show(this, "No puedes eliminar la cuenta con la que iniciaste sesión.", "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var username = Convert.ToString(_grid.CurrentRow.Cells["NombreUsuario"].Value) ?? "el usuario seleccionado";
            if (MessageBox.Show(this, $"¿Desactivar la cuenta de {username}? Se conservará su historial de ventas, pero ya no podrá iniciar sesión.", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("UPDATE dbo.Usuarios SET Activo = 0 WHERE IdUsuario = @id AND Activo = 1", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                connection.Open();
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("El usuario ya no existe o ya estaba desactivado.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo eliminar el usuario.\\n\\n" + ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleSelected()
        {
            if (_grid.CurrentRow is null) return;
            var id = Convert.ToInt32(_grid.CurrentRow.Cells["IdUsuario"].Value);
            var active = Convert.ToBoolean(_grid.CurrentRow.Cells["Activo"].Value);
            var username = Convert.ToString(_grid.CurrentRow.Cells["NombreUsuario"].Value) ?? string.Empty;
            if (MessageBox.Show(this, $"¿Deseas {(active ? "desactivar" : "activar")} al usuario {username}?", "Confirmar cambio", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                using var command = new SqlCommand("UPDATE dbo.Usuarios SET Activo = @activo WHERE IdUsuario = @id", connection);
                command.Parameters.Add("@activo", SqlDbType.Bit).Value = !active;
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                connection.Open();
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("No se encontró el usuario que se intentó actualizar.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo cambiar el estado del usuario.\n\n" + ex.Message, "Error al actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private sealed record UserRecord(int Id, string Username, string FullName, string Role, bool Active);
    }
}
