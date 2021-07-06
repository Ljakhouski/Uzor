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

            //videoView1.Start();
            try
            {
                //Assets.
                //   videoView1.Source = ImageSource.FromFile("bg.mp4");
                //mediaElement.Source = "ms-appx:///bg.mp4";
            }
            catch (Exception e) {
                int i = 0;
            }
        }

        private async void NewUzor(object sender, EventArgs e)
        {
            mediaElement.Stop();
            await Navigation.PushModalAsync(new NavigationPage(new UzorCreatingPage()));
            stack.Children.Add(new UzorItem());
            mediaElement.Play();
           

        }

        private async void TestNow(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new TestPage()));
        }
    }
}
