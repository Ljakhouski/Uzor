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
    public partial class UzorItem : ContentView
    {
        public UzorItem()
        {
            InitializeComponent();
        }

        private void TapOnItem(object sender, EventArgs e)
        {
            this.ItemFrame.BackgroundColor = Color.LightGreen;
        }
    }
}