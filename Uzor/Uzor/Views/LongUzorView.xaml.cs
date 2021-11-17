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
using Uzor.Localization;
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
        private RenderingMode renderingMode = RenderingMode.Low;
        //TODO: need to add ...mode for interactive elements (to turn off all events except zoom-moving)

        private SKBitmap doubleBufferingBitmap;
        private SKBitmap bitmap; // for full-double-buffering
        private SKCanvas bitmapCanvas;

        private bool quickViewUpdateMode = false; // if the finger are moving on canvas
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
            set { this.LongUzorGraphic.Data = value; if (doubleBufferingBitmap != null && bitmap !=null) bitmapInit(); }
        }

        public LongUzorView(LongUzorData data)
        {
            InitializeComponent();
            this.LongUzorGraphic.Data = data;
            bitmapInit();
        }

        public LongUzorView()
        {
            InitializeComponent();
        }

        private void bitmapInit()
        {
            this.renderingMode = (RenderingMode)Preferences.Get("RenderingMode", 2);

            if (renderingMode == RenderingMode.DoubleBuffering)
            {
                this.doubleBufferingBitmap = new SKBitmap((int)this.canvasView.CanvasSize.Width, (int)this.canvasView.CanvasSize.Height);
                this.bitmapCanvas = new SKCanvas(doubleBufferingBitmap);
            }
            else if (renderingMode == RenderingMode.FullDoubleBuffering)
            {
                try
                {
                    this.bitmap = new SKBitmap(LongUzorGraphic.GetResultContentWidth() >7000? 7000 : LongUzorGraphic.GetResultContentWidth(),
                                               LongUzorGraphic.GetResultContentHeight()>13000? 13000: LongUzorGraphic.GetResultContentHeight());

                }
                catch(System.Exception e)
                {
                    this.bitmap = new SKBitmap(3000, 4000);
                    var mb = new MessageBox(e.Message + "    " + AppResource.SwitchRenderingAlert);
                    mb.OkButton_Clicked += hideAlert;
                    this.mainGrid.Children.Add(mb);
                    this.renderingMode = RenderingMode.Low;
                }
                
                this.bitmapCanvas = new SKCanvas(bitmap);
            }
            else
                return;
            
            this.updateBitmap();
        }

        private void hideAlert(object sender, EventArgs e)
        {
            this.mainGrid.Children.Remove(sender as MessageBox);
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
                        this.quickViewUpdateMode = true;
                        if (renderingMode == RenderingMode.DoubleBuffering)
                            Device.StartTimer(TimeSpan.FromMilliseconds(300), () => {
                               // MainThread.BeginInvokeOnMainThread(() =>
                               // {
                                    Draw();
                               // });

                                return this.quickViewUpdateMode;
                            });

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

                    this.quickViewUpdateMode = false;
                    updateBitmap();

                    break;
            }

            this.canvasView.InvalidateSurface();
        }

        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (doubleBufferingBitmap == null && bitmap == null)
                this.bitmapInit();

            switch(renderingMode)
            {
                case RenderingMode.Low:
                    e.Surface.Canvas.SetMatrix(matrix);
                    LongUzorGraphic.Draw(e.Surface.Canvas, canvasView);
                    break;
                case RenderingMode.DoubleBuffering:
                    e.Surface.Canvas.SetMatrix(bitmapMatrix);
                    e.Surface.Canvas.Clear();
                    e.Surface.Canvas.DrawBitmap(doubleBufferingBitmap, 0, 0);
                    break;
                case RenderingMode.FullDoubleBuffering:
                    e.Surface.Canvas.SetMatrix(matrix);
                    e.Surface.Canvas.Clear(this.Data.BackColor.ToSKColor());
                    e.Surface.Canvas.DrawBitmap(bitmap, canvasView.CanvasSize.Width/2 -bitmap.Width / 2, canvasView.CanvasSize.Height / 2 - bitmap.Height / 2);
                    break;
                default:
                    break;
            }

            // TODO: draw zoom-indicator in top angle with specifical parent-view 
        }
        private void updateBitmap()
        {
            if (renderingMode == RenderingMode.DoubleBuffering)
            {
                bitmapCanvas.SetMatrix(matrix);
                LongUzorGraphic.Draw(bitmapCanvas, canvasView);
                bitmapMatrix = SKMatrix.MakeIdentity();
            }
            else if (renderingMode == RenderingMode.FullDoubleBuffering)
            {
                bitmapCanvas.ResetMatrix();
                LongUzorGraphic.Draw(bitmapCanvas, bitmap.Width, bitmap.Height);
            }


        }

        public void Draw()
        {
            if (renderingMode == RenderingMode.DoubleBuffering)
            {
                if (doubleBufferingBitmap == null)
                    return;
                this.updateBitmap();
            }
            else if (renderingMode == RenderingMode.FullDoubleBuffering)
            {
                if (bitmap == null)
                    return;
                this.bitmapInit();

            }

            this.canvasView.InvalidateSurface();
        }
    }
}