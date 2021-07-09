
/*
 Contain drawing panel (UzorPixelFieldView) and panels of tools
 */
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Algorithm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorEditElementView : ContentView
    {
        public UzorPixelFieldView UzorView { get; set; }

        public UzorEditElementView(UzorData data)
        {
            InitializeComponent();
            this.UzorView = new UzorPixelFieldView(data);
            this.editingFieldGrid.Children.Add(UzorView);
            Device.StartTimer(TimeSpan.FromMilliseconds(350), OnTimerTick);
        }
      
        private bool OnTimerTick()
        {
            if (this.UzorView.EditingMode)
                return true;

            // CALCULTION:

            BasicDrawingAlgorithm.Calculate(this.UzorView.ThisData.Layers[UzorView.LayerNumber]);
            
            //counter.Text = (Int32.Parse(counter.Text) + 1).ToString();

            this.UzorView.DrawView();
            return true;
        }
        public void StartCalculation()
        {
            this.UzorView.EditingMode = false;
            calculationButton.Text = "[stop]";
        }

        public void StopCaltulation()
        {
            this.UzorView.EditingMode = true;
            calculationButton.Text = "[start]";
            this.UzorView.DrawView();
        }

        private void CalculationButtonClick(object sender, EventArgs e)
        {
            if (this.UzorView.EditingMode)
                this.StartCalculation();
            else
                this.StopCaltulation();
        }

        private void beforeButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            if (UzorView.ThisData.Layers[UzorView.LayerNumber].Step >= 0)
                this.UzorView.ThisData.Layers[UzorView.LayerNumber].GetAndSetPreviousState(); // only set
            this.UzorView.DrawView();
            //uzorFieldCanvasView.FadeTo(0, 250);
        }

        private void nextButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            // CALCULTION:
            
            BasicDrawingAlgorithm.Calculate(this.UzorView.ThisData.Layers[UzorView.LayerNumber]);

            this.UzorView.DrawView();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            if (!UzorView.DeleteMode)
                deleteButton.Text = "[ draw ]";
            else
                deleteButton.Text = "[delete]";

            this.UzorView.DeleteMode = !this.UzorView.DeleteMode;
        }

        private void invertButtonClick(object sender, EventArgs e)
        {
            SKColor c = UzorView.ThisData.Layers[0].FrontColor;

            UzorView.ThisData.Layers[0].FrontColor = UzorView.ThisData.Layers[0].BackColor;
            UzorView.ThisData.Layers[0].BackColor = c;
            UzorView.DrawView();
        }

        private void mirrorButtonClick(object sender, EventArgs e)
        {
            if (!UzorView.MirrorMode)
                mirrorButton.Text = "[before mode]";
            else
                mirrorButton.Text = "[mirror mode]";
            UzorView.MirrorMode = !UzorView.MirrorMode;
            UzorView.DrawView();
        }
    }
}