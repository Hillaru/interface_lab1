using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace interface_lab1_2
{
    public partial class Form1 : Form
    {
        Time_calc Calculator = new Time_calc();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowCount > 10)
            {
                dataGridView2.CurrentRow.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double i;

            List<string> Joints = new List<string>();
            foreach (var txt in groupBox1.Controls)
            {
                if (txt is Label) continue;
                if ((txt as TextBox).Text != "")
                    Joints.Add((txt as TextBox).Text);
            }            

            List<List<int>> Ways = new List<List<int>>();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                List<int> cells_list = new List<int>();
                foreach (DataGridViewCell cell in row.Cells)
                    if (Int32.TryParse((string)cell.Value, out int j) == true)
                        cells_list.Add(j);
                Ways.Add(cells_list);
            }

            List<double> Way_chances = new List<double>();
            foreach (var txt in groupBox2.Controls)
            {
                if (txt is Label) continue;
                if (Double.TryParse((txt as TextBox).Text, out i) == true)
                    Way_chances.Add(i);
            }

            int Error_handler_type = 2;
            switch (comboBox1.Text)
            {
                case "возврат в начало": Error_handler_type = 0; break;
                case "возврат на пред. шаг": Error_handler_type = 1; break;
                case "повторение шага": Error_handler_type = 2; break;
                default: break;
            }

            double Error_chance = 0;
            if (Double.TryParse(errc.Text, out i) == true)
                Error_chance = i;

            double Plain_a = 0;
            if (Double.TryParse(plaina.Text, out i) == true)
                Plain_a = i;

            double Plain_b = 0;
            if (Double.TryParse(plainb.Text, out i) == true)
                Plain_b = i;

            double Avgtexp = 0;
            if (Double.TryParse(avgtexp.Text, out i) == true)
                Avgtexp = i;

            Calculator.Set_vars(Joints, Ways, Way_chances, Error_chance, Error_handler_type, Avgtexp, Plain_a, Plain_b);

            result_lbl.Text = Calculator.Calc_way().ToString();
        }
    }
}
