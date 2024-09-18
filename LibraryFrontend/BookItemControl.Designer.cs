namespace LibraryFrontend
{
	partial class BookItemControl
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
			this.SuspendLayout();
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Location = new System.Drawing.Point(23, 20);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(23, 13);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "title";
			// 
			// authorLabel
			// 
			this.authorLabel.AutoSize = true;
			this.authorLabel.Location = new System.Drawing.Point(23, 35);
			this.authorLabel.Name = "authorLabel";
			this.authorLabel.Size = new System.Drawing.Size(37, 13);
			this.authorLabel.TabIndex = 1;
			this.authorLabel.Text = "author";
			// 
			// BookItemControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.authorLabel);
			this.Controls.Add(this.titleLabel);
			this.Name = "BookItemControl";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.Size = new System.Drawing.Size(379, 68);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label authorLabel;
	}
}
