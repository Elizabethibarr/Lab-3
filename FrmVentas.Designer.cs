namespace WinFormsApp3
{
    partial class FrmVentas
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.DataGridView designerCatalog = null!;
        private System.Windows.Forms.DataGridView designerCart = null!;
        private System.Windows.Forms.NumericUpDown designerQuantity = null!;
        private System.Windows.Forms.Button designerAdd = null!;
        private System.Windows.Forms.Button designerRemove = null!;
        private System.Windows.Forms.Label designerTotal = null!;
        private System.Windows.Forms.Button designerRegisterSale = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerTitle = new System.Windows.Forms.Label();
            designerCatalog = new System.Windows.Forms.DataGridView();
            designerCart = new System.Windows.Forms.DataGridView();
            designerQuantity = new System.Windows.Forms.NumericUpDown();
            designerAdd = new System.Windows.Forms.Button();
            designerRemove = new System.Windows.Forms.Button();
            designerTotal = new System.Windows.Forms.Label();
            designerRegisterSale = new System.Windows.Forms.Button();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(1100, 760);
            Text = "Nueva venta | ElectroStore";
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(22, 18);
            designerTitle.Text = "Nueva venta";
            designerCatalog.AllowUserToAddRows = false;
            designerCatalog.ColumnHeadersHeight = 38;
            designerCatalog.Location = new System.Drawing.Point(22, 80);
            designerCatalog.Size = new System.Drawing.Size(1050, 280);
            designerCatalog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            designerQuantity.Location = new System.Drawing.Point(22, 380);
            designerQuantity.Minimum = 1;
            designerQuantity.Value = 1;
            designerQuantity.Width = 100;
            designerAdd.Location = new System.Drawing.Point(140, 375);
            designerAdd.Size = new System.Drawing.Size(150, 36);
            designerAdd.Text = "Agregar al carrito";
            designerRemove.Location = new System.Drawing.Point(300, 375);
            designerRemove.Size = new System.Drawing.Size(150, 36);
            designerRemove.Text = "Quitar seleccionado";
            designerCart.AllowUserToAddRows = false;
            designerCart.ColumnHeadersHeight = 38;
            designerCart.Location = new System.Drawing.Point(22, 430);
            designerCart.Size = new System.Drawing.Size(1050, 240);
            designerCart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            designerTotal.AutoSize = true;
            designerTotal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            designerTotal.Location = new System.Drawing.Point(650, 690);
            designerTotal.Text = "Total: $0.00";
            designerRegisterSale.Location = new System.Drawing.Point(870, 680);
            designerRegisterSale.Size = new System.Drawing.Size(200, 46);
            designerRegisterSale.Text = "Registrar venta";
            Controls.Add(designerTitle);
            Controls.Add(designerCatalog);
            Controls.Add(designerQuantity);
            Controls.Add(designerAdd);
            Controls.Add(designerRemove);
            Controls.Add(designerCart);
            Controls.Add(designerTotal);
            Controls.Add(designerRegisterSale);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
