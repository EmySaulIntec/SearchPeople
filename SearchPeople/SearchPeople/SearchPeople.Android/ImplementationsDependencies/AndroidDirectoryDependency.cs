using System;
using SearchPeople.Droid.ImplementationsDependencies;
using SearchPeople.Services;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDirectoryDependency))]
namespace SearchPeople.Droid.ImplementationsDependencies
{
    public class AndroidDirectoryDependency : IDirectoryDependency
    {
        public string OpenDirectory()
        {
            var e = Convert.ToString((Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)));



            return e;
        }
    }
}