using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorEditorPage : ContentPage
    {
        public LongUzorEditorPage(LongUzorData data)
        {
            
            InitializeComponent();
            this.longUzorView.Data = data;
        }

        private void ABC_Parameters_Clicked(object sender, EventArgs e)
        {

        }

        private void layersMenu_Clicked(object sender, EventArgs e)
        {

        }

        private void saveMenu_Clicked(object sender, EventArgs e)
        {

        }
    }
}