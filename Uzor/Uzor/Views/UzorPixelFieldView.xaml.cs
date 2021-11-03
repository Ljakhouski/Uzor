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
using Uzor.EditorObjects;
using Uzor.Data;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorPixelFieldView : ContentView
    {

        //public UzorData ThisData { get; set; }

        public List<EditorObject> EditorObjectssList {get;set;} = new List<EditorObject>();
        public bool MultiTouchEnabled { get; set; } = true;
        public bool RotationMultiTouchEnabled { get; set; } = false;


        private SKMatrix matrix = SKMatrix.MakeIdentity();
        private Dictionary<long, SKPoint> touchDictionary = new Dictionary<long, SKPoint>();
        /*public UzorPixelFieldView(UzorData data)
        {
            InitializeComponent();
            //this.ThisData = data;
            becomeSquare();
        }*/
        public UzorPixelFieldView()
        {
            InitializeComponent();
            BecomeSquare();
        }
      
        public void BecomeSquare()
        {
            this.uzorFieldCanvasView.HeightRequest = this.uzorFieldCanvasView.Width;
            this.uzorFieldCanvasView.MinimumHeightRequest = this.uzorFieldCanvasView.Width;
        }
        public void DrawView()
        {
            uzorFieldCanvasView.InvalidateSurface();
        }

      
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            foreach (EditorObject o in EditorObjectssList)
                o.TouchEffectAction(args, uzorFieldCanvasView);
            
            uzorFieldCanvasView.InvalidateSurface();
        }

       
        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas; 
            canvas.Clear(new SKColor(0,0,0,0));

            // zoom scene:
            //canvas.Scale((float)this.Scale, (float)this.Scale, uzorFieldCanvasView.CanvasSize.Width / 2, uzorFieldCanvasView.CanvasSize.Height / 2);
           // canvas.SetMatrix(matrix); // new

            foreach (EditorObject o in EditorObjectssList)
                o.Draw(canvas, uzorFieldCanvasView/*, matrix*/);

           BecomeSquare();
        }

        
    
        

        float Magnitude(SKPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }
        private void sizeChangedEvent(object sender, EventArgs e)
        {
            BecomeSquare();
        }
    }
}