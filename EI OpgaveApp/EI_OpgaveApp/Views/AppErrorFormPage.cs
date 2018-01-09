using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using Plugin.Geolocator;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections;

namespace EI_OpgaveApp.Views
{
    public class AppErrorFormPage : ContentPage
    {
        StackLayout layout;

        Entry typeEntry;
        Entry descriptionEntry;

        Picker assetPicker;

        Button done;
        Button cancel;
        Button cameraButton;

        AppError appError;
        AppErrorPicture pic;

        List<FixedAsset> assetList;

        GlobalData gd = GlobalData.GetInstance;
        MaintenanceDatabase db = App.Database;
        public AppErrorFormPage()
        {
            done = new Button { Text = "OK", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            cancel = new Button { Text = "Cancel", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.EndAndExpand };
            cameraButton = new Button { Text = "Tilføj billede", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };

            typeEntry = new Entry() { Placeholder = "Indtast fejltype..." };
            descriptionEntry = new Entry() { Placeholder = "Indtast beskrivelse..." };

            assetPicker = new Picker();

            done.Clicked += Done_Clicked;
            cancel.Clicked += Cancel_Clicked;
            cameraButton.Clicked += CameraButton_Clicked;
            layout = new StackLayout
            {
                Padding = 10,
                Children =
                {
                    typeEntry,
                    descriptionEntry,
                    assetPicker,
                    done,
                    cameraButton,
                    cancel
                }
            };
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = layout;
        }
        
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.png",
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
            });

            if (file == null)
                return;

            Byte[] ba;

            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                ba = memoryStream.ToArray();
            }
            string picture = Convert.ToBase64String(ba);
            pic = new AppErrorPicture()
            {
                Picture = picture
            };
            //await pdf.PostPicture(pic, _activity.UniqueID);
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            HandleElements();
            appError = new AppError();
            if (typeEntry.Text != null)
            {
                appError.Type = typeEntry.Text;
            }
            if (descriptionEntry == null)
            {
                await DisplayAlert("Fejl!", "Venligst indtast beskrivelse", "OK");
                return;
            }
            else
            {
                appError.Description = descriptionEntry.Text;
            }
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                appError.Latitude = position.Latitude;
                appError.Longitude = position.Longitude;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
            appError.Guid = Guid.NewGuid();
            appError.Status = "New";
            appError.Time_Logged = DateTime.UtcNow;
            appError.User = GlobalData.GetInstance.User.No;
            appError.Sent = false;
            appError.New = true;
            appError.Fixed_Asset = assetList.Where(x => x.Description == assetPicker.SelectedItem.ToString()).FirstOrDefault().No;
            await db.SaveAppErrorAsync(appError);
            if (pic != null)
            {
                pic.AppErrorGuid = appError.Guid;
                await App.Database.SaveAppErrorPicture(pic);
            }
            await Navigation.PopModalAsync();
            HandleElements();
        }

        protected override void OnAppearing()
        {
            GetFixedAssets();
        }

        private async void GetFixedAssets()
        {
            assetList = await db.GetFixedAssets();
            List<string> _list = new List<string>();
            foreach (var item in assetList)
            {
                _list.Add(item.Description);
            }
            assetPicker.ItemsSource = _list;
        }
        private void HandleElements()
        {
            if (done.IsEnabled)
            {
                typeEntry.IsEnabled = false;
                descriptionEntry.IsEnabled = false;
                cameraButton.IsEnabled = false;
                done.IsEnabled = false;
                cancel.IsEnabled = false;
            }
            else
            {
                typeEntry.IsEnabled = true;
                descriptionEntry.IsEnabled = true;
                cameraButton.IsEnabled = true;
                done.IsEnabled = true;
                cancel.IsEnabled = true;
            }
        }
    }
}
