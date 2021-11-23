using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Localization;
using Uzor.EditorObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItem : ContentView
    {
        public UzorData Data { get; set; }
        private MainPage pageForAlert;
        private string path;
        public EventHandler Tapped;
        public UzorItem(UzorData data, string path, MainPage p)
        {
            InitializeComponent();
            this.Data = data;
            this.path = path;
            UzorPixelFieldView upv = this.preview;

            DemonstrateUzorEditorObject udb = new DemonstrateUzorEditorObject();
            udb.Data = data;
            udb.GradientMode = true;

            //upv.ThisData = data;
            upv.EditorObjectssList.Add(udb);
            upv.DrawView();
            this.itemName.Text = data.Name.Split("/".ToCharArray()).Last();
            this.itemDate.Text = data.DataOfCreation.ToString();
            this.mineFrame.BackgroundColor = new Color( data.Layers[0].BackColor.R,
                                                        data.Layers[0].BackColor.G,
                                                        data.Layers[0].BackColor.B);

            this.pageForAlert = p;
        }

        bool selectItemMode = false;
        public UzorItem(UzorData data) // for SelectionItemPage
        {
            InitializeComponent();

            selectItemMode = true;
            this.Data = data;

            UzorPixelFieldView upv = this.preview;

            DemonstrateUzorEditorObject udb = new DemonstrateUzorEditorObject();
            udb.Data = data;
            udb.GradientMode = false;

            //upv.ThisData = data;
            upv.EditorObjectssList.Add(udb);
            upv.DrawView();
            this.itemName.Text = data.Name.Split("/".ToCharArray()).Last();
            this.itemDate.Text = data.DataOfCreation.ToString();
            this.mineFrame.BackgroundColor = new Color(data.Layers[0].BackColor.R,
                                                        data.Layers[0].BackColor.G,
                                                        data.Layers[0].BackColor.B);

            this.deleteButton.IsVisible = false;
        }
        public void SetUzorNameLabelText(string name)
        {
            this.itemName.Text = name;
        }
        private async void TapOnItem(object sender, EventArgs e)
        {
            if (selectItemMode)
                this.Tapped?.Invoke(this, null);
            else
                await Navigation.PushModalAsync(new NavigationPage(new UzorItemPage(Data, path, pageForAlert)));
        }

        async private void deleteItem(object sender, EventArgs e)
        {
            if (selectItemMode)
                return;

            if (await pageForAlert.DisplayAlert("", AppResource.DeleteQuestion, AppResource.Yes, AppResource.No))
            {
                //var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

                //File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"/"+this.Data.Name+".ubf");
                UzorProjectFileManager.Delete(this.path);
                pageForAlert.itemStack.Children.Remove(this);
            }
        
        }
    }
}