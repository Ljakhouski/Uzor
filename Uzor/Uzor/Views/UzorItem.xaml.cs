using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views.EditorObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UzorItem : ContentView
    {
        public UzorData Data { get; set; }
        private Page pageForAlert;
        public UzorItem()
        {
            
        }

        public UzorItem(UzorData data, Page p)
        {
            InitializeComponent();
            this.Data = data;
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
        public void SetUzorNameLabelText(string name)
        {
            this.itemName.Text = name;
        }
        private async void TapOnItem(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UzorItemPage(Data)));
        }

        async private void deleteItem(object sender, EventArgs e)
        {
            if (await pageForAlert.DisplayAlert("Question?", "Удалить элемент?", "Да", "Нет"))
            {
                //var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"/"+this.Data.Name+".ubf");
                
            }
        
        }
    }
}