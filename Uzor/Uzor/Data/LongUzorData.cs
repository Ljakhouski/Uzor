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
        public int B { get; set; } // distance between SidePattern-elements (horisontal)
        public int C { get; set; } // crop-factor of SidePattern
        public LongUzorData()
        { }

    }
}
