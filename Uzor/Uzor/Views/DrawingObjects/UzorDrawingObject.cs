using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TouchTracking;
using Xamarin.Forms;

namespace Uzor.Views.DrawingObjects
{
    class UzorDrawingObject : DrawingObject
    {
        public UzorData Data { get; set; }
        public int LayerNumber { get; set; }
        public bool EditingMode { get; set; } = true;
        public bool MirrorMode { get; set; } = true;
        public bool DeleteMode { get; set; } = false;

        public int Scale { get; set; } = 1;
        public override void Draw(SKCanvas canvas, SKCanvasView view/*, SKMatrix matrix*/)
        {
            //canvas.SetMatrix(matrix); // new

            float pixelSize = (float)view.CanvasSize.Width /  Data.FieldSize;
            var f = this.Data.Layers[LayerNumber].GetLastState();

            SKColor backColor  = this.Data.Layers[0]. BackColor.ToSKColor();
            SKColor frontColor = this.Data.Layers[0].FrontColor.ToSKColor();

            SKPaint backPaint  = new SKPaint { Color = backColor  };
            SKPaint frontPaint = new SKPaint { Color = frontColor };

            for (int w = 0; w < Data.FieldSize; w++)
                for (int h = 0; h < Data.FieldSize; h++)
                {
                    if (f[w, h] == false)
                    {
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize,  /*new SKPaint() { Color = backColor}*/ backPaint);
                    }
                    else
                        canvas.DrawRect((float)w * pixelSize, (float)h * pixelSize, pixelSize, pixelSize, /*new SKPaint() { Color = frontColor }*/ frontPaint);
                }

            if (EditingMode)
            {
                var paint = new SKPaint() { Color = Color.FromRgba(5, 5, 5, 20).ToSKColor(), StrokeWidth = 2 };

                for (int w = 0; w < Data.FieldSize; w++)
                    canvas.DrawLine((float)((w + 1) * pixelSize), 0, (float)((w + 1) * pixelSize), (float)(view.CanvasSize.Height), paint);
                for (int h = 0; h < Data.FieldSize; h++)
                    canvas.DrawLine(0, (float)((h + 1) * pixelSize), (float)(view.CanvasSize.Width), (float)((h + 1) * pixelSize), paint);
            }
        }

        public override void Touched(TouchActionEventArgs args, SKCanvasView view)
        {
            // float pixelSize = (float)((contentView.Width) / HeightField) * ((float)Device.Info.PixelScreenSize.Width / (float)contentView.Width);
            float pixelSize = (float)(view.CanvasSize.Width / Data.FieldSize);
            var f = /*(bool[,])*/this.Data.Layers[LayerNumber].GetLastState();//.Clone();

            //int x = (int)(ConvertToPixel(args.Location).X / pixelSize);
            //int y = (int)(ConvertToPixel(args.Location).Y / pixelSize);

            int xLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).X / Scale + (view.CanvasSize.Width - (view.CanvasSize.Width / Scale)) / 2));
            int yLocationAfterScaling = (int)((ConvertToPixel(args.Location, view).Y / Scale + (view.CanvasSize.Height - (view.CanvasSize.Height / Scale)) / 2));

            int x = (int)(xLocationAfterScaling / pixelSize);
            int y = (int)(yLocationAfterScaling / pixelSize);
            try
            {
                if (MirrorMode)
                {
                    if (x <= Data.FieldSize / 2 && y <= Data.FieldSize / 2)
                    {
                        f[Data.FieldSize - 1 - x, y] = DeleteMode ? false : true;

                        f[x, y] = DeleteMode ? false : true;
                        f[Data.FieldSize - 1 - x, Data.FieldSize - 1 - y] = DeleteMode ? false : true;
                        f[x, Data.FieldSize - 1 - y] = DeleteMode ? false : true;
                    }

                }
                else
                    f[x, y] = DeleteMode ? false : true;

                //this.ThisData.Layers[LayerNumber].EditLastState(f);
            }
            catch (IndexOutOfRangeException e) { }
        }

        SKPoint ConvertToPixel(Point pt, SKCanvasView view)
        {
            return new SKPoint((float)(view.CanvasSize.Width * pt.X / view.Width),
                               (float)(view.CanvasSize.Height * pt.Y / view.Height));
        }
    }
}
