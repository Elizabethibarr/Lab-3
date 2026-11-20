namespace WinFormsApp3
{
    partial class FrmProductoModal
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.Label designerCodeLabel = null!;
        private System.Windows.Forms.Label designerNameLabel = null!;
        private System.Windows.Forms.Label designerCategoryLabel = null!;
        private System.Windows.Forms.Label designerPriceLabel = null!;
        private System.Windows.Forms.Label designerStockLabel = null!;
        private System.Windows.Forms.TextBox designerCode = null!;
        private System.Windows.Forms.TextBox designerName = null!;
        private System.Windows.Forms.TextBox designerDescription = null!;
        private System.Windows.Forms.ComboBox designerCategory = null!;
        private System.Windows.Forms.NumericUpDown designerPrice = null!;
        private System.Windows.Forms.NumericUpDown designerStock = null!;
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
            designerCodeLabel = new System.Windows.Forms.Label();
            designerNameLabel = new System.Windows.Forms.Label();
            designerCategoryLabel = new System.Windows.Forms.Label();
            designerPriceLabel = new System.Windows.Forms.Label();
            designerStockLabel = new System.Windows.Forms.Label();
            designerCode = new System.Windows.Forms.TextBox();
            designerName = new System.Windows.Forms.TextBox();
            designerDescription = new System.Windows.Forms.TextBox();
            designerCategory = new System.Windows.Forms.ComboBox();
            designerPrice = new System.Windows.Forms.NumericUpDown();
            designerStock = new System.Windows.Forms.NumericUpDown();
            designerSave = new System.Windows.Forms.Button();
            designerCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(570, 600);
            Text = "Producto | ElectroStore";
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(26, 22);
            designerTitle.Text = "Registrar producto";
            designerCodeLabel.Text = "Código";
            designerNameLabel.Text = "Nombre";
            designerCategoryLabel.Text = "Categoría";
            designerPriceLabel.Text = "Precio";
            designerStockLabel.Text = "Existencias";
            designerCodeLabel.Location = new System.Drawing.Point(30, 95);
            designerNameLabel.Location = new System.Drawing.Point(30, 160);
            designerCategoryLabel.Location = new System.Drawing.Point(30, 280);
            designerPriceLabel.Location = new System.Drawing.Point(30, 340);
            designerStockLabel.Location = new System.Drawing.Point(30, 400);
            designerCode.Location = new System.Drawing.Point(165, 88);
            designerCode.Size = new System.Drawing.Size(360, 32);
            designerName.Location = new System.Drawing.Point(165, 153);
            designerName.Size = new System.Drawing.Size(360, 32);
            designerDescription.Location = new System.Drawing.Point(165, 205);
            designerDescription.Size = new System.Drawing.Size(360, 52);
            designerDescription.Multiline = true;
            designerDescription.PlaceholderText = "Descripción";
            designerCategory.Location = new System.Drawing.Point(165, 273);
            designerCategory.Size = new System.Drawing.Size(360, 32);
            designerCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            designerPrice.Location = new System.Drawing.Point(165, 333);
            designerPrice.Size = new System.Drawing.Size(360, 32);
            designerPrice.DecimalPlaces = 2;
            designerStock.Location = new System.Drawing.Point(165, 393);
            designerStock.Size = new System.Drawing.Size(360, 32);
            designerSave.Location = new System.Drawing.Point(300, 510);
            designerSave.Size = new System.Drawing.Size(125, 40);
            designerSave.Text = "Guardar";
            designerCancel.Location = new System.Drawing.Point(435, 510);
            designerCancel.Size = new System.Drawing.Size(90, 40);
            designerCancel.Text = "Cancelar";
            designerCancel.Text = "Cancelar";
            Controls.Add(designerTitle);
            Controls.Add(designerCodeLabel);
            Controls.Add(designerNameLabel);
            Controls.Add(designerCategoryLabel);
            Controls.Add(designerPriceLabel);
            Controls.Add(designerStockLabel);
            Controls.Add(designerCode);
            Controls.Add(designerName);
            Controls.Add(designerDescription);
            Controls.Add(designerCategory);
            Controls.Add(designerPrice);
            Controls.Add(designerStock);
            Controls.Add(designerSave);
            Controls.Add(designerCancel);
            ResumeLayout(false);
        }
    }
}
