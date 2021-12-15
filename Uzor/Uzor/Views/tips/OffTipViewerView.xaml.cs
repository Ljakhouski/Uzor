using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Localization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.tips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OffTipViewerView : ContentView
    {
        public OffTipViewerView()
        {
            InitializeComponent();
        }

        private void offTips_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("TipViewShow", false);
            IsCheckedIcon.FadeTo(1, 100);
            tipLabel.FadeTo(1, 300);
        }

        public static string GetTitle()
        {
            return "...";
        }
    }
}