using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class BookItemControl : UserControl
	{
		public BookItemControl(string title, string author)
		{
			InitializeComponent();
			titleLabel.Text = title;
			authorLabel.Text = author;
		}

		private void BookItemControl_Click(object sender, EventArgs e)
		{
			// Handle the click event (e.g., show book details)
			MessageBox.Show($"Title: {titleLabel.Text}\nAuthor: {authorLabel.Text}");
		}
	}
}
