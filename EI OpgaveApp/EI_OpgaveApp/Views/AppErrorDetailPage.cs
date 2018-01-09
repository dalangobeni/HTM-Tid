using EI_OpgaveApp.Models;
using EI_OpgaveApp.Views.Custom_Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class AppErrorDetailPage : ContentPage
    {
        List<DetailModel> list = new List<DetailModel>();
        ListView lv;

        public AppErrorDetailPage(AppError appError)
        {
            if (appError.Type != null)
            {
                DetailModel appErrorType = new DetailModel()
                {
                    type = "Type",
                    value = appError.Type
                };
                list.Add(appErrorType);
            }
            if (appError.Description != null)
            {
                DetailModel appErrorDetail = new DetailModel()
                {
                    type = "Detaljer",
                    value = appError.Description
                };
                list.Add(appErrorDetail);
            }
            if (appError.Longitude != 0)
            {
                DetailModel appErrorLong = new DetailModel()
                {
                    type = "Længdegrad",
                    value = appError.Longitude.ToString()
                };
                list.Add(appErrorLong);
            }
            if (appError.Latitude != 0)
            {
                DetailModel appErrorLatitude = new DetailModel()
                {
                    type = "Breddegrad",
                    value = appError.Latitude.ToString()
                };
                list.Add(appErrorLatitude);
            }
            if (appError.Time_Logged != null)
            {
                DetailModel appErrorTimeLogged = new DetailModel()
                {
                    type = "Oprettet",
                    value = appError.Time_Logged.ToString("dd/MM/yyy")
                };
                list.Add(appErrorTimeLogged);
            }
            if (appError.Time_Saved != null)
            {
                DetailModel appErrorTimeSaved = new DetailModel()
                {
                    type = "Gemt i NAV",
                    value = appError.Time_Saved.ToString("dd/MM/yyyy")
                };
                list.Add(appErrorTimeSaved);
            }
            if (appError.Fixed_Asset != null)
            {
                DetailModel appErrorFixedAsset = new DetailModel()
                {
                    type = "Anlæg",
                    value = appError.Fixed_Asset
                };
                list.Add(appErrorFixedAsset);
            }
            if (appError.User != null)
            {
                DetailModel appErrorUser = new DetailModel()
                {
                    type = "Bruger",
                    value = appError.User
                };
                list.Add(appErrorUser);
            }
            if (appError.Status != null)
            {
                DetailModel appErrorStatus = new DetailModel()
                {
                    type = "Status",
                    value = appError.Status
                };
                list.Add(appErrorStatus);
            }

            Application.Current.Properties["gridrowindex"] = 1;
            var temp = new DataTemplate(typeof(CustomTaskDetailCell));

            lv = new ListView()
            {
                HasUnevenRows = true,
                ItemTemplate = temp
            };

            lv.ItemsSource = list;

            Button button = new Button() { Text = "Tilbage", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            button.Clicked += Button_Clicked;
            StackLayout layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        lv,
                        button
                    }
            };

            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = layout;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
