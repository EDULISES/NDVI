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
                if (!(Directory.Exists(strDocumentsPath))) Directory.CreateDirectory(strDocumentsPath);
            }
            catch
            {
                // Something to do
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            OpenFileDialog findSrc = new OpenFileDialog();
            findSrc.Filter = "Select image|*.jpg";
            // 
            findSrc.FileName = "";
            findSrc.Title = "Open Image";
            findSrc.InitialDirectory = defaultFolder;
            if (findSrc.ShowDialog() == DialogResult.OK)
            {
                // 
                string strSrcPath = findSrc.FileName;
                this.picBoxRGB.ImageLocation = strSrcPath;
                Bitmap rgbImg;
                rgbImg = new Bitmap(strSrcPath);
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
