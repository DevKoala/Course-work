namespace work
{
    partial class EditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditForm));
            this.labelTitle = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.Add_country = new System.Windows.Forms.Button();
            this.Delete_country = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Berlin Sans FB", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelTitle.Location = new System.Drawing.Point(108, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(132, 32);
            this.labelTitle.TabIndex = 5;
            this.labelTitle.Text = "Рок-група";
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.SystemColors.Control;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(253, 289);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(40, 40);
            this.button4.TabIndex = 6;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Add_country
            // 
            this.Add_country.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Add_country.ForeColor = System.Drawing.SystemColors.Control;
            this.Add_country.Image = ((System.Drawing.Image)(resources.GetObject("Add_country.Image")));
            this.Add_country.Location = new System.Drawing.Point(58, 289);
            this.Add_country.Name = "Add_country";
            this.Add_country.Size = new System.Drawing.Size(40, 40);
            this.Add_country.TabIndex = 7;
            this.Add_country.UseVisualStyleBackColor = true;
            // 
            // Delete_country
            // 
            this.Delete_country.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Delete_country.ForeColor = System.Drawing.SystemColors.Control;
            this.Delete_country.Image = ((System.Drawing.Image)(resources.GetObject("Delete_country.Image")));
            this.Delete_country.Location = new System.Drawing.Point(160, 289);
            this.Delete_country.Name = "Delete_country";
            this.Delete_country.Size = new System.Drawing.Size(40, 40);
            this.Delete_country.TabIndex = 8;
            this.Delete_country.UseVisualStyleBackColor = true;
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 341);
            this.Controls.Add(this.Delete_country);
            this.Controls.Add(this.Add_country);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditForm";
            this.Text = "EditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Add_country;
        private System.Windows.Forms.Button Delete_country;
    }
}