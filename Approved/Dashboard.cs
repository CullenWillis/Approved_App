using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //the WinForm wrappers
using LiveCharts.Defaults;
using Approved.Charts;
using Approved.DatabaseConnection;
using System.Runtime.InteropServices;

namespace Approved
{
    public partial class Dashboard : Form
    {
        List<Products> products;
        List<Products> uniqueProduct;
        List<productPreference>  preference;
        List<Comments> comments;

        static Connection_Handler connection;
        Chart_Handler charts;

        public Dashboard()
        {
            m_aeroEnabled = false;

            this.FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();

            connection = new Connection_Handler();
            charts = new Chart_Handler();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            // Get database values
            products = new List<Products>();
            products = connection.GetEntries();

            int count = 0;

            foreach(Products p in products)
            {
                // Retreive picture location
                string pictureURL = "http://driftintocode.com/images/products/";
                string pictureID = products[count].ProductPicture;
                pictureURL = pictureURL + pictureID;

                // retreive title
                string title = products[count].ProductTitle;

                // Create new panel
                CreateNewPanel(pictureURL, title);

                count++;
            }

            FlowLayoutPanel_Products.Visible = true;
        }

        private void CreateNewPanel(string picture, string title)
        {
            // Create new panel
            Panel panelContainer = new Panel();
            panelContainer.Width = 200;
            panelContainer.Height = 250;
            panelContainer.Margin = new Padding(80, 0, 0, 50);
            panelContainer.BackColor = Color.DarkGray;

            // Adding panels to the container panel
            Panel panelTitle = new Panel();
            Panel panelBorder = new Panel();
            PictureBox panelPicture = new PictureBox();
            Panel panelOverlay1 = new Panel();
            Panel panelOverlay1_Picture = new Panel();
            Panel panelOverlay2 = new Panel();
            
            // add new panels as a child of the container
            panelContainer.Controls.Add(panelTitle);
            panelContainer.Controls.Add(panelBorder);
            panelContainer.Controls.Add(panelPicture);
            panelPicture.Controls.Add(panelOverlay1);
            

            // panelTitle properties
            panelTitle.Width = 200;
            panelTitle.Height = 75;
            panelTitle.Dock = DockStyle.Bottom;
            panelTitle.BackColor = Color.White;

            // labelTitle properties
            Label labelTitle = new Label();
            panelTitle.Controls.Add(labelTitle);

            labelTitle.Text = title;
            labelTitle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
            labelTitle.ForeColor = Color.Black;
            labelTitle.AutoSize = false;
            labelTitle.Size = new System.Drawing.Size(200, 75);
            
            labelTitle.Controls.Add(panelOverlay2);


            // panelBorder properties
            panelBorder.Dock = DockStyle.Top;
            panelBorder.Width = 200;
            panelBorder.Height = 10;
            panelBorder.Dock = DockStyle.Top;
            panelBorder.BackColor = Color.FromArgb(230, 230, 230);

            // panelPicture properties
            panelPicture.Width = 200;
            panelPicture.Height = 175;
            panelPicture.Dock = DockStyle.Top;
            panelPicture.BackColor = Color.DarkGray;
            panelPicture.SizeMode = PictureBoxSizeMode.StretchImage;

            Console.WriteLine(picture);
            panelPicture.LoadAsync(picture);

            int opacity = 125; // Max 255

            // Panel1
            panelOverlay1.Width = 200;
            panelOverlay1.Height = 250;
            panelOverlay1.BackColor = Color.FromArgb(opacity, 0, 0, 0);
            panelOverlay1.BringToFront();
            panelOverlay1.Cursor = Cursors.Hand;

            // Overlay_Picture
            panelOverlay1_Picture.Width = 75;
            panelOverlay1_Picture.Height = 75;

            Bitmap plusSign = Approved.Properties.Resources.plusSign2;
            panelOverlay1_Picture.BackColor = Color.Transparent;
            panelOverlay1_Picture.BackgroundImage = plusSign;

            panelOverlay1_Picture.BackgroundImageLayout = ImageLayout.Stretch;
            panelOverlay1_Picture.Location = new Point(60, 75);
            panelOverlay1_Picture.Cursor = Cursors.Hand;

            panelOverlay1.Controls.Add(panelOverlay1_Picture);

            // Panel2
            panelOverlay2.Width = 200;
            panelOverlay2.Height = 250;
            panelOverlay2.BackColor = Color.FromArgb(opacity, 0, 0, 0);
            panelOverlay2.BringToFront();
            panelOverlay2.Cursor = Cursors.Hand;

            panelOverlay1.Visible = false;
            panelOverlay1_Picture.Visible = false;
            panelOverlay2.Visible = false;

            // Event handlers
            panelOverlay1.Click += (sender, e) => GotoProduct(sender, e, title);
            panelOverlay1_Picture.Click += (sender, e) => GotoProduct(sender, e, title);
            panelOverlay2.Click += (sender, e) => GotoProduct(sender, e, title);

            // onEnter enable overlay
            panelPicture.MouseEnter += (sender, e) => ProductOverlay_Enter(sender, e, panelOverlay1, panelOverlay1_Picture, panelOverlay2);
            labelTitle.MouseEnter += (sender, e) => ProductOverlay_Enter(sender, e, panelOverlay1, panelOverlay1_Picture, panelOverlay2);
            panelTitle.MouseEnter += (sender, e) => ProductOverlay_Enter(sender, e, panelOverlay1, panelOverlay1_Picture, panelOverlay2);

            // onLeave disable overlay
            FlowLayoutPanel_Products.MouseEnter += (sender, e) => ProductOverlay_Leave(sender, e, panelOverlay1, panelOverlay1_Picture, panelOverlay2);

            // add container panel to FlowLayoutPanel
            FlowLayoutPanel_Products.Controls.Add(panelContainer);
        }

