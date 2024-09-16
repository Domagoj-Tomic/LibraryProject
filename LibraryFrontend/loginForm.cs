using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace LibraryFrontend
{
	public partial class loginForm : Form
	{
		public loginForm()
		{
			InitializeComponent();
		}

		private void usernameBox_TextChanged(object sender, EventArgs e)
		{
		}

		private void pinBox_TextChanged(object sender, EventArgs e)
		{
		}

		private async void loginButton_Click(object sender, EventArgs e)
		{
			loginButton.Enabled = false; // Disable the button to prevent multiple clicks
			string username = usernameBox.Text;
			string pin = pinBox.Text;

			var loginDetails = new { Username = username, Pin = pin };
			string json = JsonConvert.SerializeObject(loginDetails);

			using (HttpClient client = new HttpClient())
			{
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				try
				{
					HttpResponseMessage response = await client.PostAsync("https://localhost:44312/api/login", content);

					if (response.IsSuccessStatusCode)
					{
						this.Hide(); // Hide the login form
						MainForm mainForm = new MainForm();
						mainForm.ShowDialog(); // Show the main form
						this.Close(); // Close the login form after the main form is closed
					}
					else
					{
						MessageBox.Show("Login failed!");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
				finally
				{
					loginButton.Enabled = true; // Re-enable the button after the operation is complete
				}
			}
		}
	}
}
