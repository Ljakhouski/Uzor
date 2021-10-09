using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Uzor.Views.EditorObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItemPage : ContentPage
    {
        private UzorData data;
        private MainPage pageForAlert;
        public UzorItemPage(UzorData data, MainPage p)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this.data = data;
            var upfView = new UzorPixelFieldView();
            upfView.EditorObjectssList.Add(new DemonstrateUzorEditorObject() { Data = data, GradientMode = false });
            this.uzorFieldFrame.Content = upfView;
            itemNameLabel.Text = data.Name;
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UzorCreatingPage(this.data, this.pageForAlert));
        }
    }
}