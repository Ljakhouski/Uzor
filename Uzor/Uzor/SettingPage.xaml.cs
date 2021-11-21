using System;
using System.Collections.Generic;
using System.Globalization;
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
        public SettingPage()
        {
            InitializeComponent();
            this.renderingMode = (RenderingMode)Preferences.Get("RenderingMode", 2);
            this.renderingModePicker.SelectedIndex = (int)renderingMode;
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
    }
}