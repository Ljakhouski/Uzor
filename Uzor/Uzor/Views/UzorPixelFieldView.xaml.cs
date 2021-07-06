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
    public partial class UzorPixelFieldView : ContentView
    {
        public bool EditingMode { get; set; } = true;
        public bool MirrorMode { get; set; } = true;

        private UzorData ThisData { get; set; }

        private bool[,] FieldCore;
        private bool[,] FieldCoreForEditing;

        private bool DeleteMode = false;

        int WidthField; int HeightField;
     
        public UzorPixelFieldView(UzorData data)
        {
            InitializeComponent();
            this.ThisData = data;
            this.WidthField = data.FieldSize;
            this.HeightField = data.FieldSize;

            FieldCore = new bool[WidthField+1, HeightField+1];
            FieldCoreForEditing = new bool[WidthField+1, HeightField+1];

            
            Device.StartTimer(TimeSpan.FromMilliseconds(350), OnTimerTick);
        }
      
        public void SaveUzor()
        {
            //firstly saving current state
            this.ThisData.Layers[0].AddNextState(FieldCore);


        }
        public void StartCalculation()
        {
            EditingMode = false;
            calculationButton.Text = "[stop]";
        }

        public void StopCaltulation()
        {
            EditingMode = true;
            calculationButton.Text = "[start]";
            uzorFieldCanvasView.InvalidateSurface();
        }
        private bool OnTimerTick()
        {
            if (EditingMode)
                return true;

            CalculateField();
            //TODO: write new field in UzorData

            uzorFieldCanvasView.InvalidateSurface();
            return true;
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

        private void WritePixel(TouchActionEventArgs args)
        {
            float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            try
            {
                if (MirrorMode)
                {
                    if ((int)(ConvertToPixel(args.Location).X / pixelSize)<= WidthField/2 && (int)(ConvertToPixel(args.Location).Y / pixelSize)<=HeightField/2)
                    {
                        FieldCore[WidthField-1-(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;

                        FieldCore[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                        FieldCore[WidthField -1- (int)(ConvertToPixel(args.Location).X / pixelSize), HeightField -1- (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                        FieldCore[(int)(ConvertToPixel(args.Location).X / pixelSize), HeightField -1- (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
                    }
                }
                else 
                    FieldCore[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = DeleteMode ? false : true;
            }
            catch (IndexOutOfRangeException e) { }
        }
        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            float pixelSize = (float)uzorFieldCanvasView.CanvasSize.Width / WidthField;

            this.uzorFieldCanvasView.HeightRequest = contentView.Width;
           // this.uzorFieldCanvasView.WidthRequest = contentView.Width;


            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(Color.Yellow.ToSKColor());

          

            for(int w = 0; w < WidthField; w++)
                for (int h = 0; h < HeightField; h++)
                {
                    if (FieldCore[w, h] == false)
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
        int CountAround(int w, int h)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
                for (int y = -1; y < 2; y++)
                {
                    if (w+i>=0 && w+i<WidthField && h+y>=0 && h+y<HeightField)  // for replace "OutOfRangeException"  
                        if (FieldCore[w + i, h + y] && 
                                !(y == 0 && i == 0)  // current pixel
                                )
                            count++;
                }
            return count;
        }
        void CalculateField()
        {
            //firstly saving current state
            this.ThisData.Layers[0].AddNextState(FieldCore);

            for (int w = 0; w < WidthField; w++)
                for (int h = 0; h < HeightField; h++)
                {
                    int countCellsAround = CountAround(w, h);
                

                    if (countCellsAround == 2 || countCellsAround == 3)
                    {
                        FieldCoreForEditing[w, h] = true;
                        // return;
                    }
                    else
                    {
                        FieldCoreForEditing[w, h] = false;
                    }
                    
                }

            FieldCore = (bool[,])FieldCoreForEditing.Clone();
            
        }

        private void CalculationButtonClick(object sender, EventArgs e)
        {
            if (EditingMode)
                this.StartCalculation();
            else
                this.StopCaltulation();
        }

        private void beforeButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            if (ThisData.Layers[0].Step>=0)
                this.FieldCore = ThisData.Layers[0].GetPreviousState();
            uzorFieldCanvasView.InvalidateSurface();
            //uzorFieldCanvasView.FadeTo(0, 250);
        }

        private void nextButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            CalculateField();
            uzorFieldCanvasView.InvalidateSurface();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            if (!DeleteMode)
                deleteButton.Text = "[ draw ]";
            else
                deleteButton.Text = "[delete]";

            DeleteMode = !DeleteMode;
        }

        private void invertButtonClick(object sender, EventArgs e)
        {
            SKColor c = ThisData.Layers[0].FrontColor;

            ThisData.Layers[0].FrontColor = ThisData.Layers[0].BackColor;
            ThisData.Layers[0].BackColor = c;
            uzorFieldCanvasView.InvalidateSurface();
        }

        private void mirrorButtonClick(object sender, EventArgs e)
        {
            if (!MirrorMode)
                mirrorButton.Text = "[before mode]";
            else
                mirrorButton.Text = "[mirror mode]";
            MirrorMode = !MirrorMode;
            uzorFieldCanvasView.InvalidateSurface();
        }
    }
}