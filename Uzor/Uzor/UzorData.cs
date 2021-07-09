using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Uzor
{

    [Serializable]
    public class Field
    {
        public SKColor FrontColor { get; set; } = new SKColor(255, 0, 0);
        public SKColor BackColor { get; set; }  = new SKColor(255, 255, 255);
        private List<bool[,]> Content { get; set; } = new List<bool[,]>(); // List<> of all states of Content
        public bool ColorInverted { get; set; } = false;
        public int Step { get; set; } = -1;
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
            this.Content.Add(new bool[size,size]);
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

       

        public UzorData(string name, DateTime time, int size)
        {
            this.Name = name;
            this.DataOfCreation = time;
            this.FieldSize = size;
            this.Layers = new List<Field>();
            this.Layers.Add(new Field(size));
        }
    } 
}