        /*
         * ----------------------------------------- 
         *              EVENT HANDLERS
         * -----------------------------------------
         */

        // Unique product
        private void GotoProduct(object sender, EventArgs e, string productName)
        {
            panel_top.Controls.Clear();
            panel_Linechart.Controls.Clear();
            Panel_Comments.Controls.Clear();


            // Load new buttons
            Label_Title.Text = "Product";
            DynamicButton_2.Text = "Dashboard";
            DynamicButton_3.Visible = false;
            DynamicButton_4.Visible = false;

            // Get database values
            uniqueProduct = new List<Products>();
            uniqueProduct = connection.GetEntry(productName);

            // Retreive picture location
            PictureBox PictureBox_UPPicture = new PictureBox();
            panel_top.Controls.Add(PictureBox_UPPicture);

            PictureBox_UPPicture.Width = 260;
            PictureBox_UPPicture.Height = 260;
            PictureBox_UPPicture.WaitOnLoad = false;
            PictureBox_UPPicture.ImageLocation = "";
            PictureBox_UPPicture.Location = new Point(20, 89);


            string pictureURL = "http://driftintocode.com/images/products/";
            string pictureID = null;
            
            pictureID = uniqueProduct[0].ProductPicture;
            pictureURL = pictureURL + pictureID;

            TextBox textbox_Description = new TextBox();
            panel_top.Controls.Add(textbox_Description);

            textbox_Description.Width = 332;
            textbox_Description.Height = 259;
            textbox_Description.ReadOnly = true;
            textbox_Description.Location = new Point(298, 90);
            textbox_Description.BorderStyle = BorderStyle.None;
            textbox_Description.Font = new Font("Gadugi", 12f, FontStyle.Regular);
            textbox_Description.WordWrap = true;
            textbox_Description.Multiline = true;

            textbox_Description.Text = uniqueProduct[0].ProductDescription;

            PictureBox_UPPicture.LoadAsync(pictureURL);
            PictureBox_UPPicture.SizeMode = PictureBoxSizeMode.StretchImage;

            string tempDate;
            tempDate = uniqueProduct[0].ProductPublished;
            tempDate = tempDate.Substring(0, 10);

            Label Label_UPTitle = new Label();
            panel_top.Controls.Add(Label_UPTitle);

            Label_UPTitle.AutoSize = true;
            Label_UPTitle.Width = 173;
            Label_UPTitle.Height = 26;
            Label_UPTitle.Location = new Point(16, 2);
            Label_UPTitle.Font = new Font("Gadugi", 14f, FontStyle.Bold);

            Label_UPTitle.Text = uniqueProduct[0].ProductTitle;

            charts.createPieChart(productName, panel_top);
            charts.createLineChart(productName, panel_Linechart);

            createCommentSection(productName);

            FlowLayoutPanel_Products.Visible = false;
            FlowLayoutPanel_UniqueProduct.Visible = true;
            FlowLayoutPanel_UniqueProduct.BringToFront();
        }
        
