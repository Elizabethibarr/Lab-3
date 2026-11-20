using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using WinFormsApp3.Services;

namespace WinFormsApp3
{
    public partial class FrmUsuarioModal : Form
    {
        private readonly int? _userId;
        private readonly TextBox _username = UiTheme.TextBox("Ej. jperez");
        private readonly TextBox _fullName = UiTheme.TextBox("Nombre y apellido");
        private readonly TextBox _password = UiTheme.TextBox("Contraseña segura");
        private readonly ComboBox _role = new() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
        private readonly CheckBox _active = new() { Text = "Cuenta activa", Checked = true, AutoSize = true, Font = new Font("Segoe UI", 9.5f), ForeColor = UiTheme.Text };
        private string _initialUsername = string.Empty;
        private string _initialFullName = string.Empty;
        private string _initialRole = "Vendedor";
        private bool _initialActive = true;

        public FrmUsuarioModal() : this(null, string.Empty, string.Empty, "Vendedor", true) { }

        public FrmUsuarioModal(int? userId, string username = "", string fullName = "", string role = "Vendedor", bool active = true)
        {
            _userId = userId;
            _initialUsername = username;
            _initialFullName = fullName;
            _initialRole = role;
            _initialActive = active;
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface() => BuildInterface(_initialUsername, _initialFullName, _initialRole, _initialActive);

        private void BuildInterface(string username, string fullName, string role, bool active)
        {
            Text = _userId.HasValue ? "Editar usuario | ElectroStore" : "Nuevo usuario | ElectroStore";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(540, 470);
            BackColor = Color.White;
            Padding = new Padding(24);

            _username.Text = username;
            _username.ReadOnly = _userId.HasValue;
            _fullName.Text = fullName;
            _password.UseSystemPasswordChar = true;
            _password.PlaceholderText = _userId.HasValue ? "Deja vacío para conservar la actual" : "Contraseña (mínimo 8 caracteres)";
            _active.Checked = active;
            _role.Items.AddRange(new object[] { "Administrador", "Vendedor" });
            _role.SelectedItem = _role.Items.Contains(role) ? role : "Vendedor";

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 8 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            for (var index = 1; index < 6; index++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));

            var title = UiTheme.Label(_userId.HasValue ? "Editar cuenta" : "Crear cuenta", 19, true);
            layout.Controls.Add(title, 0, 0);
            layout.SetColumnSpan(title, 2);
            AddField(layout, "Usuario", _username, 1);
            AddField(layout, "Nombre completo", _fullName, 2);
            AddField(layout, "Contraseña", _password, 3);
            AddField(layout, "Rol", _role, 4);
            var activeLabel = UiTheme.Label("Estado", 9, true, UiTheme.Muted);
            activeLabel.Anchor = AnchorStyles.Left;
            layout.Controls.Add(activeLabel, 0, 5);
            _active.Anchor = AnchorStyles.Left;
            layout.Controls.Add(_active, 1, 5);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var save = UiTheme.Button("Guardar", true);
            var cancel = UiTheme.Button("Cancelar");
            save.Click += SaveClicked;
            cancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
            actions.Controls.Add(save);
            actions.Controls.Add(cancel);
            layout.Controls.Add(actions, 0, 7);
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
            input.Margin = new Padding(0, 7, 0, 7);
            layout.Controls.Add(label, 0, row);
            layout.Controls.Add(input, 1, row);
        }

        private void SaveClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_username.Text) || string.IsNullOrWhiteSpace(_fullName.Text) || _role.SelectedItem is null)
            {
                MessageBox.Show(this, "Completa usuario, nombre completo y rol.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_userId.HasValue && _password.Text.Length < 8)
            {
                MessageBox.Show(this, "La contraseña debe tener al menos 8 caracteres.", "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_userId.HasValue && _password.Text.Length > 0 && _password.Text.Length < 8)
            {
                MessageBox.Show(this, "La nueva contraseña debe tener al menos 8 caracteres.", "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var connection = new SqlConnection(WinFormsApp3.Services.AppSettings.ConnectionString);
                SqlCommand command;
                if (_userId.HasValue)
                {
                    command = new SqlCommand(_password.Text.Length == 0
                        ? "UPDATE dbo.Usuarios SET NombreCompleto = @nombre, Rol = @rol, Activo = @activo WHERE IdUsuario = @id"
                        : "UPDATE dbo.Usuarios SET NombreCompleto = @nombre, Rol = @rol, Activo = @activo, HashContrasena = @hash WHERE IdUsuario = @id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = _userId.Value;
                    if (_password.Text.Length > 0)
                        command.Parameters.Add("@hash", SqlDbType.Char, 64).Value = PasswordHasher.Hash(_password.Text);
                }
                else
                {
                    command = new SqlCommand("INSERT INTO dbo.Usuarios (NombreUsuario, HashContrasena, NombreCompleto, Rol, Activo) VALUES (@usuario, @hash, @nombre, @rol, @activo)", connection);
                    command.Parameters.Add("@usuario", SqlDbType.NVarChar, 50).Value = _username.Text.Trim();
                    command.Parameters.Add("@hash", SqlDbType.Char, 64).Value = PasswordHasher.Hash(_password.Text);
                }

                using (command)
                {
                    command.Parameters.Add("@nombre", SqlDbType.NVarChar, 120).Value = _fullName.Text.Trim();
                    command.Parameters.Add("@rol", SqlDbType.NVarChar, 50).Value = Convert.ToString(_role.SelectedItem)!;
                    command.Parameters.Add("@activo", SqlDbType.Bit).Value = _active.Checked;
                    connection.Open();
                    if (command.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("No se pudo encontrar o guardar el usuario.");
                }

                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                MessageBox.Show(this, "Ese nombre de usuario ya está registrado.", "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo guardar el usuario.\n\n" + ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
