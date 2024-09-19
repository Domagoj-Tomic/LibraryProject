namespace LibraryFrontend
{
	partial class MainForm
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.searchTextBox = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.clearSearchButton = new System.Windows.Forms.Button();
			this.bookButton = new System.Windows.Forms.Button();
			this.userButton = new System.Windows.Forms.Button();
			this.workshopButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.saveChangesButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.editTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(228, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(246, 336);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(225, 588);
			this.flowLayoutPanel1.TabIndex = 1;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// searchTextBox
			// 
			this.searchTextBox.Location = new System.Drawing.Point(228, 342);
			this.searchTextBox.Name = "searchTextBox";
			this.searchTextBox.Size = new System.Drawing.Size(245, 20);
			this.searchTextBox.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(228, 368);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(65, 25);
			this.searchButton.TabIndex = 5;
			this.searchButton.Text = "Pretraži";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(477, 0);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(775, 588);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// clearSearchButton
			// 
			this.clearSearchButton.Location = new System.Drawing.Point(299, 368);
			this.clearSearchButton.Name = "clearSearchButton";
			this.clearSearchButton.Size = new System.Drawing.Size(97, 25);
			this.clearSearchButton.TabIndex = 6;
			this.clearSearchButton.Text = "Obriši pretragu";
			this.clearSearchButton.UseVisualStyleBackColor = true;
			this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
			// 
			// bookButton
			// 
			this.bookButton.Location = new System.Drawing.Point(228, 551);
			this.bookButton.Name = "bookButton";
			this.bookButton.Size = new System.Drawing.Size(68, 21);
			this.bookButton.TabIndex = 7;
			this.bookButton.Text = "Knjige";
			this.bookButton.UseVisualStyleBackColor = true;
			// 
			// userButton
			// 
			this.userButton.Location = new System.Drawing.Point(302, 551);
			this.userButton.Name = "userButton";
			this.userButton.Size = new System.Drawing.Size(68, 21);
			this.userButton.TabIndex = 8;
			this.userButton.Text = "Korisnici";
			this.userButton.UseVisualStyleBackColor = true;
			// 
			// workshopButton
			// 
			this.workshopButton.Location = new System.Drawing.Point(376, 551);
			this.workshopButton.Name = "workshopButton";
			this.workshopButton.Size = new System.Drawing.Size(63, 21);
			this.workshopButton.TabIndex = 9;
			this.workshopButton.Text = "Radionice";
			this.workshopButton.UseVisualStyleBackColor = true;
			// 
			// editButton
			// 
			this.editButton.Location = new System.Drawing.Point(1006, 552);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(112, 27);
			this.editButton.TabIndex = 15;
			this.editButton.Text = "Izmjeni";
			this.editButton.UseVisualStyleBackColor = true;
			// 
			// saveChangesButton
			// 
			this.saveChangesButton.Location = new System.Drawing.Point(1124, 552);
			this.saveChangesButton.Name = "saveChangesButton";
			this.saveChangesButton.Size = new System.Drawing.Size(112, 27);
			this.saveChangesButton.TabIndex = 14;
			this.saveChangesButton.Text = "Spremi promjene";
			this.saveChangesButton.UseVisualStyleBackColor = true;
			// 
			// deleteButton
			// 
			this.deleteButton.Location = new System.Drawing.Point(888, 552);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(112, 27);
			this.deleteButton.TabIndex = 13;
			this.deleteButton.Text = "Obriši";
			this.deleteButton.UseVisualStyleBackColor = true;
			// 
			// editTextBox
			// 
			this.editTextBox.Location = new System.Drawing.Point(477, 0);
			this.editTextBox.Multiline = true;
			this.editTextBox.Name = "editTextBox";
			this.editTextBox.Size = new System.Drawing.Size(758, 539);
			this.editTextBox.TabIndex = 16;
			this.editTextBox.Visible = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1247, 584);
			this.Controls.Add(this.editTextBox);
			this.Controls.Add(this.editButton);
			this.Controls.Add(this.saveChangesButton);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.workshopButton);
			this.Controls.Add(this.userButton);
			this.Controls.Add(this.bookButton);
			this.Controls.Add(this.clearSearchButton);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.searchTextBox);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "MainForm";
			this.Text = "Knjižnica";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TextBox searchTextBox;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button clearSearchButton;
		private System.Windows.Forms.Button bookButton;
		private System.Windows.Forms.Button userButton;
		private System.Windows.Forms.Button workshopButton;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button saveChangesButton;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.TextBox editTextBox;
	}
}