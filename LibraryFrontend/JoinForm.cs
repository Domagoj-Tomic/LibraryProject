using LibraryShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class JoinForm : Form
	{
		private int _itemId;
		private string _itemType;
		private string _currentView;
		private readonly HttpClient _httpClient = new HttpClient();
		private const string ApiBaseUrl = "https://localhost:44312/api/";

		public JoinForm(int itemId, string itemType)
		{
			InitializeComponent();
			_itemId = itemId;
			_itemType = itemType;

			if (_itemType == "Book" || _itemType == "Workshop")
			{
				bookButton.Visible = false;
				workshopButton.Visible = false;
				_currentView = "User";
				LoadUsers();
			}
			else if (_itemType == "User")
			{
				_currentView = "Book";
				LoadBooks();
			}

			bookButton.Enabled = false;
			bookButton.Click += bookButton_Click;
			workshopButton.Click += workshopButton_Click;
			searchButton.Click += searchButton_Click;
			clearSearchButton.Click += clearSearchButton_Click;
		}

		private async void LoadItemDetails()
		{
			switch (_itemType)
			{
				case "Book":
					var books = await GetEntities<Book>("Book", 50);
					DisplayEntities(books);
					break;
				case "User":
					var users = await GetEntities<User>("User", 50);
					DisplayEntities(users);
					break;
				case "Workshop":
					var workshops = await GetEntities<Workshop>("Workshop", 50);
					DisplayEntities(workshops);
					break;
			}
		}

		private async Task<List<T>> GetEntities<T>(string endpoint, int limit)
		{
			var response = await _httpClient.GetStringAsync($"{ApiBaseUrl}{endpoint}?limit={limit}");
			return JsonConvert.DeserializeObject<List<T>>(response);
		}

		private void DisplayEntities<T>(List<T> entities)
		{
			flowLayoutPanel1.Controls.Clear();
			foreach (var entity in entities)
			{
				if (entity is Book book)
				{
					DisplayBook(book);
				}
				else if (entity is User user)
				{
					DisplayUser(user);
				}
				else if (entity is Workshop workshop)
				{
					DisplayWorkshop(workshop);
				}
			}
		}

		private void DisplayBook(Book book)
		{
			Image coverImage = null;
			if (book.CoverImage != null && book.CoverImage.Length > 0)
			{
				using (var ms = new System.IO.MemoryStream(book.CoverImage))
				{
					coverImage = Image.FromStream(ms);
				}
			}
			var bookItem = new ItemControl(book.Title, book.Author, coverImage, false);
			bookItem.ItemClicked += (s, e) => JoinItems(book);
			flowLayoutPanel1.Controls.Add(bookItem);
		}

		private void DisplayUser(User user)
		{
			var userItem = new ItemControl($"{user.FirstName} {user.LastName}", user.Email, null, false);
			userItem.ItemClicked += (s, e) => JoinItems(user);
			flowLayoutPanel1.Controls.Add(userItem);
		}

		private void DisplayWorkshop(Workshop workshop)
		{
			var workshopItem = new ItemControl(workshop.Name, $"Attendees: {workshop.NumberOfAttendees}", null, false);
			workshopItem.ItemClicked += (s, e) => JoinItems(workshop);
			flowLayoutPanel1.Controls.Add(workshopItem);
		}

		private int GetEntityId(object entity)
		{
			switch (entity)
			{
				case Book book:
					return book.BookID;
				case User user:
					return user.UserID;
				case Workshop workshop:
					return workshop.WorkshopID;
				default:
					throw new InvalidOperationException("Unknown entity type");
			}
		}

		private async void JoinItems(object entity)
		{
			try
			{
				HttpResponseMessage response = null;

				if (_itemType == "User" && entity is Book book)
				{
					// Case 1: entity is user, being joined to a book
					var userId = _itemId;
					var bookId = book.BookID;
					var borrowedDate = DateTime.Now;
					var numberOfDays = 14;

					var payload = new
					{
						userId,
						bookId,
						borrowedDate,
						numberOfDays
					};

					var json = JsonConvert.SerializeObject(payload);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					response = await _httpClient.PostAsync($"{ApiBaseUrl}UserBook/AddBookToUser", content);
				}
				else if (_itemType == "User" && entity is Workshop workshop)
				{
					// Case 2: entity is user, being joined to a workshop
					var userId = _itemId;
					var workshopId = workshop.WorkshopID;

					var payload = new
					{
						userId,
						workshopId
					};

					var json = JsonConvert.SerializeObject(payload);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					response = await _httpClient.PostAsync($"{ApiBaseUrl}UserWorkshop/AddUserToWorkshop", content);
				}
				else if (_itemType == "Book" && entity is User user)
				{
					// Case 3: entity is book, being joined to a user
					var bookId = _itemId;
					var userId = user.UserID;
					var borrowedDate = DateTime.Now;
					var numberOfDays = 14;

					var payload = new
					{
						bookId,
						userId,
						borrowedDate,
						numberOfDays
					};

					var json = JsonConvert.SerializeObject(payload);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					response = await _httpClient.PostAsync($"{ApiBaseUrl}UserBook/AddUserToBook", content);
				}
				else if (_itemType == "Workshop" && entity is User workshopUser)
				{
					// Case 4: entity is a workshop, being joined to a user
					var workshopId = _itemId;
					var userId = workshopUser.UserID;

					var payload = new
					{
						workshopId,
						userId
					};

					var json = JsonConvert.SerializeObject(payload);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					response = await _httpClient.PostAsync($"{ApiBaseUrl}UserWorkshop/AddUserToWorkshop", content);
				}

				if (response != null)
				{
					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Request succeeded.");
						MessageBox.Show("Items joined successfully.");
					}
					else
					{
						var errorContent = await response.Content.ReadAsStringAsync();
						Console.WriteLine($"Error response: {errorContent}");
						MessageBox.Show($"Error joining items: {response.ReasonPhrase}");
					}
				}
			}
			catch (HttpRequestException ex)
			{
				MessageBox.Show($"Error joining items: {ex.Message}");
				Console.WriteLine($"HttpRequestException: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Unexpected error: {ex.Message}");
				Console.WriteLine($"Exception: {ex.Message}");
			}
		}

		private async void bookButton_Click(object sender, EventArgs e)
		{
			_currentView = "Book";
			bookButton.Enabled = false;
			workshopButton.Enabled = true;
			var books = await GetEntities<Book>("Book", 50);
			DisplayEntities(books);
		}

		private async void workshopButton_Click(object sender, EventArgs e)
		{
			_currentView = "Workshop";
			workshopButton.Enabled = false;
			bookButton.Enabled = true;
			var workshops = await GetEntities<Workshop>("Workshop", 50);
			DisplayEntities(workshops);
		}

		private async void LoadUsers()
		{
			var users = await GetEntities<User>("User", 50);
			DisplayEntities(users);
		}

		private async void LoadBooks()
		{
			var books = await GetEntities<Book>("Book", 50);
			DisplayEntities(books);
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			var query = searchTextBox.Text;
			if (string.IsNullOrWhiteSpace(query))
			{
				MessageBox.Show("Pretraga ne može biti prazna.");
				return;
			}

			switch (_currentView)
			{
				case "Book":
					var books = await SearchEntities<Book>("Book", query, 50);
					DisplayEntities(books);
					break;
				case "User":
					var users = await SearchEntities<User>("User", query, 50);
					DisplayEntities(users);
					break;
				case "Workshop":
					var workshops = await SearchEntities<Workshop>("Workshop", query, 50);
					DisplayEntities(workshops);
					break;
			}
		}

		private async Task<List<T>> SearchEntities<T>(string endpoint, string query, int limit)
		{
			var url = $"{ApiBaseUrl}{endpoint}?search={query}&limit={limit}";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<T>>(jsonString);
		}

		private async Task LoadEntitiesBasedOnCurrentView()
		{
			switch (_currentView)
			{
				case "Book":
					var books = await GetEntities<Book>("Book", 50);
					DisplayEntities(books);
					break;
				case "User":
					var users = await GetEntities<Book>("Book", 50);
					DisplayEntities(users);
					break;
				case "Workshop":
					var workshops = await GetEntities<Book>("Book", 50);
					DisplayEntities(workshops);
					break;
			}
		}

		private async void clearSearchButton_Click(object sender, EventArgs e)
		{
			searchTextBox.Text = string.Empty;
			await LoadEntitiesBasedOnCurrentView();
		}
	}
}
