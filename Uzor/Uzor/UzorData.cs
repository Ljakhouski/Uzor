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
    class Pixel
    {
        public PixelType Type { get; set; }
        public bool ColorInverted { get; set; } = false;
    }

    [Serializable]  // TODO: need saving in file
    class UzorData
    {
        public List<List<Pixel>> Field { get; set; }

        public string Name { get; set; }
        public DateTime DataOfCreation { get; }

        public SkiaSharp.SKColor FrontPixelColor { get; set; }
        public SkiaSharp.SKColor BackgroundPixelColor { get; set; }

        public UzorData(string name, DateTime time)
        {
            this.Name = name;
            this.DataOfCreation = time;
        }
    } 
}
