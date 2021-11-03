
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
using Uzor.Data;
using Uzor.EditorObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorEditElementView : ContentView
    {
        public UzorPixelFieldView UzorView { get; set; }

        public UzorData ThisData { get; set; }
        private UzorEditorObject uzor {get;set;}
        public DarkField mirrorIndicator { get; set; } = new DarkField();
        public CenterMarker centerIndicator { get; set; } = new CenterMarker();
        public CropIndicator cropIndicator { get; set; }
        public MainPage PageForAlert { get; set; }
        public bool ReSave = false; // for rewriting before results
        private string SavedFilePath;
        public UzorEditElementView(UzorData data)
        {
            InitializeComponent();
            this.ThisData = data;

            this.UzorView = new UzorPixelFieldView();
            this.editingFieldFrame.Content = UzorView;

            this.uzor = new UzorEditorObject() { MirrorMode = true, Data = data};
            this.cropIndicator = new CropIndicator(data);

            if (data.FieldSize / 2 <= this.cropSlider.Minimum)
                this.cropSlider.Maximum = this.cropSlider.Minimum + 1;
            else
                this.cropSlider.Maximum = data.FieldSize/2;

            this.UzorView.EditorObjectssList.Add(new Background(data));
            this.UzorView.EditorObjectssList.Add(uzor);
            this.UzorView.EditorObjectssList.Add(centerIndicator);
            this.UzorView.EditorObjectssList.Add(mirrorIndicator);
            this.UzorView.EditorObjectssList.Add(cropIndicator);

            Device.StartTimer(TimeSpan.FromMilliseconds(350), OnTimerTick);
        }
      
        private bool OnTimerTick()
        {
            if (this.uzor.EditingMode)
                return true;

            // CALCULTION:

            BasicDrawingAlgorithm.Calculate(this.ThisData.Layers[this.uzor.LayerNumber]);
            
            //counter.Text = (Int32.Parse(counter.Text) + 1).ToString();

            this.UzorView.DrawView();
            return true;
        }
        public void StartCalculation()
        {
            this.uzor.EditingMode = false;
            calculationButton.Source = "stopButton.png";
            this.uzor.SetDefaultScale();
        }

        public void StopCaltulation()
        {
            this.uzor.EditingMode = true;
            calculationButton.Source = "startButton.png";
            this.UzorView.DrawView();
        }

        private void CalculationButtonClick(object sender, EventArgs e)
        {
            if (this.uzor.EditingMode)
                this.StartCalculation();
            else
                this.StopCaltulation();

            this.uzor.SetDefaultScale();
        }

        private void beforeButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            if (this.ThisData.Layers[uzor.LayerNumber].Step >= 0)
                this.ThisData.Layers[uzor.LayerNumber].GetAndSetPreviousState(); // only set
            this.UzorView.DrawView();

            this.uzor.SetDefaultScale();
        }

        private void SetCropValueFromPicker()
        {
            //this.UzorView.Scale = this.scaleSlider.Value;
            //this.Uzor.Scale = (int)this.scaleSlider.Value;
            this.UzorView.DrawView();
        }
        private void nextButtonClick(object sender, EventArgs e)
        {
            this.StopCaltulation();
            // CALCULTION:
            
            BasicDrawingAlgorithm.Calculate(this.ThisData.Layers[uzor.LayerNumber]);

            this.UzorView.DrawView();
            this.uzor.SetDefaultScale();
        }

        private void deleteButtonClick(object sender, EventArgs e)
        {
            if (!uzor.DeleteMode)
                deleteButton.Source = "drawButton.png";
            else
                deleteButton.Source = "deleteButton.png";

            this.uzor.DeleteMode = !this.uzor.DeleteMode;
        }

        private void invertButtonClick(object sender, EventArgs e)
        {
            PixelColor c = ThisData.Layers[0].FrontColor;

            ThisData.Layers[0].FrontColor = ThisData.Layers[0].BackColor;
            ThisData.Layers[0].BackColor = c;
            UzorView.DrawView();
        }

        private void mirrorButtonClick(object sender, EventArgs e)
        {
            if (!uzor.MirrorMode)
                mirrorButton.Source = "mirrorOffButton.png";
            else
                mirrorButton.Source = "mirrorOnButton.png";

            this.uzor.MirrorMode = !uzor.MirrorMode;
            this.mirrorIndicator.IsVisible = this.uzor.MirrorMode;
            UzorView.DrawView();
        }

        private void pickerChanged(object sender, EventArgs e)
        {
            SetCropValueFromPicker();
        }

        private void cropChanged(object sender, ValueChangedEventArgs e)
        {
            if (this.cropIndicator == null)
                return;

            if (this.mirrorIndicator.IsVisible) // to not to recalculate View
                this.mirrorButtonClick(null, null);

            this.cropIndicator.Crop = (int)e.NewValue;
            //this.UzorView.Scale = e.NewValue;
            this.UzorView.DrawView();
        }

        private void cropButtonClick(object sender, EventArgs e)
        {
            
            if (!sliderPanel.IsVisible)
            {
                cropButton.Source = "cropOffMenuButton.png";
                sliderPanel.IsVisible = true;
                sliderPanelShadow.IsVisible = true;
                return;
            }    
                
            cropButton.Source = "cropOnMenuButton.png";
            sliderPanel.HeightRequest = 20;

            sliderPanel.IsVisible = false;
            sliderPanelShadow.IsVisible = false;
        }
       
        public void SaveButton_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (ReSave) // rewrite file
            {
                FileStream fsr = new FileStream(SavedFilePath, FileMode.Truncate);
                formatter.Serialize(fsr, this.ThisData);
                fsr.Dispose();
                return;
            }

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            
            string fileName = this.ThisData.Name + ".ubf";

            if (File.Exists(Path.Combine(folderPath, this.ThisData.Name + ".ubf")))
                for (int i = 0; i<999; i++)
                    if (File.Exists(Path.Combine(folderPath, this.ThisData.Name + i.ToString() + ".ubf")))
                        continue;
                    else
                    {
                        fileName = this.ThisData.Name + i.ToString() + ".ubf";
                        this.ThisData.Name = this.ThisData.Name + i.ToString();
                        break;
                    }
                

            SavedFilePath = folderPath + "/" + fileName;
            ReSave = true;

            FileStream fs = new FileStream(folderPath+"/"+fileName, FileMode.OpenOrCreate);
            formatter.Serialize(fs, this.ThisData);
            fs.Dispose();


            if (PageForAlert != null)
                PageForAlert.MakeUzorItemList();
               // pageForAlert.itemStack.Children.Add(new UzorItem(this.ThisData, pageForAlert));
        }
           
        
    }
}