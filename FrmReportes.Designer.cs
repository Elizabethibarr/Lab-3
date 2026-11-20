namespace WinFormsApp3
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.Label designerTitle = null!;
        private System.Windows.Forms.DateTimePicker designerStart = null!;
        private System.Windows.Forms.DateTimePicker designerEnd = null!;
        private System.Windows.Forms.Button designerRefresh = null!;
        private System.Windows.Forms.Button designerCsv = null!;
        private System.Windows.Forms.Button designerExcel = null!;
        private System.Windows.Forms.Button designerPdf = null!;
        private System.Windows.Forms.TabControl designerTabs = null!;
        private System.Windows.Forms.DataGridView designerStockGrid = null!;
        private System.Windows.Forms.DataGridView designerSalesGrid = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            designerTitle = new System.Windows.Forms.Label();
            designerStart = new System.Windows.Forms.DateTimePicker();
            designerEnd = new System.Windows.Forms.DateTimePicker();
            designerRefresh = new System.Windows.Forms.Button();
            designerCsv = new System.Windows.Forms.Button();
            designerExcel = new System.Windows.Forms.Button();
            designerPdf = new System.Windows.Forms.Button();
            designerTabs = new System.Windows.Forms.TabControl();
            designerStockGrid = new System.Windows.Forms.DataGridView();
            designerSalesGrid = new System.Windows.Forms.DataGridView();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(1120, 760);
            Text = "Reportes | ElectroStore";
            designerTitle.AutoSize = true;
            designerTitle.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Bold);
            designerTitle.Location = new System.Drawing.Point(24, 20);
            designerTitle.Text = "Reportes de ventas e inventario";
            designerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            designerStart.Location = new System.Drawing.Point(26, 90);
            designerStart.Width = 150;
            designerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            designerEnd.Location = new System.Drawing.Point(190, 90);
            designerEnd.Width = 150;
            designerRefresh.Location = new System.Drawing.Point(370, 88);
            designerRefresh.Size = new System.Drawing.Size(120, 34);
            designerRefresh.Text = "Actualizar";
            designerCsv.Location = new System.Drawing.Point(26, 145);
            designerCsv.Size = new System.Drawing.Size(110, 34);
            designerCsv.Text = "Exportar CSV";
            designerExcel.Location = new System.Drawing.Point(145, 145);
            designerExcel.Size = new System.Drawing.Size(110, 34);
            designerExcel.Text = "Exportar Excel";
            designerPdf.Location = new System.Drawing.Point(264, 145);
            designerPdf.Size = new System.Drawing.Size(110, 34);
            designerPdf.Text = "Exportar PDF";
            designerTabs.Location = new System.Drawing.Point(24, 200);
            designerTabs.Size = new System.Drawing.Size(1070, 530);
            designerTabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            var stockPage = new System.Windows.Forms.TabPage("Existencias");
            var salesPage = new System.Windows.Forms.TabPage("Productos vendidos");
            designerStockGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            designerStockGrid.AllowUserToAddRows = false;
            designerSalesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            designerSalesGrid.AllowUserToAddRows = false;
            stockPage.Controls.Add(designerStockGrid);
            salesPage.Controls.Add(designerSalesGrid);
            designerTabs.TabPages.Add(stockPage);
            designerTabs.TabPages.Add(salesPage);
            Controls.Add(designerTitle);
            Controls.Add(designerStart);
            Controls.Add(designerEnd);
            Controls.Add(designerRefresh);
            Controls.Add(designerCsv);
            Controls.Add(designerExcel);
            Controls.Add(designerPdf);
            Controls.Add(designerTabs);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
