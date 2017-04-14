using Approved.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Approved
{
    public partial class AddProduct : Form
    {
        string username = "u717674429";
        string password = "Password1234";
        string addressURL = "ftp://www.driftintocode.com/images/products";

        Connection_Handler connection = new Connection_Handler();

        public AddProduct()
        {
            m_aeroEnabled = false;

            this.FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();

            pictureBox_Video.Visible = true;
            MediaPlayer.Visible = false;
        }

        private void storeVisual(OpenFileDialog search, string title)
        {
            try
            {
                // Get FileInfo for the file to be uploaded
                FileInfo toUpload = new FileInfo(search.FileName);

                // Create FtpWebRequest object
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(addressURL + "/" + toUpload.Name);

                // Method will be uploadFile
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // Set credentials
                request.Credentials = new NetworkCredential(username, password);

                // Setup streams
                Stream ftpStream = request.GetRequestStream();
                FileStream file = File.OpenRead(search.FileName);

                // Setup variables to read the file
                int length = 1024;
                byte[] buffer = new byte[length];
                int bytesRead = 0;

                // Write to request stream
                do
                {
                    bytesRead = file.Read(buffer, 0, length);
                    ftpStream.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);

                // Close streams
                file.Close();
                ftpStream.Close();
            }
            catch
            {
                MessageBox.Show("Upload Failed!");
            }
            
        }

        // Open Dialog Box so user can search for a file to upload
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int size = -1;
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    Picture_Box.SizeMode = PictureBoxSizeMode.StretchImage;
                    Picture_Box.LoadAsync(openFileDialog1.FileName);

                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }

            
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
        }

        private void pictureBox_Video_Click(object sender, EventArgs e)
        {
            int size = -1;
            openFileDialog2.Filter = "All Videos Files |*.wmv; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.webm";
            DialogResult result = openFileDialog2.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog2.FileName;
                try
                {
                    MediaPlayer.URL = file;

                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }

            pictureBox_Video.Visible = false;
            MediaPlayer.Visible = true;

            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
        }

        private void textBox_Title_MouseClick(object sender, MouseEventArgs e)
        {
            textBox_Title.ForeColor = Color.Black;
            textBox_Title.Text = "";
        }

        private void Textbox_Description_Click(object sender, EventArgs e)
        {
            Textbox_Description.ForeColor = Color.Black;
            Textbox_Description.Text = "";
        }

        private void Button_Submit_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.FileName != "openFileDialog1" && openFileDialog2.FileName != "openFileDialog2" && textBox_Title.Text != "" && Textbox_Description.Text != "")
            {
                Button_Submit.Text = "Working...";

                storeVisual(openFileDialog1, "Picture_" + textBox_Title.Text.ToString());
                storeVisual(openFileDialog2, "Video_" + textBox_Title.Text.ToString());

                string address = addressURL.Substring(28);
                string imageAddress = System.IO.Path.GetFileName(openFileDialog1.FileName);
                string videoAddress = System.IO.Path.GetFileName(openFileDialog2.FileName);

                connection.SetProductDetails(textBox_Title.Text.ToString(), Textbox_Description.Text.ToString(), imageAddress, videoAddress);

                MessageBox.Show("Upload Completed! Refresh dashboard to view product");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all details!");
            }
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
            this.Hide();
        }

        // on mouse click: minimise application
        private void Button_Minimise_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
