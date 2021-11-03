using System;
using System.Collections.Generic;
using System.Text;

namespace Uzor.Algorithms
{
    public class RhombAlgorithm
    {
        public static bool[,] GetRhombMask(int size, int fieldSize)
        {
            if (size == 0)
                return null;

            var cropFieldMask = new bool[fieldSize, fieldSize];

            int lineSize = 0;

            for (int x = (int)Math.Truncate(fieldSize / 2.0 - size - 0.1); x <= (int)Math.Truncate(fieldSize / 2.0 + size + 0.1); x++)
            {
                for (int y = (int)Math.Truncate(fieldSize / 2.0 - lineSize / 2.0 - 0.1); y <= (int)Math.Truncate(fieldSize / 2.0 + lineSize / 2.0 + 0.1); y++)
                    cropFieldMask[x, y] = true;

                if (fieldSize % 2 != 0)

                    if (x <= Math.Truncate(fieldSize / 2.0) - 1)
                        lineSize += 2;
                    else
                        lineSize -= 2;
                else

                    if (x < Math.Truncate(fieldSize / 2.0) - 1)
                        lineSize += 2;
                    else if (x > Math.Truncate(fieldSize / 2.0 - 1))
                        lineSize -= 2;
                

            }

            return cropFieldMask;
        }
    }
}
