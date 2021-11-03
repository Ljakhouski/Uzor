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
    public partial class ColorPickerView : ContentView
    {
        public ColorPickerView()
        {
            InitializeComponent();
        }

        private void colorValue_Changed(object sender, ValueChangedEventArgs e)
        {
            Slider S = (Slider)sender;
            //if (S.)
        }

        private void onPickerChanged(object sender, EventArgs e)
        {

        }
    }
}