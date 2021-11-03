using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Uzor.Data;

namespace Uzor.EditorObjects
{
    class Background : EditorObject
    {
        public UzorData Data { get; set; }
        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            canvas.DrawRect(new SKRect(0, 0, view.CanvasSize.Width, view.CanvasSize.Height), new SKPaint() { Color = Data.Layers[0].BackColor.ToSKColor() });
        }

        public Background(UzorData data)
        {
            this.Data = data;
        }
    }
}
