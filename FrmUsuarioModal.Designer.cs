namespace WinFormsApp3
{
    partial class FrmUsuarioModal
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.Label designerUsernameLabel = null!;
        private System.Windows.Forms.Label designerNameLabel = null!;
        private System.Windows.Forms.Label designerRoleLabel = null!;
        private System.Windows.Forms.Label designerPasswordLabel = null!;
        private System.Windows.Forms.TextBox designerUsername = null!;
        private System.Windows.Forms.TextBox designerName = null!;
        private System.Windows.Forms.TextBox designerPassword = null!;
        private System.Windows.Forms.ComboBox designerRole = null!;
        private System.Windows.Forms.CheckBox designerActive = null!;
        private System.Windows.Forms.Button designerSave = null!;
        private System.Windows.Forms.Button designerCancel = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerTitle = new System.Windows.Forms.Label();
            designerUsernameLabel = new System.Windows.Forms.Label();
            designerNameLabel = new System.Windows.Forms.Label();
            designerRoleLabel = new System.Windows.Forms.Label();
            designerPasswordLabel = new System.Windows.Forms.Label();
            designerUsername = new System.Windows.Forms.TextBox();
            designerName = new System.Windows.Forms.TextBox();
            designerPassword = new System.Windows.Forms.TextBox();
            designerRole = new System.Windows.Forms.ComboBox();
            designerActive = new System.Windows.Forms.CheckBox();
            designerSave = new System.Windows.Forms.Button();
            designerCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(540, 470);
            Text = "Usuario | ElectroStore";
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(24, 22);
            designerTitle.Text = "Crear cuenta de usuario";
            designerUsernameLabel.Text = "Usuario";
            designerNameLabel.Text = "Nombre completo";
            designerRoleLabel.Text = "Rol";
            designerPasswordLabel.Text = "Contraseña";
            designerUsernameLabel.Location = new System.Drawing.Point(28, 95);
            designerUsernameLabel.Size = new System.Drawing.Size(130, 25);
            designerNameLabel.Location = new System.Drawing.Point(28, 155);
            designerNameLabel.Size = new System.Drawing.Size(130, 25);
            designerRoleLabel.Location = new System.Drawing.Point(28, 215);
            designerRoleLabel.Size = new System.Drawing.Size(130, 25);
            designerPasswordLabel.Location = new System.Drawing.Point(28, 275);
            designerPasswordLabel.Size = new System.Drawing.Size(130, 25);
            designerUsername.Location = new System.Drawing.Point(170, 90);
            designerUsername.Size = new System.Drawing.Size(330, 32);
            designerName.Location = new System.Drawing.Point(170, 150);
            designerName.Size = new System.Drawing.Size(330, 32);
            designerRole.Location = new System.Drawing.Point(170, 210);
            designerRole.Size = new System.Drawing.Size(330, 32);
            designerRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            designerRole.Items.AddRange(new object[] { "Administrador", "Vendedor" });
            designerPassword.Location = new System.Drawing.Point(170, 270);
            designerPassword.Size = new System.Drawing.Size(330, 32);
            designerPassword.UseSystemPasswordChar = true;
            designerPassword.PlaceholderText = "Contraseña segura";
            designerActive.Location = new System.Drawing.Point(170, 320);
            designerActive.Text = "Cuenta activa";
            designerSave.Location = new System.Drawing.Point(270, 390);
            designerSave.Size = new System.Drawing.Size(110, 40);
            designerSave.Text = "Guardar";
            designerCancel.Location = new System.Drawing.Point(390, 390);
            designerCancel.Size = new System.Drawing.Size(110, 40);
            designerCancel.Text = "Cancelar";
            Controls.Add(designerTitle);
            Controls.Add(designerUsernameLabel);
            Controls.Add(designerNameLabel);
            Controls.Add(designerRoleLabel);
            Controls.Add(designerPasswordLabel);
            Controls.Add(designerUsername);
            Controls.Add(designerName);
            Controls.Add(designerRole);
            Controls.Add(designerPassword);
            Controls.Add(designerActive);
            Controls.Add(designerSave);
            Controls.Add(designerCancel);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
