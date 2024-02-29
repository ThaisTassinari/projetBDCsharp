using Data;
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
    public partial class Form2 : Form
    {

        internal enum Modes
        {
            INSERT,
            UPDATE
        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;

        private string[] enrollmentInitial;
        internal static bool sameProgram = true;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = true;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;


            if ((mode == Modes.UPDATE) && (c != null))
            {
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox1.Enabled = false;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                enrollmentInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            } 

            ShowDialog();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<string>("StId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };
                textBox1.Text = a.Single().Name;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            sameProgram = true;
            if (comboBox2.SelectedItem != null)
            {
                try {       
                   
                var a = from course in Data.Courses.GetCourses().AsEnumerable()
                        from student in Data.Students.GetStudents().AsEnumerable()
                        where course.Field<string>("CId") == (string)comboBox2.SelectedValue
                        where student.Field<string>("StId") == (string)comboBox1.SelectedValue
                        where student.Field<string>("ProgId") == course.Field<string>("ProgId")

                        select new { Name = course.Field<string>("CName") };
                textBox2.Text = a.Single().Name;
                     }catch (Exception)
                {
                    TP2___Thais.Form1.DALMessage("Please select a course from the same ProgId");
                    sameProgram = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.INSERT)
            {
                r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.UPDATE)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(enrollmentInitial);

                r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                if (r == 0)
                {
                    r = Data.Enrollments.DeleteData(lId);
                }
            }
            

            if (r == 0) { Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
