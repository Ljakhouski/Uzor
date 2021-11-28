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
using System.IO;

[assembly: Dependency(typeof(ProjectManager))]
namespace Uzor.Droid
{
    class ProjectManager : IProjectManager
    {
        public async Task<string[]> ExportProjects()
        {
            var fileList = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));

            string[] result = new string[0];
            foreach (string fileName in fileList)
                if (fileName.Substring(fileName.Length - 4) == ".ubf" || fileName.Substring(fileName.Length - 5) == ".lubf")
                {
                    string path = Android.App.Application.Context.GetExternalFilesDir(/*Android.OS.Environment.RootDirectory.ToString()*/ null).ToString();
                    System.IO.File.Copy(fileName, path, true);
                    Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = fileName.Split('/')[fileName.Split('/').Length - 1];
                }
            return result;
        }

        public async Task<string[]> ImportProjects()
        {
            var fileList = Directory.GetFiles(Android.App.Application.Context.GetExternalFilesDir(/*Android.OS.Environment.RootDirectory.ToString()*/ null).ToString());

            string[] result = new string[0];
            foreach (string fileName in fileList)
                if (fileName.Substring(fileName.Length - 4) == ".ubf" || fileName.Substring(fileName.Length - 5) == ".lubf")
                {
                    string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                    System.IO.File.Copy(fileName, path, true);
                    Array.Resize(ref result, result.Length + 1);
                    result[result.Length - 1] = fileName.Split('/')[fileName.Split('/').Length - 1];
                }
            return result;
        }

        public string GetExternalFolderPath()
        {
            return Android.App.Application.Context.GetExternalFilesDir(/*Android.OS.Environment.RootDirectory.ToString()*/ null).ToString();
        }
    }
}