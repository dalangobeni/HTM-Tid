using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using EI_OpgaveApp.Synchronizers;
using EI_OpgaveApp.Views.Custom_Cells;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class JobRecLineAll : ContentPage
    {
        StackLayout main;

        ListView lv;

        Button cancelButton;
        SearchBar sb;

        List<JobRecLine> jobList;
        List<JobRecLine> jobItemsSourceList;

        Color buttonColor = Color.FromRgb(135, 206, 250);

        SynchronizerFacade syncFacade = SynchronizerFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        GlobalData gd = GlobalData.GetInstance;

        string searchString = "";

        public JobRecLineAll()
        {

            cancelButton = new Button { Text = "Tilbage", BackgroundColor = buttonColor, TextColor = Color.White };
            cancelButton.Clicked += CancelButton_Clicked;

            sb = new SearchBar()
            {
                Placeholder = "Søg...",
                HeightRequest = 40
            };
            sb.TextChanged += Sb_TextChanged;
            MakeListView();
            main = new StackLayout
            {
                Children =
                {
                    sb,
                    lv,
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

        private void Sb_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                searchString = "";
                //lv.ItemsSource = itemssourceList;
                UpdateItemsSource();
            }
            else
            {
                SearchUpdate(e.NewTextValue);

                searchString = e.NewTextValue;
            }
        }
        private void SearchUpdate(string ss)
        {
            List<JobRecLine> temp = new List<JobRecLine>();

            if (jobItemsSourceList != null)
            {
                temp = jobItemsSourceList.Where(x => x.Start_Time.ToString().ToLower().Contains(ss.ToLower()) || x.Work_Type.ToLower().Contains(ss.ToLower()) || x.TaskNo.ToLower().Contains(ss.ToLower())).ToList();
                try
                {
                    temp = temp.Where(x => x.Resource_Person == gd.CurrentResource.No).OrderBy(x => x.Resource_Person).ToList();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            lv.ItemsSource = temp;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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
                    jobItemsSourceList = jobList.Where(x => x.Resource_Person == gd.CurrentResource.No).ToList();
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