using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class HomePage : ContentPage
    {
        bool loading;
        StackLayout layout;
        ActivityIndicator ai;
        Button logOutButton;
        Button SendJobRegLinesButton;
        Button AppErrorButton;
        Button StopCurrentJobRec;

        Label tasks;
        Label searchCriteriaHeader;
        Label timeRegisteredIn;
        Label timeRegisteredOut;
        Label userInfo;
        Label searchTimeFrom;
        Label searchTimeTo;
        Label searchUser;
        Label currentJobRecLabel;

        Image image;

        List<MaintenanceTask> taskList;

        GlobalData gd = GlobalData.GetInstance;
        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;

        Color buttonColor;
        Color sendColor;
        Grid grid;
        Grid gridCriteria;

        public HomePage()
        {
            buttonColor = Color.FromRgb(135, 206, 250);
            BackgroundColor = Color.White;
            Title = "Hjem";


            NavigationPage.SetHasNavigationBar(this, false);
            //MakeToolBar();
            MakeGrid();
            MakeGridCriteria();
            HandleImage();
            StopCurrentJobRec = new Button { Text = "Stop iganværende opgave", BackgroundColor = buttonColor, TextColor = Color.White, VerticalOptions = LayoutOptions.EndAndExpand };
            StopCurrentJobRec.Clicked += StopCurrentJobRec_Clicked;

            logOutButton = new Button { Text = "Log ud", BackgroundColor = buttonColor, TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            logOutButton.Clicked += LogOutButton_Clicked;

            SendJobRegLinesButton = new Button { BackgroundColor = sendColor, TextColor = Color.White, VerticalOptions = LayoutOptions.EndAndExpand };
            SendJobRegLinesButton.Clicked += SendJobRegLinesButton_Clicked;

            AppErrorButton = new Button { Text = "Fejlmeddelelser", BackgroundColor = buttonColor, TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            AppErrorButton.Clicked += AppErrorButton_Clicked;
            layout = new StackLayout { Padding = 10, };

            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            layout.Children.Add(grid);
            //layout.Children.Add(gridCriteria);
            layout.Children.Add(image);
            //layout.Children.Add(SendJobRegLinesButton);
            //layout.Children.Add(AppErrorButton);
            layout.Children.Add(StopCurrentJobRec);
            layout.Children.Add(logOutButton);

            Content = layout;
        }

        private void StopCurrentJobRec_Clicked(object sender, EventArgs e)
        {
            Stop();
        }
        private async void Stop()
        {
            if (await DisplayAlert("OBS!", "Vil du stoppe igangværende opgave?", "Ja", "Nej"))
            {
                var temp = await db.GetJobRecLinesAsync();
            JobRecLine temp2;
            try
            {
                temp2 = temp.Where(i => i.IsRunning).First();
            }
            catch
            {
                temp2 = null;
            }
            if (temp2 != null)
            {
                temp2.IsRunning = false;
                temp2.End_Time = DateTime.UtcNow;
                await db.UpdateJobRecLineAsync(temp2);
            }
            else
            {
                await DisplayAlert("OBS!", "Der er ingen igangværende opgave.", "OK");
            }
            }
            else
            {
                await DisplayAlert("OBS!", "Opgaven er ikke stoppet", "OK");
            }
            OnAppearing();
        }
        private void AppErrorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AppErrorPage());
        }

        private void SendJobRegLinesButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new JobRecLineManagercs());
        }

        private void HandleImage()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                var answer = await DisplayAlert("Support", "Vil du ringe til EliteIT for support?", "Ja", "Nej");
                if (answer)
                {
                    Device.OpenUri(new Uri("tel:+4588168810"));
                }
            };

            image = new Image();

            image.Source = "eistor.png";
            image.Opacity = 0.7;
            image.VerticalOptions = LayoutOptions.Start;
            image.Scale = 0.7;
            image.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private void MakeGridCriteria()
        {
            searchCriteriaHeader = new Label() { Text = "Søgekriterier", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = BackgroundColor };
            tasks = new Label() { TextColor = BackgroundColor, HorizontalOptions = LayoutOptions.Center };
            searchTimeFrom = new Label() { Text = "Tid fra", TextColor = BackgroundColor, HorizontalOptions = LayoutOptions.End };
            searchTimeTo = new Label() { Text = "Tid til", TextColor = BackgroundColor, HorizontalOptions = LayoutOptions.End };
            searchUser = new Label() { Text = "Bruger", TextColor = BackgroundColor };

            gridCriteria = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                },
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = buttonColor
            };

            gridCriteria.Children.Add(searchCriteriaHeader, 0, 0);
            Grid.SetColumnSpan(searchCriteriaHeader, 2);
            gridCriteria.Children.Add(searchUser, 0, 1);
            gridCriteria.Children.Add(tasks, 0, 3);
            Grid.SetColumnSpan(tasks, 2);
            gridCriteria.Children.Add(searchTimeFrom, 1, 1);
            gridCriteria.Children.Add(searchTimeTo, 1, 2);

        }
        private void MakeGrid()
        {
            userInfo = new Label() { Text = "Velkommen " + gd.User.Name, HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = BackgroundColor };
            timeRegisteredIn = new Label() { Text = "Du er ikke mødt ind endnu", TextColor = BackgroundColor };
            timeRegisteredOut = new Label() { Text = "Du er ikke meldt ud endnu", TextColor = BackgroundColor, HorizontalOptions = LayoutOptions.End };
            currentJobRecLabel = new Label() { Text = "Der er i øjeblikket ingen påbegyndte opgaver", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = BackgroundColor };
            grid = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                },
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = buttonColor
            };

            grid.Children.Add(userInfo, 0, 0);
            Grid.SetColumnSpan(userInfo, 2);
            grid.Children.Add(timeRegisteredIn, 0, 1);
            grid.Children.Add(timeRegisteredOut, 1, 1);
            grid.Children.Add(currentJobRecLabel, 0, 2);
            Grid.SetColumnSpan(currentJobRecLabel, 2);

        }

        private async void LogOutButton_Clicked(object sender, EventArgs e)
        {
            var temp = await db.GetJobRecLinesAsync();
            JobRecLine temp2 = new JobRecLine();
            try
            {
                temp2 = temp.Where(i => i.IsRunning).First();
            }
            catch
            {
                temp2 = null;
            }
            if (temp2 != null)
            {
                if (await DisplayAlert("OBS!", "Der er uafsluttede opgaver. Vil du stoppe opgaven inden du logger ud?", "Ja", "Nej"))
                {
                    temp2.IsRunning = false;
                    temp2.End_Time = DateTime.UtcNow;
                    await db.UpdateJobRecLineAsync(temp2);
                }
            }
            gd.IsLoggedIn = false;
            List<Page> pl = new List<Page>();
            foreach (var item in gd.TabbedPage.Children)
            {
                pl.Add(item);
            }
            foreach (var item in pl)
            {
                gd.TabbedPage.Children.Remove(item);
            }
            gd.TabbedPage.Children.Add(gd.LoginPage);

        }

        private void RemoveActivityIndicator()
        {

            loading = false;
            layout.Children.Remove(ai);
            logOutButton.IsEnabled = true;
        }

        private void ShowActivityIndicator()
        {
            ai = new ActivityIndicator()
            {
                IsRunning = true
            };
            layout.Children.Add(ai);

            logOutButton.IsEnabled = false;

        }

        private async Task<string> SendButtonText()
        {
            double i = 0;
            List<JobRecLine> jobList = null;
            List<JobRecLine> jobItemsSourceList = null;
            while (jobList == null)
            {
                jobList = await App.Database.GetJobRecLinesAsync();
            }
            if (jobList != null)
            {

                try
                {
                    jobItemsSourceList = jobList.Where(x => x.Resource_Person == gd.CurrentResource.No && x.Sent == false).ToList();
                    foreach (var item in jobItemsSourceList)
                    {
                        //if (item.Work_Type_Code == "TIMER" || item.Work_Type_Code == "REJSETID")
                        //{
                        i++; // = i + item.Quantity;
                             //}
                    }
                }
                catch { }

            }

            if (i == 0)
            {
                SendJobRegLinesButton.BackgroundColor = Color.Green;
                SendJobRegLinesButton.TextColor = Color.White;
            }
            else if (0 < i && i < 5)
            {
                SendJobRegLinesButton.BackgroundColor = Color.Yellow;
                SendJobRegLinesButton.TextColor = Color.Black;
            }
            else
            {
                SendJobRegLinesButton.BackgroundColor = Color.Red;
                SendJobRegLinesButton.TextColor = Color.White;
            }
            string text = "(" + i + ") usendte linjer";
            return text;
        }
        protected async override void OnAppearing()
        {
            var temp = await db.GetJobRecLinesAsync();
            JobRecLine temp2 = new JobRecLine();

            MeetLeavePage timepage = new MeetLeavePage();
            timepage.GetData();

            taskList = null;
            int notdone = 0;
            while (taskList == null)
            {
                taskList = await App.Database.GetTasksAsync();
            }
            foreach (var item in taskList)
            {
                if (item.status == "Released" && item.responsible == gd.User.No && item.planned_Date <= gd.SearchDateTimeLast && item.planned_Date >= gd.SearchDateTime)
                {
                    notdone++;
                }
            }
            try
            {
                temp2 = temp.Where(i => i.IsRunning).First();
            }
            catch
            {
                temp2 = null;
            }
            if (temp2 != null)
            {
                currentJobRecLabel.Text = "Der er startet en opgave klokken: " + temp2.Start_Time.AddHours(1).ToString("HH:mm") + " på opgave nr.: " + temp2.TaskNo + "\nDu arbejder på:  " + temp2.Work_Type_And_Machine;
            }
            else
            {
                currentJobRecLabel.Text = "Der er i øjeblikket ingen påbegyndte opgaver";
            }
            if (notdone == 1)
            {
                tasks.Text = "Du har " + notdone + " frigivet opgave.";
            }
            else
            {
                tasks.Text = "Du har " + notdone + " frigivne opgaver.";
            }
            if (gd.TimeRegisteredIn != null && gd.TimeRegisteredIn.Time.Date == DateTime.Today.Date)
            {
                timeRegisteredIn.Text = "Du er mødt ind: " + gd.TimeRegisteredIn.Time.AddHours(1).ToString("HH:mm");
            }
            if (gd.TimeRegisteredOut != null && gd.TimeRegisteredOut.Time.Date == DateTime.Today.Date)
            {
                timeRegisteredOut.Text = "Du er meldt ud: " + gd.TimeRegisteredOut.Time.AddHours(1).ToString("HH:mm");
            }
            if (gd.SearchDateTime > new DateTime(1950, 1, 1))
            {
                searchTimeFrom.Text = "Fra d.: " + gd.SearchDateTime.ToString("dd/MM/yyyy");
            }
            else
            {
                searchTimeFrom.Text = "Fra dato: Ingen dato sat";
            }
            if (gd.SearchDateTimeLast < new DateTime(2050, 1, 1))
            {
                searchTimeTo.Text = "Til d.: " + gd.SearchDateTimeLast.ToString("dd/MM/yyyy");
            }
            else
            {
                searchTimeTo.Text = "Til dato: Ingen dato sat";
            }
            if (gd.SearchUserName != null)
            {
                searchUser.Text = "Brugernavn: " + gd.SearchUserName;
            }
            else
            {
                searchUser.Text = "Brugernavn: Intet brugernavn";
            }
            SendJobRegLinesButton.Text = await SendButtonText();
        }
    }
}
