using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.ItemPages
{
    //  [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LongUzorItemPage : ContentPage
    {
        private MainPage pageForAlert;
        private string path;
        public LongUzorItemPage(LongUzorData data, string path, MainPage p)
        {
            InitializeComponent();
            this.path = path;
            this.pageForAlert = p;
            this.itemNameLabel.Text = data.Name;
            this.longUzorView.Data = data;
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var p = new LongUzorEditorPage(longUzorView.Data, pageForAlert);
            p.Closed += longUzorEditorPage_Closed;
            await Navigation.PushModalAsync(p);


        }

        private void longUzorEditorPage_Closed(object sender, EventArgs e)
        {
            var p = (LongUzorEditorPage)sender;
            if (p.Action == LongUzorEditorPage.ActionStatus.Saved)
            {
                UzorProjectFileManager.ReSave(this.longUzorView.Data, path);
                pageForAlert.MakeUzorItemList();
            }

            else if (p.Action == LongUzorEditorPage.ActionStatus.Canceled)
                this.longUzorView.Data = UzorProjectFileManager.LoadLongUzorData(path);

            this.longUzorView.Draw();

        }

        private void imageSaving_Clicked(object sender, EventArgs e)
        {
            var v = new ImageBufferSaveView(this.longUzorView.Data);
            v.BackgroundTapped += hideSavingView;
            this.backgroundGrid.Children.Add(v);
        }

        private void hideSavingView(object sender, EventArgs e)
        {
            this.backgroundGrid.Children.Remove(sender as ImageBufferSaveView);
        }

        protected override bool OnBackButtonPressed()
        {
            if (backgroundGrid.Children.Count > 1)
            {
                backgroundGrid.Children.RemoveAt(backgroundGrid.Children.Count - 1);
                return true;
            }
            else
                Navigation.PopModalAsync();
            return true;
        }

        private void nameEditorShow(object sender, EventArgs e)
        {
            var v = new NameEditorView(this.longUzorView.Data, this.backgroundGrid);
            v.NameEdited += nameEdited;
        }

        private void nameEdited(object sender, EventArgs e)
        {
            this.itemNameLabel.Text = this.longUzorView.Data.Name;
            UzorProjectFileManager.ReSave(this.longUzorView.Data, path);
            pageForAlert.MakeUzorItemList();
        }
    }
}