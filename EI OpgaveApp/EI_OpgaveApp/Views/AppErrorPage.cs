using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Synchronizers;
using EI_OpgaveApp.Views.Custom_Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class AppErrorPage : ContentPage
    {
        StackLayout main;

        ListView lv;

        Button createErrorButton;
        Button cancelButton;

        List<AppError> appErrorList;

        Color buttonColor = Color.FromRgb(135, 206, 250);

        SynchronizerFacade syncFacade = SynchronizerFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        GlobalData gd = GlobalData.GetInstance;
        public AppErrorPage()
        {
            createErrorButton = new Button { Text = "Opret Fejl", BackgroundColor = buttonColor, TextColor = Color.White };
            createErrorButton.Clicked += CreateErrorButton_Clicked;
            cancelButton = new Button { Text = "Tilbage", BackgroundColor = buttonColor, TextColor = Color.White };
            cancelButton.Clicked += CancelButton_Clicked;

            MakeListView();
            main = new StackLayout
            {
                Children =
                {
                    lv,
                    createErrorButton,
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
        }

        private void CreateErrorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AppErrorFormPage());
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
      
        private void MakeListView()
        {
            var temp = new DataTemplate(typeof(CustomAppErrorCell));
            lv = new ListView();

            lv.HasUnevenRows = true;
            lv.ItemTemplate = temp;

            lv.ItemTapped += Lv_ItemTapped;
            UpdateItemsSource();
        }

        private void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = ((ListView)sender).SelectedItem;
            AppError appError = (AppError)action;

            this.Navigation.PushModalAsync(new AppErrorDetailPage(appError));
        }
        private void UpdateItemsSource()
        {
            appErrorList = null;
            GetData();
            if (appErrorList != null)
            {
                lv.ItemsSource = appErrorList;
            }
        }
        private void GetData()
        {
            var task = Task.Run(async () => { appErrorList = await db.GetAppErrorsAsync(); });
            task.Wait();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateItemsSource();
        }
    }
}