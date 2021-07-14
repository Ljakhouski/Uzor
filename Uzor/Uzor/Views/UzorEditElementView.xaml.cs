
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
            this.editingFieldFrame.Content = UzorView;
           

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
            calculationButton.Source = "stopButton.png";
            SetDefaultZoomValue();
        }

        public void StopCaltulation()
        {
            this.UzorView.EditingMode = true;
            calculationButton.Source = "startButton.png";
            this.UzorView.DrawView();
            //SetZoomValueFromPicker();
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
            SetDefaultZoomValue();
            //uzorFieldCanvasView.FadeTo(0, 250);
        }

        private void SetZoomValueFromPicker()
        {
            this.UzorView.Scale = this.scaleSlider.Value;
            this.UzorView.DrawView();
        }
        private void SetDefaultZoomValue()
        {
            this.UzorView.Scale = 1;
            this.scaleSlider.Value = 1;
        }
        private void nextButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            // CALCULTION:
            
            BasicDrawingAlgorithm.Calculate(this.UzorView.ThisData.Layers[UzorView.LayerNumber]);

            this.UzorView.DrawView();

            SetDefaultZoomValue();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            if (!UzorView.DeleteMode)
                deleteButton.Source = "drawButton.png";
            else
                deleteButton.Source = "deleteButton.png";

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
                mirrorButton.Source = "mirrorOffButton.png";
            else
                mirrorButton.Source = "mirrorOnButton.png";
            UzorView.MirrorMode = !UzorView.MirrorMode;
            UzorView.DrawView();
        }

        private void pickerChanged(object sender, EventArgs e)
        {
            SetZoomValueFromPicker();
        }

        private void scaleChanged(object sender, ValueChangedEventArgs e)
        {
            this.UzorView.Scale = e.NewValue;
            this.UzorView.DrawView();
        }

        private void zoomButtonClick(object sender, EventArgs e)
        {
            
            if (!sliderPanel.IsVisible)
            {
                zoomButton.Source = "zoomOnMenuButton.png";
                sliderPanel.IsVisible = true;
                sliderPanelShadow.IsVisible = true;
                return;
            }    
                
            zoomButton.Source = "zoomOffMenuButton.png";
            sliderPanel.HeightRequest = 20;

            sliderPanel.IsVisible = false;
            sliderPanelShadow.IsVisible = false;
        }
    }
}