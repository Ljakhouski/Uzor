using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Views;
using Uzor.Views.DrawingObjects;
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
            upfView.DrawingObjectsList.Add(new DemonstrateUzorDrawingObject() { Data = data, GradientMode = false });
            this.uzorFieldFrame.Content = upfView;
            itemNameLabel.Text = data.Name;
        }
    }
}