using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using EI_OpgaveApp.Views.Custom_Cells;
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

namespace EI_OpgaveApp.Views
{
    public class MaintenanceDetail : ContentPage
    {
        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        GlobalData gd = GlobalData.GetInstance;

        ListView lv;
        Grid grid;
        Grid gridInfo;

        List<MaintenanceActivity> itemssourceList;
        List<MaintenanceActivity> activityList;
        List<JobRecLine> jobList;
        List<JobRecLine> jobItemsSourceList;

        MaintenanceTask taskGlobal;
        MaintenanceActivity _activity;
        FixedAsset fixedAsset;

        Button doneButton;
        Button jobLineButton;
        Button pdfButton;
        Button backButton;
        Button doneActButton;
        Button addNotesButton;
        Button cameraButton;
        Button stopCurrentJobRecButton;

        Label asset;
        Label type;
        Label text;
        Label assetDescription;
        Label header;

        StackLayout main;

        string responsibleString;

        bool showingDone = false;
        public MaintenanceDetail(MaintenanceTask task)
        {
            taskGlobal = task;

            doneButton = new Button() { BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            jobLineButton = new Button() { Text = "Tidsregistrering", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            pdfButton = new Button() { Text = "Se sendte tidsreg.", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            backButton = new Button() { Text = "Tilbage", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            doneActButton = new Button() { Text = "Udført", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            addNotesButton = new Button() { Text = "Tilføj notat", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            cameraButton = new Button() { Text = "Tilføj billede", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };
            stopCurrentJobRecButton = new Button() { Text = "Stop igangværende opgave", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White };

            if (task.responsible == "")
            {
                responsibleString = "Ansvarlig: Ingen ansvarlig";
            }
            else
            {
                responsibleString = "Ansvarlig: " + task.responsible;
            }
            asset = new Label()
            {
                Text = "Ordrenummer: " + task.Sales_Order_No,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            assetDescription = new Label()
            {
                Text = "Beskrivelse: " + task.text,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,//TextColor = Color.FromRgb(135, 206, 250),
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            type = new Label()
            {
                Text = "Kunde: " + task.CustomerName,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,//TextColor = Color.FromRgb(135, 206, 250),
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            text = new Label()
            {
                Text = responsibleString,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,//TextColor = Color.FromRgb(135, 206, 250),
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            header = new Label()
            {
                Text = "Opgave nr. " + task.no,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,//TextColor = Color.FromRgb(135, 206, 250),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold
            };

            if (task.status == "Completed")
            {
                doneButton.IsEnabled = false;
                doneButton.Text = "Udført";
                doneButton.BackgroundColor = Color.FromRgb(205, 201, 201);
            }
            else
            {
                doneButton.Text = "Sæt til udført";
            }

            doneButton.Clicked += DoneButton_Clicked;
            jobLineButton.Clicked += MapButton_Clicked;
            pdfButton.Clicked += PdfButton_Clicked;
            backButton.Clicked += BackButton_Clicked;
            addNotesButton.Clicked += AddNotesButton_Clicked;
            cameraButton.Clicked += CameraButton_Clicked;
            stopCurrentJobRecButton.Clicked += StopCurrentJobRecButton_Clicked;

            GetFixedAsset();
            MakeList();
            MakeGrid();
            main = new StackLayout
            {
                Children =
                {
                    gridInfo,
                    lv,
                    grid
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

        private void StopCurrentJobRecButton_Clicked(object sender, EventArgs e)
        {
            StopCurrentJobRec();
        }

        public async void StopCurrentJobRec()
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
        }

        private void GetFixedAsset()
        {
            //fixedAsset = await db.GetAFixedAssetAsync(taskGlobal.anlæg);
            if (fixedAsset != null)
            {
                asset.Text = "Anlæg: " + fixedAsset.Description;
            }
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

            PictureModel pic = new PictureModel()
            {
                Picture = picture,

                TaskGuid = taskGlobal.TaskGUID
            };
            await App.Database.SavePictureAsync(pic);
            //await pdf.PostPicture(pic, _activity.UniqueID);
        }

        private void AddNotesButton_Clicked(object sender, EventArgs e)
        {
            string placeholder = "Indtast notat";
            if (taskGlobal.AppNotes != "")
            {
                placeholder = taskGlobal.AppNotes;
            }
            Entry entry = new Entry()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Placeholder = placeholder,
                PlaceholderColor = Color.Gray
            };
            DisableButtons();
            Button btn = new Button() { Text = "OK", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            Button btn2 = new Button() { Text = "Cancel", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            main.Children.Remove(lv);
            main.Children.Remove(grid);

            main.Children.Add(entry);
            main.Children.Add(btn);
            main.Children.Add(btn2);
            main.Children.Add(grid);
            entry.Focus();
            btn.Clicked += async (s, b) =>
            {
                taskGlobal.AppNotes = entry.Text;
                main.Children.Remove(entry);
                main.Children.Remove(btn);
                main.Children.Remove(grid);
                main.Children.Remove(btn2);
                main.Children.Add(lv);
                main.Children.Add(grid);
                await App.Database.UpdateTaskAsync(taskGlobal);
                //PopulateDetailList();
                UpdateItemsSource();
                EnableButtons();
            };
            btn2.Clicked += (s, b) =>
            {
                main.Children.Remove(entry);
                main.Children.Remove(btn);
                main.Children.Remove(btn2);
                main.Children.Remove(grid);
                main.Children.Add(lv);
                main.Children.Add(grid);
                UpdateItemsSource();
                //PopulateDetailList();
                EnableButtons();
            };

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void PdfButton_Clicked(object sender, EventArgs e)
        {
            PopulateDetailList();
            //UpdateItemsSource();
            //try
            //{
            //    string data = await facade.PDFService.GetPDF(taskGlobal.anlæg);
            //    if (!data.Contains("NoFile"))
            //    {
            //        int i = data.Length - 2;
            //        string newdata = data.Substring(1, i);

            //        Device.OpenUri(new Uri("http://vedligehold.biomass.eliteit.dk/" + newdata));
            //    }
            //    else
            //    {
            //        await DisplayAlert("Fejl!", "Der eksisterer ingen PFD på anlæg " + taskGlobal.anlæg + ", " + taskGlobal.anlægsbeskrivelse, "OK");
            //    }

            //}
            //catch
            //{
            //    await DisplayAlert("Fejl!", "Kunne ikke hente PDF", "OK");
            //}
        }

        private void MapButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new JobRecLineForm(taskGlobal));

            //Navigation.PushModalAsync(new TimeRegistrationPage(taskGlobal));
            //if (taskGlobal.longitude != 0 && taskGlobal.latitude != 0)
            //{
            //    string s = "https://www.google.dk/maps/place/" + taskGlobal.latitude + "," + taskGlobal.longitude + "/" + taskGlobal.latitude + "," + taskGlobal.longitude + "z/";
            //    Uri uri = new Uri(s);
            //    Device.OpenUri(uri);
            //}
            //else
            //{
            //    DisplayAlert("Ingen koordinater", "Der er ingen koordinater på opgaven. Bekræft at opgaven er afsluttet, og prøv igen.", "OK");
            //}
        }

        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Færdig", "Vil du sætte opgaven til færdig?", "Ja", "Nej");
            if (response)
            {
                int i = 0;
                while (i == 0)
                {
                    DisableButtons();
                    if (taskGlobal.status == "Released")
                    {
                        //taskGlobal.status = "Completed";
                        //try
                        //{
                        //    var locator = CrossGeolocator.Current;
                        //    locator.DesiredAccuracy = 50;
                        //    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                        //    taskGlobal.latitude = position.Latitude;
                        //    taskGlobal.longitude = position.Longitude;

                        //}
                        //catch (Exception ex)
                        //{
                        //    Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                        //}
                    }

                    i = await App.Database.UpdateTaskAsync(taskGlobal);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                EnableButtons();
            }
        }

        private void MakeGrid()
        {
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
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            gridInfo = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                },
                //ColumnSpacing = 10,
                //RowSpacing = 10
            };
            if (taskGlobal.status == "Completed")
            {
                gridInfo.BackgroundColor = Color.FromRgb(135, 206, 250);
            }
            else
            {
                gridInfo.BackgroundColor = Color.FromRgb(205, 201, 201);
                header.TextColor = Color.Black;
                asset.TextColor = Color.Black;
                assetDescription.TextColor = Color.Black;
                type.TextColor = Color.Black;
                text.TextColor = Color.Black;
            }
            gridInfo.Margin = 10;

            grid.Children.Add(doneButton, 0, 3);
            Grid.SetColumnSpan(doneButton, 2);
            grid.Children.Add(stopCurrentJobRecButton, 0, 2);
            Grid.SetColumnSpan(stopCurrentJobRecButton, 2);

            grid.Children.Add(addNotesButton, 0, 0);
            grid.Children.Add(jobLineButton, 1, 0);
            grid.Children.Add(backButton, 1, 1);
            grid.Children.Add(cameraButton, 0, 1);

            gridInfo.Children.Add(header, 0, 0);
            Grid.SetColumnSpan(header, 2);
            gridInfo.Children.Add(asset, 0, 1);
            gridInfo.Children.Add(assetDescription, 0, 2);
            gridInfo.Children.Add(type, 1, 1);
            gridInfo.Children.Add(text, 1, 2);

        }
        private void MakeList()
        {
            //var temp = new DataTemplate(typeof(CustomCaseCell));

            var temp = new DataTemplate(typeof(CustomActivityCell));
            lv = new ListView();

            lv.HasUnevenRows = true;
            lv.ItemTemplate = temp;

            lv.ItemTapped += Lv_ItemTapped;
            PopulateDetailList();
            //UpdateItemsSource();
        }

        private void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var action = ((ListView)sender).SelectedItem;
            //JobRecLine c = (JobRecLine)action;

            //this.Navigation.PushModalAsync(new JobRecLineDetail(c));
            var action = ((ListView)sender).SelectedItem;
            _activity = (MaintenanceActivity)action;
            MaintenanceActivity tempAct = _activity;
            Entry entry = new Entry()
            {
                Keyboard = Keyboard.Numeric,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Placeholder = "Indtast aflæsning",
                PlaceholderColor = Color.Gray,
            };

            DisableButtons();
            Button btn = new Button() { Text = "OK", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            Button btn2 = new Button() { Text = "Cancel", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.End };
            main.Children.Remove(lv);
            main.Children.Remove(grid);

            main.Children.Add(entry);
            main.Children.Add(cameraButton);
            main.Children.Add(btn);
            main.Children.Add(btn2);
            main.Children.Add(grid);
            entry.Focus();
            btn.Clicked += async (s, b) =>
            {
                if (entry.Text == null)
                {
                    await DisplayAlert("Advarsel", "Der skal skrives en værdi i aflæsningsfeltet", "OK");
                }
                else
                {
                    int i = 0;
                    while (i == 0)
                    {
                        btn.IsEnabled = false;
                        btn2.IsEnabled = false;
                        cameraButton.IsEnabled = false;
                        try
                        {
                            var locator = CrossGeolocator.Current;
                            locator.DesiredAccuracy = 50;
                            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                            tempAct.Latitude = position.Latitude;
                            tempAct.Longitude = position.Longitude;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                        }
                        tempAct.Done = true;

                        tempAct.Reading = int.Parse(entry.Text);

                        tempAct.DoneTime = DateTime.UtcNow;

                        main.Children.Remove(entry);
                        main.Children.Remove(cameraButton);
                        main.Children.Remove(btn);
                        main.Children.Remove(grid);
                        main.Children.Remove(btn2);
                        main.Children.Add(lv);
                        main.Children.Add(grid);
                        i = await App.Database.UpdateActivityAsync(tempAct);
                        PopulateDetailList();
                        EnableButtons();
                    }
                    btn.IsEnabled = true;
                    btn2.IsEnabled = true;
                    cameraButton.IsEnabled = true;
                }
            };
            btn2.Clicked += (s, b) =>
            {
                main.Children.Remove(entry);
                main.Children.Remove(cameraButton);
                main.Children.Remove(btn);
                main.Children.Remove(btn2);
                main.Children.Remove(grid);
                main.Children.Add(lv);
                main.Children.Add(grid);
                PopulateDetailList();
                EnableButtons();
            };

            //App.Database.UpdateActivityAsync(_activity);
            //PopulateDetailList();
            //entryEdit.Text = _activity.Activity_Description;
            //reading.Text = _activity.Reading.ToString();

            //detail = new StackLayout
            //{
            //    Children = {
            //        entryEdit,
            //        reading,
            //        cancelEdit,
            //        doneActButton
            //    }
            //};

            //Content = detail;
        }
        private async void PopulateDetailList()
        {
            activityList = null;
            activityList = await App.Database.GetAcitivitiesAsync();
            if (activityList != null)
            {
                List<MaintenanceActivity> tempList = activityList.Where(x => x.TaskGUID == taskGlobal.TaskGUID).ToList();

                lv.ItemsSource = tempList.OrderBy(x => x.Task_Activity_No);
            }

        }
        private async void UpdateItemsSource()
        {
            jobList = null;


            jobList = await App.Database.GetJobRecLinesAsync();

            if (jobList != null)
            {
                List<JobRecLine> jsl = jobList.Where(x => x.TaskNo == taskGlobal.no.ToString()).ToList();
                if (jsl != null)
                {
                    try
                    {
                        //jobItemsSourceList = jsl.Where(x => x.No == gd.CurrentResource.No && x.Sent == false).ToList();

                        if (showingDone)
                        {
                            lv.ItemsSource = jsl.Where(x => x.Resource_Person == gd.CurrentResource.No && x.Sent == false).ToList();
                            showingDone = false;
                            pdfButton.Text = "Vis sendte tidsreg.";
                        }
                        else
                        {
                            lv.ItemsSource = jsl.Where(x => x.Resource_Person == gd.CurrentResource.No).ToList();
                            showingDone = true;
                            pdfButton.Text = "Fjern sendte tidsreg.";
                        }
                        // lv.ItemsSource = jobItemsSourceList;
                    }
                    catch { }
                }
            }
        }

        private void DisableButtons()
        {
            doneButton.IsEnabled = false;
            jobLineButton.IsEnabled = false;
            pdfButton.IsEnabled = false;
            backButton.IsEnabled = false;
            doneActButton.IsEnabled = false;
            addNotesButton.IsEnabled = false;
        }
        private void EnableButtons()
        {
            doneButton.IsEnabled = true;
            jobLineButton.IsEnabled = true;
            pdfButton.IsEnabled = true;
            backButton.IsEnabled = true;
            addNotesButton.IsEnabled = true;
            if (taskGlobal.status == "Completed")
            {
                doneButton.IsEnabled = false;
                doneButton.Text = "Udført";
                doneButton.BackgroundColor = Color.FromRgb(205, 201, 201);

            }
            else
            {
                doneButton.Text = "Sæt til udført";
            }
        }

        protected override void OnAppearing()
        {
            PopulateDetailList();
        }
    }
}
