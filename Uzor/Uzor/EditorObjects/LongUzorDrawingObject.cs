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
        private LongUzorData data_;
        public LongUzorData Data { get { return data_; }
            set {
                data_ = value;
                uzorPainter.Data = data_.UzorElements[0];
                sideUzorPainter.Data = data_.UzorElements[0];
                calculateSizes();
            } }
        private int pixelSize = 10;
        public LongUzorDrawingObject(LongUzorData data)
        {
            this.Data = data;
            uzorPainter.Data = data.UzorElements[0];
            calculateSizes();
        }
        
        public LongUzorDrawingObject()
        {
            //uzorPainter.Data = this.Data.UzorElements[0];
            //calculateSizes();
        }

        private void calculateSizes()
        {
            minimalSizeStep = pixelSize / 2;
            minimal_A_parameter = minimalSizeStep * 20;
        }

        private int minimalSizeStep;
        private int minimal_A_parameter;
   
        private UzorDrawingObject uzorPainter = new UzorDrawingObject();
        private UzorDrawingObject sideUzorPainter = new UzorDrawingObject();
   

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.Clear();
            if (Data == null)
                return;

            
            bool b = false;
            for (int i = -7; i < 7; i++)
            {
                drawCentralUzor(i, canvas, view);
                drawSideUzor(i, canvas, view);
            }

            
        }

        private void drawCentralUzor(int i, SKCanvas canvas, SKCanvasView view)
        {
            SKMatrix matrix = SKMatrix.Identity;
            matrix.TransX = 300;
            SKMatrix previousMatrix = canvas.TotalMatrix;

            matrix.TransY = minimal_A_parameter + i * Data.A * minimalSizeStep; // diapasone of Data.A: [0;100]
                                                                                //matrix.PostConcat(canvas.TotalMatrix);
            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            uzorPainter.DrawUzor(this.pixelSize, canvas, view);

            canvas.SetMatrix(previousMatrix);

            uzorPainter.Data = this.Data.UzorElements[i%2==0 ? 0 : 1];
        }

        private void drawSideUzor(int i, SKCanvas canvas, SKCanvasView view)
        {
            SKMatrix matrix = SKMatrix.Identity;
            matrix.TransX = 300 - pixelSize *this.Data.UzorElements[0].FieldSize/2 - this.Data.B;

            SKMatrix previousMatrix = canvas.TotalMatrix;

            matrix.TransY = minimal_A_parameter + i * Data.A * minimalSizeStep + pixelSize * this.Data.UzorElements[0].FieldSize / 2; 

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.pixelSize, canvas, view, Direction.ToRight);

            canvas.SetMatrix(previousMatrix);

            // 2:
            SKMatrix previousMatrix2 = canvas.TotalMatrix;
            matrix = SKMatrix.Identity;
            matrix.TransX = 300 + pixelSize * this.Data.UzorElements[0].FieldSize / 2 + this.Data.B;
            matrix.TransY = minimal_A_parameter + i * Data.A * minimalSizeStep + pixelSize * this.Data.UzorElements[0].FieldSize / 2; 

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.pixelSize, canvas, view, Direction.ToLeft);

            canvas.SetMatrix(previousMatrix2);
        }
        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
           //base.TouchEffectAction(args, view);
        }
    }
}
