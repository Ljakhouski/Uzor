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
        private DistanceParametersView distanceParametersView;
        private LayersParapetersView layoutParametersView;
        private SaveView saveView;
        public LongUzorEditorPage(LongUzorData data)
        {
            
            InitializeComponent();
            this.longUzorView.Data = data;
            initializeDropMenus();

            
            // this.distanceView.
        }

        private void initializeDropMenus()
        {
            this.distanceParametersView = new DistanceParametersView(longUzorView);
            AbsoluteLayout.SetLayoutFlags(distanceParametersView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(distanceParametersView, new Rectangle(1,1, 1, 0.3));

            this.layoutParametersView = new LayersParapetersView();
            AbsoluteLayout.SetLayoutFlags(layoutParametersView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layoutParametersView, new Rectangle(1, 1, 1, 0.5));

            this.saveView = new SaveView();
            AbsoluteLayout.SetLayoutFlags(saveView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(saveView, new Rectangle(1, 1, 1, 0.5));
        }
        private async void hideDownAllDropMenu()
        {
            blackBackground.FadeTo(0, 250);

            if (dropDownMenuLayout.Children.Contains(distanceParametersView))
            {
                await distanceParametersView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(distanceParametersView);
            }
            else if (dropDownMenuLayout.Children.Contains(layoutParametersView))
            {
               
                await layoutParametersView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(layoutParametersView);

            }
            else if (dropDownMenuLayout.Children.Contains(saveView))
            {
                await saveView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(saveView);
            }
                

            //dropDownMenuLayout.Children.Clear();
            //dropDownMenuLayout.Children.Add(blackBackground);

        }
        private async void ABC_Parameters_Clicked(object sender, EventArgs e)
        {


            if (!dropDownMenuLayout.Children.Contains(distanceParametersView))
            {
                hideDownAllDropMenu();

                distanceParametersView.TranslationY = 300;
                distanceParametersView.IsVisible = true;

                //blackBackground.FadeTo(0.3, 250);  
                blackBackground.Opacity = 0; // !!!

                dropDownMenuLayout.Children.Add(distanceParametersView);
                await distanceParametersView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }

        private async void layersMenu_Clicked(object sender, EventArgs e)
        {
            if (!dropDownMenuLayout.Children.Contains(layoutParametersView))
            {
                hideDownAllDropMenu();

                layoutParametersView.TranslationY = 700;
                blackBackground.FadeTo(0.3, 250);

                dropDownMenuLayout.Children.Add(layoutParametersView);
                await layoutParametersView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }

        private async void saveMenu_Clicked(object sender, EventArgs e)
        {
            if (!dropDownMenuLayout.Children.Contains(saveView))
            {
                hideDownAllDropMenu();

                saveView.TranslationY = 700;
                blackBackground.FadeTo(0.3, 250);

                dropDownMenuLayout.Children.Add(saveView);
                await saveView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }
    }
}