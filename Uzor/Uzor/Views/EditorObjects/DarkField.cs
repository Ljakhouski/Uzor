using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Uzor.Views.EditorObjects
{
    class DarkField : EditorObject
    {
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            if (IsVisible)
            {
                canvas.DrawRect((float)(view.CanvasSize.Width / 2.0), 0, (float)(view.CanvasSize.Width / 2.0),
                    view.CanvasSize.Width, new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 });
                canvas.DrawRect(0, (float)(view.CanvasSize.Width / 2.0), (float)(view.CanvasSize.Width / 2.0),
                    (float)(view.CanvasSize.Width / 2.0), new SKPaint() { Color = Color.FromRgba(10, 10, 10, 100).ToSKColor(), StrokeWidth = 10 });
            }
        }
    }
}
