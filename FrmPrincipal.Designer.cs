namespace WinFormsApp3
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Panel designerSidebar = null!;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.Label designerUser = null!;
        private System.Windows.Forms.Button designerInventory = null!;
        private System.Windows.Forms.Button designerSales = null!;
        private System.Windows.Forms.Button designerReports = null!;
        private System.Windows.Forms.Button designerUsers = null!;
        private System.Windows.Forms.TextBox designerSearch = null!;
        private System.Windows.Forms.DataGridView designerProducts = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerSidebar = new System.Windows.Forms.Panel();
            designerTitle = new System.Windows.Forms.Label();
            designerUser = new System.Windows.Forms.Label();
            designerInventory = new System.Windows.Forms.Button();
            designerSales = new System.Windows.Forms.Button();
            designerReports = new System.Windows.Forms.Button();
            designerUsers = new System.Windows.Forms.Button();
            designerSearch = new System.Windows.Forms.TextBox();
            designerProducts = new System.Windows.Forms.DataGridView();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(1200, 780);
            Text = "ElectroStore | Panel principal";
            designerSidebar.BackColor = System.Drawing.Color.FromArgb(22, 38, 67);
            designerSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            designerSidebar.Width = 218;
            designerInventory.Location = new System.Drawing.Point(18, 90);
            designerInventory.Size = new System.Drawing.Size(180, 40);
            designerInventory.Text = "Inventario";
            designerSales.Location = new System.Drawing.Point(18, 140);
            designerSales.Size = new System.Drawing.Size(180, 40);
            designerSales.Text = "Nueva venta";
            designerReports.Location = new System.Drawing.Point(18, 190);
            designerReports.Size = new System.Drawing.Size(180, 40);
            designerReports.Text = "Reportes";
            designerUsers.Location = new System.Drawing.Point(18, 240);
            designerUsers.Size = new System.Drawing.Size(180, 40);
            designerUsers.Text = "Usuarios";
            designerSidebar.Controls.Add(designerInventory);
            designerSidebar.Controls.Add(designerSales);
            designerSidebar.Controls.Add(designerReports);
            designerSidebar.Controls.Add(designerUsers);
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(255, 24);
            designerTitle.Text = "Inventario de productos";
            designerUser.AutoSize = true;
            designerUser.Location = new System.Drawing.Point(950, 32);
            designerUser.Text = "Usuario | Rol";
            designerSearch.Location = new System.Drawing.Point(260, 100);
            designerSearch.Size = new System.Drawing.Size(450, 31);
            designerSearch.PlaceholderText = "Buscar por nombre o código";
            designerProducts.AllowUserToAddRows = false;
            designerProducts.ColumnHeadersHeight = 38;
            designerProducts.Location = new System.Drawing.Point(260, 155);
            designerProducts.Size = new System.Drawing.Size(900, 560);
            designerProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Controls.Add(designerProducts);
            Controls.Add(designerSearch);
            Controls.Add(designerUser);
            Controls.Add(designerTitle);
            Controls.Add(designerSidebar);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
