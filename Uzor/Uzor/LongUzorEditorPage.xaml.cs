using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Localization;
using Uzor.Views;
using Uzor.Views.LongUzorEditorPageViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorEditorPage : ContentPage
    {
        private DistanceParametersView distanceParametersView;
        private LayersParapetersView layoutParametersView;
        private SaveView saveView;
        private string SavedFilePath;
        private MainPage pageForAlert;
        public bool ReSave { get; set; }
        public LongUzorEditorPage(LongUzorData data, MainPage p, bool isNewLongUzor = false)
        {
            InitializeComponent();
            this.longUzorView.Data = data;
            this.pageForAlert = p;
            initializeDropMenus();

            if (isNewLongUzor)
                calculateLongUzorParameters();

            setSlidersValue();
        }

        private void calculateLongUzorParameters()
        {
            var data = this.GetData();
            data.A = /*this.longUzorView.LongUzorGraphic.PixelSize*/ 2 * data.UzorElements[0].FieldSize;
            data.B = 0;
            data.D = data.UzorElements[0].FieldSize * 2 + 30;
            data.C = (int)distanceParametersView.C.Maximum;
            setSlidersValue();
        }

        private void setSlidersValue()
        {
            var data = this.GetData();

            distanceParametersView.A.Value = data.A;
            distanceParametersView.B.Value = data.B;
            distanceParametersView.C.Maximum = data.SidePattern.FieldSize / 2 + 1;
            distanceParametersView.C.Value = data.C;
            distanceParametersView.D.Value = data.D;
        }
        public LongUzorData GetData()
        {
            return this.longUzorView.Data;
        }

        public void SaveProject()
        {
            BinaryFormatter formatter = new BinaryFormatter();


            if (ReSave) // rewrite file
            {
                FileStream fsr = new FileStream(SavedFilePath, FileMode.Truncate);
                formatter.Serialize(fsr, this.GetData());
                fsr.Dispose();
                return;
            }

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


            string fileName = this.GetData().Name + ".lubf";

            if (File.Exists(Path.Combine(folderPath, this.GetData().Name + ".lubf")))
                for (int i = 0; i < 999; i++)
                    if (File.Exists(Path.Combine(folderPath, this.GetData().Name + i.ToString() + ".lubf")))
                        continue;
                    else
                    {
                        fileName = this.GetData().Name + i.ToString() + ".lubf";
                        this.GetData().Name = this.GetData().Name + i.ToString();
                        break;
                    }


            SavedFilePath = folderPath + "/" + fileName;
            ReSave = true;

            FileStream fs = new FileStream(folderPath + "/" + fileName, FileMode.OpenOrCreate);
            formatter.Serialize(fs, this.GetData());
            fs.Dispose();
        }

        public void ShowImageBufferSaveView()
        {
            var v = new ImageBufferSaveView(this.GetData());
            v.BackgroundTapped += ImageSaveView_BackgroundTapped;
            this.backgroundGrid.Children.Add(v);
        }

        private void ImageSaveView_BackgroundTapped(object sender, EventArgs e)
        {
            this.backgroundGrid.Children.Remove(sender as ImageBufferSaveView);
        }

        private void initializeDropMenus()
        {
            this.distanceParametersView = new DistanceParametersView(longUzorView);
            AbsoluteLayout.SetLayoutFlags(distanceParametersView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(distanceParametersView, new Rectangle(1,1, 1, 0.3));

            this.layoutParametersView = new LayersParapetersView();
            AbsoluteLayout.SetLayoutFlags(layoutParametersView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layoutParametersView, new Rectangle(1, 1, 1, 0.5));

            this.saveView = new SaveView(this);
            AbsoluteLayout.SetLayoutFlags(saveView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(saveView, new Rectangle(1, 1, 1, 0.5));
        }
        private async void hideDownAllDropMenu()
        {
            blackBackground.FadeTo(0, 250);
            blackBackground.IsVisible = false;
            blackBackground.InputTransparent = true;
            blackBackground.IsEnabled = false;

            if (dropDownMenuLayout.Children.Contains(distanceParametersView))
            {
                await distanceParametersView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(distanceParametersView);
            }
            else if (dropDownMenuLayout.Children.Contains(layoutParametersView))
            {
               
                await layoutParametersView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(layoutParametersView);

            }
            else if (dropDownMenuLayout.Children.Contains(saveView))
            {
                await saveView.TranslateTo(0, 1000, 250, Easing.SinInOut);
                dropDownMenuLayout.Children.Remove(saveView);
            }
                

            //dropDownMenuLayout.Children.Clear();
            //dropDownMenuLayout.Children.Add(blackBackground);

        }
        private async void ABC_Parameters_Clicked(object sender, EventArgs e)
        {


            if (!dropDownMenuLayout.Children.Contains(distanceParametersView))
            {
                hideDownAllDropMenu();

                distanceParametersView.TranslationY = 300;
                distanceParametersView.IsVisible = true;

                //blackBackground.FadeTo(0.3, 250);  
                //blackBackground.Opacity = 0; // !!!

                dropDownMenuLayout.Children.Add(distanceParametersView);
                dropDownMenuLayout.HeightRequest = distanceParametersView.HeightRequest;
                await distanceParametersView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }

        private async void layersMenu_Clicked(object sender, EventArgs e)
        {
            if (!dropDownMenuLayout.Children.Contains(layoutParametersView))
            {
                hideDownAllDropMenu();

                layoutParametersView.TranslationY = 700;

                blackBackground.IsVisible = true;
                blackBackground.InputTransparent = false;
                blackBackground.IsEnabled = true;
                blackBackground.FadeTo(0.3, 250);

                dropDownMenuLayout.Children.Add(layoutParametersView);
                dropDownMenuLayout.HeightRequest = layoutParametersView.HeightRequest;
                await layoutParametersView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }

        private async void saveMenu_Clicked(object sender, EventArgs e)
        {
            if (!dropDownMenuLayout.Children.Contains(saveView))
            {
                hideDownAllDropMenu();

                saveView.TranslationY = 700;

                blackBackground.IsVisible = true;
                blackBackground.InputTransparent = false;
                blackBackground.IsEnabled = true;
                blackBackground.FadeTo(0.3, 250);

                dropDownMenuLayout.Children.Add(saveView);
                dropDownMenuLayout.HeightRequest = saveView.HeightRequest;
                await saveView.TranslateTo(0, 0, 250, Easing.SinInOut);
            }
            else
                hideDownAllDropMenu();
        }

        private void blackBackgroundTapped(object sender, EventArgs e)
        {
            hideDownAllDropMenu();
        }

        protected override bool OnBackButtonPressed()
        {
            this.Exit();
            return true;
        }

        public async void Exit()
        {
            if (await DisplayAlert("", AppResource.ExitQuestion, AppResource.Yes, AppResource.No))
            {
                this.pageForAlert.MakeUzorItemList();
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
            }
        }
    }
}