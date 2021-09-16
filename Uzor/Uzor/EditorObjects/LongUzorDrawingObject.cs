using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;
using Uzor.Data;
using Xamarin.Forms;

namespace Uzor.Views.EditorObjects
{
    public class LongUzorDrawingObject : EditorObject
    {

        public LongUzorData Data { get; set; }
        public LongUzorDrawingObject(LongUzorData data)
        {
            this.Data = data;

        }
        public LongUzorDrawingObject()
        {

        }

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.Clear();
            if (Data == null)
                return;

            UzorDrawingObject uzor = new UzorDrawingObject();
            uzor.Data = this.Data.UzorElements[0];
            uzor.ConsuderMask = true;

            bool b = false;
            for (int i = -7; i < 7; i++)
            {
                SKMatrix matrix = SKMatrix.Identity;
                matrix.TransX = 300;
                SKMatrix previousMatrix = canvas.TotalMatrix;

                float u = previousMatrix.TransX;

                matrix.TransY = i * Data.A * 3;
                //matrix.PostConcat(canvas.TotalMatrix);
                SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

                canvas.SetMatrix(matrix);

                
                uzor.DrawUzor(10, canvas, view);


                

                canvas.SetMatrix(previousMatrix);

                b = !b;
                uzor.Data = this.Data.UzorElements[b? 0 : 1];
               

            }
        }

        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
           //base.TouchEffectAction(args, view);
        }
    }
}
