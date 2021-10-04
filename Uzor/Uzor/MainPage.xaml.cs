using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
            int i = new Random().Next(0, 2);
            switch(i)
            {
                case 1:
                    this.backGroundImage.Source = "uzor2BackGround.PNG";
                    break;
                case 2:
                    this.backGroundImage.Source = "uzor3BackGround.PNG";
                    break;
                default:
                    break;
            }
        }

        private async void NewUzor(object sender, EventArgs e)
        {
            //mediaElement.Stop();
            await Navigation.PushModalAsync(new NavigationPage(new UzorCreatingPage()), true);
            //stack.Children.Add(new UzorItem());
//      edited:      MakeUzorItemList();
            //mediaElement.Play();
            

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
                if (fileName[fileName.Length - 4] != 'l')
                    this.stack.Children.Add(new UzorItem((UzorData)formatter.Deserialize(fs), this));
                fs.Dispose();
            }
        }
        private async void TestNow(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new TestPage()));
        }

        private void settingClick(object sender, EventArgs e)
        {

        }
    }
}
