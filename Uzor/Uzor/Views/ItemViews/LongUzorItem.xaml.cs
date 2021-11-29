using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.ItemPages;
using Uzor.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorItem : ContentView
    {
        public LongUzorData Data { get; set; }
        private MainPage pageForAlert;
        private string path;
        public LongUzorItem(LongUzorData data, string path, MainPage p)
        {
            InitializeComponent();
            this.Data = data;
            this.path = path;
            LongUzorView v = this.preview;

            v.Data = data;

            this.itemName.Text = data.Name.Split("/".ToCharArray()).Last();
            this.itemDate.Text = data.DataOfCreation.ToString();
            /*this.mineFrame.BackgroundColor = new Color(data.Layers[0].BackColor.R,
                                                        data.Layers[0].BackColor.G,
                                                        data.Layers[0].BackColor.B);*/

            //this.backgroungStartGradientColor.Color = new Color( data.BackColor.R, data.BackColor.G, data.BackColor.B, /*data.BackColor.A*/ 20);
            //this.backgroungStopGradientColor.Color = new Color(data.BackColor.R, data.BackColor.G, data.BackColor.B, 20);
            this.backGround.BackgroundColor = data.BackColor.ToNativeColor(); //new Color(data.BackColor.R, data.BackColor.G, data.BackColor.B, data.BackColor.A); 

            if (data.BackColor.A >180 && data.BackColor.ToXamarinFormsColor().GetBrightness() < 0.5f)
            {
                this.itemDate.TextColor = Color.White;
                this.itemName.TextColor = Color.White;
            }
            this.pageForAlert = p;
        }

        private async void TapOnItem(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new LongUzorItemPage(Data, path, pageForAlert)));
        }

        private async void deleteItem(object sender, EventArgs e)
        {
            if (await pageForAlert.DisplayAlert("", AppResource.DeleteQuestion, AppResource.Yes, AppResource.No))
            {
                //var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + this.Data.Name + ".lubf");
                pageForAlert.itemStack.Children.Remove(this);
            }
        }
    }
}