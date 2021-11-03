using System;
using System.Collections.Generic;
using System.Text;

namespace Uzor.Data
{
    [Serializable]
    public class LongUzorData
    {
        public string Name 
        { 
            get 
            { 
                return UzorElements[0].Name; 
            } 
            set
            {
                this.UzorElements[0].Name = value;
            }
        }
        public DateTime DataOfCreation 
        { 
            get
            {
                return UzorElements[0].DataOfCreation;
            }
        }

        public UzorData[] UzorElements { get; set; } // first element is the main-element, he has a background color of long-uzor 
        public UzorData SidePattern { get; set; }

        public int A { get; set; } = 200; // distance between Uzor-elements (vertical)
        public int B { get; set; } = 100; // distance between SidePattern-elements (horisontal)
        public int C { get; set; } // crop-factor of SidePattern
        public int D { get; set; } = 100; // distance between single-long-uzor
        public LongUzorData()
        { }

        public PixelColor FrontColor 
        { 
            get 
            {
                return UzorElements[0].Layers[0].FrontColor; 
            } 
            set
            {
                UzorElements[0].Layers[0].FrontColor = value;
            }
        }

        public PixelColor BackColor
        {
            get
            {
                return UzorElements[0].Layers[0].BackColor;
            }
            set
            {
                UzorElements[0].Layers[0].BackColor = value;
            }
        }
    }
}
