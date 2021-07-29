using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;

namespace Uzor.Views.DrawingObjects
{
    public class DrawingObject
    {
        public bool IsVisible { get; set; } = true;
        public virtual void Draw(SKCanvas canvas, SKCanvasView view /*, SKMatrix matrix*/)
        {

        }

        public virtual void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view) { }
    }
}
