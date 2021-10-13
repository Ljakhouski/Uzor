using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
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
    public partial class ImageBufferSaveView : ContentView
    {
        private LongUzorData longUzorData;
        private UzorData uzorData;
        private SKBitmap bitmap = new SKBitmap(1000, 1000);
        private int bitmapHeight = 1000;
        public ImageBufferSaveView(LongUzorData data)
        {
            this.longUzorData = data;
            InitializeComponent();
            setDefaultPickerValue();
            updateLongUzorBitmap();
        }

        public ImageBufferSaveView(UzorData data)
        {
            this.uzorData = data;
            InitializeComponent();
            setDefaultPickerValue();
        } 
        
        private void setDefaultPickerValue()
        {
            this.formatPicker.SelectedIndex = 3;
        }
        
        public event EventHandler BackgroundTapped;

        private void background_Tapped(object sender, EventArgs e)
        {
            viewFrame.FadeTo(0);
            background.FadeTo(0);
            BackgroundTapped?.Invoke(this, null);
        }

        private void onFormatPickerChanged(object sender, EventArgs e)
        {

        }

        private void updateLongUzorBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(bitmap))
            {
                saveBitmapCanvas.Clear(Color.Green.ToSKColor());

                var drObj = new LongUzorDrawingObject(longUzorData);
                drObj.Draw(saveBitmapCanvas, 1000, this.bitmapHeight);

                for (int i = 500; i > 100; i -= 10)
                    saveBitmapCanvas.DrawCircle(500, 500, i, new SKPaint() { Color = new SKColor((byte)new Random().Next(10,222), (byte)new Random().Next(10, 222), (byte)new Random().Next(10, 222), 10) });
            }

            this.previewCanvas.InvalidateSurface();
        }

        private void imageSaveButton_clicked(object sender, EventArgs e)
        {
            // Get this SKImage data. This is basically an array of bytes in PNG format.
            SKData data = SKImage.FromBitmap(bitmap).Encode();

            // Fabricate a filename based on data and time.
            // NOTE: This filename is not used by iOS!
            DateTime dt = DateTime.Now;
            string filename = String.Format("SpinPaint-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
                                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            // Get the dependency service implemented in the particular platform.
            //ISpinPaintDependencyService dependencyService = DependencyService.Get<ISpinPaintDependencyService>();

            // Save the bitmap and get a boolean indicating success.
            //bool result = await dependencyService.SaveBitmap(data.ToArray(), filename);

            // Display an alert if an error occurred.
           // if (!result)
           // {
           //     await DisplayAlert("SpinPaint", "Artwork could not be saved. Sorry!", "OK");
           // }
        }

        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKCanvas c = e.Surface.Canvas;
            if (this.bitmap != null)
                c.DrawBitmap(this.bitmap, new SKPoint(0, 0));
        }

        private void heightValue_Changed(object sender, ValueChangedEventArgs e)
        {
            this.bitmapHeight = (int)e.NewValue;
            this.bitmap = new SKBitmap(1000, (int)e.NewValue);
            updateLongUzorBitmap();
        }
    }
}