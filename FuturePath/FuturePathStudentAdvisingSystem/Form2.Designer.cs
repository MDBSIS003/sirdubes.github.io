namespace FuturePathStudentAdvisingSystem
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            lblMark = new Label();
            lblTitle = new Label();
            lblSubject = new Label();
            cmbSubject = new ComboBox();
            btnAddSubject = new Button();
            btnBack = new Button();
            btnNext = new Button();
            nudMark = new NumericUpDown();
            dgvSubjects = new DataGridView();
            colSubject = new DataGridViewTextBoxColumn();
            colMark = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)nudMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).BeginInit();
            SuspendLayout();
            // 
            // lblMark
            // 
            lblMark.AutoSize = true;
            lblMark.Location = new Point(45, 179);
            lblMark.Name = "lblMark";
            lblMark.Size = new Size(137, 32);
            lblMark.TabIndex = 0;
            lblMark.Text = "Mark (%) *";
            lblMark.Click += label1_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(45, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(720, 59);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Screen 2 of 7 - Subjects and Marks";
            lblTitle.Click += label2_Click;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubject.Location = new Point(45, 91);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(116, 32);
            lblSubject.TabIndex = 2;
            lblSubject.Text = "Subject *";
            // 
            // cmbSubject
            // 
            cmbSubject.FormattingEnabled = true;
            cmbSubject.Location = new Point(45, 126);
            cmbSubject.Name = "cmbSubject";
            cmbSubject.Size = new Size(260, 40);
            cmbSubject.TabIndex = 3;
            // 
            // btnAddSubject
            // 
            btnAddSubject.Location = new Point(1020, 207);
            btnAddSubject.Name = "btnAddSubject";
            btnAddSubject.Size = new Size(162, 46);
            btnAddSubject.TabIndex = 4;
            btnAddSubject.Text = "Add Subject";
            btnAddSubject.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(45, 601);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(162, 46);
            btnBack.TabIndex = 5;
            btnBack.Text = "< Back";
            btnBack.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1020, 601);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(162, 46);
            btnNext.TabIndex = 6;
            btnNext.Text = "Next >";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // nudMark
            // 
            nudMark.Location = new Point(45, 214);
            nudMark.Name = "nudMark";
            nudMark.Size = new Size(258, 39);
            nudMark.TabIndex = 7;
            nudMark.ValueChanged += colMark_ValueChanged;
            // 
            // dgvSubjects
            // 
            dgvSubjects.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.LightGray;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvSubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSubjects.Columns.AddRange(new DataGridViewColumn[] { colSubject, colMark });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvSubjects.DefaultCellStyle = dataGridViewCellStyle2;
            dgvSubjects.EnableHeadersVisualStyles = false;
            dgvSubjects.Location = new Point(45, 275);
            dgvSubjects.Name = "dgvSubjects";
            dgvSubjects.RowHeadersWidth = 82;
            dgvSubjects.Size = new Size(1136, 300);
            dgvSubjects.TabIndex = 8;
            dgvSubjects.CellContentClick += dataGridView1_CellContentClick;
            // 
            // colSubject
            // 
            colSubject.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colSubject.FillWeight = 70F;
            colSubject.HeaderText = "Subject";
            colSubject.MinimumWidth = 10;
            colSubject.Name = "colSubject";
            colSubject.ReadOnly = true;
            // 
            // colMark
            // 
            colMark.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            colMark.FillWeight = 30F;
            colMark.HeaderText = "Mark (%)";
            colMark.MinimumWidth = 10;
            colMark.Name = "colMark";
            colMark.ReadOnly = true;
            colMark.Width = 164;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1220, 708);
            Controls.Add(dgvSubjects);
            Controls.Add(nudMark);
            Controls.Add(btnNext);
            Controls.Add(btnBack);
            Controls.Add(btnAddSubject);
            Controls.Add(cmbSubject);
            Controls.Add(lblSubject);
            Controls.Add(lblTitle);
            Controls.Add(lblMark);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Future Path - Subjects and Marks";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)nudMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMark;
        private Label lblTitle;
        private Label lblSubject;
        private ComboBox cmbSubject;
        private Button btnAddSubject;
        private Button btnBack;
        private Button btnNext;
        private NumericUpDown nudMark;
        private DataGridView dgvSubjects;
        private DataGridViewTextBoxColumn colSubject;
        private DataGridViewTextBoxColumn colMark;
    }
}