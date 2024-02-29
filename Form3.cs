using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static TP2___Thais.Form2;

namespace TP2___Thais
{
    public partial class Form3 : Form
    {
        internal enum Modes
        {
            FINAL_GRADE
        }

        private Modes mode = Modes.FINAL_GRADE;

        internal static Form3 current;

        private string[] fgInitial;

        public Form3()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            textBox1.Text = "" + c[0].Cells["StId"].Value;
            textBox2.Text = "" + c[0].Cells["StName"].Value;
            textBox3.Text = "" + c[0].Cells["CId"].Value;
            textBox4.Text = "" + c[0].Cells["CName"].Value;
            textBox5.Text = "" + c[0].Cells["FinalGrade"].Value;
            fgInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;        

            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            
            if (mode == Modes.FINAL_GRADE)
            {
                r = Business.Enrollments.UpdateFinalGrade(fgInitial, textBox5.Text);
            }

            if (r == 0) { Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
