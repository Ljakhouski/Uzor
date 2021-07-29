using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;
using Xamarin.Forms;

namespace Uzor.Views.DrawingObjects
{
    class UzorDrawingObject : DrawingObject
    {
        public UzorData Data { get; set; }
        public int LayerNumber { get; set; }
        public bool EditingMode { get; set; } = true;
        public bool MirrorMode { get; set; } = true;
        public bool DeleteMode { get; set; } = false;
        public bool ScaleChangeMode { get; set; } = true;

        SKPoint centerPointtt;

        Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
        SKMatrix matrix = SKMatrix.CreateIdentity();
        public override void Draw(SKCanvas canvas, SKCanvasView view/*, SKMatrix matrix*/)
        {
            //canvas.Save();
            //canvas.Restore();
            canvas.SetMatrix(matrix); // new
            float pixelSize = (float)view.CanvasSize.Width /  Data.FieldSize;
            var f = this.Data.Layers[LayerNumber].GetLastState();

            SKColor backColor  = this.Data.Layers[0]. BackColor.ToSKColor();
            SKColor frontColor = this.Data.Layers[0].FrontColor.ToSKColor();

            SKPaint backPaint  = new SKPaint { Color = backColor  };
            SKPaint frontPaint = new SKPaint { Color = frontColor };

            for (int w = 0; w < Data.FieldSize; w++)
                for (int h = 0; h < Data.FieldSize; h++)
                {
                    if (f[w, h] == false)
                    {
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize,  /*new SKPaint() { Color = backColor}*/ backPaint);
                    }
                    else
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, /*new SKPaint() { Color = frontColor }*/ frontPaint);
                }

            // draw grid:
            if (EditingMode)
            {
                var paint = new SKPaint() { Color = Color.FromRgba(5, 5, 5, 20).ToSKColor(), StrokeWidth = 2 };

                for (int w = 0; w < Data.FieldSize; w++)
                    canvas.DrawLine((float)((w + 1) * pixelSize), 0, (float)((w + 1) * pixelSize), (float)(view.CanvasSize.Height), paint);
                for (int h = 0; h < Data.FieldSize; h++)
                    canvas.DrawLine(0, (float)((h + 1) * pixelSize), (float)(view.CanvasSize.Width), (float)((h + 1) * pixelSize), paint);
            }

            //center of touch:
            var m = this.matrix;
            canvas.SetMatrix(SKMatrix.CreateIdentity());
            var paint2 = new SKPaint() { Color = Color.FromRgba(55, 5, 55, 100).ToSKColor(), StrokeWidth = 10 };
            canvas.DrawCircle(centerPointtt.X, centerPointtt.Y, 20, paint2);
            canvas.SetMatrix(m);

