﻿using LibraryFrontend.LibraryFrontend;
using LibraryShared.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows.Forms;
using static LibraryBackend.Controllers.BookController;

namespace LibraryFrontend
{
	namespace LibraryFrontend
	{
		public enum FormMode
		{
			None,
			Edit,
			Add
		}
	}

	public partial class MainForm : Form
	{
		private readonly HttpClient _httpClient = new HttpClient();
		private const string ApiBaseUrl = "https://localhost:44312/api/";
		private string _currentView = "Book";
		private object _currentEntity;
		private FormMode _currentMode = FormMode.None;

		public MainForm()
		{
			InitializeComponent();
			InitializeButtons();
			LoadInitialEntities();
			pictureBox1.Click += PictureBox1_Click;
			deleteButton.Click += deleteButton_Click;
			cancelButton.Click += cancelButton_Click;
			UpdateCancelButtonState();
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
			try
			{
				_currentMode = FormMode.Edit;
				ShowEditTextBox();
				saveChangesButton.Enabled = true;
				UpdateCancelButtonState();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}");
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			editTextBox.Visible = false;
			groupBox1.Visible = true;
			saveChangesButton.Enabled = false;
			_currentMode = FormMode.None;
			UpdateCancelButtonState();
		}

		private async void saveChangesButton_Click(object sender, EventArgs e)
		{
			await SaveJsonChanges();
			saveChangesButton.Enabled = false;
			_currentMode = FormMode.None;
			UpdateCancelButtonState();
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

		private void createButton_Click(object sender, EventArgs e)
		{
			_currentMode = FormMode.Add;
			ShowEditTextBox();
			saveChangesButton.Enabled = true;
			UpdateCancelButtonState();
		}
	}
}