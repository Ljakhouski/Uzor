using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.ItemPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorItemPage : ContentPage
    {
        public LongUzorItemPage(LongUzorData data)
        {
            InitializeComponent();
            this.itemNameLabel.Text = data.Name;
            this.longUzorView.Data = data;
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LongUzorEditorPage(longUzorView.Data));
        }
    }
}