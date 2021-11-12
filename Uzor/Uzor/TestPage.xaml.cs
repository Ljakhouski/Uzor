using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
           // Device.StartTimer(TimeSpan.FromMilliseconds(0), OnTimerTick);
        }
       
     
        private void chckbx_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //if (chckbx.IsChecked)
            //    this.uzorView.StartCalculation();
            //else
            //    this.uzorView.StopCaltulation();
        }

        private void sliderValue_Changed(object sender, ValueChangedEventArgs e)
        {
            this.preview.HeightRequest = e.NewValue;
            LongUzorView v = this.preview;
            v.Data = UzorProjectFileManager.LoadLongUzorData("/data/user/0/com.companyname.uzor/files/.local/share/new Uzor5.lubf");
            v.Draw();
        }
    }
}