using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using EI_OpgaveApp.Synchronizers;
using EI_OpgaveApp.Views.Custom_Cells;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class JobRecLineManagercs : ContentPage
    {
        StackLayout main;

        ListView lv;

        Button sendItemsButton;
        Button cancelButton;
        Button showStatsButton;
        Button showAllButton;

        List<JobRecLine> jobList;
        List<JobRecLine> jobItemsSourceList;

        Color buttonColor = Color.FromRgb(135, 206, 250);

        SynchronizerFacade syncFacade = SynchronizerFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        GlobalData gd = GlobalData.GetInstance;
        public JobRecLineManagercs()
        {
            sendItemsButton = new Button { Text = "Send tidsregistreringer", BackgroundColor = buttonColor, TextColor = Color.White };
            sendItemsButton.Clicked += SendItemsButton_Clicked;
            showStatsButton = new Button { Text = "Vis statistikker", BackgroundColor = buttonColor, TextColor = Color.White };
            showStatsButton.Clicked += ShowStatsButton_Clicked;
            cancelButton = new Button { Text = "Tilbage", BackgroundColor = buttonColor, TextColor = Color.White };
            cancelButton.Clicked += CancelButton_Clicked;
            showAllButton = new Button { Text = "Vis Alle Registreringer", BackgroundColor = buttonColor, TextColor = Color.White };
            showAllButton.Clicked += ShowAllButton_Clicked;
            MakeListView();
            main = new StackLayout
            {
                Padding = 10,
                Children =
                {
                    lv,
                    sendItemsButton,
                    showStatsButton,
                    showAllButton,
                    cancelButton
                },
                Spacing = 1,
            };
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                main.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = main;

            MessagingCenter.Subscribe<JobRecLine>(this, "hi", (sender) =>
            {
                Navigation.PushModalAsync(new JobRecLineUpdateForm(sender));
            });
        }

        private void ShowAllButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new JobRecLineAll());
        }

        private async void ShowStatsButton_Clicked(object sender, EventArgs e)
        {
            ServiceFacade f = ServiceFacade.GetInstance;
            TimeRegStat temp = await f.TimeRegStatsService.GetTimeRegStatsAsync(gd.CurrentResource.No);
            if (temp.User != "")
            {
                await Navigation.PushModalAsync(new TimeRegStatsPage(temp));
            }
            else
            {
                await DisplayAlert("Ingen forbindelse", "Der er ingen forbindelse til NAV. Statistikker kan ikke vises offline.", "OK");
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void SendItemsButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("OBS!", "Er du sikker på du vil godkende og sende dine tidsregistreringer?", "Ja", "Nej"))
            {
                foreach (var item in jobItemsSourceList)
                {
                    item.Sent = true;
                    item.Edited = true;
                    await db.UpdateJobRecLineAsync(item);
                }
                syncFacade.JobRecLineSynchronizer.SyncDatabaseWithNAV();
                await Navigation.PopModalAsync();
            }
        }

        private void MakeListView()
        {
            var temp = new DataTemplate(typeof(CustomCaseCell));
            lv = new ListView();

            lv.HasUnevenRows = true;
            lv.ItemTemplate = temp;

            lv.ItemTapped += Lv_ItemTapped;
            UpdateItemsSource();
        }

        private void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = ((ListView)sender).SelectedItem;
            JobRecLine c = (JobRecLine)action;

            this.Navigation.PushModalAsync(new JobRecLineDetail(c));
        }
        private void UpdateItemsSource()
        {
            jobList = null;
            GetData();
            if (jobList != null)
            {

                try
                {
                    jobItemsSourceList = jobList.Where(x => x.Resource_Person == gd.CurrentResource.No && x.Sent == false).ToList();
                    lv.ItemsSource = jobItemsSourceList;
                }
                catch { }

            }
        }
        private void GetData()
        {
            var task = Task.Run(async () => { jobList = await db.GetJobRecLinesAsync(); });
            task.Wait();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateItemsSource();
        }
    }
}