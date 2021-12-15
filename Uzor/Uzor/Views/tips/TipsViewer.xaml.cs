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
		public TipsViewer(Grid g, bool isLongUzorTips = true, int step = 0)
		{
			InitializeComponent();
            this.parentGrid = g;
            this.step = step;
            tipList = new List<View>();
            tipList.Add(new Tip1());
            tipList.Add(new Tip2());
            tipList.Add(new Tip3());
            if (isLongUzorTips)
                tipList.Add(new LongUzorMakeTip());
            tipList.Add(new Tip4());
            tipList.Add(new OffTipViewerView());
            // ...
            setStartParameters();

            titleTipsList.Add(Tip1.GetTitle());
            titleTipsList.Add(Tip2.GetTitle());
            titleTipsList.Add(Tip3.GetTitle());
            if (isLongUzorTips)
                titleTipsList.Add(LongUzorMakeTip.GetTitle());
            titleTipsList.Add(Tip4.GetTitle());
            titleTipsList.Add(OffTipViewerView.GetTitle());
            updateTipOnView();
            updateStepLabel();
		}

        private void setStartParameters()
        {
            foreach (View v in tipList)
            {
                v.Opacity = 0;
                v.TranslationY = 50;
            }
                
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
            tipList[step].FadeTo(1, 200);
            tipList[step].TranslateTo(0, 0, 250, Easing.SinInOut);
        }
        private void updateStepLabel()
        {
            this.stepLabel.Text = (this.step + 1).ToString() + " / " + tipList.Count;
            this.titleLabel.Text = titleTipsList[step];
            //this.titleLabel.Text = (this.tipList[this.step]).GetTitle();
        }
    }
}