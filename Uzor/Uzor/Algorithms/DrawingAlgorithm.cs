using System;
using System.Collections.Generic;
using System.Text;
using Uzor.Data;

namespace Uzor.Algorithm
{
    interface DrawingAlgorithm
    {
       //static void Calculate();
    }

    public class BasicDrawingAlgorithm //: DrawingAlgorithm
    {
        public static void Calculate(Field f)
        {
            //firstly saving current state
            //this.ThisData.Layers[0].AddNextState(FieldCore);
            bool[,] field = f.GetLastState();
            int size = field.GetLength(1);
            bool[,] fieldForEditing = new bool[size,size];
            
            for (int w = 0; w < size; w++)
                for (int h = 0; h < size; h++)
                {
                    int countCellsAround = CountAround(w, h, size, field);


                    if (countCellsAround == 2 || countCellsAround == 3)
                    {
                        fieldForEditing[w, h] = true;
                        // return;
                    }
                    else
                    {
                        fieldForEditing[w, h] = false;
                    }

                }

            field = (bool[,])fieldForEditing.Clone();
            f.AddNextState(field);
        }

        private static int CountAround(int w, int h, int size, bool[,] field)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
                for (int y = -1; y < 2; y++)
                {
                    if (w + i >= 0 && w + i < size && h + y >= 0 && h + y < size)  // for replace "OutOfRangeException"  
                        if (field[w + i, h + y] &&
                                !(y == 0 && i == 0)  // current pixel
                                )
                            count++;
                }
            return count;
        }
    }
}
