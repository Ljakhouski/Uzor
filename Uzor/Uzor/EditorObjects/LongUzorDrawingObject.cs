using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;
using Uzor.Algorithms;
using Uzor.Data;
using Xamarin.Forms;

namespace Uzor.EditorObjects
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

        private int _pixelSize = 10;
        public int PixelSize
        { 
            get { return _pixelSize; }
            set { this._pixelSize = value; calculateSizes(); } 
        }
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
            calculateSizes();
        }

        private void calculateSizes()
        {
            minimalStepSize = PixelSize / 2;
            //minimal_A_parameter = minimalStepSize * 20;
        }

        private int minimalStepSize;
        public int GetResultContentWidth()
        {
            const int longUzorNumber = 5;
            const int freeZone = 300;
            return this.Data.D * longUzorNumber * minimalStepSize + freeZone;
        }

        public int GetResultContentHeight()
        {
            const int freeZone = 350;
            const int verticalElementNumber = 14;
            return Data.A * minimalStepSize * verticalElementNumber + freeZone;
        }
        //private int minimal_A_parameter;
        private int sceneCenterX, sceneCenterY;
        
   
        private UzorDrawingObject uzorPainter = new UzorDrawingObject();
        private UzorDrawingObject sideUzorPainter = new UzorDrawingObject();
   

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            if (Data == null)
                return;

            this.sceneCenterX = (int)view.CanvasSize.Width / 2;
            this.sceneCenterY = (int)view.CanvasSize.Height / 2;

            /*
            for (int i = -7; i < 7; i++)
            {
                drawCentralUzor(i, canvas, view.CanvasSize.Width, view.CanvasSize.Height);
                drawSideUzor(i, canvas, view.CanvasSize.Width, view.CanvasSize.Height);
            }*/

            canvas.Clear(Data.BackColor.ToSKColor());

            drawSingleLongUzors(canvas, view.CanvasSize.Width, view.CanvasSize.Height);

            
        }
        public override void Draw(SKCanvas canvas, float width, float height)
        {
            if (Data == null)
                return;

            this.sceneCenterX = (int)width / 2;
            this.sceneCenterY = (int)height / 2;

            /*
            for (int i = -7; i < 7; i++)
            {
                drawCentralUzor(i, canvas, width, height);
                drawSideUzor(i, canvas, width, height);
            }*/

            canvas.Clear(Data.BackColor.ToSKColor());

            drawSingleLongUzors(canvas, width, height);
        }

        private void drawSingleLongUzors(SKCanvas canvas, float width, float height)
        {
            for (int i = -2; i<3; i++)
            {
                SKMatrix matrix = SKMatrix.Identity;
                SKMatrix previousMatrix = canvas.TotalMatrix;

                int distance = this.Data.D;
                int x = distance * minimalStepSize * i;

                matrix.TransX = x;
                SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);
                canvas.SetMatrix(matrix);
                int debugDistance = (int)canvas.TotalMatrix.TransX;
                int debugYdistance = (int)canvas.TotalMatrix.TransY;
                for (int j = -7; j < 7; j++)
                {
                    drawCentralUzor(j, canvas, width, height);
                    drawSideUzor(j, canvas, width, height);
                }

                canvas.SetMatrix(previousMatrix);
            }
                
        }

        private void drawCentralUzor(int i, SKCanvas canvas, float width, float height)
        {
            SKMatrix matrix = SKMatrix.Identity;
            SKMatrix previousMatrix = canvas.TotalMatrix;

            /* move Uzor to the x-center of screen */
            int uzorCenterX = this.PixelSize * uzorPainter.Data.FieldSize / 2;
            matrix.TransX = this.sceneCenterX - uzorCenterX;

            int uzorCenterY = uzorCenterX; // Uzor is square object
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY; // diapasone of Data.A: [0;100]
            matrix.TransY += this.sceneCenterY;
                                                                                
            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            uzorPainter.DrawUzor(this.PixelSize, canvas, width, height); // uzor constantly drawing in [0;0] point
            canvas.SetMatrix(previousMatrix);

            uzorPainter.Data = this.Data.UzorElements[i%2==0 ? 0 : 1];
        }

        private void drawSideUzor(int i, SKCanvas canvas, float width, float height)
        {
            update_C_Parameter();

            // left side:
            SKMatrix matrix = SKMatrix.Identity;
            SKMatrix previousMatrix = canvas.TotalMatrix;
            int uzorCenterX = this.PixelSize * this.sideUzorPainter.Data.FieldSize / 2;
            int uzorLeftShift = this.PixelSize * this.Data.UzorElements[0].FieldSize / 2; // must be equal to one of the central Uzor-size;
            matrix.TransX = this.sceneCenterX - this.Data.B * this.minimalStepSize - uzorCenterX - uzorLeftShift;

            int uzorCenterY = uzorCenterX; // Uzor is square object

            int phaseShift = Data.A * minimalStepSize / 2;
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY + phaseShift;
            matrix.TransY += this.sceneCenterY;

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.PixelSize, canvas, width, height, Direction.ToRight);

            canvas.SetMatrix(previousMatrix);

            // right side:
            SKMatrix previousMatrix2 = canvas.TotalMatrix;
            matrix = SKMatrix.Identity;
            int uzorRightShift = this.PixelSize * (this.Data.UzorElements[0].FieldSize / 2 - this.sideUzorPainter.Data.FieldSize / 2);
            matrix.TransX = this.sceneCenterX + this.Data.B * this.minimalStepSize + uzorRightShift;
            matrix.TransY = i * Data.A * minimalStepSize - uzorCenterY + phaseShift;
            matrix.TransY += this.sceneCenterY;

            SKMatrix.PostConcat(ref matrix, canvas.TotalMatrix);

            canvas.SetMatrix(matrix);
            sideUzorPainter.DrawUzor(this.PixelSize, canvas, width, height, Direction.ToLeft);

            canvas.SetMatrix(previousMatrix2);
        }
        private void update_C_Parameter()
        {
            //if (this.sideUzorPainter.Data.CropMask != null && !this.sideUzorPainter.Data.CropMaskIsEmpty())
            this.sideUzorPainter.Data.CropMask = RhombAlgorithm.GetRhombMask(this.Data.C, this.sideUzorPainter.Data.FieldSize);
        }
        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
           //base.TouchEffectAction(args, view);
        }

        //TODO: touch event for selecting uzor-items (long-tap)
    }
}
