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
    public enum Direction
    {
        None,
        ToRight,
        ToLeft
    }
    class UzorDrawingObject : EditorObject
    {
        public UzorData Data { get; set; }
        public int LayerNumber { get; set; } = 0;
        public bool GradientMode { get; set; } = false;
        public bool ConsuderMask { get; set; } = false;
        protected Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
        protected SKMatrix matrix = SKMatrix.CreateIdentity();

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.SetMatrix(matrix); // default this view can transform matrix

            //canvas.Clear(Color.DarkGray.ToSKColor());
            float pixelSize = (float)view.CanvasSize.Width / Data.FieldSize;
            this.DrawUzor(pixelSize, canvas, view);

        }

        public void DrawUzor(float pixelSize, SKCanvas canvas, SKCanvasView view, Direction direction = Direction.None)
        {
            
            var f = this.Data.Layers[LayerNumber].GetLastState();
            var mask = this.Data.CropMask;
            bool isEmptyMask = this.Data.CropMaskIsEmpty();

            SKColor backColor = this.Data.Layers[0].BackColor.ToSKColor();
            SKColor frontColor = this.Data.Layers[0].FrontColor.ToSKColor();

            SKPaint backPaint = new SKPaint { Color = backColor };
            SKPaint frontPaint = new SKPaint { Color = frontColor };

            if (GradientMode)
            {
                frontPaint.Shader = SKShader.CreateLinearGradient(
                        new SKPoint(view.CanvasSize.Width / 4, view.CanvasSize.Height / 2),
                        new SKPoint(view.CanvasSize.Width, view.CanvasSize.Height / 2),
                        new SKColor[] { frontColor, new SKColor(frontColor.Red, frontColor.Green, frontColor.Blue, 0) },
                        new float[] { 0, 1 },
                        SKShaderTileMode.Clamp);


                backPaint.Shader = SKShader.CreateLinearGradient(
                        new SKPoint(view.CanvasSize.Width / 4, view.CanvasSize.Height / 2),
                        new SKPoint(view.CanvasSize.Width, view.CanvasSize.Height / 2),
                        new SKColor[] { backColor, new SKColor(backColor.Red, backColor.Green, backColor.Blue, 0) },
                        new float[] { 0, 1 },
                        SKShaderTileMode.Clamp);
            }

                             /********* visible-optimization *********/

            int xLeftTopLocationAfterScaling = (int)((int)(GetLeftTopPoint(view).X - matrix.TransX) / matrix.ScaleX);
            int yLeftTopLocationAfterScaling = (int)((int)(GetLeftTopPoint(view).Y - matrix.TransY) / matrix.ScaleY);

            int xRightDownLocationAfterScaling = (int)((int)(GetRightDownPoint(view).X - matrix.TransX) / matrix.ScaleX);
            int yRightDownLocationAfterScaling = (int)((int)(GetRightDownPoint(view).Y - matrix.TransY) / matrix.ScaleY);

            //oldCenter.X = xLocationAfterScaling; oldCenter.Y = yLocationAfterScaling;

            int xLeftTop = (int)(xLeftTopLocationAfterScaling / pixelSize);
            int yLeftTop = (int)(yLeftTopLocationAfterScaling / pixelSize);

            int xRightDown = (int)(xRightDownLocationAfterScaling / pixelSize);
            int yRightDown = (int)(yRightDownLocationAfterScaling / pixelSize);

            
            for (int w = (xLeftTop - 1) < 0 ? 0 : xLeftTop - 1; 
                w </*Data.FieldSize*/(xRightDown + 1 > Data.FieldSize ? Data.FieldSize : xRightDown + 1);
                w++)

                for (int h = (yLeftTop - 1) < 0 ? 0 : yLeftTop - 1; 
                    h </*Data.FieldSize*/(yRightDown + 1 > Data.FieldSize ? Data.FieldSize : yRightDown + 1); 
                    h++)
                {
                            /***** check for masks *****/

                    if ((isEmptyMask || (mask[w,h] && !isEmptyMask))
                        && 
                        (direction == Direction.None || 
                        (direction == Direction.ToLeft && w<=Data.FieldSize/2) ||
                        (direction == Direction.ToRight && w >= Data.FieldSize/2)
                        ))

                        if (f[w, h] == false)
                            canvas.DrawRect((float)w * pixelSize, 
                                            (float)h * pixelSize, pixelSize, pixelSize, 
                                             backPaint);
                        else
                            canvas.DrawRect((float)w * pixelSize, 
                                            (float)h * pixelSize, pixelSize, pixelSize,
                                             frontPaint);
                }
        }

      

        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(view.CanvasSize.Width * pt.X / view.Width),
                            (float)(view.CanvasSize.Height * pt.Y / view.Height));


            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Add(args.Id, point);
                    //SetPixelsValue(args, view);

                    break;
                case TouchActionType.Moved:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        if (touchDictionary.Count == 1)
                        {
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index of non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points involved in the transform
                            // SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            SKPoint delta = newPoint - prevPoint;  // new_p = newPoint; old_p = prevPoint;

                            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(delta.X, delta.Y));
                            touchDictionary[args.Id] = point;
                        }
                        else if (touchDictionary.Count >= 2)
                        {
                            // Copy two dictionary keys into array
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            double distance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - newPoint.X), 2) + Math.Pow(Math.Abs(pivotPoint.Y - newPoint.Y), 2));
                            double oldDistance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - prevPoint.X), 2) + Math.Pow(Math.Abs(pivotPoint.Y - prevPoint.Y), 2));

                            double differentDistance = distance / oldDistance;

                            SKPoint centerPoint = new SKPoint()
                            {
                                X = (newPoint.X + pivotPoint.X) / 2,
                                Y = (newPoint.Y + pivotPoint.Y) / 2
                            };


                            SKPoint oldCenterPoint = new SKPoint()
                            {
                                X = (prevPoint.X + pivotPoint.X) / 2,
                                Y = (prevPoint.Y + pivotPoint.Y) / 2
                            };


                            SKMatrix translationMatrix = SKMatrix.CreateTranslation((centerPoint.X - oldCenterPoint.X)/*/matrix.ScaleX*/, (centerPoint.Y - oldCenterPoint.Y) /*/ matrix.ScaleX*/);
                            SKMatrix scaleMatrix = SKMatrix.Identity;

                            if (!(matrix.ScaleX > 8 && differentDistance > 1))
                                scaleMatrix = SKMatrix.CreateScale((float)differentDistance, (float)differentDistance, centerPoint.X, centerPoint.Y/* pivotPoint.X, pivotPoint.Y*/);

                            SKMatrix.PostConcat(ref matrix, scaleMatrix);   // NEW "PostContact() DOES NOT WORKING CORRECTLY!!!" 
                            SKMatrix.PostConcat(ref matrix, translationMatrix);


                            touchDictionary[args.Id] = point;
                        }
                    }
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:

                    if (touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Remove(args.Id);

                    if (matrix.ScaleX < 1)
                        matrix = SKMatrix.Identity;
                    break;
            }
        }
        protected SKPoint ConvertToPixel(Point pt, SKCanvasView view)
        {
            return new SKPoint((float)(view.CanvasSize.Width * pt.X / view.Width),
                               (float)(view.CanvasSize.Height * pt.Y / view.Height));
        }
        private SKPoint GetLeftTopPoint(SKCanvasView view)
        {
            return new SKPoint((float)(view.CanvasSize.Width * 0 / view.Width),
                               (float)(view.CanvasSize.Height * 0 / view.Height));
        }
        private SKPoint GetRightDownPoint(SKCanvasView view)
        {
            return new SKPoint((float)(view.CanvasSize.Width * view.Width / view.Width),
                               (float)(view.CanvasSize.Height * view.Height / view.Height));
        }
    }
}
