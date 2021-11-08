using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Uzor.EditorObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItemPage : ContentPage
    {
        private UzorData data;
        private MainPage pageForAlert;
        private string path;
        public UzorItemPage(UzorData data, string path, MainPage p)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this.path = path;
            this.data = data;
            this.pageForAlert = p;
            var upfView = new UzorPixelFieldView();
            this.uzorFieldFrame.Content = upfView;
            buildUzorPreview();
            itemNameLabel.Text = data.Name;
        }

        private void buildUzorPreview()
        {
            var upfView = (UzorPixelFieldView)this.uzorFieldFrame.Content;
            upfView.EditorObjectssList.Clear();
            upfView.EditorObjectssList.Add(new Background(data));
            upfView.EditorObjectssList.Add(new DemonstrateUzorEditorObject() { Data = data, GradientMode = false });
            upfView.DrawView();
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var p = new UzorCreatingPage(this.data, this.pageForAlert);
            p.Closed += uzorCreatingPage_Closed;
            await Navigation.PushModalAsync(p);

            /*
            if (p.Action == UzorCreatingPage.ActionStatus.Saved)
                UzorProjectFileManager.ReSave(this.data, path);
            else if (p.Action == UzorCreatingPage.ActionStatus.Canceled)
                this.data = UzorProjectFileManager.LoadUzorData(path);

            var v = (UzorPixelFieldView)this.uzorFieldFrame.Content;
            v.DrawView();*/
        }

        private void uzorCreatingPage_Closed(object sender, EventArgs e)
        {
            var p = (UzorCreatingPage)sender;

            if (p.Action == UzorCreatingPage.ActionStatus.Saved)
            {
                UzorProjectFileManager.ReSave(this.data, path);
                pageForAlert.MakeUzorItemList();
            }
                
            else if (p.Action == UzorCreatingPage.ActionStatus.Canceled)
                this.data = UzorProjectFileManager.LoadUzorData(path);

            buildUzorPreview();
        }

        private void hideSavingView(object sender, EventArgs e)
        {
            this.backgroundGrid.Children.Remove(sender as ImageBufferSaveView);
        }

        private void imageSaving_Clicked(object sender, EventArgs e)
        {
            var v = new ImageBufferSaveView(this.data);
            v.BackgroundTapped += hideSavingView;
            this.backgroundGrid.Children.Add(v);
        }
    }
}