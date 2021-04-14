using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUzorSetting : ContentView
    {
        public NewUzorSetting()
        {
            InitializeComponent();
        }

        private void sizeSliderChanged(object sender, ValueChangedEventArgs e)
        {
            sizeSliderLabel.Text =((int)e.NewValue).ToString();
        }
    }
}