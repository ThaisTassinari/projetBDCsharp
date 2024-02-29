using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP2___Thais
{
    public partial class Form1 : Form
    {

        internal enum Grids
        {
            Students,
            Enrollments,
            Courses,
            Programs
        }

        internal static Form1 current;

        private Grids grid;

        public Form1()
        {
            current = this;
            InitializeComponent();
        }
               

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2();
            Form2.current.Visible = false;
            new Form3();
            Form3.current.Visible = false;

            dataGridView1.Dock = DockStyle.Fill;
        }

        private void programToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Programs;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = Data.Programs.GetPrograms();
            bindingSource1.Sort = "ProgId";
            dataGridView1.DataSource = bindingSource1;

            dataGridView1.Columns["ProgName"].HeaderText = "Program Name";
            dataGridView1.Columns["ProgId"].DisplayIndex = 0;
            dataGridView1.Columns["ProgName"].DisplayIndex = 1;

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            Data.Programs.UpdatePrograms();
        }

        private void coursToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            grid = Grids.Courses;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource2.DataSource = Data.Courses.GetCourses();
            bindingSource2.Sort = "CId";
            dataGridView1.DataSource = bindingSource2;

            dataGridView1.Columns["CName"].HeaderText = "Cours Name";
            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2; // talvez tenha que fazer formato assigments
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            Data.Courses.UpdateCourses();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Students;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3.DataSource = Data.Students.GetStudents();
            bindingSource3.Sort = "StId";
            dataGridView1.DataSource = bindingSource3;

            dataGridView1.Columns["StName"].HeaderText = "Student Name";
            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["StName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2; // talvez tenha que fazer formato assigments
        }

        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            Data.Students.UpdateStudents();
        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != Grids.Enrollments)
            {
                grid = Grids.Enrollments;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource4.DataSource = Data.Enrollments.GetDisplayEnrollments();
                bindingSource4.Sort = "StId, CId";    // using bindingSource to sort by two columns
                dataGridView1.DataSource = bindingSource4;

            }
        }        

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            Data.Programs.UpdatePrograms();
            Data.Courses.UpdateCourses();
            Data.Students.UpdateStudents();
        }

        private void ajouterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.INSERT, null);
        }

        private void modifierToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.UPDATE, c);
            }
        }

        private void suprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }
            else 
            {

                    List<string[]> lId = new List<string[]>();
                    for (int i = 0; i < c.Count; i++)
                    {
                        lId.Add(new string[] { ("" + c[i].Cells["StId"].Value),
                                        ("" + c[i].Cells["CId"].Value) }); // exclui o parse
                    }
                    Data.Enrollments.DeleteData(lId);             
            }
        }

        private void finalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for final grade");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for final grade");
            }
            else
            {
                Form3.current.Start(Form3.Modes.FINAL_GRADE, c);
            }
        }

        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Layer: " + s);
        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Business Layer: " + s);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
            e.Cancel = false;  // includes and "improves" dataGridView1.CancelEdit();
        }
    }
}
