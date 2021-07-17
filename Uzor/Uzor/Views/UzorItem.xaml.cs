using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItem : ContentView
    {
        public UzorData Data { get; set; }
        public UzorItem()
        {
            
        }

        public UzorItem(UzorData data)
        {
            InitializeComponent();
            this.Data = data;
            UzorPixelFieldView upv = this.preview;
            upv.GradientPreviewMode = true;
            upv.ThisData = data;
            upv.DrawView();
            this.itemName.Text = data.Name.Split("/".ToCharArray()).Last();
        }
        public void SetUzorNameLabelText(string name)
        {
            this.itemName.Text = name;
        }
        private async void TapOnItem(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UzorItemPage(Data)));
        }
    }
}