using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NameEditorView : ContentView
    {
        private Grid grid;
        private UzorData data;
        private LongUzorData ldata;

        public EventHandler NameEdited { get; set; }
        public NameEditorView(UzorData data, Grid grid)
        {
            InitializeComponent();
            this.grid = grid;
            grid.Children.Add(this);
            this.data = data;
            this.nameEntry.Text = data.Name == null ? "" : data.Name;
        }

        public NameEditorView(LongUzorData data, Grid grid)
        {
            InitializeComponent();
            this.grid = grid;
            grid.Children.Add(this);
            this.ldata = data;
            this.nameEntry.Text = data.Name == null ? "" : data.Name;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            this.grid.Children.Remove(this);
        }

        private void OK_Clicked(object sender, EventArgs e)
        {
            if (data != null)
                this.data.Name = nameEntry.Text == null? "" : nameEntry.Text;
            else
                this.ldata.Name = nameEntry.Text == null ? "" : nameEntry.Text;
            NameEdited?.Invoke(this, null);
            Cancel_Clicked(null, null);
        }

        private void editEntry_Clicked(object sender, EventArgs e)
        {
            this.nameEntry.Text = "";// this.nameEntry.
            this.nameEntry.Focus();
        }
    }
}