using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.EditorObjects;
using Uzor.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views.tips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tip1 : ContentView
    {
        public static string GetTitle()
        {
            return AppResource.Basics;
        }

        private UzorPixelFieldView v;
        private DemonstrateUzorEditorObject uzor;
        private int uzorStep = 0;
        public Tip1()
        {
            InitializeComponent();
            var d = makeDemonstrateUzor();
            v = new UzorPixelFieldView();
            this.demonstrateUzorFrame.Content = v;
            uzor = new DemonstrateUzorEditorObject();
            uzor.Data = d;
            
            v.EditorObjectssList.Add(new Background(d));
            v.EditorObjectssList.Add(uzor);

            Device.StartTimer(TimeSpan.FromMilliseconds(350), () => {

                if (uzorStep > 15)
                {
                    uzorStep = 0;
                    uzor.Data.Replace(makeDemonstrateUzor());
                }
                else
                {
                    Algorithm.BasicDrawingAlgorithm.Calculate(uzor.Data.Layers[0]);
                    uzorStep++;
                }

                v.DrawView();
                return true; 
            });
        }

        private UzorData makeDemonstrateUzor()
        {
            var d = new UzorData("", DateTime.Now, 24);
            d.Clear();

            var b = new bool[24,24];
            b[11, 11] = true;
            b[11, 12] = true;
            b[12, 11] = true;
            b[12, 12] = true;
            d.Layers[0].AddNextState(b);

            return d;
        }

        private void frameSizeChanged(object sender, EventArgs e)
        {
            demonstrateUzorFrame.HeightRequest = demonstrateUzorFrame.Width;
            (demonstrateUzorFrame.Content as UzorPixelFieldView).BecomeSquare();
        }
    }
}