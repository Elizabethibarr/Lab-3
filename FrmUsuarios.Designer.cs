namespace WinFormsApp3
{
    partial class FrmUsuarios
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.TextBox designerSearch = null!;
        private System.Windows.Forms.Button designerRefresh = null!;
        private System.Windows.Forms.Button designerAdd = null!;
        private System.Windows.Forms.Button designerEdit = null!;
        private System.Windows.Forms.Button designerDelete = null!;
        private System.Windows.Forms.DataGridView designerUsers = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerTitle = new System.Windows.Forms.Label();
            designerSearch = new System.Windows.Forms.TextBox();
            designerRefresh = new System.Windows.Forms.Button();
            designerAdd = new System.Windows.Forms.Button();
            designerEdit = new System.Windows.Forms.Button();
            designerDelete = new System.Windows.Forms.Button();
            designerUsers = new System.Windows.Forms.DataGridView();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(1050, 700);
            Text = "Usuarios | ElectroStore";
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(24, 22);
            designerTitle.Text = "Administración de usuarios";
            designerSearch.Location = new System.Drawing.Point(24, 94);
            designerSearch.Size = new System.Drawing.Size(400, 31);
            designerSearch.PlaceholderText = "Buscar por usuario o nombre";
            designerRefresh.Location = new System.Drawing.Point(440, 91);
            designerRefresh.Size = new System.Drawing.Size(120, 36);
            designerRefresh.Text = "Actualizar";
            designerAdd.Location = new System.Drawing.Point(570, 91);
            designerAdd.Size = new System.Drawing.Size(150, 36);
            designerAdd.Text = "Nuevo usuario";
            designerEdit.Location = new System.Drawing.Point(730, 91);
            designerEdit.Size = new System.Drawing.Size(120, 36);
            designerEdit.Text = "Editar";
            designerDelete.Location = new System.Drawing.Point(860, 91);
            designerDelete.Size = new System.Drawing.Size(140, 36);
            designerDelete.Text = "Eliminar";
            designerUsers.AllowUserToAddRows = false;
            designerUsers.ColumnHeadersHeight = 38;
            designerUsers.Location = new System.Drawing.Point(24, 150);
            designerUsers.Size = new System.Drawing.Size(1000, 510);
            designerUsers.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Controls.Add(designerTitle);
            Controls.Add(designerSearch);
            Controls.Add(designerRefresh);
            Controls.Add(designerAdd);
            Controls.Add(designerEdit);
            Controls.Add(designerDelete);
            Controls.Add(designerUsers);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
