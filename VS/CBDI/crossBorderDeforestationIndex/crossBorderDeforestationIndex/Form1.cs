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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace crossBorderDeforestationIndex
{
    public partial class Form1 : Form
    {
        string strDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string defaultFolder = "";
        // 
        Image<Bgr, Byte> bgrImage;
        int widthSrc;
        int heightSrc;
        double minPercentile = 5;
        double maxPercentile = 95;
        Image<Gray, Byte>[] channels;
        Image<Gray, Byte> bChannel;
        Image<Gray, Byte> gChannel;
        Image<Gray, Byte> rChannel;
        Image<Gray, double> ndvi;

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
                bgrImage = new Image<Bgr, Byte>(findSrc.FileName);
                widthSrc = bgrImage.Width;
                heightSrc = bgrImage.Height;
                // Show image in a picture Box
                picBoxRGB.Image = bgrImage.ToBitmap();
                // Separate channels from BRG Image
                channels = bgrImage.Split();
                bChannel = channels[0];
                gChannel = channels[1];
                rChannel = channels[2];
                // Set the size to the NDVI image
                ndvi = new Image<Gray, double>(widthSrc, heightSrc);
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

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show a channel into the picture box
            picBoxRGB.Image = bChannel.ToBitmap();
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show a channel into the picture box
            picBoxRGB.Image = gChannel.ToBitmap();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show a channel into the picture box
            picBoxRGB.Image = rChannel.ToBitmap();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the Brg image into the picture box
            picBoxRGB.Image = bgrImage.ToBitmap();
        }

        private void nDVIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Red Channel + Infrared Channel
            Image<Gray, double> bottom = (rChannel.Convert<Gray, double>() + bChannel.Convert<Gray, double>());
            // Avoid divide by zero
            for (int i = 0; i < heightSrc; i++)
            {
                for (int j = 0; j < widthSrc; j++)
                {
                    double pxBottom = bottom.Data[i, j, 0];
                    if (pxBottom == 0)
                    {
                        bottom.Data[i, j, 0] = 0.01d;
                    }
                    
                }
            }
            // Red Channel - Infrared Channel
            Image<Gray, double> redDiffBlueCh = (rChannel.Convert<Gray, double>() - bChannel.Convert<Gray, double>());
            // NDVI = (Red - Blue)/(Red + Blue) => redDiffBlueCh / bottom
            for (int i = 0; i < heightSrc; i++)
            {
                for (int j = 0; j < widthSrc; j++)
                {
                    double pxRDiffBCh = redDiffBlueCh.Data[i, j, 0];
                    double pxBottom = bottom.Data[i, j, 0];
                    //System.Diagnostics.Debug.WriteLine("h:" + i + " w:" + j + "out:" + pxRDiffBCh/pxBottom);
                    ndvi.Data[i, j, 0] = pxRDiffBCh/pxBottom;
                }
            }

            // Convert an Image to a flat array
            Int64 lenNdviArray = (widthSrc * heightSrc);
            Int64 idx = 0;
            double [] flatNdvi = new double[lenNdviArray];
            for (int i = 0; i < heightSrc; i++)
            {
                for (int j = 0; j < widthSrc; j++)
                {
                    //System.Diagnostics.Debug.WriteLine("h:" + i + " w:" + j + "out:" + pxRDiffBCh/pxBottom);
                    flatNdvi[idx] = ndvi.Data[i, j, 0];
                    idx++;
                }
            }

            // Calculate Percentile
            Percentile centile;
            centile = new Percentile();
            double inMin = centile.percentileInc(flatNdvi, minPercentile);
            double inMax = centile.percentileInc(flatNdvi, maxPercentile);

            System.Diagnostics.Debug.WriteLine("min:" + inMin);
            System.Diagnostics.Debug.WriteLine("max:" + inMax);
            double outMin = 0;
            double outMax = 255;

            ndvi -= inMin;
            ndvi *= ((outMin - outMax )/ (inMin - inMax));
            ndvi += inMin;

            Image<Gray, Byte> outImg = new Image<Gray, Byte>(widthSrc, heightSrc);
            outImg = ndvi.Convert<Gray, Byte>();

            picBoxRGB.Image = outImg.ToBitmap();
        }
    }
}