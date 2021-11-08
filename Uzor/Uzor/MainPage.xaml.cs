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
            await Navigation.PushModalAsync(new NavigationPage(new UzorCreatingPage(this)), true);
            //stack.Children.Add(new UzorItem());
//      edited:      MakeUzorItemList();
            //mediaElement.Play();
            

        }

        public void MakeUzorItemList()
        {
            this.itemStack.Children.Clear();

            var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            foreach(string fileName in fileList)
                if (fileName.Substring(fileName.Length - 4) == ".ubf")
                    this.itemStack.Children.Add(new UzorItem(UzorProjectFileManager.LoadUzorData(fileName), fileName, this));
                else if (fileName.Substring(fileName.Length - 5) == ".lubf") //{   File.Delete(fileName); }
                    this.itemStack.Children.Add(new LongUzorItem(UzorProjectFileManager.LoadLongUzorData(fileName), fileName, this));
        }

        private async void settingClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingPage());
        }
    }
}
