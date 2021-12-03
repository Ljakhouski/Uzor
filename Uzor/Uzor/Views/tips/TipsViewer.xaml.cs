using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.tips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TipsViewer : ContentView
	{
        private Grid parentGrid;
        List<View> tipList;
        List<string> titleTipsList = new List<string>();
        int step;
		public TipsViewer(Grid g, int step = 0)
		{
			InitializeComponent();
            this.parentGrid = g;
            this.step = step;
            tipList = new List<View>();
            tipList.Add(new Tip1());
            // ...

            titleTipsList.Add(Tip1.GetTitle());
            updateTipOnView();
            updateStepLabel();
		}

        private void background_Tapped(object sender, EventArgs e)
        {
            parentGrid.Children.Remove(this);
        }

        private void nextTip_Clicked(object sender, EventArgs e)
        {
            if (step + 1 < tipList.Count)
                step++;

            if (step + 1 == tipList.Count)
            {

            }

            updateTipOnView();
            updateStepLabel();
        }

        private void beforeTip_Clicked(object sender, EventArgs e)
        {
            step--;

            if (step < 0)
            {
                step = 0;
                return;
            }    
            updateTipOnView();
            updateStepLabel();
        }

        private void updateTipOnView()
        {
            this.tipContainer.Children.Clear();
            this.tipContainer.Children.Add(tipList[step]);
        }
        private void updateStepLabel()
        {
            this.stepLabel.Text = (this.step + 1).ToString() + " / " + tipList.Count;
            this.titleLabel.Text = titleTipsList[step];
            //this.titleLabel.Text = (this.tipList[this.step]).GetTitle();
        }
    }
}