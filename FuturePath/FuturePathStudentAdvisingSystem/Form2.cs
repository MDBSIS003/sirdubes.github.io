using System;
using System.Windows.Forms;
using FuturePathStudentAdvisingSystem.DataAccess;

namespace FuturePathStudentAdvisingSystem
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Load += Form2_Load;
            btnAddSubject.Click += btnAddSubject_Click;
        }

        private void Form2_Load(object? sender, EventArgs e)
        {
            try
            {
                SubjectDAL subjectDAL = new SubjectDAL();

                cmbSubject.DataSource = subjectDAL.GetSubjects();
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
                cmbSubject.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load subjects.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAddSubject_Click(object? sender, EventArgs e)
        {
            if (cmbSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject.");
                return;
            }

            string subjectName = cmbSubject.Text;

            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == subjectName)
                {
                    MessageBox.Show("This subject has already been added.");
                    return;
                }
            }

            dgvSubjects.Rows.Add(subjectName, nudMark.Value);

            cmbSubject.SelectedIndex = -1;
            nudMark.Value = 0;
        }

        private void label1_Click(object? sender, EventArgs e)
        {
        }

        private void label2_Click(object? sender, EventArgs e)
        {
        }

        private void colMark_ValueChanged(object? sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
        }
    }
}