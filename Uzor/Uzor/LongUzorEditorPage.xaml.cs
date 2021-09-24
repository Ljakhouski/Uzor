using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views.LongUzorEditorPageViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorEditorPage : ContentPage
    {
        private DistanceParametersView distanceView; 
        public LongUzorEditorPage(LongUzorData data)
        {
            
            InitializeComponent();
            this.longUzorView.Data = data;
            this.distanceView = new DistanceParametersView(longUzorView);
            AbsoluteLayout.SetLayoutFlags(distanceView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(distanceView, new Rectangle(1,1, 1, 0.3));
           // this.distanceView.
        }

        private async void ABC_Parameters_Clicked(object sender, EventArgs e)
        {
            if (!menuLayout.Children.Contains(distanceView))
            {
                distanceView.TranslationY = 300;
                menuLayout.IsVisible = true;
                backrect.FadeTo(0.3, 250);
                
                menuLayout.Children.Add(distanceView);
                await distanceView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
            {
                backrect.FadeTo(0, 250);
                await distanceView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                menuLayout.IsVisible = false;

                menuLayout.Children.Remove(distanceView);
            }
        }

        private async void layersMenu_Clicked(object sender, EventArgs e)
        {
            //distanceView.TranslateTo(0, distanceView.TranslationY+10);

            await Task.WhenAll(
            distanceView.TranslateTo(0, 0, 2000, Easing.SinInOut));
        }

        private void saveMenu_Clicked(object sender, EventArgs e)
        {

        }
    }
}