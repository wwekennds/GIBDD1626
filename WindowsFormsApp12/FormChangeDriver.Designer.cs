namespace WindowsFormsApp12
{
    partial class FormChangeDriver
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
            this.components = new System.ComponentModel.Container();
            this.maskedTxtBoxPhoneNumber = new System.Windows.Forms.MaskedTextBox();
            this.btnChangeOkDriver = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxStreetResident = new System.Windows.Forms.TextBox();
            this.textBoxStreetRegister = new System.Windows.Forms.TextBox();
            this.btnChoseImage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.textBoxPost = new System.Windows.Forms.TextBox();
            this.textBoxWorkPlace = new System.Windows.Forms.TextBox();
            this.textBoxCityResident = new System.Windows.Forms.TextBox();
            this.textBoxCityRegister = new System.Windows.Forms.TextBox();
            this.textBoxPassport = new System.Windows.Forms.TextBox();
            this.textBoxPatromic = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxSurName = new System.Windows.Forms.TextBox();
            this.textBoxIDDriver = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // maskedTxtBoxPhoneNumber
            // 
            this.maskedTxtBoxPhoneNumber.Location = new System.Drawing.Point(180, 355);
            this.maskedTxtBoxPhoneNumber.Mask = "+7 (000) 000-00-00";
            this.maskedTxtBoxPhoneNumber.Name = "maskedTxtBoxPhoneNumber";
            this.maskedTxtBoxPhoneNumber.RejectInputOnFirstFailure = true;
            this.maskedTxtBoxPhoneNumber.Size = new System.Drawing.Size(100, 20);
            this.maskedTxtBoxPhoneNumber.TabIndex = 78;
            this.maskedTxtBoxPhoneNumber.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // btnChangeOkDriver
            // 
            this.btnChangeOkDriver.Location = new System.Drawing.Point(688, 394);
            this.btnChangeOkDriver.Name = "btnChangeOkDriver";
            this.btnChangeOkDriver.Size = new System.Drawing.Size(75, 23);
            this.btnChangeOkDriver.TabIndex = 77;
            this.btnChangeOkDriver.Text = "Готово";
            this.btnChangeOkDriver.UseVisualStyleBackColor = true;
            this.btnChangeOkDriver.Click += new System.EventHandler(this.btnChangeOkDriver_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(412, 313);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(85, 111);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 76;
            this.pictureBox1.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(596, 108);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(42, 13);
            this.label16.TabIndex = 75;
            this.label16.Text = "Улица:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(387, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 13);
            this.label15.TabIndex = 74;
            this.label15.Text = "Улица:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(598, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 73;
            this.label14.Text = "Город:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(387, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 13);
            this.label13.TabIndex = 72;
            this.label13.Text = "Город:";
            // 
            // textBoxStreetResident
            // 
            this.textBoxStreetResident.Location = new System.Drawing.Point(433, 108);
            this.textBoxStreetResident.Name = "textBoxStreetResident";
            this.textBoxStreetResident.Size = new System.Drawing.Size(100, 20);
            this.textBoxStreetResident.TabIndex = 71;
            // 
            // textBoxStreetRegister
            // 
            this.textBoxStreetRegister.Location = new System.Drawing.Point(644, 108);
            this.textBoxStreetRegister.Name = "textBoxStreetRegister";
            this.textBoxStreetRegister.Size = new System.Drawing.Size(100, 20);
            this.textBoxStreetRegister.TabIndex = 70;
            // 
            // btnChoseImage
            // 
            this.btnChoseImage.Location = new System.Drawing.Point(485, 259);
            this.btnChoseImage.Name = "btnChoseImage";
            this.btnChoseImage.Size = new System.Drawing.Size(87, 23);
            this.btnChoseImage.TabIndex = 69;
            this.btnChoseImage.Text = "Выбрать фото";
            this.btnChoseImage.UseVisualStyleBackColor = true;
            this.btnChoseImage.Click += new System.EventHandler(this.btnChoseImage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 68;
            this.label2.Text = "Имя:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(398, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 67;
            this.label7.Text = "Замечания:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(409, 264);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 66;
            this.label8.Text = "Фотография:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(139, 411);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "Email:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 13);
            this.label10.TabIndex = 64;
            this.label10.Text = "Мобильный телефон:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(103, 301);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 63;
            this.label11.Text = "Должность :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(92, 246);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Место работы:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(427, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "Адрес проживания:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(641, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Адрес регистрации:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "Паспорт (серия и номер):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Отчество:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "Фамилия:";
            // 
            // textBoxRemarks
            // 
            this.textBoxRemarks.Location = new System.Drawing.Point(470, 165);
            this.textBoxRemarks.Multiline = true;
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.Size = new System.Drawing.Size(168, 81);
            this.textBoxRemarks.TabIndex = 56;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(180, 408);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(100, 20);
            this.textBoxEmail.TabIndex = 55;
            // 
            // textBoxPost
            // 
            this.textBoxPost.Location = new System.Drawing.Point(180, 298);
            this.textBoxPost.Name = "textBoxPost";
            this.textBoxPost.Size = new System.Drawing.Size(100, 20);
            this.textBoxPost.TabIndex = 54;
            // 
            // textBoxWorkPlace
            // 
            this.textBoxWorkPlace.Location = new System.Drawing.Point(180, 243);
            this.textBoxWorkPlace.Name = "textBoxWorkPlace";
            this.textBoxWorkPlace.Size = new System.Drawing.Size(100, 20);
            this.textBoxWorkPlace.TabIndex = 53;
            // 
            // textBoxCityResident
            // 
            this.textBoxCityResident.Location = new System.Drawing.Point(433, 39);
            this.textBoxCityResident.Name = "textBoxCityResident";
            this.textBoxCityResident.Size = new System.Drawing.Size(100, 20);
            this.textBoxCityResident.TabIndex = 52;
            // 
            // textBoxCityRegister
            // 
            this.textBoxCityRegister.Location = new System.Drawing.Point(644, 39);
            this.textBoxCityRegister.Name = "textBoxCityRegister";
            this.textBoxCityRegister.Size = new System.Drawing.Size(100, 20);
            this.textBoxCityRegister.TabIndex = 51;
            // 
            // textBoxPassport
            // 
            this.textBoxPassport.Location = new System.Drawing.Point(180, 185);
            this.textBoxPassport.Name = "textBoxPassport";
            this.textBoxPassport.Size = new System.Drawing.Size(100, 20);
            this.textBoxPassport.TabIndex = 50;
            // 
            // textBoxPatromic
            // 
            this.textBoxPatromic.Location = new System.Drawing.Point(180, 125);
            this.textBoxPatromic.Name = "textBoxPatromic";
            this.textBoxPatromic.Size = new System.Drawing.Size(100, 20);
            this.textBoxPatromic.TabIndex = 49;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(180, 69);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 48;
            // 
            // textBoxSurName
            // 
            this.textBoxSurName.Location = new System.Drawing.Point(180, 14);
            this.textBoxSurName.Name = "textBoxSurName";
            this.textBoxSurName.Size = new System.Drawing.Size(100, 20);
            this.textBoxSurName.TabIndex = 47;
            // 
            // textBoxIDDriver
            // 
            this.textBoxIDDriver.Location = new System.Drawing.Point(653, 423);
            this.textBoxIDDriver.Name = "textBoxIDDriver";
            this.textBoxIDDriver.ReadOnly = true;
            this.textBoxIDDriver.Size = new System.Drawing.Size(110, 20);
            this.textBoxIDDriver.TabIndex = 79;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(579, 427);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(68, 13);
            this.label17.TabIndex = 80;
            this.label17.Text = "ID водителя";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"Image Files|*.jpg;*.jpeg;*.png;\"";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FormChangeDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBoxIDDriver);
            this.Controls.Add(this.maskedTxtBoxPhoneNumber);
            this.Controls.Add(this.btnChangeOkDriver);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxStreetResident);
            this.Controls.Add(this.textBoxStreetRegister);
            this.Controls.Add(this.btnChoseImage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxRemarks);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxPost);
            this.Controls.Add(this.textBoxWorkPlace);
            this.Controls.Add(this.textBoxCityResident);
            this.Controls.Add(this.textBoxCityRegister);
            this.Controls.Add(this.textBoxPassport);
            this.Controls.Add(this.textBoxPatromic);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxSurName);
            this.Name = "FormChangeDriver";
            this.Text = "FormChangeDriver";
            this.Load += new System.EventHandler(this.FormChangeDriver_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTxtBoxPhoneNumber;
        private System.Windows.Forms.Button btnChangeOkDriver;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxStreetResident;
        private System.Windows.Forms.TextBox textBoxStreetRegister;
        private System.Windows.Forms.Button btnChoseImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRemarks;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxPost;
        private System.Windows.Forms.TextBox textBoxWorkPlace;
        private System.Windows.Forms.TextBox textBoxCityResident;
        private System.Windows.Forms.TextBox textBoxCityRegister;
        private System.Windows.Forms.TextBox textBoxPassport;
        private System.Windows.Forms.TextBox textBoxPatromic;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxSurName;
        private System.Windows.Forms.TextBox textBoxIDDriver;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}