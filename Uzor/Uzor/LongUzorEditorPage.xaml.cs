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

        private void ABC_Parameters_Clicked(object sender, EventArgs e)
        {
            if (!menuLayout.Children.Contains(distanceView))
            {
                menuLayout.IsVisible = true;
                menuLayout.Children.Add(distanceView);
            }
            else
            {
                menuLayout.IsVisible = false;

                menuLayout.Children.Remove(distanceView);
            }
        }

        private void layersMenu_Clicked(object sender, EventArgs e)
        {

        }

        private void saveMenu_Clicked(object sender, EventArgs e)
        {

        }
    }
}