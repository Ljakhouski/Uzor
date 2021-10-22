using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Uzor.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(FileOpener))]
namespace Uzor.Droid
{
    class FileOpener : IFileOpener
    {
        public void Show(string path)
        {
            Java.IO.File file = new Java.IO.File(path);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(uri);
            Xamarin.Forms.Forms.Context.StartActivity(intent);

            

        }
    }
}