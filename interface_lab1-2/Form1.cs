using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


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
            comboBox1.SelectedIndex = 2;
            errc.Text = "0";
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
            int j;

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
                    if (Int32.TryParse((string)cell.Value, out j) == true)
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

            double Sum = 0;
            foreach (double ch in Way_chances)
                Sum += ch;
            if (Sum != 1)
            {
                MessageBox.Show("Некорректные данные, вероятности выбора маршрутов не дают 100% в сумме");
                return;
            }

            double Error_chance = 0;
            if (Double.TryParse(errc.Text, out i) == true)
                Error_chance = i;

            if (Error_chance >= 1 || Error_chance < 0)
            {
                MessageBox.Show("Некорректные данные, вероятность ошибки должна быть не меньше 0% и меньше 100%");
                return;
            }          

            int Repeat_count = 1;
            if (Int32.TryParse(rptN.Text, out j) == true)
                Repeat_count = j;

            Calculator.Set_vars(Joints, Ways, Way_chances, Error_chance, Error_handler_type);

            double[] Result = new double[Repeat_count];
            double tmp;
            for (int indx = 0; indx < Repeat_count; indx++)
            {
                tmp = Calculator.Calc_way();
                if (tmp <= 0)
                {
                    MessageBox.Show("Некорректные данные, проверьте функции в узлах");
                    return;
                }
                else
                    Result[indx] = tmp;
            }

            //построение гистограммы
            chart1.Series[0].Points.Clear();
            var K = Convert.ToInt32(1 + 3.22 * Math.Log10(Repeat_count));
            double min = double.MaxValue, max = double.MinValue;
            foreach (var item in Result)
            {
                min = (min > item) ? (item) : (min);
                max = (max < item) ? (item) : (max);
            }
            double[] intervals = new double[K + 1];
            for (var indx = 0; indx < K + 1; indx++)
                intervals[indx] = min + (max - min) / K * indx;
            int[] y = new int[K];
            double[] x = new double[K];
            for (var indx = 0; indx < intervals.Length - 1; indx++)
            {
                x[indx] = Math.Round((intervals[indx + 1] + intervals[indx]) / 2, 2);
                foreach (var jndx in Result)
                    if (jndx >= intervals[indx] && jndx < intervals[indx + 1])
                        y[indx]++;
            }
            for (var indx = 0; indx < K; indx++)
                chart1.Series[0].Points.AddXY(x[indx], y[indx]);

        }
    }
}
