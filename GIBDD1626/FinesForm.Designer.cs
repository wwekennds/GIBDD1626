namespace GIBDD1626
{
    partial class FinesForm
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
            this.btnLoadJson = new System.Windows.Forms.Button();
            this.dgvFines = new System.Windows.Forms.DataGridView();
            this.txtManualNumber = new System.Windows.Forms.TextBox();
            this.cmbAccidentType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(15, 210);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(121, 23);
            this.btnLoadJson.TabIndex = 0;
            this.btnLoadJson.Text = "Загрузить JSON";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // dgvFines
            // 
            this.dgvFines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFines.Location = new System.Drawing.Point(12, 12);
            this.dgvFines.Name = "dgvFines";
            this.dgvFines.Size = new System.Drawing.Size(776, 150);
            this.dgvFines.TabIndex = 1;
            this.dgvFines.SelectionChanged += new System.EventHandler(this.dgvFines_SelectionChanged);
            // 
            // txtManualNumber
            // 
            this.txtManualNumber.Location = new System.Drawing.Point(15, 178);
            this.txtManualNumber.Name = "txtManualNumber";
            this.txtManualNumber.Size = new System.Drawing.Size(248, 20);
            this.txtManualNumber.TabIndex = 2;
            // 
            // cmbAccidentType
            // 
            this.cmbAccidentType.FormattingEnabled = true;
            this.cmbAccidentType.Location = new System.Drawing.Point(142, 212);
            this.cmbAccidentType.Name = "cmbAccidentType";
            this.cmbAccidentType.Size = new System.Drawing.Size(121, 21);
            this.cmbAccidentType.TabIndex = 3;
            this.cmbAccidentType.SelectedIndexChanged += new System.EventHandler(this.cmbAccidentType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(431, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Информация о штрафе (Дата, статья, сумма)";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 249);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(201, 249);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(170, 23);
            this.btnSkip.TabIndex = 6;
            this.btnSkip.Text = "Пропустить";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Номер транспорта";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(269, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Выберите тип ДТП";
            // 
            // FinesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAccidentType);
            this.Controls.Add(this.txtManualNumber);
            this.Controls.Add(this.dgvFines);
            this.Controls.Add(this.btnLoadJson);
            this.Name = "FinesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обработка штрафов";
            this.Load += new System.EventHandler(this.FinesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadJson;
        private System.Windows.Forms.DataGridView dgvFines;
        private System.Windows.Forms.TextBox txtManualNumber;
        private System.Windows.Forms.ComboBox cmbAccidentType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}