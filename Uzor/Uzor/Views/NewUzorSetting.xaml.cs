using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor;
using Uzor.Data;
using Uzor.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Uzor.UzorCreatingPage;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUzorSetting : ContentView
    {
        SaveSetting_ saveSettingDelegateFunc;
        public bool IsSquareUzorMode = true;
        public NewUzorSetting(SaveSetting_ ss)
        {
            saveSettingDelegateFunc = ss;
            InitializeComponent();
            SquareUzorModeFrame.ScaleTo(1.1, 100);
            this.SquareUzorModeTapped(null, null);
        }

        private void sizeSliderChanged(object sender, ValueChangedEventArgs e)
        {
            sizeSliderLabel.Text = ((int)e.NewValue).ToString();
        }

        private void OK_click(object sender, EventArgs e)
        {
            saveSettingDelegateFunc(makeAndFillUzor());
        }

        private UzorData makeAndFillUzor()
        {
            var d = new UzorData(entryName.Text, DateTime.Now, (int)sizeSlider.Value);
            if (d.FieldSize % 2 == 0)
            {
                switch (new Random().Next(1, 3))
                {
                    
                    case 1: //while (new Random().Next(1, 3) != 3) { }
                        var b = new bool[d.FieldSize, d.FieldSize];
                        b[d.FieldSize / 2, d.FieldSize / 2] = true;
                        b[d.FieldSize / 2, d.FieldSize / 2 - 1] = true;
                        b[d.FieldSize / 2 - 1, d.FieldSize / 2 - 1] = true;
                        b[d.FieldSize / 2 - 1, d.FieldSize / 2] = true;

                        d.Layers[0].AddNextState(b);

                        return d;
                    case 2:
                    case 3:
                        var b2 = new bool[d.FieldSize, d.FieldSize];
                        //Central
                        b2[d.FieldSize / 2, d.FieldSize / 2] = true;
                        b2[d.FieldSize / 2, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2] = true;

                        b2[d.FieldSize / 2, d.FieldSize / 2 + 1] = true;
                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2 + 1] = true;
                        b2[d.FieldSize / 2, d.FieldSize / 2 + 2] = true;
                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2 + 2] = true;

                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2 - 2] = true;
                        b2[d.FieldSize / 2, d.FieldSize / 2 - 2] = true;
                        b2[d.FieldSize / 2 - 1, d.FieldSize / 2 - 3] = true;
                        b2[d.FieldSize / 2, d.FieldSize / 2 - 3] = true;

                        b2[d.FieldSize / 2 - 2, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 - 2, d.FieldSize / 2] = true;
                        b2[d.FieldSize / 2 - 3, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 - 3, d.FieldSize / 2] = true;

                        b2[d.FieldSize / 2 + 1, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 + 1, d.FieldSize / 2] = true;
                        b2[d.FieldSize / 2 + 2, d.FieldSize / 2 - 1] = true;
                        b2[d.FieldSize / 2 + 2, d.FieldSize / 2] = true;

                        d.Layers[0].AddNextState(b2);

                        return d;
                }
            }
            else
            {
                switch (new Random().Next(1, 3))
                {
                    case 1:
                        var b = new bool[d.FieldSize, d.FieldSize];
                        b[d.FieldSize / 2, d.FieldSize / 2] = true;
                        b[d.FieldSize / 2, d.FieldSize / 2 - 1] = true;
                        b[d.FieldSize / 2, d.FieldSize / 2 + 1] = true;
                        b[d.FieldSize / 2 - 1, d.FieldSize / 2] = true;
                        b[d.FieldSize / 2 + 1, d.FieldSize / 2] = true;

                        d.Layers[0].AddNextState(b);

                        return d;
                    case 2:
                    case 3:
                        // TODO: remake
                        var b2 = new bool[d.FieldSize, d.FieldSize];
                        //Central
                        var arr = new bool[5, 5]
                        {
                            { false, false, true, false,false },
                            { false, true, true, true,  false},
                            { true,  true, true, true,  true },
                            { false, true, true, true,  false},
                            { false, false, true, false,false},
                        };

                        for (int i = b2.GetLength(0) / 2 - 2; i <= b2.GetLength(0) / 2 + 2; i++)
                            for (int j = b2.GetLength(1) / 2 - 2; j <= b2.GetLength(1) / 2 + 2; j++)
                            {
                                b2[i, j] = arr[i - (b2.GetLength(0) / 2 - 2), j - (b2.GetLength(1) / 2 - 2)];
                            }

                        d.Layers[0].AddNextState(b2);

                        return d;

                }
            }

            return null;
        }
        private async void SquareUzorModeTapped(object sender, EventArgs e)
        {
            SquareUzorModeEllipse.IsVisible = true;
            LongUzorModeEllipse.IsVisible = false;
            this.IsSquareUzorMode = true;

            SquareUzorModeFrame.ScaleTo(1.1, 100);
            LongUzorModeFrame.ScaleTo(1, 100);

            this.descriptionLabel.Text = AppResource.Description + " " + AppResource.SquareUzorDescription;
        }

        private async void LongUzorModeTapped(object sender, EventArgs e)
        {
            SquareUzorModeEllipse.IsVisible = false;
            LongUzorModeEllipse.IsVisible = true;
            this.IsSquareUzorMode = false;

            SquareUzorModeFrame.ScaleTo(1, 100); //draw a single element of the pattern
            LongUzorModeFrame.ScaleTo(1.1, 100); // a full-fledged pattern of simpler elements

            this.descriptionLabel.Text = AppResource.Description + " " + AppResource.LongUzorDescription;
        }

        private void editEntry_Clicked(object sender, EventArgs e)
        {
            this.entryName.Text = "";
            this.entryName.Focus();
        }
    }
}