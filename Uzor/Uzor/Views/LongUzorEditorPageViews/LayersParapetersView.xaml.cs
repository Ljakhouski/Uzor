using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.LongUzorEditorPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LayersParapetersView : ContentView
    {
        private LongUzorEditorPage editorPage;
        private MainPage pageForAlert;
        public LayersParapetersView(LongUzorEditorPage page, MainPage pageForAlert)
        {
            this.editorPage = page;
            this.pageForAlert = pageForAlert;
            InitializeComponent();
        }

       
        private void scrollToDown(object sender, EventArgs e)
        {
            scroll.ScrollToAsync(forScrollingToEnd, ScrollToPosition.End, true);
            downButton.FadeTo(0);
        }

        private async void firstUzorEdit_Clicked(object sender, EventArgs e)
        {
            var p = new UzorCreatingPage(this.editorPage.GetData().UzorElements[0], this.pageForAlert);
            p.Closed += anyPage_Closed;
            await Navigation.PushModalAsync(p);
        }

        
        private async void secondUzorEdit_Clicked(object sender, EventArgs e)
        {
            var p = new UzorCreatingPage(this.editorPage.GetData().UzorElements[1], this.pageForAlert);
            p.Closed += anyPage_Closed;
            await Navigation.PushModalAsync(p);
        }
        
        private async void colorChange_Clicked(object sender, EventArgs e)
        {
            var c = new ColorPickerView(this.editorPage.GetData());
            c.BackgroundTapped += colorPicker_Background_Tapped;
            this.editorPage.backgroundGrid.Children.Add(c);
            this.editorPage.UpdateView();
        }
        private void anyPage_Closed(object sender, EventArgs e)
        {
            this.editorPage.UpdateView();
        }

        private void colorPicker_Background_Tapped(object sender, EventArgs e)
        {
            this.editorPage.backgroundGrid.Children.Remove(sender as ColorPickerView);
            this.editorPage.UpdateView();
        }

        private async void sideUzorEdit_Clicked(object sender, EventArgs e)
        {
            var p = new UzorCreatingPage(this.editorPage.GetData().SidePattern, this.pageForAlert);
            p.Closed += anyPage_Closed;
            await Navigation.PushModalAsync(p);
        }
    }
}