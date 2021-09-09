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

        public PixelColor (int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
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
        public PixelColor BackColor { get; set; }  = new PixelColor(255, 255, 255);
        private List<bool[,]> Content { get; set; } = new List<bool[,]>(); // List<> of all states of Content
        public bool ColorInverted { get; set; } = false;
        public int Step { get; set; } = -1;
        public bool[,] CropMask { get; set; } 
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
            this.CropMask = new bool[size, size];
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
            for (int i = 0; i <= this.CropMask.GetUpperBound(0); i++)
                for (int j = 0; j <= this.CropMask.GetUpperBound(1); j++)
                    if (this.CropMask[i, j])
                        return false;
            return true;
        }
    } 
}
