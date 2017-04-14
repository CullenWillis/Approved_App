using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Approved.DatabaseConnection;

namespace Approved
{
    public partial class Login : Form
    {
        //Custom font
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        FontFamily ff;
        Font font;

        Connection_Handler connection;

        public Login()
        {
            m_aeroEnabled = false;

            this.FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();

            connection = new Connection_Handler();
        }

        private void Button_Submit_Click(object sender, EventArgs e)
        {
            if(connection.GetLogin(Textbox_Username.Text, Textbox_Password.Text))
            {
                Dashboard dash = new Dashboard();

                dash.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or Password is incorrect!");
                Textbox_Username.Clear();
                Textbox_Password.Clear();
                Textbox_Username.Focus();
            }
        }

        /*
         * ----------------------------------------- 
         *              UI EVENTS
         * -----------------------------------------
         */

        // On mouse enter: change button and text color
        private void Button_Submit_MouseEnter(object sender, EventArgs e)
        {
            // Get colors
            System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#48aef3");
            System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color borderColor = System.Drawing.ColorTranslator.FromHtml("#48aef3");

            // Apply colors
            Button_Submit.BackColor = backColor;
            Button_Submit.ForeColor = foreColor;
            Button_Submit.FlatAppearance.BorderColor = borderColor;
        }

        // On mouse exit: revert button and text color to original state
        private void Button_Submit_MouseLeave(object sender, EventArgs e)
        {
            // Get colors
            System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#666666");
            System.Drawing.Color borderColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            // Apply colors
            Button_Submit.BackColor = backColor;
            Button_Submit.ForeColor = foreColor;
            Button_Submit.FlatAppearance.BorderColor = borderColor;
        }

        // On mouse enter: change background image
        private void Button_Exit_MouseEnter(object sender, EventArgs e)
        {
            // Get image
            Bitmap bmp = new Bitmap(Approved.Properties.Resources.CloseHoverImage);

            // Apply image
            Button_Exit.BackgroundImage = (Image)bmp;
        }

        // On mouse exit: revert image to original image
        private void Button_Exit_MouseLeave(object sender, EventArgs e)
        {
            // Get image
            Bitmap bmp = new Bitmap(Approved.Properties.Resources.CloseImage);

            // Apply image
            Button_Exit.BackgroundImage = (Image)bmp;
        }

        // On mouse enter: change background image
        private void Button_Minimise_MouseEnter(object sender, EventArgs e)
        {
            // Get image
            Bitmap bmp = new Bitmap(Approved.Properties.Resources.MinimiseHoverImage);

            // Apply image
            Button_Minimise.BackgroundImage = (Image)bmp;
        }

        // On mouse exit: revert image to original image
        private void Button_Minimise_MouseLeave(object sender, EventArgs e)
        {
            // Get image
            Bitmap bmp = new Bitmap(Approved.Properties.Resources.MinimiseImage);

            // Apply image
            Button_Minimise.BackgroundImage = (Image)bmp;
        }

        // on mouse click: exit application
        private void Button_Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        // on mouse click: minimise application
        private void Button_Minimise_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
         * ----------------------------------------- 
         *              CUSTOM FONTS
         * -----------------------------------------
         */

        private void LoadFont(String type)
        {
            // Get data for regular font
            if (type == "Regular")
            {
                byte[] fontArray = Approved.Properties.Resources.gadugi;
                int dataLength = Approved.Properties.Resources.gadugi.Length;

                IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

                Marshal.Copy(fontArray, 0, ptrData, dataLength);

                uint cFonts = 0;

                AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

                PrivateFontCollection pfc = new PrivateFontCollection();

                pfc.AddMemoryFont(ptrData, dataLength);

                Marshal.FreeCoTaskMem(ptrData);

                ff = pfc.Families[0];
                font = new Font(ff, 15f, FontStyle.Regular);
            }
            // Get data for bold font
            else if (type == "Bold")
            {
                byte[] fontArray = Approved.Properties.Resources.gadugi;
                int dataLength = Approved.Properties.Resources.gadugi.Length;

                IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

                Marshal.Copy(fontArray, 0, ptrData, dataLength);

                uint cFonts = 0;

                AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

                PrivateFontCollection pfc = new PrivateFontCollection();

                pfc.AddMemoryFont(ptrData, dataLength);

                Marshal.FreeCoTaskMem(ptrData);

                ff = pfc.Families[0];
                font = new Font(ff, 15f, FontStyle.Bold);
            }
        }

        private void AllocateFont(Font f, Control c, float size, String type)
        {
            FontStyle fs;

            // Assign regular font
            if (type == "Regular")
            {
                fs = FontStyle.Regular;

                c.Font = new Font(ff, size, fs);
            }
            // Assign bold font
            else if (type == "Bold")
            {
                fs = FontStyle.Bold;

                c.Font = new Font(ff, size, fs);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Assigning regular fonts
            LoadFont("Regular");
            AllocateFont(font, this.Textbox_Username, 15, "Regular");
            AllocateFont(font, this.Textbox_Password, 15, "Regular");
            AllocateFont(font, this.Label_Footer1, 9, "Regular");

            // Assigning bold fonts
            LoadFont("Bold");
            AllocateFont(font, this.Button_Submit, 10, "Bold");
            AllocateFont(font, this.Label_Footer2, 9, "Bold");
        }

        /*
         * ----------------------------------------- 
         *              Drop Shadow
         * -----------------------------------------
         */

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
                m.Result = (IntPtr)HTCAPTION;

        }
    }
}
