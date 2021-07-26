using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Uzor.Views.DrawingObjects
{
    class CenterMarker : DrawingObject
    {
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.Restore();

            canvas.DrawLine((float)((view.CanvasSize.Width / 2.0) - 50),
                            (float)(view.CanvasSize.Height / 2.0),
                            (float)(view.CanvasSize.Width / 2.0) + 50,
                            (float)(view.CanvasSize.Height / 2.0),
                            new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 }
                            );

            canvas.DrawLine((float)((view.CanvasSize.Width / 2.0)),
                            (float)(view.CanvasSize.Height / 2.0 - 50),
                            (float)(view.CanvasSize.Width / 2.0),
                            (float)(view.CanvasSize.Height / 2.0) + 50,
                            new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 }
                            );
        }
    }
}
