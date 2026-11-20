using System.ComponentModel;
using WinFormsApp3.Services;

namespace WinFormsApp3
{
    public partial class FrmLogin : Form
    {
        private readonly AuthenticationService _authenticationService = new();
        private readonly TextBox _username = UiTheme.TextBox("Tu usuario");
        private readonly TextBox _password = UiTheme.TextBox("Tu contraseña");
        private readonly CheckBox _showPassword = new() { Text = "Mostrar contraseña", AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = UiTheme.Muted, Margin = new Padding(0, 0, 0, 8) };
        private readonly Label _message = UiTheme.Label("Accede con tu cuenta de empleado.", 9, color: UiTheme.Muted);

        public FrmLogin()
        {
            InitializeComponent();
            if (!IsDesignTime())
            {
                UiTheme.ClearDesignerPreview(this);
                BuildInterface();
            }
        }

        private bool IsDesignTime() => LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

        private void BuildInterface()
        {
            Text = "ElectroStore | Inicio de sesión";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(780, 520);
            ClientSize = new Size(980, 600);
            BackColor = UiTheme.Background;
            Font = new Font("Segoe UI", 10);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57));

            var brand = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Navy, Padding = new Padding(48) };
            var brandContent = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
            brandContent.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
            brandContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            brandContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            brandContent.RowStyles.Add(new RowStyle(SizeType.Percent, 66));
            brandContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            brandContent.Controls.Add(UiTheme.Label("ELECTRO\nSTORE", 27, true, Color.White), 0, 1);
            brandContent.Controls.Add(UiTheme.Label("Ventas e inventario,\nsin complicaciones.", 13, color: Color.FromArgb(202, 215, 237)), 0, 2);
            brandContent.Controls.Add(UiTheme.Label("PLATAFORMA DE GESTIÓN  •  v1.0", 8, true, Color.FromArgb(159, 181, 216)), 0, 4);
            brand.Controls.Add(brandContent);

            var right = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Background };
            var card = new Panel { Size = new Size(390, 420), BackColor = Color.White, Padding = new Padding(34), Anchor = AnchorStyles.None, BorderStyle = BorderStyle.FixedSingle };
            var center = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1 };
            center.Controls.Add(card, 0, 0);
            center.Layout += (_, _) => card.Location = new Point((center.ClientSize.Width - card.Width) / 2, (center.ClientSize.Height - card.Height) / 2);

            var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 9 };
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            form.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            form.Controls.Add(UiTheme.Label("Bienvenido", 23, true), 0, 0);
            form.Controls.Add(UiTheme.Label("Inicia sesión para continuar", 10, color: UiTheme.Muted), 0, 1);
            form.Controls.Add(UiTheme.Label("USUARIO", 8, true, UiTheme.Muted), 0, 2);
            form.Controls.Add(_username, 0, 3);
            form.Controls.Add(UiTheme.Label("CONTRASEÑA", 8, true, UiTheme.Muted), 0, 4);
            _password.UseSystemPasswordChar = true;
            form.Controls.Add(_password, 0, 5);
            _showPassword.CheckedChanged += (_, _) => _password.UseSystemPasswordChar = !_showPassword.Checked;
            form.Controls.Add(_showPassword, 0, 6);
            var login = UiTheme.Button("Iniciar sesión", true);
            login.Dock = DockStyle.Fill;
            login.Click += LoginClicked;
            form.Controls.Add(login, 0, 7);
            form.Controls.Add(_message, 0, 8);
            card.Controls.Add(form);
            right.Controls.Add(center);
            layout.Controls.Add(brand, 0, 0);
            layout.Controls.Add(right, 1, 0);
            Controls.Add(layout);
            AcceptButton = login;
        }

        private void LoginClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_username.Text) || string.IsNullOrEmpty(_password.Text))
            {
                _message.Text = "Escribe tu usuario y contraseña.";
                _message.ForeColor = Color.Firebrick;
                return;
            }

            try
            {
                var usuario = _authenticationService.Authenticate(_username.Text, _password.Text);
                if (usuario is null)
                {
                    _message.Text = "Usuario o contraseña incorrectos.";
                    _message.ForeColor = Color.Firebrick;
                    return;
                }

                using var principal = new FrmPrincipal(usuario.IdUsuario, usuario.NombreCompleto, usuario.Rol);
                Hide();
                principal.ShowDialog();
                _password.Clear();
                Show();
                Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "No se pudo validar el acceso. Ejecuta Database\\Laboratorio3.sql y verifica la conexión SQL Server.\n\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
