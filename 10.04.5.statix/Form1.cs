namespace _10._04._5.statix
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int quantity = 1;
        public static Point startPos;
        public static Point endPos;
        public static LinkedList<Panel> panels = new LinkedList<Panel>();


        // https://stackoverflow.com/questions/60517524/detecting-click-anywhere-on-form-including-controls-in-winforms
        // Constants for decoding the Win32 messages: https://wiki.winehq.org/List_Of_Windows_Messages
        protected const int WM_MOUSEACTIVATE = 0x0021;
        protected const int WM_LBUTTONDOWN = 0x201;
        protected const int WM_LBUTTONUP = 0x202;
        protected const int WM_LBUTTONDBLCLK = 0x203;
        protected const int WM_RBUTTONDOWN = 0x204;

        protected override void WndProc(ref Message m)
        {
            Point point;
            switch(m.Msg)
            {
                case WM_LBUTTONDBLCLK:
                    point = this.PointToClient(Cursor.Position);
                    MessageBox.Show(String.Format("X: {0} Y: {1}", point.X, point.Y));
                    foreach (Panel panel in panels)
                        if (Within(point, panel))
                        {
                            panels.Remove(panel);
                            panel.Dispose();
                            break;
                        }
                    break;

                case WM_LBUTTONDOWN:
                    startPos = this.PointToClient(Cursor.Position);
                    break;

                case WM_LBUTTONUP:
                    endPos = this.PointToClient(Cursor.Position);
                    Panel newpanel = new Panel();
                    newpanel.Location = new Point(
                        Math.Min(startPos.X, endPos.X),
                        Math.Min(startPos.Y, endPos.Y));

                    int w = Math.Max(startPos.X, endPos.X) - Math.Min(startPos.X, endPos.X);
                    int h = Math.Max(startPos.Y, endPos.Y) - Math.Min(startPos.Y, endPos.Y);
                    if (h < 10 || w < 10)
                    {
                        MessageBox.Show("Ошибка! Панель не может быть меньше чем 10x10",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        break;
                    }
                    newpanel.Size = new Size(w, h);

                    newpanel.BorderStyle = BorderStyle.FixedSingle;
                    newpanel.BackColor = randomColor();
                    panels.AddLast(newpanel);

                    Label lbl = new Label();
                    lbl.ForeColor = contrastColor(newpanel.BackColor);
                    lbl.Text = String.Format(" {0} ", quantity);
                    lbl.Dock = DockStyle.Fill;
                    newpanel.Controls.Add(lbl);
                    this.Controls.Add(newpanel);
                    newpanel.BringToFront();
                    ++quantity;
                    break;

                case WM_RBUTTONDOWN:
                    point = this.PointToClient(Cursor.Position);
                    MessageBox.Show(String.Format("X: {0} Y: {1}", point.X, point.Y));
                    foreach (Panel panel in panels)
                    {
                        if (Within(point, panel))
                        {
                            this.Text = String.Format("Coords: {0}, {1}; Area: {2}",
                            panel.Location.X,
                            panel.Location.Y,
                            panel.Height * panel.Width);
                        }
                    }
                    break;
                
            }

            base.WndProc(ref m);
        }

        public bool Within(Point point, Panel panel)
        {
            return (point.X >= panel.Location.X 
                && point.X <= panel.Location.X + panel.Width
                && point.Y >= panel.Location.Y
                && point.Y <= panel.Location.Y + panel.Height);
        }

        private void Newpanel_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("test");
                this.Text = String.Format("Coords: {0}, {1}; Area: {2}",
                    this.Location.X,
                    this.Location.Y,
                    this.Height * this.Width);
            }
        }

        private Color randomColor()
        {
            Random random = new Random();
            int r, g, b;
            r = random.Next(0, 255);
            g = random.Next(0, 255);
            b = random.Next(0, 255);
            return Color.FromArgb(r, g, b);
        }

        private Color contrastColor(Color c)
        {
            int r, g, b;
            r = (c.R + 128) % 256;
            g = (c.G + 128) % 256;
            b = (c.B + 128) % 256;
            return Color.FromArgb(r, g, b);
        }
    }
}