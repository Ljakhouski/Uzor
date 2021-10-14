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
                sideUzorPainter.Data = data_.SidePattern;
                calculateSizes();
            } }
        private int pixelSize = 10;
        public LongUzorDrawingObject(LongUzorData data)
        {
            this.Data = data;
            uzorPainter.Data = data.UzorElements[0];
            sideUzorPainter.Data = data.SidePattern;
            calculateSizes();
        }
        
        public LongUzorDrawingObject()
        {
            //uzorPainter.Data = this.Data.UzorElements[0];
            //calculateSizes();
        }

        private void calculateSizes()
        {
            minimalStepSize = pixelSize / 2;
            //minimal_A_parameter = minimalStepSize * 20;
        }

        private int minimalStepSize;
        //private int minimal_A_parameter;
        private int sceneCenterX, sceneCenterY;
        
   
        private UzorDrawingObject uzorPainter = new UzorDrawingObject();
        private UzorDrawingObject sideUzorPainter = new UzorDrawingObject();
   

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.Clear();
            if (Data == null)
                return;

            this.sceneCenterX = (int)view.CanvasSize.Width / 2;
            this.sceneCenterY = (int)view.CanvasSize.Height / 2;

            bool b = false;
            for (int i = -7; i < 7; i++)
            {
                drawCentralUzor(i, canvas, view.CanvasSize.Width, view.CanvasSize.Height);
                drawSideUzor(i, canvas, view.CanvasSize.Width, view.CanvasSize.Height);
            }

            
        }
        public override void Draw(SKCanvas canvas, float width, float height)
        {
            canvas.Clear();
            if (Data == null)
                return;

            this.sceneCenterX = (int)width / 2;
            this.sceneCenterY = (int)height / 2;

            bool b = false;
            for (int i = -7; i < 7; i++)
            {
                drawCentralUzor(i, canvas, width, height);
                drawSideUzor(i, canvas, width, height);
            }
        }

        private void drawCentralUzor(int i, SKCanvas canvas, float width, float height)
        {
            SKMatrix matrix = SKMatrix.Identity;
            SKMatrix previousMatrix = canvas.TotalMatrix;

            /* move Uzor to the x-center of screen */
            int uzorCenterX = this.pixelSize * uzorPainter.Data.FieldSize / 2;
            matrix.TransX = this.sceneCenterX - uzorCenterX;

            int uzorCenterY = uzorCenterX; // Uzor is square object
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY; // diapasone of Data.A: [0;100]
            matrix.TransY += this.sceneCenterY;
                                                                                
            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            uzorPainter.DrawUzor(this.pixelSize, canvas, width, height); // uzor constantly drawing in [0;0] point
            canvas.SetMatrix(previousMatrix);

            uzorPainter.Data = this.Data.UzorElements[i%2==0 ? 0 : 1];
        }

        private void drawSideUzor(int i, SKCanvas canvas, float width, float height)
        {
            // left side:
            SKMatrix matrix = SKMatrix.Identity;
            SKMatrix previousMatrix = canvas.TotalMatrix;
            int uzorCenterX = this.pixelSize * this.sideUzorPainter.Data.FieldSize / 2;
            int uzorLeftShift = this.pixelSize * this.Data.UzorElements[0].FieldSize / 2; // must be equal to one of the central Uzor-size;
            matrix.TransX = this.sceneCenterX - this.Data.B * this.minimalStepSize - uzorCenterX - uzorLeftShift;

            int uzorCenterY = uzorCenterX; // Uzor is square object

            int phaseShift = Data.A * minimalStepSize / 2;
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY + phaseShift;
            matrix.TransY += this.sceneCenterY;

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.pixelSize, canvas, width, height, Direction.ToRight);

            canvas.SetMatrix(previousMatrix);

            // right side:
            SKMatrix previousMatrix2 = canvas.TotalMatrix;
            matrix = SKMatrix.Identity;
            int uzorRightShift = this.pixelSize * (this.Data.UzorElements[0].FieldSize / 2 - this.sideUzorPainter.Data.FieldSize / 2);
            matrix.TransX = this.sceneCenterX + this.Data.B * this.minimalStepSize + uzorRightShift;
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY + phaseShift;
            matrix.TransY += this.sceneCenterY;

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.pixelSize, canvas, width, height, Direction.ToLeft);

            canvas.SetMatrix(previousMatrix2);
        }
        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
           //base.TouchEffectAction(args, view);
        }

        //TODO: touch event for selecting uzor-items (long-tap)
    }
}
