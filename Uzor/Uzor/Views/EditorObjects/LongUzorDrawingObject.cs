using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;
using Uzor.Data;

namespace Uzor.Views.EditorObjects
{
    class LongUzorEditorObjects : EditorObject
    {

        public LongUzorData Data { get; set; }
        public LongUzorEditorObjects(LongUzorData data)
        {
            this.Data = data;
        }

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            
        }

        public override void TouchEffectAction(TouchActionEventArgs args, SKCanvasView view)
        {
            base.TouchEffectAction(args, view);
        }
    }
}
