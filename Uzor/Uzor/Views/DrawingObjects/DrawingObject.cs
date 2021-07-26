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

        // for all event-type:
        public virtual void Pressed() { }
        public virtual void Moved() { }
        public virtual void Released() { }

        // actual event (if pressed on view):
        public virtual void Touched(TouchActionEventArgs args, SKCanvasView view)
        {

        }
    }
}
