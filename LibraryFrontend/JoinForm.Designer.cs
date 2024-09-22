namespace LibraryFrontend
{
	partial class JoinForm
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.workshopButton = new System.Windows.Forms.Button();
			this.bookButton = new System.Windows.Forms.Button();
			this.clearSearchButton = new System.Windows.Forms.Button();
			this.searchButton = new System.Windows.Forms.Button();
			this.searchTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(350, 590);
			this.flowLayoutPanel1.TabIndex = 2;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// workshopButton
			// 
			this.workshopButton.Location = new System.Drawing.Point(427, 69);
			this.workshopButton.Name = "workshopButton";
			this.workshopButton.Size = new System.Drawing.Size(68, 21);
			this.workshopButton.TabIndex = 15;
			this.workshopButton.Text = "Radionice";
			this.workshopButton.UseVisualStyleBackColor = true;
			// 
			// bookButton
			// 
			this.bookButton.Location = new System.Drawing.Point(353, 69);
			this.bookButton.Name = "bookButton";
			this.bookButton.Size = new System.Drawing.Size(68, 21);
			this.bookButton.TabIndex = 13;
			this.bookButton.Text = "Knjige";
			this.bookButton.UseVisualStyleBackColor = true;
			// 
			// clearSearchButton
			// 
			this.clearSearchButton.Location = new System.Drawing.Point(424, 38);
			this.clearSearchButton.Name = "clearSearchButton";
			this.clearSearchButton.Size = new System.Drawing.Size(97, 25);
			this.clearSearchButton.TabIndex = 12;
			this.clearSearchButton.Text = "Obriši pretragu";
			this.clearSearchButton.UseVisualStyleBackColor = true;
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(353, 38);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(65, 25);
			this.searchButton.TabIndex = 11;
			this.searchButton.Text = "Pretraži";
			this.searchButton.UseVisualStyleBackColor = true;
			// 
			// searchTextBox
			// 
			this.searchTextBox.Location = new System.Drawing.Point(353, 12);
			this.searchTextBox.Name = "searchTextBox";
			this.searchTextBox.Size = new System.Drawing.Size(245, 20);
			this.searchTextBox.TabIndex = 10;
			// 
			// JoinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(608, 555);
			this.Controls.Add(this.workshopButton);
			this.Controls.Add(this.bookButton);
			this.Controls.Add(this.clearSearchButton);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.searchTextBox);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "JoinForm";
			this.Text = "Pridjeli";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button workshopButton;
		private System.Windows.Forms.Button bookButton;
		private System.Windows.Forms.Button clearSearchButton;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.TextBox searchTextBox;
	}
}