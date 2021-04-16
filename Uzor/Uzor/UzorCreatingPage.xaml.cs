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
    public partial class UzorCreatingPage : ContentPage
    {
        public UzorCreatingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SaveSetting_ ss = SaveSetting;
            var nus = new NewUzorSetting(ss);
            gridCreatingPage.Children.Add(nus);
            
        }
        public delegate void SaveSetting_(UzorData data);
        public void SaveSetting(UzorData data)
        {
            gridCreatingPage.Children.Clear();
            gridCreatingPage.Children.Add(new UzorPixelFieldView(data));
           // gridCreatingPage.Children.Add(new NewUzorSetting(SaveSetting));
        }
    }
}