            //canvas.Save();
        }

        public void SetPixelsValue(TouchActionEventArgs args, SKCanvasView view)
        {
            // float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            float pixelSize = (float)(view.CanvasSize.Width / Data.FieldSize);
            var f = /*(bool[,])*/this.Data.Layers[LayerNumber].GetLastState();//.Clone();

            //int x = (int)(ConvertToPixel(args.Location).X / pixelSize);
            //int y = (int)(ConvertToPixel(args.Location).Y / pixelSize);

            int xLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).X / /*Scale*/1 + (view.CanvasSize.Width - (view.CanvasSize.Width / /*Scale*/1)) / 2));
           int yLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).Y / /*Scale*/1 + (view.CanvasSize.Height - (view.CanvasSize.Height / /*Scale*/1)) / 2));

            int x = (int)(xLocationAfterScaling / pixelSize);
            int y = (int)(yLocationAfterScaling / pixelSize);
            try
            {
                if (MirrorMode)
                {
                    if (x <= Data.FieldSize / 2 && y <= Data.FieldSize / 2)
                    {
                        f[Data.FieldSize - 1 - x, y] = DeleteMode ? false : true;

                        f[x, y] = DeleteMode ? false : true;
                        f[Data.FieldSize - 1 - x, Data.FieldSize - 1 - y] = DeleteMode ? false : true;
                        f[x, Data.FieldSize - 1 - y] = DeleteMode ? false : true;
                    }

                }
                else
                    f[x, y] = DeleteMode ? false : true;

                //this.ThisData.Layers[LayerNumber].EditLastState(f);
            }
            catch (IndexOutOfRangeException e) { }
        }
        
        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(view.CanvasSize.Width * pt.X / view.Width),
                            (float)(view.CanvasSize.Height * pt.Y / view.Height));

            
            switch(args.Type)
            {
                case TouchActionType.Pressed:
                    if (!touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Add(args.Id, point);
                    // start timer
                    break;
                case TouchActionType.Moved:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        if (touchDictionary.Count == 1)
                        {
                            SetPixelsValue(args, view);
                            
                        }
                        else if (touchDictionary.Count >= 2 && ScaleChangeMode)
                        {
                            // Copy two dictionary keys into array
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index of non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points involved in the transform
                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            double distance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - newPoint.X), 2) + Math.Pow(Math.Abs(pivotPoint.Y - newPoint.Y), 2));
                            double oldDistance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - prevPoint.X), 2)+ Math.Pow(Math.Abs(pivotPoint.Y - prevPoint.Y), 2));

                            double differentDistance = distance / oldDistance;

                            // make center-point:
                            //SKPoint centerPoint = new SKPoint() { X = pivotPoint.X + Math.Abs(newPoint.X - pivotPoint.X) / 2,
                                                      //            Y = pivotPoint.Y + Math.Abs(newPoint.Y - pivotPoint.Y) / 2};
                            SKPoint centerPoint = new SKPoint()
                            {
                                X = (newPoint.X + pivotPoint.X) / 2,
                                Y = (newPoint.Y + pivotPoint.Y) / 2
                            };

                            this.centerPointtt = centerPoint;
                            SKPoint oldCenterPoint = new SKPoint()
                            {
                                X = (prevPoint.X + pivotPoint.X) / 2,
                                Y = (prevPoint.Y + pivotPoint.Y) / 2
                            };  

                            SKMatrix translationMatrix = SKMatrix.CreateTranslation((centerPoint.X - oldCenterPoint.X)/matrix.ScaleX, (centerPoint.Y - oldCenterPoint.Y) / matrix.ScaleX);
                            //SKMatrix scaleMatrix = SKMatrix.CreateScaleTranslation((float)differentDistance, (float)differentDistance, centerPoint.X - oldCenterPoint.X, centerPoint.Y - oldCenterPoint.Y);
                            SKMatrix scaleMatrix = SKMatrix.CreateScale((float)differentDistance,(float)differentDistance, centerPoint.X / matrix.ScaleX, centerPoint.Y / matrix.ScaleX);
                            

                            matrix = SKMatrix.Concat(matrix, scaleMatrix);
                            matrix = SKMatrix.Concat(matrix, translationMatrix);


                           // SKMatrix.Concat(ref matrix,
 // SKMatrix.CreateTranslation(centerPoint.X - oldCenterPoint.X, centerPoint.Y - oldCenterPoint.Y),
 // SKMatrix.MakeScale((float)differentDistance, (float)differentDistance, centerPoint.X, centerPoint.Y));
                            //SKMatrix.Concat(ref matrix, SKMatrix.MakeTranslation(centerPoint.X - oldCenterPoint.X, centerPoint.Y - oldCenterPoint.Y),
                            //SKMatrix.CreateScale((float)differentDistance, (float)differentDistance, centerPoint.X, centerPoint.Y));


                            // SKMatrix.Concat(ref matrix, SKMatrix.CreateTranslation(centerPoint.X - oldCenterPoint.X, centerPoint.Y - oldCenterPoint.Y),
                            //             SKMatrix.CreateScale((float)differentDistance, (float)differentDistance, centerPoint.X, centerPoint.Y));
                            //SKMatrix scaleMatrix =
                            //   SKMatrix.MakeScale((float)differentDistance, (float)differentDistance, centerPoint.X, centerPoint.Y); // edited pivotPoint.X [2]

                            //           SKMatrix.PostConcat(ref matrix, scaleMatrix);
                            touchDictionary[args.Id] = point;
                        }
                    }
                        break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Remove(args.Id);
                    
                    break;
            }
        }
        SKPoint ConvertToPixel(Point pt, SKCanvasView view)
        {
            return new SKPoint((float)(view.CanvasSize.Width * pt.X / view.Width),
                               (float)(view.CanvasSize.Height * pt.Y / view.Height));
        }

        /*
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
                    // start timer
                    break;
                case TouchActionType.Moved:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        if (touchDictionary.Count == 1)
                        {
                            SetPixelsValue(args, view);

                        }
                        else if (touchDictionary.Count >= 2 && ScaleChangeMode)
                        {
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index of non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points involved in the transform
                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            SKMatrix touchMatrix = SKMatrix.MakeIdentity(); 
                            SKPoint oldVector = prevPoint - pivotPoint;
                            SKPoint newVector = newPoint - pivotPoint;

                            if (true)
                            {
                                // Find angles from pivot point to touch points
                                float oldAngle = (float)Math.Atan2(oldVector.Y, oldVector.X);
                                float newAngle = (float)Math.Atan2(newVector.Y, newVector.X);

                                // Calculate rotation matrix
                                float angle = newAngle - oldAngle;
                                touchMatrix = SKMatrix.CreateRotation(angle, pivotPoint.X, pivotPoint.Y);

                                // Effectively rotate the old vector
                                float magnitudeRatio = Magnitude(oldVector) / Magnitude(newVector);
                                //oldVector.X = magnitudeRatio * newVector.X;
                                //oldVector.Y = magnitudeRatio * newVector.Y;
                            }

                            float scaleX2 = 1;
                            float scaleY2 = 1;

                            if (false)
                            {
                                scaleX2 = newVector.X / oldVector.X;
                                scaleY2 = newVector.Y / oldVector.Y;

                            }
                            else if (true)
                            {
                                scaleX2 = scaleY2 = Magnitude(newVector) / Magnitude(oldVector);
                            }

                            if (!float.IsNaN(scaleX2) && !float.IsInfinity(scaleX2) &&
                                !float.IsNaN(scaleY2) && !float.IsInfinity(scaleY2))
                            {
                                SKMatrix.PostConcat(ref matrix,
                                    SKMatrix.CreateScaleTranslation(scaleX2, scaleY2, pivotPoint.X, pivotPoint.Y));
                                //SKMatrix.PostConcat(ref matrix,
                                //    touchMatrix);
                            }

                            //this.matrix = touchMatrix;
                            touchDictionary[args.Id] = point;
                        }
                    }
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Remove(args.Id);

                    break;

            }

        }

        float Magnitude(SKPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }*/
    }
}
