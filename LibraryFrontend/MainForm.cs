using LibraryShared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
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
				var bookItem = new BookItemControl(book.Title, book.Author);
				flowLayoutPanel1.Controls.Add(bookItem);
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