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
            this.frontColor = data.Layers[0].FrontColor.ToNativeColor();
            this.backColor = data.Layers[0].BackColor.ToNativeColor();
            updateView();
        }

        private void colorValue_Changed(object sender, ValueChangedEventArgs e)
        {
            Color c = Color.FromHsla(((Slider)FindByName("H_Slider")).Value,
                                     ((Slider)FindByName("S_Slider")).Value,
                                     ((Slider)FindByName("L_Slider")).Value,
                                     ((Slider)FindByName("A_Slider")).Value);

            updateSelectColor(c);


            /*Slider S = (Slider)sender;
            if (S == FindByName("H_Slider"))
            {
                //this.getCurrentColor().Hue = S.Value;
            }
            else if (S == FindByName("S_Slider"))
            {

            }
            else if (S == FindByName("L_Slider"))
            {

            }
            else if (S == FindByName("A_Slider"))
            {

            }*/
            //if (S.)
        }

        private void updateSelectColor(Color c)
        {
            if (this.picker.SelectedIndex == 0)
            {
                this.frontColor = c;
                this.data.Layers[0].FrontColor = PixelColor.FromNativeColor(c);
            }
            else
            {
                this.backColor = c;
                this.data.Layers[0].BackColor = PixelColor.FromNativeColor(c);
            }

            updateView();
        }

        private void updateView()
        {
            this.frontEllipse.Fill = new SolidColorBrush(this.frontColor);
            this.backEllipse.Fill = new SolidColorBrush(this.backColor);
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