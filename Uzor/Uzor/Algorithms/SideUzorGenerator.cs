using System;
using System.Collections.Generic;
using System.Text;
using Uzor.Data;

namespace Uzor.Algorithms
{
    class SideUzorGenerator
    {
        public static UzorData GetNewSideUzor(int sizeField)
        {
            if (sizeField % 2 == 0)
                sizeField--;

            var data = new UzorData("", DateTime.Now, sizeField < 5 ? 7 : sizeField);

            bool[,] baseSample = new bool[5, 5] { { false, true, true, true, false },
                                                  { true , true, true, true, true  },
                                                  { true , true, true, true, true  },
                                                  { true , true, true, true, true  },
                                                  { false, true, true, true, false },
            };

            bool[,] field = new bool[sizeField, sizeField];

            for (int i = 0; i < 5; i++)
                for (int y = 0; y < 5; y++)
                    field[sizeField / 2 - 2 + i, sizeField / 2 - 2 + y] = baseSample[i, y];
            data.Layers[0].AddNextState(field);
            return data;
        }
    }
}
