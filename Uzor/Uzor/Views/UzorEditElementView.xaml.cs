
/*
 Contain drawing panel (UzorPixelFieldView) and panels of tools
 */
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Uzor.Algorithm;
using Uzor.Views.DrawingObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorEditElementView : ContentView
    {
        public UzorPixelFieldView UzorView { get; set; }

        private UzorDrawingObject Uzor {get;set;}
        private DarkField MirrorIndicator { get; set; } = new DarkField();
        private CenterMarker CenterIndicator { get; set; } = new CenterMarker();


        private bool ReSave = false; // for rewriting before results
        private string SavedFilePath;
        public UzorEditElementView(UzorData data)
        {
            InitializeComponent();
            this.UzorView = new UzorPixelFieldView(data);
            this.editingFieldFrame.Content = UzorView;

            this.Uzor = new UzorDrawingObject() { MirrorMode = true, Data = data};

            this.UzorView.DrawingObjectsList.Add(Uzor);
            this.UzorView.DrawingObjectsList.Add(CenterIndicator);
            this.UzorView.DrawingObjectsList.Add(MirrorIndicator);


            Device.StartTimer(TimeSpan.FromMilliseconds(350), OnTimerTick);
        }
      
        private bool OnTimerTick()
        {
            if (this.Uzor.EditingMode)
                return true;

            // CALCULTION:

            BasicDrawingAlgorithm.Calculate(this.UzorView.ThisData.Layers[this.Uzor.LayerNumber]);
            
            //counter.Text = (Int32.Parse(counter.Text) + 1).ToString();

            this.UzorView.DrawView();
            return true;
        }
        public void StartCalculation()
        {
            this.Uzor.EditingMode = false;
            calculationButton.Source = "stopButton.png";
            SetDefaultZoomValue();
        }

        public void StopCaltulation()
        {
            this.Uzor.EditingMode = true;
            calculationButton.Source = "startButton.png";
            this.UzorView.DrawView();
            //SetZoomValueFromPicker();
        }

        private void CalculationButtonClick(object sender, EventArgs e)
        {
            if (this.Uzor.EditingMode)
                this.StartCalculation();
            else
                this.StopCaltulation();
        }

        private void beforeButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            if (UzorView.ThisData.Layers[Uzor.LayerNumber].Step >= 0)
                this.UzorView.ThisData.Layers[Uzor.LayerNumber].GetAndSetPreviousState(); // only set
            this.UzorView.DrawView();
            SetDefaultZoomValue();
            //uzorFieldCanvasView.FadeTo(0, 250);
        }

        private void SetZoomValueFromPicker()
        {
            //this.UzorView.Scale = this.scaleSlider.Value;
            //this.Uzor.Scale = (int)this.scaleSlider.Value;
            this.UzorView.DrawView();
        }
        private void SetDefaultZoomValue()
        {
           // this.UzorView.Scale = 1;
            //this.Uzor.Scale = 1;
            this.scaleSlider.Value = 1;
        }
        private void nextButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            // CALCULTION:
            
            BasicDrawingAlgorithm.Calculate(this.UzorView.ThisData.Layers[Uzor.LayerNumber]);

            this.UzorView.DrawView();

            SetDefaultZoomValue();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            if (!Uzor.DeleteMode)
                deleteButton.Source = "drawButton.png";
            else
                deleteButton.Source = "deleteButton.png";

            this.Uzor.DeleteMode = !this.Uzor.DeleteMode;
        }

        private void invertButtonClick(object sender, EventArgs e)
        {
            PixelColor c = UzorView.ThisData.Layers[0].FrontColor;

            UzorView.ThisData.Layers[0].FrontColor = UzorView.ThisData.Layers[0].BackColor;
            UzorView.ThisData.Layers[0].BackColor = c;
            UzorView.DrawView();
        }

        private void mirrorButtonClick(object sender, EventArgs e)
        {
            if (!Uzor.MirrorMode)
                mirrorButton.Source = "mirrorOffButton.png";
            else
                mirrorButton.Source = "mirrorOnButton.png";

            this.Uzor.MirrorMode = !Uzor.MirrorMode;
            this.MirrorIndicator.IsVisible = this.Uzor.MirrorMode;
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
        

        private void saveClick(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (ReSave) // rewrite file
            {
                FileStream fsr = new FileStream(SavedFilePath, FileMode.Truncate);
                formatter.Serialize(fsr, this.UzorView.ThisData);
                fsr.Dispose();
                return;
            }

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            
            string fileName = this.UzorView.ThisData.Name + ".ubf";

            if (File.Exists(Path.Combine(folderPath, this.UzorView.ThisData.Name + ".ubf")))
                for (int i = 0; i<999; i++)
                    if (File.Exists(Path.Combine(folderPath, this.UzorView.ThisData.Name + i.ToString() + ".ubf")))
                        continue;
                    else
                    {
                        fileName = this.UzorView.ThisData.Name + i.ToString() + ".ubf";
                        this.UzorView.ThisData.Name = this.UzorView.ThisData.Name + i.ToString();
                        break;
                    }
                

            SavedFilePath = folderPath + "/" + fileName;
            ReSave = true;

            FileStream fs = new FileStream(folderPath+"/"+fileName, FileMode.OpenOrCreate);
            formatter.Serialize(fs, this.UzorView.ThisData);
            fs.Dispose();
        }
            // перезаписываем файл
           // File.WriteAllText(Path.Combine(folderPath, this.UzorView.ThisData.Name), textEditor.Text);

        
    }
}