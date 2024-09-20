using LibraryShared.Converters;
using LibraryShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LibraryBackend.Controllers.BookController;

namespace LibraryFrontend
{
	public partial class MainForm : Form
	{
		private readonly HttpClient _httpClient = new HttpClient();
		private const string ApiBaseUrl = "https://localhost:44312/api/";
		private string _currentView = "Book"; // Track the current view
		private object _currentEntity; // Track the current entity

		public MainForm()
		{
			InitializeComponent();
			InitializeButtons();
			LoadInitialEntities();
			pictureBox1.Click += PictureBox1_Click;
			deleteButton.Click += deleteButton_Click;
		}

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
			flowLayoutPanel1.Controls.Add(bookItem);
		}

		private void DisplayUser(User user)
		{
			var userItem = new ItemControl($"{user.FirstName} {user.LastName}", user.Email, null);
			userItem.ItemClicked += (sender, e) => ShowUserDetails(user);
			flowLayoutPanel1.Controls.Add(userItem);
		}

		private void DisplayWorkshop(Workshop workshop)
		{
			var workshopItem = new ItemControl(workshop.Name, $"Attendees: {workshop.NumberOfAttendees}", null);
			workshopItem.ItemClicked += (sender, e) => ShowWorkshopDetails(workshop);
			flowLayoutPanel1.Controls.Add(workshopItem);
		}

		private void ShowBookDetails(Book book)
		{
			_currentEntity = book;
			string borrowingAllowedHrv = book.BorrowingAllowed ? "Posudba dozvoljena" : "Posudba nije dozvoljena";

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

		private void ShowUserDetails(User user)
		{
			_currentEntity = user;
			groupBox1.Controls.Clear();

			var nameLabel = new Label { Text = $"Name: {user.FirstName} {user.LastName}", AutoSize = true, Location = new System.Drawing.Point(10, 20) };
			var emailLabel = new Label { Text = $"Email: {user.Email}", AutoSize = true, Location = new System.Drawing.Point(10, 50) };
			var addressLabel = new Label { Text = $"Address: {user.Address}", AutoSize = true, Location = new System.Drawing.Point(10, 80) };
			var dobLabel = new Label { Text = $"Date of Birth: {user.DateOfBirth.ToShortDateString()}", AutoSize = true, Location = new System.Drawing.Point(10, 110) };
			var phoneLabel = new Label { Text = $"Phone: {user.PhoneNumber}", AutoSize = true, Location = new System.Drawing.Point(10, 140) };

			groupBox1.Controls.Add(nameLabel);
			groupBox1.Controls.Add(emailLabel);
			groupBox1.Controls.Add(addressLabel);
			groupBox1.Controls.Add(dobLabel);
			groupBox1.Controls.Add(phoneLabel);
		}

		private void ShowWorkshopDetails(Workshop workshop)
		{
			_currentEntity = workshop;
			groupBox1.Controls.Clear();

			var nameLabel = new Label { Text = $"Name: {workshop.Name}", AutoSize = true, Location = new System.Drawing.Point(10, 20) };
			var attendeesLabel = new Label { Text = $"Number of Attendees: {workshop.NumberOfAttendees}", AutoSize = true, Location = new System.Drawing.Point(10, 50) };
			var durationLabel = new Label { Text = $"Duration: {workshop.DurationMinutes} minutes", AutoSize = true, Location = new System.Drawing.Point(10, 80) };
			var startDateLabel = new Label { Text = $"Start Date: {workshop.StartDate.ToShortDateString()}", AutoSize = true, Location = new System.Drawing.Point(10, 110) };

			groupBox1.Controls.Add(nameLabel);
			groupBox1.Controls.Add(attendeesLabel);
			groupBox1.Controls.Add(durationLabel);
			groupBox1.Controls.Add(startDateLabel);
		}

		private void ShowEditTextBox()
		{
			if (_currentEntity != null)
			{
				editTextBox.Text = JsonConvert.SerializeObject(_currentEntity, Formatting.Indented);
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

		private async void clearSearchButton_Click(object sender, EventArgs e)
		{
			searchTextBox.Text = string.Empty;
			await LoadEntitiesBasedOnCurrentView();
		}
		private async void PictureBox1_Click(object sender, EventArgs e)
		{
			if (_currentEntity is Book book)
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.Filter = "JPG files (*.jpg)|*.jpg";
					openFileDialog.Title = "Odaberite .jpg sliku";

					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						try
						{
							byte[] imageBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);
							string base64Image = Convert.ToBase64String(imageBytes);

							var model = new ImageUploadModel { Image = base64Image };
							var jsonContent = JsonConvert.SerializeObject(model);
							var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

							var response = await _httpClient.PostAsync($"{ApiBaseUrl}Book/uploadCoverImage/{book.BookID}", content);
							response.EnsureSuccessStatusCode();

							MessageBox.Show("Slika naslovnice uspiješno uploadana.");
							book.CoverImage = imageBytes; // Update the local book object
							ShowBookDetails(book); // Refresh the book details to show the new image
						}
						catch (Exception ex)
						{
							MessageBox.Show($"Error uploading image: {ex.Message}");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Samo knjige mogu imati sliku.");
			}
		}
		private void editButton_Click(object sender, EventArgs e)
		{
			if (editTextBox.Visible)
			{
				// Cancel editing
				editTextBox.Visible = false;
				groupBox1.Visible = true;
				editButton.Text = "Izmjeni";
				saveChangesButton.Enabled = false;
			}
			else
			{
				// Start editing
				ShowEditTextBox();
				editButton.Text = "Otkaži";
				saveChangesButton.Enabled = true;
			}
		}
		private async void saveChangesButton_Click(object sender, EventArgs e)
		{
			await SaveJsonChanges();
			editButton.Text = "Izmjeni";
			saveChangesButton.Enabled = false;
		}
		private async void deleteButton_Click(object sender, EventArgs e)
		{
			if (_currentEntity == null)
			{
				MessageBox.Show("Odaberite predmet za brisanje.");
				return;
			}

			var result = MessageBox.Show("Jeste li sigurni da želite obrisati odabrani predmet?", "Brisanje", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				try
				{
					var entityId = GetEntityId(_currentEntity);
					var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}{_currentView}/{entityId}");
					response.EnsureSuccessStatusCode();

					MessageBox.Show("Brisanje uspješno.");
					_currentEntity = null; // Clear the current entity
					await LoadEntitiesBasedOnCurrentView(); // Refresh the list of entities
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error deleting item: {ex.Message}");
				}
			}
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
						BorrowingAllowed = false,
						CoverImage = (byte[])null
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
		private void createButton_Click(object sender, EventArgs e)
		{
			var jsonTemplate = GetJsonTemplate(_currentView);
			editTextBox.Text = jsonTemplate;
			editTextBox.Visible = true;
			groupBox1.Visible = false;
			editButton.Text = "Otkaži";
			saveChangesButton.Enabled = true;
			_currentEntity = null; // Clear the current entity to indicate a new creation
		}
	}
}