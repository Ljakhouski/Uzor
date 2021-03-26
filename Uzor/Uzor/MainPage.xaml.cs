using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Views;
using Xamarin.Forms;

namespace Uzor
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void NewUzor(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UzorCreatingPage()));
            stack.Children.Add(new UzorItem());
        }
    }
}
