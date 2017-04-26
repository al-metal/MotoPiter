namespace MotoPiter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbLoginNethouse = new System.Windows.Forms.TextBox();
            this.tbPassNethouse = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbLoginMotopiter = new System.Windows.Forms.TextBox();
            this.tbPassMotopiter = new System.Windows.Forms.TextBox();
            this.btnActual = new System.Windows.Forms.Button();
            this.rtbMiniText = new System.Windows.Forms.RichTextBox();
            this.rtbFullText = new System.Windows.Forms.RichTextBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.btnImages = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLoginNethouse
            // 
            this.tbLoginNethouse.Location = new System.Drawing.Point(6, 19);
            this.tbLoginNethouse.Name = "tbLoginNethouse";
            this.tbLoginNethouse.Size = new System.Drawing.Size(100, 20);
            this.tbLoginNethouse.TabIndex = 0;
            // 
            // tbPassNethouse
            // 
            this.tbPassNethouse.Location = new System.Drawing.Point(112, 19);
            this.tbPassNethouse.Name = "tbPassNethouse";
            this.tbPassNethouse.Size = new System.Drawing.Size(100, 20);
            this.tbPassNethouse.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLoginNethouse);
            this.groupBox1.Controls.Add(this.tbPassNethouse);
            this.groupBox1.Location = new System.Drawing.Point(575, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 55);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "nethouse";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbLoginMotopiter);
            this.groupBox2.Controls.Add(this.tbPassMotopiter);
            this.groupBox2.Location = new System.Drawing.Point(575, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 55);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "motopiter";
            // 
            // tbLoginMotopiter
            // 
            this.tbLoginMotopiter.Location = new System.Drawing.Point(6, 19);
            this.tbLoginMotopiter.Name = "tbLoginMotopiter";
            this.tbLoginMotopiter.Size = new System.Drawing.Size(100, 20);
            this.tbLoginMotopiter.TabIndex = 0;
            // 
            // tbPassMotopiter
            // 
            this.tbPassMotopiter.Location = new System.Drawing.Point(112, 19);
            this.tbPassMotopiter.Name = "tbPassMotopiter";
            this.tbPassMotopiter.Size = new System.Drawing.Size(100, 20);
            this.tbPassMotopiter.TabIndex = 1;
            // 
            // btnActual
            // 
            this.btnActual.Location = new System.Drawing.Point(581, 134);
            this.btnActual.Name = "btnActual";
            this.btnActual.Size = new System.Drawing.Size(206, 23);
            this.btnActual.TabIndex = 4;
            this.btnActual.Text = "Обработать сайт";
            this.btnActual.UseVisualStyleBackColor = true;
            // 
            // rtbMiniText
            // 
            this.rtbMiniText.Location = new System.Drawing.Point(12, 12);
            this.rtbMiniText.Name = "rtbMiniText";
            this.rtbMiniText.Size = new System.Drawing.Size(557, 116);
            this.rtbMiniText.TabIndex = 5;
            this.rtbMiniText.Text = "";
            // 
            // rtbFullText
            // 
            this.rtbFullText.Location = new System.Drawing.Point(12, 134);
            this.rtbFullText.Name = "rtbFullText";
            this.rtbFullText.Size = new System.Drawing.Size(557, 116);
            this.rtbFullText.TabIndex = 6;
            this.rtbFullText.Text = "";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(12, 256);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(557, 20);
            this.tbTitle.TabIndex = 7;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(12, 282);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(557, 20);
            this.tbDescription.TabIndex = 8;
            // 
            // tbKeywords
            // 
            this.tbKeywords.Location = new System.Drawing.Point(12, 308);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(557, 20);
            this.tbKeywords.TabIndex = 9;
            // 
            // btnImages
            // 
            this.btnImages.Location = new System.Drawing.Point(581, 163);
            this.btnImages.Name = "btnImages";
            this.btnImages.Size = new System.Drawing.Size(206, 23);
            this.btnImages.TabIndex = 10;
            this.btnImages.Text = "Обработать картинки";
            this.btnImages.UseVisualStyleBackColor = true;
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(581, 192);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(206, 23);
            this.btnSaveTemplate.TabIndex = 11;
            this.btnSaveTemplate.Text = "Сохранить шаблон";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 335);
            this.Controls.Add(this.btnSaveTemplate);
            this.Controls.Add(this.btnImages);
            this.Controls.Add(this.tbKeywords);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.rtbFullText);
            this.Controls.Add(this.rtbMiniText);
            this.Controls.Add(this.btnActual);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "MotoPiter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLoginNethouse;
        private System.Windows.Forms.TextBox tbPassNethouse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbLoginMotopiter;
        private System.Windows.Forms.TextBox tbPassMotopiter;
        private System.Windows.Forms.Button btnActual;
        private System.Windows.Forms.RichTextBox rtbMiniText;
        private System.Windows.Forms.RichTextBox rtbFullText;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.Button btnImages;
        private System.Windows.Forms.Button btnSaveTemplate;
    }
}

