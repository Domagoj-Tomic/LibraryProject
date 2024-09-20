using LibraryShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class JoinForm : Form
	{
		private int _itemId;
		private string _itemType;
		private readonly HttpClient _httpClient = new HttpClient();
		private const string ApiBaseUrl = "https://localhost:44312/api/";

		public JoinForm(int itemId, string itemType)
		{
			InitializeComponent();
			_itemId = itemId;
			_itemType = itemType;
			LoadItemDetails();
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
			var bookItem = new ItemControl(book.Title, book.Author, coverImage);
			flowLayoutPanel1.Controls.Add(bookItem);
		}

		private void DisplayUser(User user)
		{
			var userItem = new ItemControl($"{user.FirstName} {user.LastName}", user.Email, null);
			flowLayoutPanel1.Controls.Add(userItem);
		}

		private void DisplayWorkshop(Workshop workshop)
		{
			var workshopItem = new ItemControl(workshop.Name, $"Attendees: {workshop.NumberOfAttendees}", null);
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

		private void JoinItems(object entity)
		{
			// Implement the logic to join the item
			MessageBox.Show($"Joining {_itemType} with ID {_itemId}");
		}
	}
}
