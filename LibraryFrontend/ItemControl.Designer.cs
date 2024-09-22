namespace LibraryFrontend
{
	partial class ItemControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.titleLabel = new System.Windows.Forms.Label();
			this.authorLabel = new System.Windows.Forms.Label();
			this.thumbnailPictureBox = new System.Windows.Forms.PictureBox();
			this.joinButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Location = new System.Drawing.Point(52, 20);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(23, 13);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "title";
			// 
			// authorLabel
			// 
			this.authorLabel.AutoSize = true;
			this.authorLabel.Location = new System.Drawing.Point(52, 35);
			this.authorLabel.Name = "authorLabel";
			this.authorLabel.Size = new System.Drawing.Size(37, 13);
			this.authorLabel.TabIndex = 1;
			this.authorLabel.Text = "author";
			// 
			// thumbnailPictureBox
			// 
			this.thumbnailPictureBox.Location = new System.Drawing.Point(11, 10);
			this.thumbnailPictureBox.Name = "thumbnailPictureBox";
			this.thumbnailPictureBox.Size = new System.Drawing.Size(35, 47);
			this.thumbnailPictureBox.TabIndex = 2;
			this.thumbnailPictureBox.TabStop = false;
			// 
			// joinButton
			// 
			this.joinButton.Location = new System.Drawing.Point(270, 23);
			this.joinButton.Name = "joinButton";
			this.joinButton.Size = new System.Drawing.Size(57, 23);
			this.joinButton.TabIndex = 3;
			this.joinButton.Text = "Pridjeli";
			this.joinButton.UseVisualStyleBackColor = true;
			this.joinButton.Visible = false;
			// 
			// ItemControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.joinButton);
			this.Controls.Add(this.thumbnailPictureBox);
			this.Controls.Add(this.authorLabel);
			this.Controls.Add(this.titleLabel);
			this.Name = "ItemControl";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.Size = new System.Drawing.Size(350, 65);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label authorLabel;
		private System.Windows.Forms.PictureBox thumbnailPictureBox;
		private System.Windows.Forms.Button joinButton;
	}
}
