using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Uzor.Data;
using Uzor.Localization;
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
        private SKBitmap bitmap = new SKBitmap(1000, 1600);

        private int bitmapWidth = 1000;
        private int bitmapHeight = 1600;
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
            this.bitmapWidth = data.FieldSize * 20;
            this.bitmapHeight = bitmapWidth;
            this.bitmap = new SKBitmap(bitmapWidth, bitmapHeight);
            InitializeComponent();
            setDefaultPickerValue();
            this.heightSlider.IsVisible = false;
            updateSquareUzorBitmap();
        } 
        
        private void setDefaultPickerValue()
        {
            this.formatPicker.SelectedIndex = 3;
        }
        
        public event EventHandler BackgroundTapped;

        enum Mode
        {
            Square,
            Long
        }

        private Mode getUzorMode()
        {
            if (this.longUzorData == null)
                return Mode.Square;
            return Mode.Long;
        }
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
                var o = new LongUzorDrawingObject(longUzorData);
                o.Draw(saveBitmapCanvas, 1000, this.bitmapHeight);
              //  for (int i = 500; i > 100; i -= 10)
               //     saveBitmapCanvas.DrawCircle(500, 500, i, new SKPaint() { Color = new SKColor((byte)new Random().Next(10,222), (byte)new Random().Next(10, 222), (byte)new Random().Next(10, 222), 10) });
            }

            this.previewCanvas.InvalidateSurface();
        }

        private void updateSquareUzorBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(bitmap))
            {
                var o = new UzorDrawingObject(uzorData);
                o.DrawUzor(20, saveBitmapCanvas, bitmapWidth, bitmapHeight);
            }

            this.previewCanvas.InvalidateSurface();
        }

        private async void imageSaveButton_clicked(object sender, EventArgs e)
        {
            // Get this SKImage data. This is basically an array of bytes in PNG format.
            SKData data = SKImage.FromBitmap(bitmap).Encode();

            // NOTE: This filename is not used by iOS!
            DateTime dt = DateTime.Now;
            string filename = String.Format("Uzor" + "-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
                                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            string savedFilePath = await BitmapStreamWriter.SaveBitmap(bitmap, (SKEncodedImageFormat)formatPicker.SelectedItem, 100, filename, "UzorApp");

            if (savedFilePath == null)
                savingStatusLabel.Text = AppResource.PermissionDenied;
            else
                savingStatusLabel.Text = AppResource.SavedInLabel + " \"" + savedFilePath + "\"";

            imageFileShowPath = savedFilePath;
            this.showFileButton.IsVisible = true;
        }

        private void onCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKCanvas c = e.Surface.Canvas;
            if (this.bitmap != null)
            {
                c.Clear();
                c.DrawBitmap(this.bitmap, new SKPoint(0, 0));
            }
                
        }

        private void heightValue_Changed(object sender, ValueChangedEventArgs e)
        {
            this.bitmapHeight = (int)e.NewValue;
            this.bitmap = new SKBitmap(1000, bitmapHeight);
            updateLongUzorBitmap();
        }
        private string imageFileShowPath;
        private void showImageFile_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IFileOpener>().Show(imageFileShowPath);
        }
    }
}