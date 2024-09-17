namespace LibraryFrontend
{
	partial class loginForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.usernameBox = new System.Windows.Forms.TextBox();
			this.usernameLabel = new System.Windows.Forms.Label();
			this.pinLabel = new System.Windows.Forms.Label();
			this.pinBox = new System.Windows.Forms.TextBox();
			this.loginButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// usernameBox
			// 
			this.usernameBox.Location = new System.Drawing.Point(41, 53);
			this.usernameBox.Name = "usernameBox";
			this.usernameBox.Size = new System.Drawing.Size(166, 20);
			this.usernameBox.TabIndex = 0;
			this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
			// 
			// usernameLabel
			// 
			this.usernameLabel.AutoSize = true;
			this.usernameLabel.Location = new System.Drawing.Point(38, 37);
			this.usernameLabel.Name = "usernameLabel";
			this.usernameLabel.Size = new System.Drawing.Size(75, 13);
			this.usernameLabel.TabIndex = 1;
			this.usernameLabel.Text = "Korisničko ime";
			// 
			// pinLabel
			// 
			this.pinLabel.AutoSize = true;
			this.pinLabel.Location = new System.Drawing.Point(38, 87);
			this.pinLabel.Name = "pinLabel";
			this.pinLabel.Size = new System.Drawing.Size(25, 13);
			this.pinLabel.TabIndex = 3;
			this.pinLabel.Text = "PIN";
			// 
			// pinBox
			// 
			this.pinBox.Location = new System.Drawing.Point(41, 103);
			this.pinBox.Name = "pinBox";
			this.pinBox.PasswordChar = '*';
			this.pinBox.Size = new System.Drawing.Size(166, 20);
			this.pinBox.TabIndex = 2;
			this.pinBox.UseSystemPasswordChar = true;
			this.pinBox.TextChanged += new System.EventHandler(this.pinBox_TextChanged);
			// 
			// loginButton
			// 
			this.loginButton.Location = new System.Drawing.Point(131, 138);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(76, 23);
			this.loginButton.TabIndex = 4;
			this.loginButton.Text = "Log in";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// loginForm
			// 
			this.AcceptButton = this.loginButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(264, 211);
			this.Controls.Add(this.loginButton);
			this.Controls.Add(this.pinLabel);
			this.Controls.Add(this.pinBox);
			this.Controls.Add(this.usernameLabel);
			this.Controls.Add(this.usernameBox);
			this.MaximizeBox = false;
			this.Name = "loginForm";
			this.Text = "Prijava";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label usernameLabel;
		private System.Windows.Forms.TextBox usernameBox;
		private System.Windows.Forms.Label pinLabel;
		private System.Windows.Forms.TextBox pinBox;
		private System.Windows.Forms.Button loginButton;
	}
}

