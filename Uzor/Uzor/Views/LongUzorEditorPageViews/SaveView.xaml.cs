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
        }
        
        private void Ok_Clicked(object sender, EventArgs e)
        {
            this.editorPage.Ok();
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