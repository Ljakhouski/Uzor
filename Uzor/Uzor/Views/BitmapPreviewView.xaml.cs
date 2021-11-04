using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BitmapPreviewView : ContentView
    {
        public SKBitmap Bitmap { get; set; }

        private Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
        private SKMatrix matrix = SKMatrix.CreateIdentity();
        public BitmapPreviewView(SKBitmap bitmap)
        {
            this.Bitmap = bitmap;
            InitializeComponent();
            this.matrix.ScaleX = 0.5f;
            this.matrix.ScaleY = 0.5f;
            this.matrix.TransX = 20;
            this.matrix.TransY = 20;
        }

        public void Draw()
        {
            this.view.InvalidateSurface();
        }
        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKCanvas c = e.Surface.Canvas;
            c.Clear();
            drawBackgroundGrid(c);
            c.SetMatrix(matrix);
            if (this.Bitmap != null)
            {
                c.DrawBitmap(this.Bitmap, new SKPoint(0, 0));
            }
        }

        private void drawBackgroundGrid(SKCanvas canvas)
        {
            var currentMatrix = canvas.TotalMatrix;
            var matrix = SKMatrix.Identity;
            canvas.SetMatrix(matrix);

            var p1 = new SKPaint() { Color = Color.White.ToSKColor() };
            var p2 = new SKPaint() { Color = Color.Gray.ToSKColor() };
            bool b = false;
            for (int i = 0; i<201; i++)
                for (int j = 0; j<201; j++)
                {
                    canvas.DrawRect(new SKRect(i * 15, j * 15, i * 15 + 15, j * 15 + 15), b? p1:p2);
                    b = !b;
                }

            canvas.SetMatrix(currentMatrix);

        }
        private void OnTouchEffectAction(object sender, TouchTracking.TouchActionEventArgs args)
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

                    break;

                
            }
            
            this.view.InvalidateSurface();
        }
    }
}