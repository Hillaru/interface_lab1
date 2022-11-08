using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interface_lab1_2
{
    internal class Joint
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        string _value;

        public double value
        {
            get 
            {
                if (string.IsNullOrEmpty(_value))
                {
                    return -1;
                }

                string[] split_str = _value.Split('-');
                if (split_str.Length == 2)
                {
                    if (Double.TryParse(split_str[0], out double plain_a) && Double.TryParse(split_str[1], out double plain_b))
                        return(rand.NextDouble() * (plain_b - plain_a) + plain_a);
                }

                if (Double.TryParse(_value, out double i) == true)
                {
                    return i;
                }
                else
                    return -1;
            }
        }

        public Joint(string s)
        {
            _value = s;
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

        public void Set_vars(List<string> _Joints, List<List<int>> _Ways, List<double> _Way_chances, double _Error_chance, int _Error_handler_type)
        {
            Joints = new List<Joint>();
            foreach (string j in _Joints)
            {
                Joints.Add(new Joint(j));
            }
            Ways = new List<List<int>>(_Ways);
            Way_chances = _Way_chances;
            Error_chance = _Error_chance;
            Error_handler_type = _Error_handler_type;  
        }

        public double Calc_way()
        {
            rand = new Random(DateTime.Now.Millisecond);
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
                if (Joints[Ways[Way_index][i] - 1].value == -1)
                    return -1;

                Result_time += Joints[Ways[Way_index][i] - 1].value;
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