        private void createCommentSection(string productName)
        {
            button_Refresh.Click += (sender, e) => GotoProduct(sender, e, productName);

            if (comments != null)
                comments.Clear();

            comments = new List<Comments>();
            
            comments = connection.GetComments(productName);

            Panel commentDivider = new Panel();
            Panel_Comments.Controls.Add(commentDivider);
            commentDivider.Width = 1265;
            commentDivider.Height = 1;
            commentDivider.Dock = DockStyle.Top;
            commentDivider.BackColor = Color.FromArgb(230, 230, 230);

            Panel_Comments.Height = 250;

            int commentBoxHeight = 50;
            for (int i = 0; i < comments.Count; i++)
            {
                Panel commentBox = new Panel();
                Panel commentDetails = new Panel();
                Panel deletePanel = new Panel();
                PictureBox delete = new PictureBox();
                Label detailsLabel = new Label();
                Panel divider = new Panel();
                Panel commentText = new Panel();
                Label textLabel = new Label();

                string productTitle = comments[i].CommentTitle;
                string comment = comments[i].CommentText;

                Panel_Comments.Controls.Add(commentBox);
                
                commentBox.Controls.Add(divider);
                commentBox.Controls.Add(commentDetails);
                commentBox.Controls.Add(commentText);

                commentDetails.Controls.Add(detailsLabel);
                commentText.Controls.Add(textLabel);

                commentDetails.Controls.Add(deletePanel);
                deletePanel.Controls.Add(delete);
                
                commentBox.Width = 800;
                commentBox.Height = 200;
                commentBox.BackColor = Color.White;

                commentDetails.Width = 800;
                commentDetails.Height = 40;
                commentDetails.Dock = DockStyle.Top;

                detailsLabel.Width = 760;
                detailsLabel.Height = 40;
                detailsLabel.Font = new Font("Gadugi", 12f, FontStyle.Regular);
                detailsLabel.TextAlign = ContentAlignment.MiddleLeft;

                deletePanel.Width = 40;
                deletePanel.Height = 40;
                deletePanel.Dock = DockStyle.Right;

                delete.Width = 40;
                delete.Height = 40;
                
                delete.Image = Approved.Properties.Resources.CloseImage;
                delete.BackgroundImageLayout = ImageLayout.Center;

                delete.Cursor = Cursors.Hand;
                delete.MouseClick += (sender, e) => deleteComment(sender, e, productTitle, comment);


                divider.Width = 800;
                divider.Height = 1;
                divider.Dock = DockStyle.Top;
                divider.BackColor = Color.FromArgb(230, 230, 230);

                commentText.Width = 800;
                commentText.Height = 159;
                commentText.BackColor = Color.White;
                commentText.Dock = DockStyle.Bottom;

                textLabel.Font = new Font("Gadugi", 11f, FontStyle.Regular);
                textLabel.TextAlign = ContentAlignment.TopLeft;
                textLabel.Width = 780;
                textLabel.Height = 139;
                textLabel.Location = new Point(10, 10);

                detailsLabel.Text = "     " + comments[i].CommentUser + " | " + comments[i].CommentDate.Substring(0,10);
                textLabel.Text = comments[i].CommentText;

                if(i == 0)
                    commentBox.Location = new Point(234, 50);
                else
                    commentBox.Location = new Point(234, commentBoxHeight += 250);

                if(i != comments.Count - 1)
                    Panel_Comments.Height += 250;
                else
                    Panel_Comments.Height += 50;
            }
        }

        // Dynamic button event that changes the display Disboard to control visversa
        private void DynamicButton_2_Click(object sender, EventArgs e)
        {
            // IF title = dashboard switch to Control panel menu
            if (Label_Title.Text == "Dashboard")
            {
                Label_Title.Text = "Control Panel";
                DynamicButton_2.Text = "Dashboard";


                //FlowLayoutPanel_Product.Visible = false;
                FlowLayoutPanel_UniqueProduct.Visible = false;
                FlowLayoutPanel_Products.Visible = false;

                //Button visibility
                DynamicButton_2.Visible = true;
                DynamicButton_3.Visible = true;
                DynamicButton_4.Visible = true;

                DynamicButton_3.Text = "Product Settings";
            }
            // IF title = Control Panel switch to Dashboard menu
            else if (Label_Title.Text == "Control Panel")
            {
                Label_Title.Text = "Dashboard";
                DynamicButton_2.Text = "Control Panel";

                //Button visibility
                DynamicButton_2.Visible = true;
                DynamicButton_3.Visible = true;
                DynamicButton_4.Visible = false;

                DynamicButton_3.Text = "Create Product";

                //FlowLayoutPanel_Product.Visible = false;
                FlowLayoutPanel_UniqueProduct.Visible = false;
                FlowLayoutPanel_Products.Visible = true;
            }
            // IF title = Product switch to Dashboard menu
            else if (Label_Title.Text == "Product")
            {
                Label_Title.Text = "Dashboard";
                DynamicButton_2.Text = "Control Panel";

                charts.dispose();

                //FlowLayoutPanel_Product.Visible = false;
                FlowLayoutPanel_UniqueProduct.Visible = false;
                FlowLayoutPanel_Products.Visible = true;

                //Button visibility
                button_Refresh.Visible = true;
                DynamicButton_2.Visible = true;
                DynamicButton_3.Visible = true;
                DynamicButton_4.Visible = false;
            }
        }

        private void button_Refresh_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel_Products.Controls.Clear();
            LoadDashboard();
        }

        /*
         * ----------------------------------------- 
         *              UI EVENT HANDLERS
         * -----------------------------------------
         */

        private static void deleteComment(object sender, EventArgs e, string productName, string comment)
        {
            if (connection.deleteComment(productName, comment))
                MessageBox.Show("Message has been deleted. Please refresh!");
            else
                MessageBox.Show("Message could not be deleted!");
        }

        private static void ProductOverlay_Enter(object sender, EventArgs e, Panel panel1, Panel panel2, Panel panel3)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
        }

        private static void ProductOverlay_Leave(object sender, EventArgs e, Panel panel1, Panel panel2, Panel panel3)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void Button_Website_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.DriftIntoCode.com");
        }

        private void DynamicButton_3_Click(object sender, EventArgs e)
        {
            if (Label_Title.Text == "Dashboard")
            {
                AddProduct create = new AddProduct();
                create.Show();
            }
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

        private void DynamicButton_1_Click(object sender, EventArgs e)
        {
            Login myLogin = new Login();
            myLogin.Show();
            this.Close();
        }
    }
}
