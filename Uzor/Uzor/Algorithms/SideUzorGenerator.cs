using System;
using System.Collections.Generic;
using System.Text;
using Uzor.Algorithm;
using Uzor.Data;

namespace Uzor.Algorithms
{
    class SideUzorGenerator
    {
        private static bool edgeFilled (bool[,] field)
        {
            for (int i = 0; i < field.GetLongLength(0); i++)
                if (field[0, i] && field[1, i]) 
                    return true;
            return false;
        }

        private static void fillFieldInBaseSample(ref bool[,] field, int fieldSize)
        {
            bool[,] baseSample = new bool[5, 5] { { false, true, true, true, false },
                                                  { true , true, true, true, true  },
                                                  { true , true, true, true, true  },
                                                  { true , true, true, true, true  },
                                                  { false, true, true, true, false },
            };
            for (int i = 0; i < 5; i++)
                for (int y = 0; y < 5; y++)
                    field[fieldSize / 2 - 2 + i, fieldSize / 2 - 2 + y] = baseSample[i, y];
        }
        public static UzorData GetNewSideUzor(int fieldSize)
        {
            if (fieldSize % 2 == 0)
                fieldSize--;

            var data = new UzorData("", DateTime.Now, fieldSize < 5 ? 7 : fieldSize);

            bool[,] field = new bool[fieldSize, fieldSize];
            fillFieldInBaseSample(ref field, fieldSize);
            data.Layers[0].AddNextState(field);

            do
            {
                BasicDrawingAlgorithm.Calculate(data.Layers[0]);
            } while (!edgeFilled(data.Layers[0].GetLastState()));

            data.CropMask = RhombAlgorithm.GetRhombMask(fieldSize / 2, fieldSize);
            return data;
        }
    }
}
