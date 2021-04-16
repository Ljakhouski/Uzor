using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uzor
{

    [Serializable]
    public class Layer
    {
        public SKColor FrontColor { get; set; }// = new SKColor() { };
        public SKColor BackColor { get; set; }
        private List<bool[,]> Field { get; set; } = new List<bool[,]>();
        public bool ColorInverted { get; set; } = false;
        public int Step { get; set; } = -1;
        public bool[,] GetPreviousState ()
        {
            var lastState = (bool[,])Field[Field.Count - 1].Clone();
            Field.RemoveAt(Field.Count - 1);
            Step--;
            return lastState;
        }
        public void AddNextState(bool[,] field)
        {
            Field.Add(field);
            Step++;
        }
    }

    [Serializable]  // TODO: need saving in file
    public class UzorData
    {
       
        
        public List<Layer> Layers { get; set; }

       /* public List<List<Pixel>> GetField (int number)
        {
            return Fields[number];
        }*/
        public string Name { get; set; }
        public int FieldSize { get; set; }
        public DateTime DataOfCreation { get; }

       

        public UzorData(string name, DateTime time, int size)
        {
            this.Name = name;
            this.DataOfCreation = time;
            this.FieldSize = size;
            this.Layers = new List<Layer>();
            this.Layers.Add(new Layer());
        }
    } 
}
