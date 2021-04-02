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

namespace Uzor
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
           // Device.StartTimer(TimeSpan.FromMilliseconds(0), OnTimerTick);
        }

        private bool OnTimerTick()
        {
            uzorFieldCanvasView.InvalidateSurface();
            return true;
        }

    
        private void onCanvasViewPaintSurface2(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
          //  canvas.Clear();
          //  canvas.DrawColor(Color.DarkGray.ToSKColor());
            Random rand = new Random();
            for (int i = 0; i < 25; i++)
                for (int i_ = 0; i_ < 25; i_++)
                {
                    SKPaint paint = new SKPaint
                    {
                        Color = Color.FromRgb(rand.Next(0, 255), 100, 100).ToSKColor(),

                    };
                    // RectangleField[i, i_].Background = new SolidColorBrush(Color.FromRgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));

                    canvas.DrawRect((float)i * (350 / 40.0f) * 2.7f, (float)i_ * (267 / 30.0f) * 2.7f, (350 / 44.0f) * 2.7f, (350 / 40.0f) * 2.7f, paint);
                }
            SKPaint paint2 = new SKPaint
            {
                Color = Color.FromRgb(200, 200, 200).ToSKColor(),

            };
            if (last_point!=null)
                canvas.DrawCircle(last_point, 50, paint2);
        }

        SKPoint last_point;
     
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> completedPaths = new List<SKPath>();

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            TextSize = 50,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(uzorFieldCanvasView.CanvasSize.Width * pt.X / uzorFieldCanvasView.Width),
                               (float)(uzorFieldCanvasView.CanvasSize.Height * pt.Y / uzorFieldCanvasView.Height));
        }
      

        string event_name ="";
        int i = 0;
        private void onCanvasViewPaintSurface_drawing(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();

            foreach (SKPath path in completedPaths)
            {
                canvas.DrawPath(path, paint);
            }

            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }

            canvas.DrawText(event_name, new SKPoint { X = 60, Y=60}, paint);
            canvas.DrawText(i.ToString(), new SKPoint { X = 60, Y = 100 }, paint);
            i++;
        }


        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        completedPaths.Add(inProgressPaths[args.Id]);
                        inProgressPaths.Remove(args.Id);
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        inProgressPaths.Remove(args.Id);
                        uzorFieldCanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private void onCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();
            

            foreach (SKPath path in completedPaths)
            {
                canvas.DrawPath(path, paint);
            }

            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }
        }
    }
}