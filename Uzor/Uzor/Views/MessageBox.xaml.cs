using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageBox : ContentView
    {
        public string Text { get; set; }
        public EventHandler OkButton_Clicked;
        public MessageBox(string text)
        {
            InitializeComponent();
            this.Text = text;
            this.mainLabel.Text = text;
        }

        private void OK_Clicked(object sender, EventArgs e)
        {
            OkButton_Clicked?.Invoke(this, null);
        }
    }
}