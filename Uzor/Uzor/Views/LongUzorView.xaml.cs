using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Uzor.EditorObjects;
using SkiaSharp;
using Uzor.Data;
using TouchTracking;
using Xamarin.Essentials;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorView : ContentView
    {
        public LongUzorDrawingObject LongUzorGraphic { get; set; } = new LongUzorDrawingObject();

        private SKMatrix matrix = SKMatrix.MakeIdentity(); // scene-matrix, only for drawing Uzor-scene on screen or on bitmap
        private SKMatrix bitmapMatrix = SKMatrix.MakeIdentity(); // for double-buffering, reseted when the buffer is redrawing
        private SKMatrix backgroundMatrix = SKMatrix.MakeIdentity();

        private Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();

        private bool demonstrateMode_ = false;
        private bool doubleBuffering = true;
        //TODO: need to add ...mode for interactive elements (to turn off all events except zoom-moving)

        private SKBitmap bitmap;
        private SKBitmap backgroundBitmap;
        private SKCanvas bitmapCanvas;
        public bool DemonstrateMode { get { return demonstrateMode_; } set
            {
                demonstrateMode_ = value;
                this.InputTransparent = !value;
            }
        } 

        private void GridTouchEffect_TouchAction(object sender, TouchActionEventArgs args)
        {
            throw new NotImplementedException();
        }

        public LongUzorData Data 
        { 
            get { return LongUzorGraphic.Data; }
            set { this.LongUzorGraphic.Data = value; if (bitmap != null) bitmapInit(); }
        }

        public LongUzorView(LongUzorData data)
        {
            InitializeComponent();
            this.LongUzorGraphic.Data = data;
        }

        public LongUzorView()
        {
            InitializeComponent();
        }

        private void bitmapInit()
        {
            this.bitmap = new SKBitmap((int)this.canvasView.CanvasSize.Width, (int)this.canvasView.CanvasSize.Height);
            this.bitmapCanvas = new SKCanvas(bitmap);
            this.doubleBuffering = Preferences.Get("doubleBuffering", true);
            this.updateBitmap();
        }

        private void OnTouchEffectAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));


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
                            SKMatrix.PostConcat(ref bitmapMatrix, SKMatrix.MakeTranslation(delta.X, delta.Y));
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

                            SKMatrix.PostConcat(ref bitmapMatrix, scaleMatrix);   // NEW "PostContact() DOES NOT WORKING CORRECTLY!!!" 
                            SKMatrix.PostConcat(ref bitmapMatrix, translationMatrix);


                            touchDictionary[args.Id] = point;
                        }
                    }
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:

                    if (touchDictionary.ContainsKey(args.Id))
                        touchDictionary.Remove(args.Id);

                    if (matrix.ScaleX < 0.2)
                        matrix = SKMatrix.Identity;

                    updateBitmap();

                    break;
            }

            this.canvasView.InvalidateSurface();
        }

        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (bitmap == null)
                this.bitmapInit();

            if (doubleBuffering)
            {
                // TODO: background rendering

                e.Surface.Canvas.SetMatrix(bitmapMatrix);
                e.Surface.Canvas.Clear();
                e.Surface.Canvas.DrawBitmap(bitmap, 0, 0);
            }
            else
            {
                e.Surface.Canvas.SetMatrix(matrix);
                LongUzorGraphic.Draw(e.Surface.Canvas, canvasView);
            }

            // TODO: draw zoom-indicator in top angle with specifical parent-view 
        }
        private void updateBitmap()
        {
            bitmapCanvas.SetMatrix(matrix);
            LongUzorGraphic.Draw(bitmapCanvas, canvasView);
            bitmapMatrix = SKMatrix.MakeIdentity();
        }

        public void Draw()
        {
            if (bitmap == null)
                //bitmapInit();
                return;

            this.updateBitmap();
            this.canvasView.InvalidateSurface();
        }
    }
}