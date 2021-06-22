using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ModelData
    {
        public int Nodes_count { get; set; }

        public double P { get; set; }

        public double[] Nodes { get; set; }

        public double[] Function_Values { get; set;}


        public static double pMin { get { return -5; } }

        public static double pMax { get { return 5; } }

        public static int nMin { get { return 2; } }

        public static int nMax { get { return 20; } }

        public ModelData(int n, double p)
        {
            Nodes_count = n;
            P = p;
            Nodes = new double[n];
            Function_Values = new double[n];

            for ( int i = 0; i < Nodes_count; i++)
            {
                Nodes[i] = (double) i / (n - 1);
                Function_Values[i] = Func(Nodes[i], p);
            }
        }

        public static double Func(double x, double p)
        {
            return x * x + p;
        }

    }
}
