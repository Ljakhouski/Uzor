using System;
using System.Collections.Generic;
using System.Text;

namespace Uzor
{
    public enum PixelType
    {
        Front,
        Back
    }

    [Serializable]
    public class Pixel
    {
        public PixelType Type { get; set; }
        public bool ColorInverted { get; set; } = false;
    }

    [Serializable]  // TODO: need saving in file
    public class UzorData
    {
        public List<List<Pixel>> CurrentField { get; set; }
        public List<List<List<Pixel>>> Fields { get; set; }

       /* public List<List<Pixel>> GetField (int number)
        {
            return Fields[number];
        }*/
        public string Name { get; set; }
        public int FieldSize { get; set; }
        public DateTime DataOfCreation { get; }

        public SkiaSharp.SKColor FrontPixelColor { get; set; }
        public SkiaSharp.SKColor BackgroundPixelColor { get; set; }

        public UzorData(string name, DateTime time, int size)
        {
            this.Name = name;
            this.DataOfCreation = time;
            this.FieldSize = size;
        }
    } 
}
