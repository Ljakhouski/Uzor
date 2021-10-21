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

            /* WORK ONLY UP TO ANDROID 12 VERSION !!! */

           // string picturesDirectory = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath, folder);
            string picturesDirectory = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), folder);
                        /*  /storage/emulated/0/UzorApp  */

            if (await Permissions.RequestAsync<Permissions.StorageWrite>() == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.StorageRead>()  == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.Photos>()       == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.Media>()        == PermissionStatus.Granted
                )
            {
                System.IO.Directory.CreateDirectory(picturesDirectory);

                using (File bitmapFile = new File(picturesDirectory, filename))
                {
                    try
                    {
                        bitmapFile.CreateNewFile();
                    }
                    catch (Exception e)
                    {
                        var s = e.Message;
                        return false;
                    }

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(data);
                    }
                }
            }
            else
                return false;

                
            return true;
        }
    }
}