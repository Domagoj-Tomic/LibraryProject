using LibraryShared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class MainForm : Form
	{
		private readonly HttpClient _httpClient = new HttpClient();
		private const string ApiBaseUrl = "https://localhost:44312/api/Book";

		public MainForm()
		{
			InitializeComponent();
			LoadBooks();
		}

		private async void LoadBooks()
		{
			var books = await GetBooks(50); // Load the first 50 books
			DisplayBooks(books);
		}

		private async Task<List<Book>> GetBooks(int limit)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}?limit={limit}");
			response.EnsureSuccessStatusCode();
			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Book>>(jsonString);
		}

		private void DisplayBooks(List<Book> books)
		{
			flowLayoutPanel1.Controls.Clear();
			foreach (var book in books)
			{
				Image coverImage = null;
				if (book.CoverImage != null && book.CoverImage.Length > 0)
				{
					using (var ms = new System.IO.MemoryStream(book.CoverImage))
					{
						coverImage = Image.FromStream(ms);
					}
				}
				var bookItem = new ItemControl(book.Title, book.Author, coverImage);
				bookItem.ItemClicked += (sender, e) => ShowBookDetails(book);
				flowLayoutPanel1.Controls.Add(bookItem);
			}
		}

		private void ShowBookDetails(Book book)
		{
			string borrowingAllowedHrv;
			if (book.BorrowingAllowed == true) borrowingAllowedHrv = "Posudba dozvoljena";
			else borrowingAllowedHrv = "Posudba nije dozvoljena";

			groupBox1.Controls.Clear();

			var titleLabel = new Label { Text = $"Naslov: {book.Title}", AutoSize = true, Location = new System.Drawing.Point(10, 20) };
			var authorLabel = new Label { Text = $"Autor: {book.Author}", AutoSize = true, Location = new System.Drawing.Point(10, 50) };
			var isbnLabel = new Label { Text = $"ISBN: {book.ISBN}", AutoSize = true, Location = new System.Drawing.Point(10, 80) };
			var copiesLabel = new Label { Text = $"Broj primjeraka: {book.NumberOfCopies}", AutoSize = true, Location = new System.Drawing.Point(10, 110) };
			var categoryLabel = new Label { Text = $"Kategorija: {book.Category}", AutoSize = true, Location = new System.Drawing.Point(10, 140) };
			var borrowingAllowedLabel = new Label { Text = $"{borrowingAllowedHrv}", AutoSize = true, Location = new System.Drawing.Point(10, 170) };

			groupBox1.Controls.Add(titleLabel);
			groupBox1.Controls.Add(authorLabel);
			groupBox1.Controls.Add(isbnLabel);
			groupBox1.Controls.Add(copiesLabel);
			groupBox1.Controls.Add(categoryLabel);
			groupBox1.Controls.Add(borrowingAllowedLabel);

			// Set the cover image in pictureBox1
			if (book.CoverImage != null && book.CoverImage.Length > 0)
			{
				using (var ms = new System.IO.MemoryStream(book.CoverImage))
				{
					pictureBox1.Image = Image.FromStream(ms);
				}
				pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			}
			else
			{
				pictureBox1.Image = null; // Clear the image if no cover image is available
			}
		}

		private async void searchButton_Click(object sender, System.EventArgs e)
		{
			var query = searchTextBox.Text;
			if (string.IsNullOrWhiteSpace(query))
			{
				MessageBox.Show("Please enter a search term.");
				return;
			}

			var books = await SearchBooks(query, 50); // Cap the search results to 50 books
			DisplayBooks(books);
		}

		private async Task<List<Book>> SearchBooks(string query, int limit)
		{
			var url = $"{ApiBaseUrl}?search={query}&limit={limit}";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Book>>(jsonString);
		}
	}
}