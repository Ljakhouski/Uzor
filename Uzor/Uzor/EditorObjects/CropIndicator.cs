using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Uzor.Algorithms;
using Uzor.Data;
using Xamarin.Forms;

namespace Uzor.EditorObjects
{
    public class CropIndicator : EditorObject
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
                cropFieldMask = RhombAlgorithm.GetRhombMask(crop, Data.FieldSize);
                this.Data.CropMask = cropFieldMask;
                this.IsVisible = true;
            }

        }
        private bool[,] cropFieldMask;

        public CropIndicator(UzorData data)
        {
            this.Data = data;

            if (data.CropMaskIsEmpty())
                //this.crop = data.FieldSize / 2 - 1;
                this.cropFieldMask = null;
            else
            {
                //this.cropFieldMask = data.CropMask;
                this.crop = data.GetMaskSize() / 2;
            }
                
            this.IsVisible = false;
        }
        
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            if (!IsVisible || cropFieldMask == null)
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
