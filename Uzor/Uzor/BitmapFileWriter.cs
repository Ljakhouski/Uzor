﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Uzor
{
    class BitmapStreamWriter
    {
        public async static void SaveBitmap(SKBitmap bitmap, SKEncodedImageFormat format, int quality, string name, string folder)
        {
            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                bitmap.Encode(wstream, format, quality);
                byte[] data = memStream.ToArray();

                if (data == null || data.Length == 0)
                {
                    throw new BitmapFileWriteException("Encode returned null or empty array");
                }
                else
                {
                    bool success = await DependencyService.Get<IPhotoLibrary>().
                        SavePhotoAsync(data, folder, name);

                    if (!success)
                    {
                        throw new BitmapFileWriteException("file save error");
                    }
                }
            }
        }
    }

    [Serializable]
    public class BitmapFileWriteException : Exception
    {
        public BitmapFileWriteException() { }

        public BitmapFileWriteException(string message)
            : base(message) { }

        public BitmapFileWriteException(string message, Exception inner)
            : base(message, inner) { }
    }
}
