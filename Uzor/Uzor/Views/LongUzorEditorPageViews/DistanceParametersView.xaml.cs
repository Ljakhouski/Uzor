using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.LongUzorEditorPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DistanceParametersView : ContentView
    {
        private LongUzorView parentView;
        public DistanceParametersView(LongUzorView view)
        {
            InitializeComponent();
            this.parentView = view;
        }


       

        private void slider_Changed(object sender, ValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;

                    /* reflection */

            if (slider == FindByName("A"))  
                parentView.Data.A = (int)e.NewValue;
            else if (slider == FindByName("B"))
                parentView.Data.B = (int)e.NewValue;
            else if (slider == FindByName("C"))
                parentView.Data.C = (int)e.NewValue;
            else if (slider == FindByName("D"))
                parentView.Data.D = (int)e.NewValue;

            parentView.Draw();
        }
    }
}