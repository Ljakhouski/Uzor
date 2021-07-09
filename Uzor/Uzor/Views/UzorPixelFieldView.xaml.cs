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

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorPixelFieldView : ContentView
    {
        public int LayerNumber { get; set; } = 0;
        public bool EditingMode { get; set; } = true;
        public bool MirrorMode { get; set; } = true;
                                 
        public UzorData ThisData { get; set; }

        //private bool[,] EditableField;
        //private bool[,] FieldCoreForEditing;

        public bool DeleteMode = false;

        int WidthField; int HeightField;
        public UzorPixelFieldView(UzorData data)
        {
            InitializeComponent();
            this.ThisData = data;
            this.WidthField = data.FieldSize;
            this.HeightField = data.FieldSize;

            //EditableField = new bool[WidthField, HeightField];
           // FieldCoreForEditing = new bool[WidthField+1, HeightField+1];

            
           // Device.StartTimer(TimeSpan.FromMilliseconds(350), OnTimerTick);
        }
      
        /*public void SaveState()
        {
            //firstly saving current state
            this.ThisData.Layers[LayerNumber].AddNextState(EditableField);
            BasicDrawingAlgorithm.Calculate(this.ThisData.Layers[LayerNumber]);
            EditableField = this.ThisData.Layers[LayerNumber].GetLastState();
        }*/
        
       /* private bool OnTimerTick()
        {
            if (EditingMode)
                return true;

            CalculateField();
            //TODO: write new field in UzorData

            uzorFieldCanvasView.InvalidateSurface();
            return true;
        }*/

        public void DrawView()
        {
            uzorFieldCanvasView.InvalidateSurface();
        }
      
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (!EditingMode)
                return;

            //float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                case TouchActionType.Moved: 
                    {
                        try
                        {
                            WritePixel(args);
                        }
                        catch (IndexOutOfRangeException e) { }
                       
                        
                    }
                    break;
            }
            uzorFieldCanvasView.InvalidateSurface();
        }

        private void WritePixel(TouchActionEventArgs args) // TODO: debug clone array
        {
            float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            var f = /*(bool[,])*/this.ThisData.Layers[LayerNumber].GetLastState();//.Clone();
            try
            {
                if (MirrorMode)
                {
                    if ((int)(ConvertToPixel(args.Location).X / pixelSize)<= WidthField/2 && (int)(ConvertToPixel(args.Location).Y / pixelSize)<=HeightField/2)
                    {
                        f[WidthField-1-(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;

                        f[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                        f[WidthField-1-(int)(ConvertToPixel(args.Location).X / pixelSize), HeightField-1- (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                        f[(int)(ConvertToPixel(args.Location).X / pixelSize), HeightField-1- (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                    }
                }
                else
                    f[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;

                //this.ThisData.Layers[LayerNumber].EditLastState(f);
            }
            catch (IndexOutOfRangeException e) { }
        }
        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            float pixelSize = (float)uzorFieldCanvasView.CanvasSize.Width / WidthField;
            var f = this.ThisData.Layers[LayerNumber].GetLastState();
            this.uzorFieldCanvasView.HeightRequest = contentView.Width;
           // this.uzorFieldCanvasView.WidthRequest = contentView.Width;


            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(Color.Yellow.ToSKColor());


            for(int w = 0; w < WidthField; w++)
                for (int h = 0; h < HeightField; h++)
                {
                    if (f[w, h] == false)
                    {
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize,  new SKPaint() { Color = ThisData.Layers[0].BackColor});
                    }
                    else
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, new SKPaint() { Color = ThisData.Layers[0].FrontColor });
                }

            // drawing '+' in center
            if (EditingMode)
            {
                canvas.DrawLine((float)((uzorFieldCanvasView.CanvasSize.Width / 2.0) - 50),
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0),
                                (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0) + 50,
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0),
                                new SKPaint() { Color = Color.FromRgba(10,10,10,100).ToSKColor(), StrokeWidth=10}
                                );

                canvas.DrawLine((float)((uzorFieldCanvasView.CanvasSize.Width / 2.0)), 
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0-50), 
                                (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0),
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0) + 50, 
                                new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth=10 }
                                );
            }

            // drawing gray indicator of !drawable field
            if (MirrorMode)
            {
                canvas.DrawLine((float)((uzorFieldCanvasView.CanvasSize.Width / 2.0) - 50),
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0),
                                (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0) + 50,
                                (float)(uzorFieldCanvasView.CanvasSize.Height / 2.0),
                                new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 }
                                );

                canvas.DrawRect((float)(uzorFieldCanvasView.CanvasSize.Width / 2.0), 0, (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0),
                    uzorFieldCanvasView.CanvasSize.Width, new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 });
                canvas.DrawRect(0, (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0), (float)(uzorFieldCanvasView.CanvasSize.Width / 2.0),
                    (float)(uzorFieldCanvasView.CanvasSize.Width/2.0), new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 });
            }
            
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(uzorFieldCanvasView.CanvasSize.Width * pt.X / uzorFieldCanvasView.Width),
                               (float)(uzorFieldCanvasView.CanvasSize.Height * pt.Y / uzorFieldCanvasView.Height));
        }
      

       /* public void SaveCurrentState(int layerNumber = 0)
        {
            this.ThisData.Layers[layerNumber].AddNextState(FieldCore);
        }*/
        

        
    }
}