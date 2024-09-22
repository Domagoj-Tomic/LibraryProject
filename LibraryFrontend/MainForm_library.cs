using LibraryFrontend.LibraryFrontend;
using LibraryShared.Converters;
using LibraryShared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LibraryFrontend
{
	public partial class MainForm
	{
		private List<ItemControl> _itemControls = new List<ItemControl>();
		//private ItemControl _selectedItemControl;

		private async void LoadInitialEntities()
		{
			bookButton.Enabled = false; // Grey out the book button initially
			await LoadEntities<Book>("Book", 50);
		}

		private void InitializeButtons()
		{
			bookButton.Click += (sender, e) => SwitchView<Book>("Book", bookButton);
			userButton.Click += (sender, e) => SwitchView<User>("User", userButton);
			workshopButton.Click += (sender, e) => SwitchView<Workshop>("Workshop", workshopButton);
			editButton.Click += editButton_Click;
			saveChangesButton.Click += saveChangesButton_Click;
			deleteButton.Click += deleteButton_Click;
			createButton.Click += createButton_Click;

			saveChangesButton.Enabled = false;
		}

		private async void SwitchView<T>(string endpoint, Button selectedButton)
		{
			// Grey out the selected button and reset others
			bookButton.Enabled = true;
			userButton.Enabled = true;
			workshopButton.Enabled = true;
			selectedButton.Enabled = false;

			// Update the current view
			_currentView = endpoint;

			// Load and display the entities
			await LoadEntities<T>(endpoint, 50);
		}

		private async Task LoadEntities<T>(string endpoint, int limit)
		{
			var entities = await GetEntities<T>(endpoint, limit);
			DisplayEntities(entities);
		}

		private async Task<List<T>> GetEntities<T>(string endpoint, int limit)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}{endpoint}?limit={limit}");
			//Console.WriteLine("Response content: " + await response.Content.ReadAsStringAsync());
			response.EnsureSuccessStatusCode();
			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<T>>(jsonString);
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
			bookItem.ItemClicked += (sender, e) => ShowBookDetails(book);
			bookItem.JoinButtonClicked += (sender, e) => JoinItems(book);
			flowLayoutPanel1.Controls.Add(bookItem);
		}

		private void DisplayUser(User user)
		{
			var userItem = new ItemControl($"{user.FirstName} {user.LastName}", user.Email, null);
			userItem.ItemClicked += (sender, e) => ShowUserDetails(user);
			userItem.JoinButtonClicked += (sender, e) => JoinItems(user);
			flowLayoutPanel1.Controls.Add(userItem);
		}

		private void DisplayWorkshop(Workshop workshop)
		{
			var workshopItem = new ItemControl(workshop.Name, $"Attendees: {workshop.NumberOfAttendees}", null);
			workshopItem.ItemClicked += (sender, e) => ShowWorkshopDetails(workshop);
			workshopItem.JoinButtonClicked += (sender, e) => JoinItems(workshop);
			flowLayoutPanel1.Controls.Add(workshopItem);
		}

		private async void ShowBookDetails(Book book)
		{
			_currentEntity = book;
			string borrowingAllowedHrv = book.BorrowingAllowed ? "Posudba dozvoljena" : "Posudba nije dozvoljena";
			var users = await GetUsersByBookId(book.BookID);

			groupBox1.Controls.Clear();

			var titleLabel = new Label { Text = $"Naslov: {book.Title}", AutoSize = true, Location = new Point(10, 20) };
			var authorLabel = new Label { Text = $"Autor: {book.Author}", AutoSize = true, Location = new Point(10, 50) };
			var isbnLabel = new Label { Text = $"ISBN: {book.ISBN}", AutoSize = true, Location = new Point(10, 80) };
			var copiesLabel = new Label { Text = $"Broj primjeraka: {book.NumberOfCopies}", AutoSize = true, Location = new Point(10, 110) };
			var categoryLabel = new Label { Text = $"Kategorija: {book.Category}", AutoSize = true, Location = new Point(10, 140) };
			var borrowingAllowedLabel = new Label { Text = $"{borrowingAllowedHrv}", AutoSize = true, Location = new Point(10, 170) };

			groupBox1.Controls.Add(titleLabel);
			groupBox1.Controls.Add(authorLabel);
			groupBox1.Controls.Add(isbnLabel);
			groupBox1.Controls.Add(copiesLabel);
			groupBox1.Controls.Add(categoryLabel);
			groupBox1.Controls.Add(borrowingAllowedLabel);

			if (users != null && users.Count > 0)
			{
				var borrowedByLabel = new Label { Text = "Knjigu posudio:", AutoSize = true, Location = new Point(10, 200) };
				groupBox1.Controls.Add(borrowedByLabel);

				int i = 0;
				foreach (var user in users)
				{
					var userLabel = new Label { Text = $"{user.FirstName} {user.LastName} ({user.Email})", AutoSize = true, Location = new Point(30, 230 + i) };
					groupBox1.Controls.Add(userLabel);
					i += 30;
				}
			}
			else
			{
				var noBorrowersLabel = new Label { Text = "Knjigu nitko nije posudio.", AutoSize = true, Location = new Point(10, 200) };
				groupBox1.Controls.Add(noBorrowersLabel);
			}

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

		private async void ShowUserDetails(User user)
		{
			_currentEntity = user;
			groupBox1.Controls.Clear();
			var books = await GetBooksByUserId(user.UserID);
			var workshops = await GetWorkshopsByUserId(user.UserID);

			var nameLabel = new Label { Text = $"Ime: {user.FirstName} {user.LastName}", AutoSize = true, Location = new Point(10, 20) };
			var emailLabel = new Label { Text = $"Email: {user.Email}", AutoSize = true, Location = new Point(10, 50) };
			var addressLabel = new Label { Text = $"Adresa: {user.Address}", AutoSize = true, Location = new Point(10, 80) };
			var dobLabel = new Label { Text = $"Datum rođenja: {user.DateOfBirth.ToShortDateString()}", AutoSize = true, Location = new Point(10, 110) };
			var phoneLabel = new Label { Text = $"Telefon: {user.PhoneNumber}", AutoSize = true, Location = new Point(10, 140) };

			int linesWritten = 0;

			groupBox1.Controls.Add(nameLabel);
			groupBox1.Controls.Add(emailLabel);
			groupBox1.Controls.Add(addressLabel);
			groupBox1.Controls.Add(dobLabel);
			groupBox1.Controls.Add(phoneLabel);

			if (books != null && books.Count > 0)
			{
				var borrowedBooksLabel = new Label { Text = "Posuđene knjige:", AutoSize = true, Location = new Point(10, 170) };
				groupBox1.Controls.Add(borrowedBooksLabel);

				foreach (var book in books)
				{
					var bookLabel = new Label { Text = $"{book.Title} ({book.Author})", AutoSize = true, Location = new Point(30, 200 + linesWritten) };
					groupBox1.Controls.Add(bookLabel);
					linesWritten += 30;
				}
			}
			else
			{
				var noBooksLabel = new Label { Text = "Korisnik nije posudio nijednu knjigu.", AutoSize = true, Location = new Point(10, 170) };
				groupBox1.Controls.Add(noBooksLabel);
			}

			if (workshops != null && workshops.Count > 0)
			{
				var enrolledWorkshopsLabel = new Label { Text = "Upisane radionice:", AutoSize = true, Location = new Point(10, 200 + linesWritten) };
				groupBox1.Controls.Add(enrolledWorkshopsLabel);

				foreach (var workshop in workshops)
				{
					var workshopLabel = new Label { Text = $"{workshop.Name} ({workshop.StartDate.ToShortDateString()})", AutoSize = true, Location = new Point(30, 230 + linesWritten) };
					groupBox1.Controls.Add(workshopLabel);
					linesWritten += 30;
				}
			}
			else
			{
				var noWorkshopsLabel = new Label { Text = "Korisnik nije upisan ni na jednu radionicu.", AutoSize = true, Location = new Point(10, 200 + linesWritten) };
				groupBox1.Controls.Add(noWorkshopsLabel);
			}
		}

		private async void ShowWorkshopDetails(Workshop workshop)
		{
			_currentEntity = workshop;
			groupBox1.Controls.Clear();
			var users = await GetUsersByWorkshopId(workshop.WorkshopID);

			var nameLabel = new Label { Text = $"Ime: {workshop.Name}", AutoSize = true, Location = new Point(10, 20) };
			var numberOfAttendeesLabel = new Label { Text = $"Broj polaznika: {workshop.NumberOfAttendees}", AutoSize = true, Location = new Point(10, 50) };
			var durationLabel = new Label { Text = $"Trajanje u minutama: {workshop.DurationMinutes} minutes", AutoSize = true, Location = new Point(10, 80) };
			var startDateLabel = new Label { Text = $"Datum početka: {workshop.StartDate.ToShortDateString()}", AutoSize = true, Location = new Point(10, 110) };

			int linesWritten = 0;

			groupBox1.Controls.Add(nameLabel);
			groupBox1.Controls.Add(numberOfAttendeesLabel);
			groupBox1.Controls.Add(durationLabel);
			groupBox1.Controls.Add(startDateLabel);

			if (users != null && users.Count > 0)
			{
				var attendeesLabel = new Label { Text = "Polaznici radionice:", AutoSize = true, Location = new Point(10, 140) };
				groupBox1.Controls.Add(attendeesLabel);

				foreach (var user in users)
				{
					var userLabel = new Label { Text = $"{user.FirstName} {user.LastName} ({user.Email})", AutoSize = true, Location = new Point(30, 170 + linesWritten) };
					groupBox1.Controls.Add(userLabel);
					linesWritten += 30;
				}
			}
			else
			{
				var noAttendeesLabel = new Label { Text = "Na radionicu nije upisan nijedan polaznik.", AutoSize = true, Location = new Point(10, 140) };
				groupBox1.Controls.Add(noAttendeesLabel);
			}
		}

		private void ShowEditTextBox()
		{
			if (_currentEntity != null || _currentMode == FormMode.Add)
			{
				string jsonContent;
				if (_currentMode == FormMode.Add)
				{
					jsonContent = GetJsonTemplate(_currentView);
				}
				else
				{
					jsonContent = FilterPropertiesForEditOrAdd(_currentEntity);
				}

				editTextBox.Text = jsonContent;
				editTextBox.Visible = true;
				groupBox1.Visible = false;
			}
		}

		private async Task SaveJsonChanges()
		{
			try
			{
				var settings = new JsonSerializerSettings();
				settings.Converters.Add(new DecimalToIntConverter());

				// Deserialize the JSON from the editTextBox to the appropriate entity type
				if (_currentView == "Book")
				{
					_currentEntity = JsonConvert.DeserializeObject<Book>(editTextBox.Text, settings);
				}
				else if (_currentView == "User")
				{
					_currentEntity = JsonConvert.DeserializeObject<User>(editTextBox.Text, settings);
				}
				else if (_currentView == "Workshop")
				{
					_currentEntity = JsonConvert.DeserializeObject<Workshop>(editTextBox.Text, settings);
				}

				// Serialize the updated entity to JSON
				var jsonContent = JsonConvert.SerializeObject(_currentEntity, settings);
				var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

				/*// Log the JSON content being sent
				Console.WriteLine("Sending JSON content:");
				Console.WriteLine(jsonContent);*/

				HttpResponseMessage response;
				if (_currentEntity == null)
				{
					// Create new entity
					response = await _httpClient.PostAsync($"{ApiBaseUrl}{_currentView}", content);
				}
				else
				{
					// Update existing entity
					response = await _httpClient.PutAsync($"{ApiBaseUrl}{_currentView}/{GetEntityId(_currentEntity)}", content);
				}

				response.EnsureSuccessStatusCode();

				MessageBox.Show("Spremanje uspješno.");
				editTextBox.Visible = false;
				groupBox1.Visible = true;

				// Refresh entities based on the search field content
				var query = searchTextBox.Text;
				if (string.IsNullOrWhiteSpace(query))
				{
					await LoadEntitiesBasedOnCurrentView();
				}
				else
				{
					await SearchEntitiesBasedOnCurrentView(query);
				}

				ReselectUpdatedEntity();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving changes: {ex.Message}");
			}
		}

		private async Task LoadEntitiesBasedOnCurrentView()
		{
			switch (_currentView)
			{
				case "Book":
					await LoadEntities<Book>("Book", 50);
					break;
				case "User":
					await LoadEntities<User>("User", 50);
					break;
				case "Workshop":
					await LoadEntities<Workshop>("Workshop", 50);
					break;
			}
		}

		private async Task SearchEntitiesBasedOnCurrentView(string query)
		{
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

		private void ReselectUpdatedEntity()
		{
			switch (_currentEntity)
			{
				case Book book:
					ShowBookDetails(book);
					break;
				case User user:
					ShowUserDetails(user);
					break;
				case Workshop workshop:
					ShowWorkshopDetails(workshop);
					break;
			}
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

		private async Task<List<T>> SearchEntities<T>(string endpoint, string query, int limit)
		{
			var url = $"{ApiBaseUrl}{endpoint}?search={query}&limit={limit}";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var jsonString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<T>>(jsonString);
		}

		private string GetJsonTemplate(string entityType)
		{
			switch (entityType)
			{
				case "Book":
					return JsonConvert.SerializeObject(new
					{
						Title = "Unesite naslov",
						Author = "Unesite autora",
						ISBN = "Unesite ISBN",
						NumberOfCopies = 0,
						Category = "Unesite kategoriju",
						BorrowingAllowed = false/*,
										CoverImage = (byte[])null*/
					}, Formatting.Indented);
				case "User":
					return JsonConvert.SerializeObject(new
					{
						FirstName = "Unesite ime",
						LastName = "Unesite prezime",
						OIB = (decimal?)null,
						Address = "Unesite adresu",
						DateOfBirth = DateTime.Now,
						Username = "Unesite korisničko ime",
						Email = "Unesite email",
						Sex = "Unesite spol",
						UserStatus = "Unesite status korisnika",
						NumberOfBooks = 0,
						PhoneNumber = "Unesite broj telefona",
						MembershipCardNumber = 0,
						PIN = 0
					}, Formatting.Indented);
				case "Workshop":
					return JsonConvert.SerializeObject(new
					{
						Name = "Unesite ime radionice",
						NumberOfAttendees = 0,
						DurationMinutes = 0,
						StartDate = DateTime.Now,
						NumberOfTerms = 0,
						Monday = false,
						Tuesday = false,
						Wednesday = false,
						Thursday = false,
						Friday = false
					}, Formatting.Indented);
				default:
					throw new InvalidOperationException("Unknown entity type");
			}
		}

		private void UpdateCancelButtonState()
		{
			cancelButton.Enabled = _currentMode == FormMode.Edit || _currentMode == FormMode.Add;
		}

		private string FilterPropertiesForEditOrAdd(object entity)
		{
			var jsonObject = JObject.FromObject(entity);
			jsonObject.Remove("BookID");
			jsonObject.Remove("UserID");
			jsonObject.Remove("WorkshopID");
			jsonObject.Remove("CoverImage");
			return jsonObject.ToString(Formatting.Indented);
		}

		private void JoinItems(object entity)
		{
			int itemId;
			string itemType;

			switch (entity)
			{
				case Book book:
					itemId = book.BookID;
					itemType = "Book";
					break;
				case User user:
					itemId = user.UserID;
					itemType = "User";
					break;
				case Workshop workshop:
					itemId = workshop.WorkshopID;
					itemType = "Workshop";
					break;
				default:
					throw new InvalidOperationException("Unknown entity type");
			}

			var joinForm = new JoinForm(itemId, itemType);
			joinForm.ShowDialog();
		}

		private async Task<List<User>> GetUsersByBookId(int bookId)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}UserBook/{bookId}/Users");
			response.EnsureSuccessStatusCode();
			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
		}

		private async Task<List<Book>> GetBooksByUserId(int userId)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}UserBook/{userId}/Books");
			response.EnsureSuccessStatusCode();
			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Book>>(jsonResponse);
		}

		private async Task<List<User>> GetUsersByWorkshopId(int workshopId)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}UserWorkshop/{workshopId}/Users");
			response.EnsureSuccessStatusCode();
			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
		}

		private async Task<List<Workshop>> GetWorkshopsByUserId(int userId)
		{
			var response = await _httpClient.GetAsync($"{ApiBaseUrl}UserWorkshop/{userId}/Workshops");
			response.EnsureSuccessStatusCode();
			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Workshop>>(jsonResponse);
		}
	}
}
