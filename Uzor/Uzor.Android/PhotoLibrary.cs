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
        public async Task<string> SavePhotoAsync(byte[] data, string folder, string filename)
        {

            string picturesDirectory;



            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                /*  own path for new file-access politic  */
                picturesDirectory = Android.App.Application.Context.GetExternalFilesDir(/*Android.OS.Environment.RootDirectory.ToString()*/ null).ToString();
            }
            else
            {
                /*  in android 10 create '/storage/emulated/0/UzorApp' */
                picturesDirectory = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), folder);
            }



            if (await Permissions.RequestAsync<Permissions.StorageWrite>() == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.StorageRead>() == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.Photos>() == PermissionStatus.Granted &&
                await Permissions.RequestAsync<Permissions.Media>() == PermissionStatus.Granted
                )
            {
                //System.IO.Directory.CreateDirectory(picturesDirectory); 
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
                        return null;
                    }

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(data);
                    }
                }
            }
            else
            {
                //TODO: throw new Excepton
                return null;
            }


            //if ()
            return new File(picturesDirectory, filename).ToString();
        }
    }
}

// string picturesDirectory = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath, folder);

/*
File f = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
File filesDirectory = Android.App.Application.Context.GetExternalFilesDir(null);
string f3 = Xamarin.Essentials.FileSystem.AppDataDirectory.ToString();
File f4 = 
*/