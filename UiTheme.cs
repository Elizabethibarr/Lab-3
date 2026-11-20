using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsApp3
{
    internal static class UiTheme
    {
        public static readonly Color Navy = Color.FromArgb(22, 38, 67);
        public static readonly Color Blue = Color.FromArgb(43, 102, 210);
        public static readonly Color Background = Color.FromArgb(245, 247, 251);
        public static readonly Color Text = Color.FromArgb(35, 45, 62);
        public static readonly Color Muted = Color.FromArgb(117, 129, 148);
        public static readonly Color Border = Color.FromArgb(226, 231, 239);
        public static readonly Color Green = Color.FromArgb(32, 150, 112);

        public static Label Label(string text, int size = 10, bool bold = false, Color? color = null)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color ?? Text,
                Margin = new Padding(0, 3, 0, 3)
            };
        }

        public static Button Button(string text, bool primary = false)
        {
            var button = new Button
            {
                Text = text,
                AutoSize = true,
                MinimumSize = new Size(110, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = primary ? Blue : Color.White,
                ForeColor = primary ? Color.White : Text,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 4, 12, 4),
                Margin = new Padding(4)
            };
            button.FlatAppearance.BorderColor = primary ? Blue : Border;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(32, 83, 180) : Background;
            return button;
        }

        public static TextBox TextBox(string placeholder = "")
        {
            return new TextBox
            {
                PlaceholderText = placeholder,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                ForeColor = Text,
                BackColor = Color.White,
                Height = 34,
                Margin = new Padding(0, 5, 0, 12)
            };
        }

        public static DataGridView Grid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Border,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 42,
                RowTemplate = { Height = 36 },
                Font = new Font("Segoe UI", 9.5f),
                ColumnHeadersDefaultCellStyle =
                {
                    BackColor = Background,
                    ForeColor = Muted,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    SelectionBackColor = Background,
                    SelectionForeColor = Muted
                },
                DefaultCellStyle =
                {
                    BackColor = Color.White,
                    ForeColor = Text,
                    SelectionBackColor = Color.FromArgb(229, 238, 255),
                    SelectionForeColor = Text,
                    Padding = new Padding(6, 0, 6, 0)
                },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(250, 251, 253) }
            };
        }

        public static Panel Card()
        {
            return new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 14, 14),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        public static void ClearDesignerPreview(Form form)
        {
            foreach (Control control in form.Controls.Cast<Control>().ToArray())
                control.Dispose();
            form.Controls.Clear();
        }

    }
}
