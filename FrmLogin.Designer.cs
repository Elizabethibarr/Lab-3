namespace WinFormsApp3
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Panel designerBrandPanel = null!;
        private System.Windows.Forms.Panel designerLoginPanel = null!;
        private System.Windows.Forms.Label designerBrandTitle = null!;
        private System.Windows.Forms.Label designerBrandCaption = null!;
        private System.Windows.Forms.Label designerLoginTitle = null!;
        private System.Windows.Forms.Label designerUserLabel = null!;
        private System.Windows.Forms.Label designerPasswordLabel = null!;
        private System.Windows.Forms.TextBox designerUsername = null!;
        private System.Windows.Forms.TextBox designerPassword = null!;
        private System.Windows.Forms.CheckBox designerShowPassword = null!;
        private System.Windows.Forms.Button designerLoginButton = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerBrandPanel = new System.Windows.Forms.Panel();
            designerLoginPanel = new System.Windows.Forms.Panel();
            designerBrandTitle = new System.Windows.Forms.Label();
            designerBrandCaption = new System.Windows.Forms.Label();
            designerLoginTitle = new System.Windows.Forms.Label();
            designerUserLabel = new System.Windows.Forms.Label();
            designerPasswordLabel = new System.Windows.Forms.Label();
            designerUsername = new System.Windows.Forms.TextBox();
            designerPassword = new System.Windows.Forms.TextBox();
            designerShowPassword = new System.Windows.Forms.CheckBox();
            designerLoginButton = new System.Windows.Forms.Button();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(980, 600);
            Text = "ElectroStore | Inicio de sesión";
            designerBrandPanel.BackColor = System.Drawing.Color.FromArgb(22, 38, 67);
            designerBrandPanel.Controls.Add(designerBrandTitle);
            designerBrandPanel.Controls.Add(designerBrandCaption);
            designerBrandPanel.Dock = System.Windows.Forms.DockStyle.Left;
            designerBrandPanel.Width = 420;
            designerBrandTitle.AutoSize = true;
            designerBrandTitle.Font = new System.Drawing.Font("Segoe UI", 27F, System.Drawing.FontStyle.Bold);
            designerBrandTitle.ForeColor = System.Drawing.Color.White;
            designerBrandTitle.Location = new System.Drawing.Point(48, 180);
            designerBrandTitle.Text = "ELECTRO\nSTORE";
            designerBrandCaption.AutoSize = true;
            designerBrandCaption.Font = new System.Drawing.Font("Segoe UI", 13F);
            designerBrandCaption.ForeColor = System.Drawing.Color.White;
            designerBrandCaption.Location = new System.Drawing.Point(50, 290);
            designerBrandCaption.Text = "Ventas e inventario,\nsin complicaciones.";
            designerLoginPanel.BackColor = System.Drawing.Color.White;
            designerLoginPanel.Controls.Add(designerLoginTitle);
            designerLoginPanel.Controls.Add(designerUserLabel);
            designerLoginPanel.Controls.Add(designerUsername);
            designerLoginPanel.Controls.Add(designerPasswordLabel);
            designerLoginPanel.Controls.Add(designerPassword);
            designerLoginPanel.Controls.Add(designerShowPassword);
            designerLoginPanel.Controls.Add(designerLoginButton);
            designerLoginPanel.Location = new System.Drawing.Point(510, 85);
            designerLoginPanel.Size = new System.Drawing.Size(390, 430);
            designerLoginTitle.AutoSize = true;
            designerLoginTitle.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold);
            designerLoginTitle.Location = new System.Drawing.Point(34, 30);
            designerLoginTitle.Text = "Bienvenido";
            designerUserLabel.AutoSize = true;
            designerUserLabel.Location = new System.Drawing.Point(35, 105);
            designerUserLabel.Text = "USUARIO";
            designerUsername.Location = new System.Drawing.Point(35, 130);
            designerUsername.Size = new System.Drawing.Size(320, 31);
            designerUsername.PlaceholderText = "Tu usuario";
            designerPasswordLabel.AutoSize = true;
            designerPasswordLabel.Location = new System.Drawing.Point(35, 180);
            designerPasswordLabel.Text = "CONTRASEÑA";
            designerPassword.Location = new System.Drawing.Point(35, 205);
            designerPassword.Size = new System.Drawing.Size(320, 31);
            designerPassword.UseSystemPasswordChar = true;
            designerPassword.PlaceholderText = "Tu contraseña";
            designerShowPassword.AutoSize = true;
            designerShowPassword.Location = new System.Drawing.Point(35, 250);
            designerShowPassword.Text = "Mostrar contraseña";
            designerLoginButton.BackColor = System.Drawing.Color.FromArgb(43, 102, 210);
            designerLoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            designerLoginButton.ForeColor = System.Drawing.Color.White;
            designerLoginButton.Location = new System.Drawing.Point(35, 300);
            designerLoginButton.Size = new System.Drawing.Size(320, 48);
            designerLoginButton.Text = "Iniciar sesión";
            Controls.Add(designerLoginPanel);
            Controls.Add(designerBrandPanel);
            ResumeLayout(false);
        }
    }
}
