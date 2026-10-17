namespace FuturePathStudentAdvisingSystem
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtFirstName = new Label();
            textFirstName = new TextBox();
            lblLastName = new Label();
            txtLastName = new TextBox();
            lblSchoolName = new Label();
            lblProvince = new Label();
            lblGrade = new Label();
            txtSchoolName = new TextBox();
            btnBack = new Button();
            btnNext = new Button();
            btnCancel = new Button();
            cmbProvince = new ComboBox();
            cmbGrade = new ComboBox();
            txtEmail = new TextBox();
            lblEmail = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 29);
            label1.Name = "label1";
            label1.Size = new Size(604, 59);
            label1.TabIndex = 0;
            label1.Text = "Screen 1 of 7 - Leaner Profile";
            // 
            // txtFirstName
            // 
            txtFirstName.AutoSize = true;
            txtFirstName.Location = new Point(51, 124);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(155, 32);
            txtFirstName.TabIndex = 1;
            txtFirstName.Text = "First Name *";
            txtFirstName.Click += label2_Click;
            // 
            // textFirstName
            // 
            textFirstName.Location = new Point(51, 168);
            textFirstName.Name = "textFirstName";
            textFirstName.Size = new Size(370, 39);
            textFirstName.TabIndex = 2;
            // 
            // lblLastName
            // 
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(50, 228);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(133, 32);
            lblLastName.TabIndex = 3;
            lblLastName.Text = "Surname *";
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(51, 272);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(370, 39);
            txtLastName.TabIndex = 4;
            // 
            // lblSchoolName
            // 
            lblSchoolName.AutoSize = true;
            lblSchoolName.Location = new Point(51, 414);
            lblSchoolName.Name = "lblSchoolName";
            lblSchoolName.Size = new Size(182, 32);
            lblSchoolName.TabIndex = 5;
            lblSchoolName.Text = "School Name *";
            // 
            // lblProvince
            // 
            lblProvince.AutoSize = true;
            lblProvince.Location = new Point(51, 518);
            lblProvince.Name = "lblProvince";
            lblProvince.Size = new Size(132, 32);
            lblProvince.TabIndex = 6;
            lblProvince.Text = "Province *";
            lblProvince.Click += label4_Click;
            // 
            // lblGrade
            // 
            lblGrade.AutoSize = true;
            lblGrade.Location = new Point(51, 622);
            lblGrade.Name = "lblGrade";
            lblGrade.Size = new Size(194, 32);
            lblGrade.TabIndex = 7;
            lblGrade.Text = "Current Grade *";
            // 
            // txtSchoolName
            // 
            txtSchoolName.Location = new Point(51, 457);
            txtSchoolName.Name = "txtSchoolName";
            txtSchoolName.Size = new Size(370, 39);
            txtSchoolName.TabIndex = 8;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(293, 758);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(150, 46);
            btnBack.TabIndex = 11;
            btnBack.Text = "< Back";
            btnBack.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(489, 758);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(150, 46);
            btnNext.TabIndex = 12;
            btnNext.Text = "Next >";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(700, 758);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 46);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += button3_Click;
            // 
            // cmbProvince
            // 
            cmbProvince.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProvince.FormattingEnabled = true;
            cmbProvince.Location = new Point(51, 562);
            cmbProvince.Name = "cmbProvince";
            cmbProvince.Size = new Size(370, 40);
            cmbProvince.TabIndex = 14;
            // 
            // cmbGrade
            // 
            cmbGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGrade.FormattingEnabled = true;
            cmbGrade.Location = new Point(51, 669);
            cmbGrade.Name = "cmbGrade";
            cmbGrade.Size = new Size(370, 40);
            cmbGrade.TabIndex = 15;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(51, 357);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(370, 39);
            txtEmail.TabIndex = 16;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(51, 314);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(94, 32);
            lblEmail.TabIndex = 17;
            lblEmail.Text = "Email *";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 842);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(cmbGrade);
            Controls.Add(cmbProvince);
            Controls.Add(btnCancel);
            Controls.Add(btnNext);
            Controls.Add(btnBack);
            Controls.Add(txtSchoolName);
            Controls.Add(lblGrade);
            Controls.Add(lblProvince);
            Controls.Add(lblSchoolName);
            Controls.Add(txtLastName);
            Controls.Add(lblLastName);
            Controls.Add(textFirstName);
            Controls.Add(txtFirstName);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Name = "Form1";
            Text = "Future Path - Leaner Profile";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label txtFirstName;
        private TextBox textFirstName;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblSchoolName;
        private Label lblProvince;
        private Label lblGrade;
        private TextBox txtSchoolName;
        private Button btnBack;
        private Button btnNext;
        private Button btnCancel;
        private ComboBox cmbProvince;
        private ComboBox cmbGrade;
        private TextBox txtEmail;
        private Label lblEmail;
    }
}
