using EI_OpgaveApp.Database;
using EI_OpgaveApp.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(App_iOS))]

namespace EI_OpgaveApp.iOS
{
    class App_iOS : IApp
    {
        public string GetVersionCode()
        {
            return NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
        }
    }
}