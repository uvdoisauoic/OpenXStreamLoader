using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenXStreamLoader
{
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }

        public void setImage(Image image)
        {
            if (image != null)
            {
                _image.Image = image;
            }
            else
            {
                _image.Image = _defaultImage.Image;
            }
        }

        private void PreviewForm_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
