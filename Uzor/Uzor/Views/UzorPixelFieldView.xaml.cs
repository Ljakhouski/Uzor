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
        int TickOfGame = 0;
        bool IsActiveMode = false;

        private bool[,] FieldCore;
        private bool[,] FieldCoreForEditing;

        int WidthField = 35; int HeightField = 35;
        
        public UzorPixelFieldView()
        {
            
            InitializeComponent();
            double i = this.Content.Width;
            

            FieldCore = new bool[WidthField, HeightField];
            FieldCoreForEditing = new bool[WidthField, HeightField];

            Device.StartTimer(TimeSpan.FromMilliseconds(500), OnTimerTick);
        }

        public void StartCalculation()
        {
            EditingMode = false;
        }

        public void StopCaltulation()
        {
            EditingMode = true;
        }
        private bool OnTimerTick()
        {
            if (EditingMode)
                return true;

            CalculateField();
            uzorFieldCanvasView.InvalidateSurface();
            return true;
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (!EditingMode)
                return;

            float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    {
                        try
                        {
                            FieldCore[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = true;

                            labelPostition.Text = args.Location.X + " <- args location X, " + args.Location.Y + " <- Y\n"+
                                ConvertToPixel(args.Location).X + " <- converttopixel X, "+ (ConvertToPixel(args.Location).Y + " <-Y\n");
                        }
                        catch (IndexOutOfRangeException e) { }
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved: 
                    {
                        try
                        {
                            FieldCore[(int)(ConvertToPixel(args.Location).X / pixelSize), (int)(ConvertToPixel(args.Location).Y / pixelSize)] = true;

                            labelPostition.Text = args.Location.X + " <- args location X, " + args.Location.Y + " <- Y\n" +
                               ConvertToPixel(args.Location).X + " <- converttopixel X, " + (ConvertToPixel(args.Location).Y + " <-Y\n");
                        }
                        catch (IndexOutOfRangeException e) { }
                       
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            float pixelSize = (float)((contentView.Width) / HeightField)*((float)Device.Info.PixelScreenSize.Width/ (float)contentView.Width);

        
            this.uzorFieldCanvasView.HeightRequest = contentView.Width;
            this.uzorFieldCanvasView.WidthRequest = contentView.Width;


            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(Color.Yellow.ToSKColor());


            for(int w = 1; w < WidthField - 1; w++)
                for (int h = 1; h < HeightField - 1; h++)
                {
                    if (FieldCore[w, h] == false)
                    {
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, new SKPaint() { Color = Color.FromRgb(255, 255, 255).ToSKColor() });
                    }
                    else
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, new SKPaint() { Color = Color.Red.ToSKColor() });
                }

            this.labelinfowah.Text = gridField.Height + " <- Height, " + gridField.HeightRequest + " <- HeightRequest"+ gridField.Width+ " <- Width \n"
                +this.uzorFieldCanvasView.Height+" <-uzorFieldXanvasView.Height "+ this.uzorFieldCanvasView.Width + " <-uzorFieldXanvasView.Width ";
            
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
                    if (FieldCore[w + i, h + y] && !(y == 0 && i == 0))
                        count++;
                }
            return count;
        }
        void CalculateField()
        {
            for (int w = 1; w < WidthField - 1; w++)
                for (int h = 1; h < HeightField - 1; h++)
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
    }
}