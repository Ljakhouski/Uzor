using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Uzor.Views.EditorObjects;
using SkiaSharp;
using Uzor.Data;
using TouchTracking;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorView : ContentView
    {
        public LongUzorDrawingObject LongUzorGraphic { get; set; } = new LongUzorDrawingObject();

        private SKMatrix matrix = SKMatrix.MakeIdentity();
        private Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();

        public LongUzorData Data 
        { 
            get { return LongUzorGraphic.Data; }
            set { this.LongUzorGraphic.Data = value; }
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

            this.canvasView.InvalidateSurface();
        }

        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.SetMatrix(matrix);
            LongUzorGraphic.Draw(e.Surface.Canvas, canvasView);

            // TODO: draw zoom-indicator in top angle with specifical parent-view 
        }

        public void Draw()
        {
            this.canvasView.InvalidateSurface();
        }
    }
}