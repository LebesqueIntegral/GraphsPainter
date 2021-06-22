using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Interpolate
    {
        public static double F(double x, ModelData modelData)
        {
            if (x >= 1)
                return modelData.Function_Values[modelData.Nodes_count - 1];
            if (x <= 0)
                return modelData.Function_Values[0];

            for (int i = 0; i < modelData.Nodes_count - 1; i++)
            {
                if (x == modelData.Nodes[i])
                    return modelData.Function_Values[i];
                if (x > modelData.Nodes[i] && x < modelData.Nodes[i + 1])
                {
                    return (x - modelData.Nodes[i]) * (modelData.Nodes_count - 1) * (modelData.Function_Values[i + 1] - modelData.Function_Values[i]) + modelData.Function_Values[i];
                }
            }

            return 0;
        }
    }
}
