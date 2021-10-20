using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.OS;
using Java.IO;
using Uzor.Droid;
using Uzor;
using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using Android;
using AndroidX.Core.Content;
using Android.Content.PM;

[assembly: Dependency(typeof(PhotoLibrary))]

namespace Uzor.Droid
{
    public class PhotoLibrary : IPhotoLibrary
    {
        public async Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        { 

            File picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            File folderDirectory = picturesDirectory;

            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            var statu3s = await Permissions.RequestAsync<Permissions.StorageRead>();
            var status4 = await Permissions.RequestAsync<Permissions.Photos>();
            var status5 = await Permissions.RequestAsync<Permissions.Media>();

            /*if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == (int)Permission.Granted)
            {
                // We have permission, go ahead and use the camera.
            }
            else
            {
                // Camera permission is not granted. If necessary display rationale & request.
            }*/


            string root = "";
            if (!string.IsNullOrEmpty(folder))
            {
                folderDirectory = new File(picturesDirectory, folder);
                root = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "MyFolder");
                folderDirectory.Mkdirs();
                folderDirectory.Mkdir();
                System.IO.Directory.CreateDirectory(root);
            }

            using (File bitmapFile = new File(/*folderDirectory*/ root, filename))
            {
            try {
                bitmapFile.CreateNewFile(); }
            catch(Exception e)
            {
                string S = e.Message;
            }

                using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                {
                    await outputStream.WriteAsync(data);
                }

                // Make sure it shows up in the Photos gallery promptly.
                //MediaScannerConnection.ScanFile(MainActivity.Instance,
                    //                               new string[] { bitmapFile.Path },
                //                                new string[] { "image/png", "image/jpeg" }, null);
            }
         

            return true;
        }
    }
}