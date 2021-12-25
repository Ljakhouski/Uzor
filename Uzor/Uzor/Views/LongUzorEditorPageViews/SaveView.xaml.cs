using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.LongUzorEditorPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaveView : ContentView
    {
        private LongUzorEditorPage editorPage;
        public SaveView(LongUzorEditorPage longUzorEditorPage)
        {
            this.editorPage = longUzorEditorPage;
            InitializeComponent();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            this.editorPage.SaveProject();
            IsCheckedIcon.IsVisible = true;
        }
        
        private async void Ok_Clicked(object sender, EventArgs e)
        {
            this.loadingIndicator.IsVisible = true;
            this.loadingIndicator.IsRunning = true;
            
            Device.BeginInvokeOnMainThread(async () =>
            {
                this.editorPage.Ok();

                this.loadingIndicator.IsVisible = false;
                this.loadingIndicator.IsRunning = false;
            });
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            this.editorPage.Cancel();
        }
        private void ImageSave_Clicked(object sender, EventArgs e)
        {
            var i = new ImageBufferSaveView(editorPage.GetData());
            i.BackgroundTapped += imageBufferSaver_background_Tapped;
            this.editorPage.backgroundGrid.Children.Add(i);
        }

        private void imageBufferSaver_background_Tapped(object sender, EventArgs e)
        {
            this.editorPage.backgroundGrid.Children.Remove(sender as ImageBufferSaveView);
        }
    }
}