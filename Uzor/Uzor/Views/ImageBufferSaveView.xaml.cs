using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading.Tasks;
using Uzor.Data;
using Uzor.EditorObjects;
using Uzor.Localization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Uzor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageBufferSaveView : ContentView
    {
        private LongUzorData longUzorData;
        private UzorData uzorData;
        private SKBitmap bitmap;

        private int bitmapWidth;
        private int bitmapHeight = 3600;

        const int currentPixelSize = 15; // using for size-calculation & bitmap drawing
        public ImageBufferSaveView(LongUzorData data)
        {
            this.longUzorData = data;
            InitializeComponent();
            setDefaultPickerValue();
            updateLongUzorBitmap();
            this.bitmapPreviewViewFrame.Content = new BitmapPreviewView(bitmap);
        }

        public ImageBufferSaveView(UzorData data)
        {
            this.uzorData = data;
            this.bitmapWidth = data.FieldSize * currentPixelSize;
            this.bitmapHeight = bitmapWidth;
            InitializeComponent();
            setDefaultPickerValue();
            this.heightSlider.IsVisible = false;
            this.bitmapPreviewViewFrame.Content = new BitmapPreviewView(bitmap);
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

        private void updateBitmapInPreviewer()
        {
            if (this.bitmapPreviewViewFrame.Content != null)
            {
                var v = (BitmapPreviewView)this.bitmapPreviewViewFrame.Content;
                v.Bitmap = bitmap;
            }
        }
        private void updateLongUzorBitmap()
        {
            var o = new LongUzorDrawingObject(longUzorData) { PixelSize = currentPixelSize };
            this.bitmapWidth = o.GetResultContentWidth() >3500? 3500 : o.GetResultContentWidth();
            this.bitmap = new SKBitmap(this.bitmapWidth, this.bitmapHeight);

            using (SKCanvas saveBitmapCanvas = new SKCanvas(bitmap))
            {
                o.Draw(saveBitmapCanvas, this.bitmapWidth, this.bitmapHeight);
            }

            updateBitmapInPreviewer();
        }

        private void updateSquareUzorBitmap()
        {
            this.bitmapWidth = uzorData.FieldSize * currentPixelSize;
            this.bitmapHeight = bitmapWidth;

            this.bitmap = new SKBitmap(this.bitmapWidth, this.bitmapHeight);
            
            using (SKCanvas saveBitmapCanvas = new SKCanvas(bitmap))
            {
                saveBitmapCanvas.Clear(uzorData.Layers[0].BackColor.ToSKColor()); // UzorDrawingObject does not support background rendering
                var o = new UzorDrawingObject(uzorData);
                o.DrawUzor(currentPixelSize, saveBitmapCanvas, bitmapWidth, bitmapHeight);
            }

            updateBitmapInPreviewer();
        }

        private void updateBitmapPreviewView()
        {
            var v = (BitmapPreviewView)this.bitmapPreviewViewFrame.Content;
            v.Draw();
        }

        private async void imageSaveButton_clicked(object sender, EventArgs e)
        {
            await saveImageAsync();
            // Get this SKImage data. This is basically an array of bytes in PNG format.


            /*SKData data = SKImage.FromBitmap(bitmap).Encode();

            // NOTE: This filename is not used by iOS!
            DateTime dt = DateTime.Now;
            string filename = String.Format("Uzor" + "-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
                                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            string savedFilePath = await BitmapStreamWriter.SaveBitmap(bitmap, (SKEncodedImageFormat)formatPicker.SelectedItem, 100, filename, "UzorApp");

            if (savedFilePath == null)
                savingStatusLabel.Text = AppResource.PermissionDenied;
            else
            {
                savingStatusLabel.Text = AppResource.SavedInLabel + " \"" + savedFilePath + "\"";
                imageFileShowPath = savedFilePath;
            }*/
        }

        private async Task saveImageAsync()
        {
            SKData data = SKImage.FromBitmap(bitmap).Encode();

            // NOTE: This filename is not used by iOS!
            DateTime dt = DateTime.Now;
            string filename = String.Format("Uzor" + "-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
                                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

            string savedFilePath = await BitmapStreamWriter.SaveBitmap(bitmap, (SKEncodedImageFormat)formatPicker.SelectedItem, 100, filename, "UzorApp");

            if (savedFilePath == null)
                savingStatusLabel.Text = AppResource.PermissionDenied;
            else
            {
                savingStatusLabel.Text = AppResource.SavedInLabel + " \"" + savedFilePath + "\"";
                imageFileShowPath = savedFilePath;
            }
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
            updateBitmapPreviewView();
        }
        private string imageFileShowPath = "";
        private async void showImageFile_Clicked(object sender, EventArgs e)
        {
            if (imageFileShowPath.Length == 0)
                await saveImageAsync().ConfigureAwait(false);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = AppResource.Share,
                File = new ShareFile(imageFileShowPath)
            });

            imageFileShowPath = "";
        }

        private void frameSizeChandged(object sender, EventArgs e)
        {
            this.bitmapPreviewViewFrame.HeightRequest = this.bitmapPreviewViewFrame.Width;
        }
    }
}