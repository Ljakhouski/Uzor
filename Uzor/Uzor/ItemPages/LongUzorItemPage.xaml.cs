using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.ItemPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorItemPage : ContentPage
    {
        private MainPage pageForAlert;
        public LongUzorItemPage(LongUzorData data, MainPage p)
        {
            InitializeComponent();
            this.pageForAlert = p;
            this.itemNameLabel.Text = data.Name;
            this.longUzorView.Data = data;
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LongUzorEditorPage(longUzorView.Data, pageForAlert));
        }

        private void imageSaving_Clicked(object sender, EventArgs e)
        {
            var v = new ImageBufferSaveView(this.longUzorView.Data);
            v.BackgroundTapped += hideSavingView;
            this.backgroundGrid.Children.Add(v);
        }

        private void hideSavingView(object sender, EventArgs e)
        {
            this.backgroundGrid.Children.Remove(sender as ImageBufferSaveView);
        }
    }
}