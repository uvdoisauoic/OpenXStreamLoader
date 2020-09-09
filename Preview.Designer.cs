namespace OpenXStreamLoader
{
    partial class PreviewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewForm));
            this._image = new System.Windows.Forms.PictureBox();
            this._defaultImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._defaultImage)).BeginInit();
            this.SuspendLayout();
            // 
            // _image
            // 
            this._image.Dock = System.Windows.Forms.DockStyle.Fill;
            this._image.Location = new System.Drawing.Point(0, 0);
            this._image.Name = "_image";
            this._image.Size = new System.Drawing.Size(360, 270);
            this._image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._image.TabIndex = 0;
            this._image.TabStop = false;
            // 
            // _defaultImage
            // 
            this._defaultImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._defaultImage.Image = ((System.Drawing.Image)(resources.GetObject("_defaultImage.Image")));
            this._defaultImage.Location = new System.Drawing.Point(0, 0);
            this._defaultImage.Name = "_defaultImage";
            this._defaultImage.Size = new System.Drawing.Size(360, 270);
            this._defaultImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._defaultImage.TabIndex = 1;
            this._defaultImage.TabStop = false;
            this._defaultImage.Visible = false;
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 270);
            this.Controls.Add(this._defaultImage);
            this.Controls.Add(this._image);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Preview";
            this.Deactivate += new System.EventHandler(this.PreviewForm_Deactivate);
            ((System.ComponentModel.ISupportInitialize)(this._image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._defaultImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _image;
        private System.Windows.Forms.PictureBox _defaultImage;
    }
}