using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AltiumSharp;
using AltiumSharp.BasicTypes;
using AltiumSharp.Drawing;

namespace CelestialPinArranger
{
    public partial class FormZoom : Form
    {
        private SchLib _schLib;
        private Renderer _renderer;

        public FormZoom()
        {
            InitializeComponent();

            using (var path = new GraphicsPath())
            {
                var r = ClientRectangle;
                r.Inflate(-r.Width / 4, -r.Height / 4);
                path.AddEllipse(r);
                Region = new Region(path);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                const int WS_EX_TRANSPARENT = 0x00000020;
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT;
                return cp;
            }
        }
        protected override bool ShowWithoutActivation => true;

        private void FormZoom_Paint(object sender, PaintEventArgs e)
        {
            if (_schLib == null) return;

            _renderer.Render(e.Graphics, ClientSize.Width, ClientSize.Height, false, false);

        }

        public void SetSchLib(SchLib schLib, int index = 0)
        {
            if (schLib == null)
            {
                _renderer?.Dispose();
                _renderer = null;
                return;
            }

            index = Math.Max(index, 0);

            _schLib = schLib;
            _renderer = new SchLibRenderer(null, null)
            {
                Component = schLib.Items[index],
                BackgroundColor = BackColor
            };

            Invalidate();
        }

        public void Update(int centerX, int centerY, CoordPoint viewCenter, double viewScale)
        {
            Location = new Point(centerX - Width / 2, centerY - Height / 2);
            _renderer.Center = viewCenter;
            _renderer.Scale = viewScale;

            Invalidate();
            Update();
        }

        private void FormZoom_MouseMove(object sender, MouseEventArgs e)
        {
            Hide();
        }
    }
}
