using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerView : ContentView
    {
        private UzorData data;
        private Color frontColor;
        private Color backColor;

        public event EventHandler BackgroundTapped;
        public ColorPickerView(UzorData data)
        {
            this.data = data;
            InitializeComponent();
            this.frontColor = data.Layers[0].FrontColor.ToNativeColor(); var c = Color.FromHsla(1,2,3,4); 
            this.backColor = data.Layers[0].BackColor.ToNativeColor();
        }

        private void colorValue_Changed(object sender, ValueChangedEventArgs e)
        {
            Slider S = (Slider)sender;
            if (S == FindByName("H"))
            {
                //this.getCurrentColor().Hue = S.Value;
            }
            else if (S == FindByName("S"))
            {

            }
            else if (S == FindByName("L"))
            {

            }
            else if (S == FindByName("A"))
            {

            }
            //if (S.)
        }

        private Color getCurrentColor()
        {
            if (this.picker.SelectedIndex == 0)
                return this.frontColor;
            else
                return this.backColor;
        }
        private void onPickerChanged(object sender, EventArgs e)
        {

        }

        private void background_Tapped(object sender, EventArgs e)
        {
            viewFrame.FadeTo(0);
            background.FadeTo(0);
            BackgroundTapped?.Invoke(this, null);
        }
    }
}