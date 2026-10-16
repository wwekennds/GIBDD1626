namespace GIBDD1626
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.btnCards = new System.Windows.Forms.Button();
            this.btnFine = new System.Windows.Forms.Button();
            this.btnCreateDriver = new System.Windows.Forms.Button();
            this.btnCreateVehicle = new System.Windows.Forms.Button();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCards
            // 
            this.btnCards.Location = new System.Drawing.Point(344, 157);
            this.btnCards.Name = "btnCards";
            this.btnCards.Size = new System.Drawing.Size(82, 40);
            this.btnCards.TabIndex = 1;
            this.btnCards.Text = "Карточки";
            this.btnCards.UseVisualStyleBackColor = true;
            this.btnCards.Click += new System.EventHandler(this.btnCards_Click);
            // 
            // btnFine
            // 
            this.btnFine.Location = new System.Drawing.Point(479, 157);
            this.btnFine.Name = "btnFine";
            this.btnFine.Size = new System.Drawing.Size(82, 40);
            this.btnFine.TabIndex = 2;
            this.btnFine.Text = "Штрафы";
            this.btnFine.UseVisualStyleBackColor = true;
            this.btnFine.Click += new System.EventHandler(this.btnFine_Click);
            // 
            // btnCreateDriver
            // 
            this.btnCreateDriver.Location = new System.Drawing.Point(205, 157);
            this.btnCreateDriver.Name = "btnCreateDriver";
            this.btnCreateDriver.Size = new System.Drawing.Size(84, 40);
            this.btnCreateDriver.TabIndex = 4;
            this.btnCreateDriver.Text = "Создание водителя";
            this.btnCreateDriver.UseVisualStyleBackColor = true;
            this.btnCreateDriver.Click += new System.EventHandler(this.btnCreateDriver_Click);
            // 
            // btnCreateVehicle
            // 
            this.btnCreateVehicle.Location = new System.Drawing.Point(400, 240);
            this.btnCreateVehicle.Name = "btnCreateVehicle";
            this.btnCreateVehicle.Size = new System.Drawing.Size(120, 40);
            this.btnCreateVehicle.TabIndex = 5;
            this.btnCreateVehicle.Text = "Создать транспорт";
            this.btnCreateVehicle.UseVisualStyleBackColor = true;
            this.btnCreateVehicle.Click += new System.EventHandler(this.btnCreateVehicle_Click);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Location = new System.Drawing.Point(248, 240);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(120, 40);
            this.btnExportCSV.TabIndex = 6;
            this.btnExportCSV.Text = "Экспорт информации (CSV)";
            this.btnExportCSV.UseVisualStyleBackColor = true;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(324, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(134, 116);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExportCSV);
            this.Controls.Add(this.btnCreateVehicle);
            this.Controls.Add(this.btnCreateDriver);
            this.Controls.Add(this.btnFine);
            this.Controls.Add(this.btnCards);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Система";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCards;
        private System.Windows.Forms.Button btnFine;
        private System.Windows.Forms.Button btnCreateDriver;
        private System.Windows.Forms.Button btnCreateVehicle;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}