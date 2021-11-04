using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.LongUzorEditorPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LayersParapetersView : ContentView
    {
        public LayersParapetersView()
        {
            InitializeComponent();
        }

       

        private void colorChange_Clicked(object sender, EventArgs e)
        {

        }

        private void scrollToDown(object sender, EventArgs e)
        {
            scroll.ScrollToAsync(forScrollingToEnd, ScrollToPosition.End, true);
            downButton.FadeTo(0);
        }
    }
}