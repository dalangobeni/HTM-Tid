using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using EI_OpgaveApp.Synchronizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class LoginPage : ContentPage
    {
        bool usernameText = false;
        bool passwordText;
        bool dbcon;

        Button button;
        Button connectionSettingsButton;
        Entry username;
        Entry password;

        ResourcePerson person = null;

        ServiceFacade facade = ServiceFacade.GetInstance;
        SynchronizerFacade syncFacade = SynchronizerFacade.GetInstance;
        GlobalData gd = GlobalData.GetInstance;
        MaintenanceDatabase db = App.Database;
        public LoginPage()
        {
            string imagesource = "eistor.png";
            NavigationPage.SetHasNavigationBar(this, false);

            var layout = new StackLayout { Padding = 10, };

            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
                //imagesource = "Images/app opsætning 2.jpg";
            }


            BackgroundColor = Color.White;
            Image image = new Image() { Source = imagesource, Opacity = 0.7, HorizontalOptions = LayoutOptions.CenterAndExpand };
            Title = "Log ind";
            var label = new Label
            {
                Text = "Log ind",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center
            };

            username = new Entry { Placeholder = "Brugernavn" };


            layout.Children.Add(username);

            password = new Entry { Placeholder = "Password", IsPassword = true };
            layout.Children.Add(password);
            connectionSettingsButton = new Button { Text = "Opkoblingsindstillinger", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            button = new Button { Text = "Log ind", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, IsEnabled = false };
            layout.Children.Add(button);
            layout.Children.Add(connectionSettingsButton);
            layout.Children.Add(image);

            password.TextChanged += Password_TextChanged;
            username.TextChanged += Username_TextChanged;

            connectionSettingsButton.Clicked += async (s, e) =>
            {
                await Navigation.PushModalAsync(new ConnectionSettingsPage());
            };
            button.Clicked += async (s, e) =>
            {
                bool succes = false;
                bool loading = true;
                bool connection = false;
                while (loading)
                {
                    ActivityIndicator ai = new ActivityIndicator()
                    {
                        IsRunning = true
                    };
                    layout.Children.Add(ai);
                    button.IsEnabled = false;
                    connectionSettingsButton.IsEnabled = false;

                    try
                    {
                        person = db.GetResourcePersonAsync(username.Text.ToUpper()).Result;
                        if (person == null)
                        {
                            dbcon = false;
                            try
                            {
                                connection = await syncFacade.MaintenanceTaskSynchronizer.HasConnectionToNAV();
                            }
                            catch
                            {
                                connection = false;
                            }
                            string ss = username.Text;
                            var persons = await facade.ResourcePersonService.GetResroucePersons();
                            person = persons.Where(x => x.No == username.Text).FirstOrDefault();
                            //person = await facade.ResourcePersonService.GetResroucePerson(username.Text);

                        }
                        else
                        {
                            dbcon = true;
                        }
                        if (password.Text == person.Password)
                        {
                            succes = true;
                        }
                        gd.User = person;
                        gd.SearchUserName = person.No;

                    }
                    catch
                    {

                    }
                    if (!succes && (connection || dbcon))
                    {
                        layout.Children.Remove(ai);
                        await DisplayAlert("Advarsel", "Forkert brugernavn eller password", "OK");
                    }
                    else if (!connection && !dbcon)
                    {
                        layout.Children.Remove(ai);
                        await DisplayAlert("Advarsel", "Enheden har ikke forbindelse til NAV", "OK");
                    }
                    else
                    {
                        List<Resources> rl = await db.GetResourcesAsync();
                        gd.CurrentResource = rl.Where(x => x.Name == gd.User.Name).FirstOrDefault();

                        gd.SearchDateTime = DateTime.Today.AddDays(-7);
                        gd.SearchDateTimeLast = DateTime.Today.AddDays(7);

                        gd.TabbedPage.Children.Add(new HomePage());
                        gd.TabbedPage.Children.Add(new MeetLeavePage());
                        gd.TabbedPage.Children.Add(new MaintenancePage());
                        gd.TabbedPage.Children.Add(new SettingsPage());

                        gd.TabbedPage.Children.Remove(gd.LoginPage);
                        gd.IsLoggedIn = true;

                        password.Text = null;
                        //if (Device.OS != TargetPlatform.iOS)
                        //{
                        Task t = Task.Factory.StartNew(() => facade.ThreadManager.StartSynchronizationThread());
                        //t.Start();
                        //}
                        ConnectionSettings cs = db.GetConnectionSetting(0).Result;
                        cs.LastUser = person.No;
                        cs.lastpw = person.Password;
                        await db.UpdateConnectionSetting(cs);

                    }
                    loading = false;
                    button.IsEnabled = true;
                    connectionSettingsButton.IsEnabled = true;

                    layout.Children.Remove(ai);
                }
            };

            Content = new ScrollView { Content = layout };

        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckUserName();
        }

        private void CheckUserName()
        {
            usernameText = true;
            if (username.Text == null)
            {
                usernameText = false;
            }
            if (usernameText && passwordText)
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPassword();
        }

        private void CheckPassword()
        {
            passwordText = true;
            if (password.Text == null)
            {
                passwordText = false;
            }
            if (usernameText && passwordText)
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }

        protected override void OnAppearing()
        {
            if (gd.ConnectionSettings != null)
            {
                if (gd.ConnectionSettings.LastUser != null)
                {
                    username.Text = gd.ConnectionSettings.LastUser;
                    password.Text = gd.ConnectionSettings.lastpw;
                }
            }
            //bool usernameText = false;
            passwordText = false;
            button.IsEnabled = false;
            CheckPassword();
            CheckUserName();
        }
    }
}
