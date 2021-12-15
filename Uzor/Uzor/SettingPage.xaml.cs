using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    enum RenderingMode
    {
        Low,
        DoubleBuffering,
        FullDoubleBuffering,
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        private RenderingMode renderingMode;
        private MainPage mainPage;
        public SettingPage(MainPage p)
        {
            InitializeComponent();
            this.mainPage = p;
            this.renderingMode = (RenderingMode)Preferences.Get("RenderingMode", 2);
            this.renderingModePicker.SelectedIndex = (int)renderingMode;
            this.tipsViewingCheckbox.IsChecked = Preferences.Get("TipViewShow", true);
            //this.languagePicker.SelectedIndex = 0;
        }

        private void languageChanged(object sender, EventArgs e)
        {
            CultureInfo ci = null;

            if (languagePicker.Items[languagePicker.SelectedIndex] == "English")
            {
                ci = new CultureInfo("en-US");
                Preferences.Set("Language", "en-US");
            }
            else if (languagePicker.Items[languagePicker.SelectedIndex] == "Русский")
            {
                ci = new CultureInfo("ru");
                Preferences.Set("Language", "ru");
            }

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        private async void openURL_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.instagram.com/ljakhousky/", BrowserLaunchMode.SystemPreferred);
        }

        private void testButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TestPage());
        }


        private void renderingMode_Changed(object sender, EventArgs e)
        {
            this.renderingMode = (RenderingMode)((Picker)sender).SelectedIndex;
            Preferences.Set("RenderingMode", (int)this.renderingMode);

            this.unsafeLabel.IsVisible = this.renderingMode == RenderingMode.FullDoubleBuffering ? true : false;
        }

        private void exportProjects_Clicked(object sender, EventArgs e)
        {
            var fileList = DependencyService.Get<IProjectManager>().ExportProjects().Result;
            this.metaDataLabel.IsVisible = true;
            string content="";
            foreach (string s in fileList)
                content += s + '\n';

            string path = DependencyService.Get<IProjectManager>().GetExternalFolderPath();

            if (fileList.Length != 0)
                this.metaDataLabel.Text = "Exported files:\n" + content + "\nfiles placed in \""+path+"\"";
            else
                this.metaDataLabel.Text = "Exported files not found. Create new projects to main-menu";
        }

        private void importProject_Clicked(object sender, EventArgs e)
        {
            var fileList = DependencyService.Get<IProjectManager>().ImportProjects().Result;
            string path = DependencyService.Get<IProjectManager>().GetExternalFolderPath();

            this.metaDataLabel.IsVisible = true;
            string content = "";
            foreach (string s in fileList)
                content += s + '\n';

            if (fileList.Length != 0)
            {
                this.metaDataLabel.Text = "Imported files:\n" + content;
                mainPage.MakeUzorItemList();
            }
                
            else
                this.metaDataLabel.Text = "Imported files not found. Add .ubf or .lubf projects to \"" + path + '"';
        }

        private void TipsViewing_Changed(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("TipViewShow", e.Value);
        }
    }
}