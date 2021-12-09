using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.tips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tip3 : ContentView
    {
        public static string GetTitle()
        {
            return AppResource.Mirror;
        }
        public Tip3()
        {
            InitializeComponent();
        }
    }
}