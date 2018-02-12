using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp
{
    public partial class App : Application
    {
        static MaintenanceDatabase database;
        static GlobalData gd = GlobalData.GetInstance;

        public App()
        {
            if (gd.IsLoggedIn)
            {
                MainPage = gd.TabbedPage;
            }
            else
            {
                gd.TabbedPage.Children.Add(gd.LoginPage);
                MainPage = gd.TabbedPage;
            }
            checkedConnectionSettings(); 
        }

        private async void checkedConnectionSettings()
        {
            if (await Database.GetConnectionSetting(0) != null)
            {
                gd.Done = false;
                var s = await Database.GetConnectionSetting(0);

                gd.ConnectionSettings = s;
            }
            else
            {
                ConnectionSettings settings = new ConnectionSettings()
                {
                    ID = 0,
                    BaseAddress = "http://htm.api.eliteit.dk/"
                };
                await Database.SaveConnectionSetting(settings);
                gd.ConnectionSettings = settings;
            }
        }

        public static MaintenanceDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MaintenanceDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("MaintenanceSQLite.db3"));
                }
                return database;
            }
        }

        public int ResumeAtMaintenanceId { get; set; }


        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            gd.Done = true;
            Debug.WriteLine("SLEEP");
        }
        protected override void OnResume()
        {
            gd.Done = false;
        }
    }
}
