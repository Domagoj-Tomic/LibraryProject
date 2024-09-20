using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class ItemControl : UserControl
	{
		public event EventHandler ItemClicked;
		public event EventHandler JoinButtonClicked;
		public string Category;
		public int Id;

		public ItemControl(string title, string author, Image coverImage)
		{
			InitializeComponent();
			titleLabel.Text = title;
			authorLabel.Text = author;
			thumbnailPictureBox.Image = coverImage;
			thumbnailPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			this.Click += itemControl_Click;
			titleLabel.Click += itemControl_Click;
			authorLabel.Click += itemControl_Click;
			thumbnailPictureBox.Click += itemControl_Click;

			joinButton.Click += joinButton_Click;
		}

		public void itemControl_Click(object sender, EventArgs e)
		{
			ItemClicked?.Invoke(this, e);
		}

		public void joinButton_Click(object sender, EventArgs e)
		{
			JoinButtonClicked?.Invoke(this, e);
		}

		public void SetJoinButtonText(string text)
		{
			joinButton.Text = text;
		}
	}
}
