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
    public partial class LongUzorMakeTip : ContentView
    {
        public LongUzorMakeTip()
        {
            InitializeComponent();
        }

        internal static string GetTitle()
        {
            return AppResource.Finishing;
        }
    }
}