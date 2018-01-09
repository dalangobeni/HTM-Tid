using EI_OpgaveApp.Models;
using EI_OpgaveApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp
{
    public class GlobalData
    {
        private static GlobalData globaldata;
        private TabbedPage tabbedPage;
        private LoginPage loginPage;
        private bool isLoggedIn = false;
        private ResourcePerson user;
        private DateTime searchDateTime;
        private DateTime searchDateTimeLast;
        private string searchUserName;
        private TimeRegistrationModel timeRegisteredIn;
        private TimeRegistrationModel timeRegisteredOut;
        private string baseAddress;
        private bool done;
        private Resources currentResource;
        ConnectionSettings cs = new ConnectionSettings();

        private GlobalData() { }

        public static GlobalData GetInstance
        {
            get
            {
                if (globaldata == null)
                {
                    globaldata = new GlobalData();
                }
                return globaldata;
            }
        }
        public TabbedPage TabbedPage
        {
            get
            {
                if (tabbedPage == null)
                {
                    tabbedPage = new TabbedPage();
                }
                if (isLoggedIn)
                {
                    tabbedPage = new TabbedPage();
                    tabbedPage.Children.Add(new HomePage());
                    tabbedPage.Children.Add(new MeetLeavePage());
                    tabbedPage.Children.Add(new MaintenancePage());
                    tabbedPage.Children.Add(new SettingsPage());
                }
                return tabbedPage;
            }
        }
        public LoginPage LoginPage
        {
            get
            {
                if (loginPage == null)
                {
                    loginPage = new LoginPage();
                }
                return loginPage;
            }
        }
        public ResourcePerson User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public DateTime SearchDateTime
        {
            get
            {
                return searchDateTime;
            }
            set
            {
                searchDateTime = value;
            }
        }
        public DateTime SearchDateTimeLast
        {
            get
            {
                return searchDateTimeLast;
            }
            set
            {
                searchDateTimeLast = value;
            }
        }
        public string SearchUserName
        {
            get
            {
                return searchUserName;
            }
            set
            {
                searchUserName = value;
            }
        }
        public TimeRegistrationModel TimeRegisteredIn
        {
            get
            {
                return timeRegisteredIn;
            }
            set
            {
                timeRegisteredIn = value;
            }
        }
        public TimeRegistrationModel TimeRegisteredOut
        {
            get
            {
                return timeRegisteredOut;
            }
            set
            {
                timeRegisteredOut = value;
            }
        }
        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }
            set
            {
                isLoggedIn = value;
            }
        }

        public string BaseAddress
        {
            get
            {
                return baseAddress;
            }
            set
            {
                baseAddress = value;
            }
        }

        public bool Done
        {
            get
            {
                return done;
            }
            set
            {
                done = value;
            }
        }

        public ConnectionSettings ConnectionSettings
        {
            get
            {
                if (cs.BaseAddress == null)
                {
                    GetConSet();
                }
                return cs;
            }
            set
            {
                cs = value;
            }
        }
        public Resources CurrentResource
        {
            get
            {
                return currentResource;
            }
            set
            {
                currentResource = value;
            }
        }
        private void GetConSet()
        {
            cs = App.Database.GetConnectionSetting(0).Result;
        }
    }
}
