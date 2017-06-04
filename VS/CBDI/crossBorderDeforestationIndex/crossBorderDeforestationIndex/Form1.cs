using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace crossBorderDeforestationIndex
{
    public partial class Form1 : Form
    {
        string strDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string defaultFolder = "";

        public Form1()
        {            
            InitializeComponent();
            defaultFolder = strDocumentsPath + "\\Deforestation";
            imgPnl.AutoScroll = true;
            picBoxRGB.SizeMode = PictureBoxSizeMode.AutoSize;

            try
            {
                if (!(Directory.Exists(defaultFolder))) Directory.CreateDirectory(defaultFolder);
            }
            catch
            {
                // Something to do
            }
        }

        /// <summary>
        /// This function will open a JPEG image and it will show in a pictureBox
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            OpenFileDialog findSrc = new OpenFileDialog();
            // Enable the JPG filter
            findSrc.Filter = "Select image|*.jpg";
            findSrc.FileName = "";
            findSrc.Title = "Open Image";
            findSrc.InitialDirectory = defaultFolder;
            if (findSrc.ShowDialog() == DialogResult.OK)
            {          
                // Open image with OpenCV
                Image<Bgr, Byte> rgbImage = new Image<Bgr, byte>(findSrc.FileName);
                // Show image in a picture Box
                picBoxRGB.Image = rgbImage.ToBitmap();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
