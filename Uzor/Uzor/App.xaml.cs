using System;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            setCulture();
            MainPage = new MainPage();
        }

        private void setCulture()
        {
            if (Preferences.ContainsKey("Language"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Preferences.Get("Language", "en-US"));// (CultureInfo)Application.Current.Resources["Language"];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Preferences.Get("Language", "en-US"));
            }
            //Xamarin.Forms.DependencyService.Get<ILocalize>().SetLocale(ci);
           
            //Preferences.Set("Language", "ru");
            
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}
