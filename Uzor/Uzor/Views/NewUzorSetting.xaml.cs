using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor;
using Uzor.Data;
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
        }
      
        private void sizeSliderChanged(object sender, ValueChangedEventArgs e)
        {
            sizeSliderLabel.Text =((int)e.NewValue).ToString();
        }

        private void OK_click(object sender, EventArgs e)
        {
            saveSettingDelegateFunc(new UzorData(entryName.Text, DateTime.Now, (int)sizeSlider.Value));
        }

        private async void SquareUzorModeTapped(object sender, EventArgs e)
        {
            SquareUzorModeEllipse.IsVisible = true;
            LongUzorModeEllipse.IsVisible = false;
            this.IsSquareUzorMode = true;

            SquareUzorModeFrame.ScaleTo(1.1, 100);
            LongUzorModeFrame.ScaleTo(1, 100);
        }

        private async void LongUzorModeTapped(object sender, EventArgs e)
        {
            SquareUzorModeEllipse.IsVisible = false;
            LongUzorModeEllipse.IsVisible = true;
            this.IsSquareUzorMode = false;
            
            SquareUzorModeFrame.ScaleTo(1, 100); //draw a single element of the pattern
            LongUzorModeFrame.ScaleTo(1.1, 100); // a full-fledged pattern of simpler elements
        }
    }
}