using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Uzor.UzorCreatingPage;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUzorSetting : ContentView
    {
        SaveSetting_ saveSettingDelegateFunc;
        public NewUzorSetting(SaveSetting_ ss)
        {
            saveSettingDelegateFunc = ss;
            InitializeComponent();
        }
      
        private void sizeSliderChanged(object sender, ValueChangedEventArgs e)
        {
            sizeSliderLabel.Text =((int)e.NewValue).ToString();
        }

        private void OK_click(object sender, EventArgs e)
        {
            saveSettingDelegateFunc(new UzorData(entryName.Text, DateTime.Now, (int)sizeSlider.Value));
        }
    }
}