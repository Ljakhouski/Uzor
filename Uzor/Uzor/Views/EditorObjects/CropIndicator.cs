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
        private bool[,] cropFieldModel;

        public CropIndicator()
        {
            this.IsVisible = false;
        }
        private void CalculateCropField()
        {

            cropFieldModel = new bool[Data.FieldSize, Data.FieldSize];

            for (int i = Data.FieldSize / 2 - Crop; i < Data.FieldSize - Data.FieldSize / 2 - Crop; i++)
            {
                int radius = Math.Abs(Data.FieldSize / 2 - i);
                for (int y = Data.FieldSize / 2 - radius; y < Data.FieldSize / 2 + radius; y++)
                    cropFieldModel[i, y] = true;
            }
        }
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            if (!IsVisible)
                return;
            return; // TODO: fix this
            float pixelSize = (float)view.CanvasSize.Width / Data.FieldSize;


            SKPaint paint = new SKPaint { Color = Color.FromRgba(100,100,100,100).ToSKColor() };

            for (int w = 0; w < Data.FieldSize; w++)
                for (int h = 0; h < Data.FieldSize; h++)
                    if (cropFieldModel[w, h] == false)
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, paint);

        }


    }
}
