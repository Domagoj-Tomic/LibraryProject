using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class ItemControl : UserControl
	{
		public event EventHandler ItemClicked;

		public ItemControl(string title, string author, Image coverImage)
		{
			InitializeComponent();
			titleLabel.Text = title;
			authorLabel.Text = author;
			thumbnailPictureBox.Image = coverImage;
			thumbnailPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			this.Click += ItemControl_Click;
			titleLabel.Click += ItemControl_Click;
			authorLabel.Click += ItemControl_Click;
			thumbnailPictureBox.Click += ItemControl_Click;
		}

		private void ItemControl_Click(object sender, EventArgs e)
		{
			ItemClicked?.Invoke(this, e);
		}
	}
}
