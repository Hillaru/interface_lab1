using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interface_lab1_2
{
    internal class Joint
    {
        Random rand = new Random();
        public double value;

        public Joint(string s, double exp, double plain_a, double plain_b)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = -1;
                return;
            }

            if (s == "эксп")
            {
                value = exp * Math.Log(rand.NextDouble());
            }
            else
            if (s == "равн")
            {
                value = rand.NextDouble() * (plain_b - plain_a) + plain_a;
            }
            else
            if (Int32.TryParse(s, out int i) == true)
            {
                value = i;
            }
            else
                value = -1;
        }
    }

    internal class Time_calc
    {
        List<Joint> Joints;
        List<double> Way_chances;
        List<List<int>> Ways;
        double Error_chance;
        int Error_handler_type;
        Random rand = new Random();

        public Time_calc()
        {

        }

        public void Set_vars(List<string> _Joints, List<List<int>> _Ways, List<double> _Way_chances, double _Error_chance, int _Error_handler_type, double _exp, double _plain_a, double _plain_b)
        {
            Joints = new List<Joint>();
            foreach (string j in _Joints)
            {
                Joints.Add(new Joint(j, _exp, _plain_a, _plain_b));
            }
            Ways = new List<List<int>>(_Ways);
            Way_chances = _Way_chances;
            Error_chance = _Error_chance;
            Error_handler_type = _Error_handler_type;  
        }

        public double Calc_way()
        {
            double r = rand.NextDouble();
            double cr = 0;
            int Way_index = 0;
            while (Way_index < Way_chances.Count)
            {
                cr += Way_chances[Way_index];
                if (cr >= r)
                {
                    break;
                }
                Way_index++;
            }

            double Result_time = 0;
            int i = 0;
            while ( i < Ways[Way_index].Count )
            {
                Result_time += Joints[Ways[Way_index][i]].value;
                if (rand.NextDouble() < Error_chance)
                {
                    switch (Error_handler_type)
                    {
                        case 0: i = 0; break;
                        case 1: i = i == 0 ? 0 : i - 1; break;
                        case 2: break;
                    }
                }
                else
                    i++;
            }

            return Result_time;
        }
    }
}
