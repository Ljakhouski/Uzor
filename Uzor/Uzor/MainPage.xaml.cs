using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
          
                //Assets.
                //   videoView1.Source = ImageSource.FromFile("bg.mp4");
                //mediaElement.Source = "ms-appx:///bg.mp4";

                MakeUzorItemList();
            
        }

        private async void NewUzor(object sender, EventArgs e)
        {
            mediaElement.Stop();
            await Navigation.PushModalAsync(new NavigationPage(new UzorCreatingPage()));
            //stack.Children.Add(new UzorItem());
            MakeUzorItemList();
            mediaElement.Play();
           

        }

        private void MakeUzorItemList()
        {
            this.stack.Children.Clear();

            BinaryFormatter formatter = new BinaryFormatter();
            var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            foreach(string fileName in fileList)
            {
                var i = new UzorItem();
                FileStream fs = new FileStream(fileName, FileMode.Open);
                //i.Data = 
                //i.SetUzorNameLabelText( fileName.Split("/".ToCharArray()).Last());

                this.stack.Children.Add(new UzorItem((UzorData)formatter.Deserialize(fs)));
                fs.Dispose();
            }
        }
        private async void TestNow(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new TestPage()));
        }
    }
}
