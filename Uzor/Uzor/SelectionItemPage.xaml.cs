using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionItemPage : ContentPage
    {
        public EventHandler UzorSelected;
        public UzorData SelectedUzor { get; set; }
        public bool SaveProjectColor { get; set; }
        public SelectionItemPage()
        {
            InitializeComponent();
            this.MakeUzorItemList();
        }

        public void MakeUzorItemList()
        {
            this.itemStack.Children.Clear();

            var fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            foreach (string fileName in fileList)
                if (fileName.Substring(fileName.Length - 4) == ".ubf")
                {
                    var i = new UzorItem(UzorProjectFileManager.LoadUzorData(fileName));
                    i.Tapped += item_Tapped;
                    this.itemStack.Children.Add(i);
                }
                    
               
        }

        private void item_Tapped(object sender, EventArgs e)
        {
            this.SelectedUzor = (sender as UzorItem).Data;
            this.UzorSelected?.Invoke(this, null);
            this.SaveProjectColor = saveProjectColor_Checkbox.IsChecked;
            Navigation.PopModalAsync();
        }
    }
}