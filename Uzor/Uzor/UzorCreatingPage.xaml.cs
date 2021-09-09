﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public UzorCreatingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            SaveSetting_ ss = SaveSetting;
            this.newUzorSettingView = new NewUzorSetting(ss);
            newUzorSettingView.Opacity = 0;
            creatingPageGrid.Children.Add(newUzorSettingView, 0, 1);
            newUzorSettingView.FadeTo(1);
        }
        public delegate void SaveSetting_(UzorData data);
        private NewUzorSetting newUzorSettingView;
        private UzorEditorSaveTopPanel saveTopPanel;
        private UzorEditorStepsTopPanel stepsPanel;

        private int stepNumber = 1;
        private int maxStepValue = 2;

        private UzorData currentUzorData;
        private LongUzorData longUzorData; // only for long-uzor-constructor-mode 

        private List<UzorData> uzorDataList = new List<UzorData>();
        private List<UzorEditElementView> uzorEditElementViewList = new List<UzorEditElementView>();
        public void SaveSetting(UzorData data)
        {
            creatingPageGrid.Children.Remove(newUzorSettingView);

            var v = new UzorEditElementView(data);
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

            }

            
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            if (stepNumber<maxStepValue)
            {
                if (this.longUzorData.UzorElements[stepNumber-1].CropMaskIsEmpty())
                {
                    uzorEditElementViewList[stepNumber-1].sliderPanel.IsVisible = true;
                    uzorEditElementViewList[stepNumber-1].sliderPanelShadow.IsVisible = true;
                    uzorEditElementViewList[stepNumber - 1].cropIndicator.Crop = (int)uzorEditElementViewList[stepNumber - 1].cropSlider.Value;
                    uzorEditElementViewList[stepNumber - 1].UzorView.DrawView();

                    return;
                }

                if (this.longUzorData.UzorElements[stepNumber] == null ) // expand array of UzorElements and creating new View
                {
                    var current = this.longUzorData.UzorElements[stepNumber - 1];

                    var nextUzorData = new UzorData(current.Name, current.DataOfCreation, uzorEditElementViewList[stepNumber-1].cropIndicator.Crop);
                    this.longUzorData.UzorElements[stepNumber-1 + 1] = nextUzorData;

                    uzorEditElementViewList.Add(new UzorEditElementView(nextUzorData));
                }
                
                this.creatingPageGrid.Children.Remove(uzorEditElementViewList[stepNumber - 1]);
                stepNumber++;
                this.creatingPageGrid.Children.Add(uzorEditElementViewList[stepNumber - 1], 0, 1);
                

                if (stepNumber==maxStepValue)
                {
                    // TODO: replace icon
                }
            }
            else
            {

            }
        }

        // for square-mode
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            /*if (this.stepNumber == 1)
                
            else
            {
                this.stepNumber--;
                stepsPanel.StepLabel.Text = stepNumber.ToString()+'/'+"2";
            }*/

            if (uzorEditElementViewList[0].ReSave || await DisplayAlert("", AppResource.ExitQuestion, AppResource.Yes, AppResource.No))
            {

                await Navigation.PopModalAsync();
            }
                
        
        }

        

     
    }
}