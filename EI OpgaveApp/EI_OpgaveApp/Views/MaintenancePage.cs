using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using EI_OpgaveApp.Synchronizers;
using EI_OpgaveApp.Views.Custom_Cells;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class MaintenancePage : ContentPage
    {
        ListView lv;

        SynchronizerFacade syncFacade = SynchronizerFacade.GetInstance;
        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        GlobalData gd = GlobalData.GetInstance;

        List<MaintenanceTask> tasks;
        IEnumerable<MaintenanceTask> itemssourceList;

        Button showDoneButton;
        Button createTaskButton;

        Grid buttonGrid;

        bool showDone = false;
        string searchString = "";
        SearchBar sb;
        public MaintenancePage()
        {
            Title = "Opgaver";
            NavigationPage.SetHasNavigationBar(this, false);


            showDoneButton = new Button() { Text = "Vis udførte opgaver", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            showDoneButton.Clicked += ShowDoneButton_Clicked;

            createTaskButton = new Button() { Text = "Opret sagsopgave", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            createTaskButton.Clicked += CreateTaskButton_Clicked;

            MakeListView();
            MakeGrid();

            sb = new SearchBar()
            {
                Placeholder = "Søg...",
                HeightRequest = 40
            };
            sb.TextChanged += Sb_TextChanged;
            StackLayout layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        buttonGrid,
                        sb,
                        lv
                    }

            };
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = layout;
        }

        private void MakeGrid()
        {
            buttonGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    //new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                }
            };

            //buttonGrid.Children.Add(createTaskButton, 0, 0);
            buttonGrid.Children.Add(showDoneButton, 0, 0);
        }

        private void CreateTaskButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MaintenanceTaskForm());
        }

        private void Sb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //lv.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                searchString = "";
                //lv.ItemsSource = itemssourceList;
                UpdateItemSource();
            }
            else
            {
                SearchUpdate(e.NewTextValue);

                searchString = e.NewTextValue;
            }
            //lv.EndRefresh();
        }

        private void SearchUpdate(string ss)
        {
            List<MaintenanceTask> temp = new List<MaintenanceTask>();
            
            if (itemssourceList != null)
            {
                temp = itemssourceList.Where(x => x.responsible.ToLower().Contains(ss.ToLower()) || x.text.ToLower().Contains(ss.ToLower()) || x.CustomerName.ToLower().Contains(ss.ToLower()) || x.no.ToString().ToLower().Contains(ss.ToLower())).ToList();
                
                if (showDone)
                {
                    try
                    {
                        temp = temp.Where(x => x.status == "Completed").OrderBy(x => x.no).ToList();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                else
                {
                    try
                    {
                        temp = temp.Where(x => x.status == "Released").OrderBy(x => x.no).ToList();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }

                lv.ItemsSource = temp;
            }
        }

        public void ShowOnMap(MaintenanceTask _task)
        {
            //if (_task.longitude != 0 && _task.latitude != 0)
            //{
            //    string s = "https://www.google.dk/maps/place/" + _task.latitude + "," + _task.longitude + "/" + _task.latitude + "," + _task.longitude + "z/";
            //    Uri uri = new Uri(s);
            //    Device.OpenUri(uri);
            //}
            //else
            //{
            //    DisplayAlert("Ingen koordinater", "Der er ingen koordinater på opgaven. Bekræft at opgaven er afsluttet, og prøv igen.", "OK");
            //}
        }
        public async void SetDone(MaintenanceTask _task)
        {
            var response = await DisplayAlert("Færdig", "Vil du sætte opgaven til færdig?", "Ja", "Nej");
            if (response)
            {
                int i = 0;
                while (i == 0)
                {
                    if (_task.status == "Released")
                    {
                        _task.status = "Completed";
                        try
                        {
                            //var locator = CrossGeolocator.Current;
                            //locator.DesiredAccuracy = 50;
                            //var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);

                            //_task.latitude = position.Latitude;
                            //_task.longitude = position.Longitude;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                        }
                        i = await App.Database.UpdateTaskAsync(_task);
                    }
                    else
                    {
                        i = 1;
                        await DisplayAlert("OBS!", "Opgaven er allerede markeret som udført", "OK");
                    }
                }
                UpdateItemSource();
            }
        }

        public async void ShowPDF(MaintenanceTask _task)
        {
            //try
            //{
            //    string data = await facade.PDFService.GetPDF(_task.anlæg);
            //    if (!data.Contains("NoFile"))
            //    {
            //        int i = data.Length - 2;
            //        string newdata = data.Substring(1, i);

            //        Device.OpenUri(new Uri("http://demo.biomass.eliteit.dk" + newdata));
            //    }
            //    else
            //    {
            //        await DisplayAlert("Fejl!", "Der eksisterer ingen PDF på anlæg " + _task.anlæg + ", " + _task.anlægsbeskrivelse, "OK");
            //    }

            //}
            //catch
            //{
            //    await DisplayAlert("Fejl!", "Har ingen forbindelse til NAV", "OK");
            //}
        }

        private void MakeListView()
        {
            var temp = new DataTemplate(typeof(CustomTaskCell));
            
            lv = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                HasUnevenRows = true,
                ItemTemplate = temp,
                IsPullToRefreshEnabled = true
            };

            lv.Refreshing += Lv_Refreshing;
            lv.ItemTapped += Lv_ItemTapped;

        }

        private void ShowDoneButton_Clicked(object sender, EventArgs e)
        {
            if (showDone)
            {
                showDone = false;
                showDoneButton.Text = "Vis udførte opgaver";
            }
            else
            {
                showDone = true;
                showDoneButton.Text = "Skjul udførte opgaver";
            }
            UpdateItemSource();
        }

        void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = ((ListView)sender).SelectedItem;
            MaintenanceTask tsk = (MaintenanceTask)action;

            this.Navigation.PushModalAsync(new MaintenanceDetail(tsk));
        }

        async void Lv_Refreshing(object sender, EventArgs e)
        {
            await syncFacade.MaintenanceActivitySynchronizer.SyncDatabaseWithNAV();
            syncFacade.JobRecLineSynchronizer.SyncDatabaseWithNAV();
            await syncFacade.MaintenanceTaskSynchronizer.SyncDatabaseWithNAV();

            UpdateItemSource();
            if (lv.IsRefreshing)
            {
                lv.EndRefresh();
            }
        }

        private void UpdateItemSource()
        {
            if (sb.Text != null)
            {
                SearchUpdate(sb.Text);
            }
            else
            {
                tasks = null;
                //tasks = db.GetTasksAsync().Result;
                GetData();
                if (tasks != null)
                {
                    //DateTime nullDate = new DateTime(1900, 1, 1);
                    //if (gd.SearchUserName != null && gd.SearchDateTime > nullDate && gd.SearchDateTimeLast > nullDate)
                    //{
                    //    itemssourceList = tasks.Where(x => x.planned_Date.Date >= gd.SearchDateTime.Date && x.planned_Date <= gd.SearchDateTimeLast && (x.responsible == gd.SearchUserName || x.responsible == ""));
                    //}
                    //else if (gd.SearchUserName == null && gd.SearchDateTime > nullDate && gd.SearchDateTimeLast > nullDate)
                    //{
                    //    itemssourceList = tasks.Where(x => x.planned_Date.Date >= gd.SearchDateTime.Date && x.planned_Date <= gd.SearchDateTimeLast);
                    //}
                    //else if (gd.SearchUserName != null && gd.SearchDateTime < new DateTime(1900, 1, 1))
                    //{
                    //    itemssourceList = tasks.Where(x => (x.responsible == gd.SearchUserName || x.responsible == ""));
                    //}
                    //else
                    //{
                    itemssourceList = tasks;
                    //}

                    if (showDone)
                    {
                        lv.ItemsSource = itemssourceList.Where(x => x.status == "Completed").OrderBy(x => x.no);
                    }
                    else
                    {
                        lv.ItemsSource = itemssourceList.Where(x => x.status == "Released").OrderBy(x => x.no);
                    }
                }
            }
        }
        private void GetData()
        {
            var task = Task.Run(async () => { tasks = await db.GetTasksAsync(); });
            task.Wait();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateItemSource();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            tasks = null;
        }
    }
}

