/*
 
 
 */
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
using Uzor.Algorithm;
using Uzor.Views.DrawingObjects;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorPixelFieldView : ContentView
    {

        public UzorData ThisData { get; set; }

        public List<DrawingObject> DrawingObjectsList {get;set;} = new List<DrawingObject>();
        public bool MultiTouchEnabled { get; set; } = true;
        public bool RotationMultiTouchEnabled { get; set; } = false;
        
        public UzorPixelFieldView(UzorData data)
        {
            InitializeComponent();
            this.ThisData = data;
        }
        public UzorPixelFieldView()
        {
            InitializeComponent();
        }
      
        public void DrawView()
        {
            uzorFieldCanvasView.InvalidateSurface();
        }
      
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {

            //float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                case TouchActionType.Moved: 
                    {
                        try
                        {
                           // WritePixel(args) ;
                            foreach (DrawingObject o in DrawingObjectsList)
                                o.Touched(args, uzorFieldCanvasView);
                        }
                        catch (IndexOutOfRangeException e) { }
                       
                        
                    }
                    break;
            }  
            uzorFieldCanvasView.InvalidateSurface();
        }

       
        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas; 
            canvas.Clear(new SKColor(0,0,0,0));

            // zoom scene:
            //canvas.Scale((float)this.Scale, (float)this.Scale, uzorFieldCanvasView.CanvasSize.Width / 2, uzorFieldCanvasView.CanvasSize.Height / 2);
            canvas.SetMatrix(matrix); // new

            foreach (DrawingObject o in DrawingObjectsList)
                o.Draw(canvas, uzorFieldCanvasView/*, matrix*/);
            
        }

        SKMatrix matrix = SKMatrix.MakeIdentity();
        Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
    
    void OnTouchEffectAction2(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(uzorFieldCanvasView.CanvasSize.Width * pt.X / uzorFieldCanvasView.Width),
                            (float)(uzorFieldCanvasView.CanvasSize.Height * pt.Y / uzorFieldCanvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    // Find transformed bitmap rectangle
                    SKRect rect = new SKRect(0, 0, uzorFieldCanvasView.CanvasSize.Width, uzorFieldCanvasView.CanvasSize.Height); // 'bitmap' replaced
                    rect = matrix.MapRect(rect);

                    // Determine if the touch was within that rectangle
                    if (rect.Contains(point) && !touchDictionary.ContainsKey(args.Id))
                    {
                        touchDictionary.Add(args.Id, point);
                    }
                    try
                    {
                        // WritePixel(args) ;
                        foreach (DrawingObject o in DrawingObjectsList)
                            o.Touched(args, uzorFieldCanvasView);
                    }
                    catch (IndexOutOfRangeException e) { }
                    uzorFieldCanvasView.InvalidateSurface();
                    break;
                case TouchActionType.Moved:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        // Single-finger drag
                        if (touchDictionary.Count == 1)
                        {
                            try
                            {
                                // WritePixel(args) ;
                                foreach (DrawingObject o in DrawingObjectsList)
                                    o.Touched(args, uzorFieldCanvasView);
                            }
                            catch (IndexOutOfRangeException e) { }
                        }
                        // Double-finger scale and drag
                        else if (touchDictionary.Count >= 2 && MultiTouchEnabled)
                        {
                            // Copy two dictionary keys into array
                            /*long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index of non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points involved in the transform
                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            // Calculate two vectors
                            SKPoint oldVector = prevPoint - pivotPoint;
                            SKPoint newVector = newPoint - pivotPoint;

                            // Scaling factors are ratios of those
                            float scaleX = newVector.X / oldVector.X;
                            float scaleY = newVector.Y / oldVector.Y;

                            if (!float.IsNaN(scaleX) && !float.IsInfinity(scaleX) &&
                                !float.IsNaN(scaleY) && !float.IsInfinity(scaleY))
                            {
                                // If something bad hasn't happened, calculate a scale and translation matrix
                                SKMatrix scaleMatrix =
                                    SKMatrix.MakeScale(scaleX, scaleX, pivotPoint.X, pivotPoint.Y); // edited pivotPoint.X [2]

                                SKMatrix.PostConcat(ref matrix, scaleMatrix);
                                uzorFieldCanvasView.InvalidateSurface();
                            }*/

                            // Copy two dictionary keys into array
                            long[] keys = new long[touchDictionary.Count];
                            touchDictionary.Keys.CopyTo(keys, 0);

                            // Find index non-moving (pivot) finger
                            int pivotIndex = (keys[0] == args.Id) ? 1 : 0;

                            // Get the three points in the transform
                            SKPoint pivotPoint = touchDictionary[keys[pivotIndex]];
                            SKPoint prevPoint = touchDictionary[args.Id];
                            SKPoint newPoint = point;

                            // Calculate two vectors
                            SKPoint oldVector = prevPoint - pivotPoint;
                            SKPoint newVector = newPoint - pivotPoint;

                            // Find angles from pivot point to touch points
                            float oldAngle = (float)Math.Atan2(oldVector.Y, oldVector.X);
                            float newAngle = (float)Math.Atan2(newVector.Y, newVector.X);

                            // Calculate rotation matrix

                            float angle = 0;
                            if (RotationMultiTouchEnabled)
                                angle = newAngle - oldAngle;
                                
                            SKMatrix touchMatrix = SKMatrix.MakeRotation(angle, pivotPoint.X, pivotPoint.Y);

                            // Effectively rotate the old vector
                            float magnitudeRatio = Magnitude(oldVector) / Magnitude(newVector);
                            oldVector.X = magnitudeRatio * newVector.X;
                            oldVector.Y = magnitudeRatio * newVector.Y;

                            // Isotropic scaling!
                            float scale = Magnitude(newVector) / Magnitude(oldVector);

                            if (!float.IsNaN(scale) && !float.IsInfinity(scale))
                            {
                                SKMatrix.PostConcat(ref touchMatrix,
                                    SKMatrix.MakeScale(scale, scale, pivotPoint.X, pivotPoint.Y));

                                SKMatrix.PostConcat(ref matrix, touchMatrix);
                                this.uzorFieldCanvasView.InvalidateSurface();
                            }
                        }

                        // Store the new point in the dictionary
                        touchDictionary[args.Id] = point;
                    }
                    uzorFieldCanvasView.InvalidateSurface();

                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchDictionary.ContainsKey(args.Id))
                    {
                        touchDictionary.Remove(args.Id);
                    }
                    break;
            }
        }
        float Magnitude(SKPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }
        private void sizeChangedEvent(object sender, EventArgs e)
        {
            this.uzorFieldCanvasView.HeightRequest = this.uzorFieldCanvasView.Width;
        }
    }
}