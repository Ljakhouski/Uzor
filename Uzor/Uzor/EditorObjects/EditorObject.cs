﻿using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;

namespace Uzor.Views.EditorObjects
{
    public class EditorObject
    {
        public bool IsVisible { get; set; } = true;
        public virtual void Draw(SKCanvas canvas, SKCanvasView view /*, SKMatrix matrix*/)
        {

        }

        public virtual void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view) { }
    }
}