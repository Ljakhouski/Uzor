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
        private UzorData     data;
        private LongUzorData longUzorData;

        private Color frontColor;
        private Color backColor;

        public event EventHandler BackgroundTapped;
        public ColorPickerView(UzorData data)
        {
            this.data = data;
            InitializeComponent();
            this.frontColor = data.Layers[0].FrontColor.ToNativeColor();
            this.backColor = data.Layers[0].BackColor.ToNativeColor();
            this.picker.SelectedIndex = 0;
            setColorSliderValues();
            updateView();
        }

        public ColorPickerView(LongUzorData data)
        {
            this.longUzorData = data;
            InitializeComponent();
            this.frontColor = data.FrontColor.ToNativeColor();
            this.backColor = data.BackColor.ToNativeColor();
            this.picker.SelectedIndex = 0;
            setColorSliderValues();
            updateView();
        }

        private void colorValue_Changed(object sender, ValueChangedEventArgs e)
        {
            Color c = Color.FromHsla(((Slider)FindByName("H_Slider")).Value,
                                     ((Slider)FindByName("S_Slider")).Value,
                                     ((Slider)FindByName("L_Slider")).Value,
                                     ((Slider)FindByName("A_Slider")).Value);

            setSelectColor(c);
            updateView();
            /*
            Slider S = (Slider)sender;
            if (S == FindByName("H_Slider"))
            {
                H_Label.Text = ""
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

        private void setColorSliderValues()
        {
            if (picker.SelectedIndex == 0)
            {
                H_Slider.Value = frontColor.Hue;
                S_Slider.Value = frontColor.Saturation;
                L_Slider.Value = frontColor.Luminosity;
                A_Slider.Value = frontColor.A;
            }
            else if (picker.SelectedIndex == 1)
            {
                H_Slider.Value = backColor.Hue;
                S_Slider.Value = backColor.Saturation;
                L_Slider.Value = backColor.Luminosity;
                A_Slider.Value = backColor.A;
            }
        }
        private void updateColorSLiderLabel()
        {
            H_Label.Text = "Hue: " + (H_Slider.Value.ToString().Length > 4 ? H_Slider.Value.ToString().Substring(0,4): H_Slider.Value.ToString());
            S_Label.Text = "Sat: " + (S_Slider.Value.ToString().Length > 4 ? S_Slider.Value.ToString().Substring(0,4) : S_Slider.Value.ToString());
            L_Label.Text = "Lum: " + (L_Slider.Value.ToString().Length > 4 ? L_Slider.Value.ToString().Substring(0, 4) : L_Slider.Value.ToString());
            A_Label.Text = A_Slider.Value.ToString().Length > 4? A_Slider.Value.ToString().Substring(0, 4) : A_Slider.Value.ToString();
        }

        private void setSelectColor(Color c)
        {
            if (this.picker.SelectedIndex == 0)
                this.frontColor = c;
            else
                this.backColor = c;
        }

        private void updateView()
        {
            this.frontEllipse.Fill = new SolidColorBrush(this.frontColor);
            this.backEllipse.Fill = new SolidColorBrush(this.backColor);
            updateColorSLiderLabel();
        }
        private void onPickerChanged(object sender, EventArgs e)
        {
            setColorSliderValues();
        }

        private void background_Tapped(object sender, EventArgs e)
        {
            viewFrame.FadeTo(0);
            background.FadeTo(0);
            BackgroundTapped?.Invoke(this, null);
        }

        private void OK_Clicked(object sender, EventArgs e)
        {
            if (this.longUzorData != null)
            {
                this.longUzorData.FrontColor = PixelColor.FromNativeColor(this.frontColor);
                this.longUzorData.BackColor = PixelColor.FromNativeColor(this.backColor);

                if (saveAllColorCheckbox.IsChecked)
                {
                    foreach (UzorData d in this.longUzorData.UzorElements)
                    {
                        d.Layers[0].FrontColor = PixelColor.FromNativeColor(this.frontColor);
                        d.Layers[0].BackColor = PixelColor.FromNativeColor(this.backColor);
                    }

                    this.longUzorData.SidePattern.Layers[0].FrontColor = PixelColor.FromNativeColor(this.frontColor);
                    this.longUzorData.SidePattern.Layers[0].BackColor = PixelColor.FromNativeColor(this.backColor);
                }
                else
                    // edit only backGround color for LongUzorData
                    this.longUzorData.SidePattern.Layers[0].BackColor = PixelColor.FromNativeColor(this.backColor);
            }
            else if (this.data != null)
            {
                this.data.Layers[0].FrontColor = PixelColor.FromNativeColor(this.frontColor);
                this.data.Layers[0].BackColor = PixelColor.FromNativeColor(this.backColor);
            }
            BackgroundTapped?.Invoke(this, null);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            BackgroundTapped?.Invoke(this, null);
        }
    }
}