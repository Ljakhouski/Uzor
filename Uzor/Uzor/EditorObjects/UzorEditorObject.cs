using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Uzor.Views.EditorObjects
{
    class UzorEditorObject : UzorDrawingObject
    {
        //public UzorData Data { get; set; }
       // public [] ... // TODO: этот объект должен хранить только информацих-
        //public int LayerNumber { get; set; }
        public bool EditingMode { get; set; } = true;
        public bool MirrorMode { get; set; } = true;
        public bool DeleteMode { get; set; } = false;
        public bool ScaleChangeMode { get; set; } = true;

        SKPoint centerPointtt, new_p, old_p, newPointMultitouth, oldPointMultitouth, pivotPointMultitouth, oldCenter;
        

        //Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
        //SKMatrix matrix = SKMatrix.CreateIdentity();

        public override void Draw(SKCanvas canvas, SKCanvasView view/*, SKMatrix matrix*/)
        {
            base.Draw(canvas, view); 
            float pixelSize = (float)view.CanvasSize.Width / Data.FieldSize;
            //canvas.Save();
            //canvas.Restore();
            /*canvas.SetMatrix(matrix);
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
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize,  /*new SKPaint() { Color = backColor}*/// backPaint);
                                                                                                                                                 //  }
                                                                                                                                                 //    else
                                                                                                                                                 //          canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, /*new SKPaint() { Color = frontColor }*/// frontPaint);
                                                                                                                                                 //   }

            // draw grid:
            if (EditingMode)
            {
                var paint = new SKPaint() { Color = Color.FromRgba(5, 5, 5, 20).ToSKColor(), StrokeWidth = 2 };

                for (int w = 0; w < Data.FieldSize; w++)
                    canvas.DrawLine((float)((w + 1) * pixelSize), 0, (float)((w + 1) * pixelSize), (float)(view.CanvasSize.Height), paint);
                for (int h = 0; h < Data.FieldSize; h++)
                    canvas.DrawLine(0, (float)((h + 1) * pixelSize), (float)(view.CanvasSize.Width), (float)((h + 1) * pixelSize), paint);
            }

        }

        
        public void SetDefaultScale()
        {
            this.matrix = SKMatrix.Identity;
        }
        private void SetPixelsValue(TouchActionEventArgs args, SKCanvasView view)
        {
            // float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            float pixelSize = (float)(view.CanvasSize.Width / Data.FieldSize);
            var f = /*(bool[,])*/this.Data.Layers[LayerNumber].GetLastState();//.Clone();


            // for simple scaling (in center):

            // int xLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).X / /*Scale*/1 + (view.CanvasSize.Width - (view.CanvasSize.Width / /*Scale*/1)) / 2));
            // int yLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).Y / /*Scale*/1 + (view.CanvasSize.Height - (view.CanvasSize.Height / /*Scale*/1)) / 2));

            // scale with transfer (not working):

            //int xLocationAfterScaling = (int)(ConvertToPixel(args.Location, view).X / matrix.ScaleX - matrix.TransX);
            //int yLocationAfterScaling = (int)(ConvertToPixel(args.Location, view).Y / matrix.ScaleY - matrix.TransY);

            int xLocationAfterScaling = (int)((int)(ConvertToPixel(args.Location, view).X - matrix.TransX) / matrix.ScaleX);
            int yLocationAfterScaling = (int)((int)(ConvertToPixel(args.Location, view).Y - matrix.TransY) / matrix.ScaleY);

            //oldCenter.X = xLocationAfterScaling; oldCenter.Y = yLocationAfterScaling;

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

        private bool OneFingerPushMode = false;
        private bool PostReleasedMode = false; // required after "release" or "canceled" types of event to moving without drawing on the field
        private async void OneFingerTouchTimer(TouchActionEventArgs args, SKCanvasView view)
        {
            await Task.Delay(50);
            if (touchDictionary.Count == 1)
                SetPixelsValue(args, view);
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                view.InvalidateSurface();
            });
        }

        private async void PostReleaseModeOn()
        {
            PostReleasedMode = true;
            await Task.Delay(50);
            PostReleasedMode = false;
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
                    //SetPixelsValue(args, view);

                    if (touchDictionary.Count == 1)
                        OneFingerPushMode = true;
                    else 
                        OneFingerPushMode = false;

                    OneFingerTouchTimer(args, view);
                    break;
                case TouchActionType.Moved:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        if (touchDictionary.Count == 1)
                        {
                            if (!PostReleasedMode)
                                SetPixelsValue(args, view);
                            OneFingerPushMode = false;
                            /*long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index of non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points involved in the transform
                           // SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            SKPoint delta = newPoint - prevPoint;   new_p = newPoint; old_p = prevPoint;

                            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(delta.X, delta.Y));
                            touchDictionary[args.Id] = point;*/
                        }
                        else if (touchDictionary.Count >= 2 && ScaleChangeMode)
                        {
                            // Copy two dictionary keys into array
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];  
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;  pivotPointMultitouth = pivotPoint; newPointMultitouth = newPoint; oldPointMultitouth = prevPoint;

                            double distance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - newPoint.X), 2) + Math.Pow(Math.Abs(pivotPoint.Y - newPoint.Y), 2));
                            double oldDistance = Math.Sqrt(Math.Pow(Math.Abs(pivotPoint.X - prevPoint.X), 2)+ Math.Pow(Math.Abs(pivotPoint.Y - prevPoint.Y), 2));

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
                            };this.centerPointtt = centerPoint; oldCenter = oldCenterPoint;


                            SKMatrix translationMatrix = SKMatrix.CreateTranslation((centerPoint.X - oldCenterPoint.X)/*/matrix.ScaleX*/, (centerPoint.Y - oldCenterPoint.Y) /*/ matrix.ScaleX*/);
                            SKMatrix scaleMatrix = SKMatrix.Identity;

                            if ( !(matrix.ScaleX>8 && differentDistance>1))
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

                    // for one finger quick tap:
                    if (OneFingerPushMode)  
                        SetPixelsValue(args, view);
                    OneFingerPushMode = false;


                    if (touchDictionary.Count == 0)
                        PostReleasedMode = false;

                    if (touchDictionary.Count == 1)
                        PostReleaseModeOn();

                    if (matrix.ScaleX < 1)
                        SetDefaultScale();

                    break;
            }
        }
       

        

    }
}
