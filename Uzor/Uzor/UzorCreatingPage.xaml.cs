using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Algorithms;
using Uzor.Data;
using Uzor.Localization;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorCreatingPage : ContentPage
    {
        public enum ActionStatus
        {
            Saved,
            Canceled
        }
        private ActionStatus _action = ActionStatus.Canceled;
        public ActionStatus Action { get { return _action; }  }
        public EventHandler Closed;
        public UzorCreatingPage(MainPage p)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.pageForAlert = p;
            SaveSetting_ ss = SaveSetting;
            this.newUzorSettingView = new NewUzorSetting(ss);
            newUzorSettingView.Opacity = 0;
            creatingPageGrid.Children.Add(newUzorSettingView, 0, 1);
            newUzorSettingView.FadeTo(1);
        }
        private MainPage pageForAlert;
        public UzorCreatingPage(UzorData data, MainPage p) // for editing a previously created Uzor 
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var v = new UzorEditElementView(data);
            this.pageForAlert = p;
            uzorEditElementViewList.Add(v);
            creatingPageGrid.Children.Add(v, 0, 1);

            this.saveTopPanel = new UzorEditorSaveTopPanel();
            saveTopPanel.SaveButton.Clicked += saveChanges_Clicked;
            saveTopPanel.BackButton.Clicked += cancelChanges_Clicked;
            saveTopPanel.BackButton.Text = AppResource.Cancel;
            saveTopPanel.SaveButton.Text = AppResource.Ok;
            creatingPageGrid.Children.Add(saveTopPanel, 0, 0);
            creatingPageGrid.RowDefinitions[0].Height = 60;
            v.cropButton.IsVisible = true;
        }

        public delegate void SaveSetting_(UzorData data);
        private NewUzorSetting newUzorSettingView;
        private UzorEditorSaveTopPanel saveTopPanel;
        private UzorEditorStepsTopPanel stepsPanel;

        private int stepNumber = 1;
        private int maxStepValue = 2; // only for long-uzor-constructor-mode 

        private LongUzorData longUzorData; // only for long-uzor-constructor-mode 

        private List<UzorEditElementView> uzorEditElementViewList = new List<UzorEditElementView>();
        public void SaveSetting(UzorData data)
        {
            creatingPageGrid.Children.Remove(newUzorSettingView);

            var v = new UzorEditElementView(data);
            v.PageForAlert = this.pageForAlert;
            //uzorDataList.Add(v.UzorView.ThisData);
            uzorEditElementViewList.Add(v);

            creatingPageGrid.Children.Add(v, 0, 1);
            
            if (newUzorSettingView.IsSquareUzorMode)
            {
                this.saveTopPanel = new UzorEditorSaveTopPanel();
                saveTopPanel.SaveButton.Clicked += v.SaveButton_Click;
                saveTopPanel.BackButton.Clicked += BackButton_Clicked;
                creatingPageGrid.Children.Add(saveTopPanel, 0, 0);
                v.cropButton.IsVisible = false;
                v.cropButtonShadow.IsVisible = false;
                isSquareMode = true;
                //v.cropSlider.IsVisible = false;
                
            }
            else
            {
                this.stepsPanel = new UzorEditorStepsTopPanel();
                this.longUzorData = new LongUzorData();
                longUzorData.UzorElements = new UzorData[2] { data, null };
                stepsPanel.NextButton.Clicked += NextButton_Clicked;
                stepsPanel.BeforeButton.Clicked += BeforeButton_Clicked;
                creatingPageGrid.Children.Add(stepsPanel, 0, 0);

                this.stepsPanel.StepLabel.Text = this.stepNumber + "/" + this.maxStepValue;

            }
            // gridCreatingPage.Children.Add(new NewUzorSetting(SaveSetting));

            creatingPageGrid.RowDefinitions[0].Height = 60;
        }



        // for long-uzor-constructor
        private async void BeforeButton_Clicked(object sender, EventArgs e)
        {
            if (stepNumber == 1)
                if (await DisplayAlert("", AppResource.ExitQuestion, AppResource.Yes, AppResource.No))
                {
                    await Navigation.PopModalAsync();
                }
                    
                else
                    return;
            else
            {
                this.creatingPageGrid.Children.Remove(uzorEditElementViewList[stepNumber - 1]);
                stepNumber--;
                this.creatingPageGrid.Children.Add(uzorEditElementViewList[stepNumber - 1], 0, 1);

                this.stepsPanel.StepLabel.Text = this.stepNumber + "/" + this.maxStepValue;
            }

            
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (stepNumber<maxStepValue)
            {
                if (this.longUzorData.UzorElements[stepNumber-1].CropMaskIsEmpty())
                {
                    uzorEditElementViewList[stepNumber - 1].mirrorIndicator.IsVisible = false;

                    uzorEditElementViewList[stepNumber-1].sliderPanel.IsVisible = true;
                    uzorEditElementViewList[stepNumber-1].sliderPanelShadow.IsVisible = true;
                    uzorEditElementViewList[stepNumber - 1].cropIndicator.Crop = (int)uzorEditElementViewList[stepNumber - 1].cropSlider.Value;
                    uzorEditElementViewList[stepNumber - 1].UzorView.DrawView();

                    return;
                }

                if (this.longUzorData.UzorElements[stepNumber] == null ) // expand array of UzorElements and creating new View
                {
                    var current = this.longUzorData.UzorElements[0];

                    var nextUzorData = new UzorData(current.Name, current.DataOfCreation, current.GetMaskSize());

                    nextUzorData.Layers[0].FrontColor = new PixelColor(current.Layers[0].FrontColor.R, 
                                                                       current.Layers[0].FrontColor.G, 
                                                                       current.Layers[0].FrontColor.B,
                                                                       current.Layers[0].FrontColor.A);

                    nextUzorData.Layers[0].BackColor =  new PixelColor(current.Layers[0].BackColor.R,
                                                                       current.Layers[0].BackColor.G, 
                                                                       current.Layers[0].BackColor.B,
                                                                       current.Layers[0].BackColor.A);

                    this.longUzorData.UzorElements[stepNumber-1 + 1] = nextUzorData;

                    uzorEditElementViewList.Add(new UzorEditElementView(nextUzorData));

                    uzorEditElementViewList[uzorEditElementViewList.Count - 1].UzorView.BecomeSquare();
                }
                
                this.creatingPageGrid.Children.Remove(uzorEditElementViewList[stepNumber - 1]);
                stepNumber++;
                this.creatingPageGrid.Children.Add(uzorEditElementViewList[stepNumber - 1], 0, 1);

                this.stepsPanel.StepLabel.Text = this.stepNumber + "/" + this.maxStepValue;


                if (stepNumber==maxStepValue)
                {
                    // TODO: replace icon
                }
                    

                        /*************** CRUTCHES ***************/

                uzorEditElementViewList[stepNumber - 1].UzorView.MinimumHeightRequest = 0;
                uzorEditElementViewList[stepNumber - 1].BatchBegin();

                        /***  UzorView does not change its size ***/

            }
            else
            {
                longUzorData.SidePattern = SideUzorGenerator.GetNewSideUzor(longUzorData.UzorElements[0].FieldSize);
                longUzorData.SidePattern.Layers[0].FrontColor = longUzorData.UzorElements[0].Layers[0].FrontColor;

                longUzorData.SidePattern.Layers[0].FrontColor = new PixelColor(longUzorData.UzorElements[0].Layers[0].FrontColor.R,
                                                                               longUzorData.UzorElements[0].Layers[0].FrontColor.G,
                                                                               longUzorData.UzorElements[0].Layers[0].FrontColor.B,
                                                                               longUzorData.UzorElements[0].Layers[0].FrontColor.A);

                LongUzorEditorPage longUzorPage = new LongUzorEditorPage(longUzorData, pageForAlert, true);
                longUzorPage.Closed += longUzorEditorPage_Closed;

                await Navigation.PushModalAsync(new NavigationPage(longUzorPage), true);
            }
        }


        //crutches
        private bool? isSquareMode = null;
        protected override bool OnBackButtonPressed()
        {
            if (isSquareMode == null)
            {
                ForPopModalAsync();
                return true;
            }

            if (isSquareMode == true)
                BackButton_Clicked(null, null);
            else
                BackButton_Clicked(null, null);
            // if (await DisplayAlert("", AppResource.ExitQuestion, AppResource.Yes, AppResource.No))
            //    Navigation.PopModalAsync();

            return true;
        }

        // for square-mode
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            if (uzorEditElementViewList[0].ReSave || await DisplayAlert("", AppResource.ExitQuestion, AppResource.Yes, AppResource.No))
                await Navigation.PopModalAsync();
            this.Closed?.Invoke(this, null);

        }


        private async void saveChanges_Clicked(object sender, EventArgs e) // if this editor is open for editing a previously created Uzor
        {
            this._action = ActionStatus.Saved;
            this.pageForAlert.MakeUzorItemList();
            this.Closed?.Invoke(this, null);
            await Navigation.PopModalAsync();
        }
        private async void cancelChanges_Clicked(object sender, EventArgs e)
        {
            this._action = ActionStatus.Canceled;
            this.Closed?.Invoke(this, null);
            await Navigation.PopModalAsync();
        }

        private async void ForPopModalAsync()
        {
            this.Closed?.Invoke(this, null);
            await Navigation.PopModalAsync();
        }

        private void longUzorEditorPage_Closed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        private UzorEditElementView getUzorEditElementView()
        {
            return uzorEditElementViewList[stepNumber - 1];
        }
    }
}