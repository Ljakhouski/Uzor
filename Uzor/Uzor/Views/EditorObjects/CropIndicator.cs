using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Uzor.Views.EditorObjects
{
    class CropIndicator : EditorObject
    {
        public UzorData Data { get; set; }
        
        private int crop { get; set; }
        public int Crop 
        { 
            get 
            { 
                return crop; 
            } 
            set
            {
                crop = value;
                CalculateCropField();
                this.IsVisible = true;
            }

        }
        private bool[,] cropFieldMask;

        public CropIndicator()
        {
            this.IsVisible = false;
        }
        private void CalculateCropField()
        {

            cropFieldMask = new bool[Data.FieldSize, Data.FieldSize];

            /*int lineSize = 0;
            for (int i = Data.FieldSize / 2 - Crop - 1; i <= Data.FieldSize / 2 + Crop; i++)
            {
                for (int y = Data.FieldSize / 2 - lineSize / 2 ; y <= Data.FieldSize / 2 + lineSize / 2 - 1; y++)
                        cropFieldModel[i,  y] = true;

                if (i <= Data.FieldSize / 2 - 1 /* for odd*/ /*)
                    
                    lineSize += 2; // top and down of each line of crop-rhomb
                
                else
                    lineSize -= 2; // top and down of each line of crop-rhomb
            }
        */

            int lineSize = 0;

            for (int x = (int)Math.Truncate(Data.FieldSize / 2.0 - crop - 0.1); x <= (int)Math.Truncate(Data.FieldSize / 2.0 + crop + 0.1); x++)
            {
                for (int y = (int)Math.Truncate(Data.FieldSize / 2.0 - lineSize/2.0 - 0.1); y <= (int)Math.Truncate(Data.FieldSize / 2.0 + lineSize/2.0 + 0.1); y++)
                    cropFieldMask[x, y] = true;

                if (Data.FieldSize%2!=0)
                {
                    if (x <= Math.Truncate(Data.FieldSize / 2.0) - 1)
                        lineSize += 2;
                    else
                        lineSize -= 2;
                }
                else
                {

                    if (x < Math.Truncate(Data.FieldSize / 2.0) - 1)
                        lineSize += 2;
                    else if (x > Math.Truncate(Data.FieldSize / 2.0 - 1))
                        lineSize -= 2;
                }
                
            }

        }
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            if (!IsVisible)
                return;
           // return; // TODO: fix this
            float pixelSize = (float)view.CanvasSize.Width / Data.FieldSize;


            SKPaint paint = new SKPaint { Color = Color.FromRgba(100,100,100,100).ToSKColor() };

            for (int w = 0; w < Data.FieldSize; w++)
                for (int h = 0; h < Data.FieldSize; h++)
                    if (cropFieldMask[w, h] == false)
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, paint);

        }


    }
}
