using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace Uzor.Views.DrawingObjects
{
    class DemonstrateUzorDrawingObject : DrawingObject
    {
        public UzorData Data { get; set; }
        public int LayerNumber { get; set; } = 0;
        public bool GradientMode { get; set; } = false;

        public override void Draw(SKCanvas canvas, SKCanvasView view)
        {
            float pixelSize = (float)view.CanvasSize.Width / Data.FieldSize;
            var f = this.Data.Layers[LayerNumber].GetLastState();

            SKColor backColor = this.Data.Layers[0].BackColor.ToSKColor();
            SKColor frontColor = this.Data.Layers[0].FrontColor.ToSKColor();

            SKPaint backPaint = new SKPaint { Color = backColor };
            SKPaint frontPaint = new SKPaint { Color = frontColor };

            frontPaint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(view.CanvasSize.Width / 4, view.CanvasSize.Height / 2),
                                    new SKPoint(view.CanvasSize.Width, view.CanvasSize.Height / 2),
                                    new SKColor[] { frontColor, new SKColor(frontColor.Red, frontColor.Green, frontColor.Blue, 0) },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Clamp);


            backPaint.Shader = SKShader.CreateLinearGradient(
                                new SKPoint(view.CanvasSize.Width / 4, view.CanvasSize.Height / 2),
                                new SKPoint(view.CanvasSize.Width, view.CanvasSize.Height / 2),
                                new SKColor[] { backColor, new SKColor(backColor.Red, backColor.Green, backColor.Blue, 0) },
                                new float[] { 0, 1 },
                                SKShaderTileMode.Clamp);

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

        }
    }
}
