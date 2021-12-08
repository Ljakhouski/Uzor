using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Uzor.Data
{

    [Serializable]
    public class PixelColor
    {
        public int R { get; set; } = 255;
        public int G { get; set; } = 255;
        public int B { get; set; } = 255;
        public int A { get; set; } = 255;

        public SKColor ToSKColor()
        {
            return new SKColor((byte)R, (byte)G, (byte)B, (byte)A);
        }

        public Color ToNativeColor()
        {
            return Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
        }

        public Color ToXamarinFormsColor()
        {
            return Xamarin.Forms.Color.FromRgba(R, G, B, A);
        }

        public PixelColor (int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static PixelColor FromNativeColor(Color color)
        {
            return new PixelColor(color.R, color.G, color.B, color.A);
        }

        public PixelColor(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

    }

    [Serializable]
    public class Field
    {
        public PixelColor FrontColor { get; set; } = new PixelColor(255, 0, 0);
        public PixelColor BackColor { get; set; }  = new PixelColor(255, 255, 255, 255);
        private List<bool[,]> Content { get; set; } = new List<bool[,]>(); // List<> of all states of Content
        public bool ColorInverted { get; set; } = false;
        public int Step { get; set; } = -1;
        public bool[,] FieldCropMask { get; set; } 
        public bool[,] GetAndSetPreviousState ()
        {
            var lastState = (bool[,])Content[Content.Count - 1].Clone();
            Content.RemoveAt(Content.Count - 1);
            Step--;
            return lastState;
        }
        
        public void EditLastState(bool[,] c)
        {
            this.Content[Content.Count - 1] = c;
        }
        public void AddNextState(bool[,] c)
        {
            Content.Add(c);
            Step++;
        }
        public bool[,] GetLastState()
        { 
            return Content[Content.Count - 1];
        }
        public Field(int size)
        {
            this.Content.Add(new bool[size, size]);
            this.FieldCropMask = new bool[size, size];
        }
    }

    [Serializable]  // TODO: need saving in file
    public class UzorData
    {
       
        
        public List<Field> Layers { get; set; }

       /* public List<List<Pixel>> GetField (int number)
        {
            return Fields[number];
        }*/
        public string Name { get; set; }
        public int FieldSize { get; set; }
        public DateTime DataOfCreation { get; }
        public bool[,] CropMask { get; set; }


        public UzorData(string name, DateTime time, int size)
        {
            this.Name = name;
            this.DataOfCreation = time;
            this.FieldSize = size;
            this.Layers = new List<Field>();
            this.Layers.Add(new Field(size));
            this.CropMask = new bool[size, size];
        } 
        
        public bool CropMaskIsEmpty()
        {
            if (this.CropMask == null)
                return true;

            for (int i = 0; i <= this.CropMask.GetUpperBound(0); i++)
                for (int j = 0; j <= this.CropMask.GetUpperBound(1); j++)
                    if (this.CropMask[i, j])
                        return false;
            return true;
        }

        public int GetMaskSize()
        {
            int arrayCenterIndex = this.CropMask.GetUpperBound(1) / 2;
            int maskSize = 0;

            for (int i = 0; i <= this.CropMask.GetUpperBound(0); i++)
                if (this.CropMask[i, arrayCenterIndex])
                    maskSize++;

            return maskSize;
        }

        public void Replace(UzorData data, bool saveColor = false)
        {
            var b = this.Layers[0].BackColor;
            var f = this.Layers[0].FrontColor;

            this.FieldSize = data.FieldSize;
            this.Layers = data.Layers; // TODO: make copy

            if (data.CropMask != null)
                this.CropMask = (bool[,])data.CropMask.Clone();
            else
                this.CropMask = null;

            if (saveColor)
            {
                this.Layers[0].BackColor = b;
                this.Layers[0].FrontColor = f;
            }
        }

        public void Clear()
        {
            this.Replace(new UzorData(this.Name, this.DataOfCreation, this.FieldSize), true);
        }
    } 
}
