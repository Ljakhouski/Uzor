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
        public UzorItemPage(UzorData data)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            var upfView = new UzorPixelFieldView();
            upfView.EditorObjectssList.Add(new DemonstrateUzorEditorObject() { Data = data, GradientMode = false });
            this.uzorFieldFrame.Content = upfView;
            itemNameLabel.Text = data.Name;
        }
    }
}