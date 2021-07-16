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
        public UzorData Data { get; set; }
        public UzorItem()
        {
            InitializeComponent();
        }

        public void SetUzorNameLabelText(string name)
        {
            this.itemName.Text = name;
        }
        private async void TapOnItem(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UzorItemPage()));
        }
    }
}