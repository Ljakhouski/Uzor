using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Views;
using Xamarin.Forms;

namespace Uzor
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void add_item(object sender, EventArgs e)
        {
            list.Children.Add(new UzorItem());
        }
    }
}
