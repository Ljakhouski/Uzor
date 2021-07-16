using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItemPage : ContentPage
    {
        public UzorItemPage(UzorData data)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            var upfView = new UzorPixelFieldView(data);
            this.uzorFieldFrame.Content = upfView;
            upfView.EditingMode = false;
            itemNameLabel.Text = data.Name;
        }
    }
}