using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EI_OpgaveApp.Database;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EI_OpgaveApp.Droid;
using Xamarin.Forms;
using Android.Content.PM;

[assembly: Dependency(typeof(App_Droid))]

namespace EI_OpgaveApp.Droid
{
    class App_Droid : IApp
    {

        public string GetVersionCode()
        {
            Context context = Forms.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);
            return info.VersionName;
        }
    }
}