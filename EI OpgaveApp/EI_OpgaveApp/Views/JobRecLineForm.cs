using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class JobRecLineForm : ContentPage
    {
        StackLayout layout;

        WorkType workType = null;
        Resources resourceMachine = null;
        Button cancel;
        Button start;

        Picker workTypePicker;
        Picker resourceMachinePicker;

        DatePicker datePicker;

        JobRecLine recLine;
        MaintenanceTask taskGlobal;
        GlobalData gd = GlobalData.GetInstance;
        MaintenanceDatabase db = App.Database;
        public JobRecLineForm(MaintenanceTask task)
        {
            recLine = new JobRecLine();
            taskGlobal = task;

            workTypePicker = new Picker() { VerticalOptions = LayoutOptions.Start };
            workTypePicker.Title = "Vælg arbejdstype";
            workTypePicker.SelectedIndexChanged += WorkTypePicker_SelectedIndexChanged;
            resourceMachinePicker = new Picker() { VerticalOptions = LayoutOptions.Start };
            resourceMachinePicker.Title = "Vælg Maskine";
            resourceMachinePicker.SelectedIndexChanged += ResourceMachinePicker_SelectedIndexChanged;
            SetItemssources();
            start = new Button { Text = "Start opgave", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.Start };
            cancel = new Button { Text = "Cancel", BackgroundColor = Color.FromRgb(135, 206, 250), TextColor = Color.White, VerticalOptions = LayoutOptions.Start };

            datePicker = new DatePicker()
            {
                Format = "D",
                Date = DateTime.Today
            };
            start.Clicked += Start_Clicked;
            cancel.Clicked += Cancel_Clicked;
            layout = new StackLayout
            {
                Children =
                    {
                        //datePicker,
                        workTypePicker,
                        resourceMachinePicker,
                        start,
                        cancel
                    }
            };
            layout.VerticalOptions = LayoutOptions.Center;
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                // move layout under the status bar
                layout.Padding = new Thickness(0, 20, 0, 0);
            }
            Content = layout;
        }

        private void WorkTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int index = workTypePicker.SelectedIndex;

            if (index != -1)
            {
                WorkType item = (WorkType)picker.ItemsSource[index];
                workType = item;
            }
            //else if (picker.)
            else
            {
                resourceMachinePicker.SelectedIndex = -1;
            }
        }

        private void ResourceMachinePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int index = resourceMachinePicker.SelectedIndex;

            if (index != -1 && index != 0)
            {
                Resources item = (Resources)picker.ItemsSource[index];
                resourceMachine = item;
            }
            else
            {
                resourceMachinePicker.SelectedIndex = -1;
            }
        }

        private async void SetItemssources()
        {
            List<Resources> resources = await db.GetResourcesAsync();
            resources.OrderBy(x => x.Name);
            if (resources != null)
            {
                resources.Insert(0, new Resources() { No = "", Name = "" });
            };
            resourceMachinePicker.ItemsSource = resources;
            resourceMachinePicker.ItemDisplayBinding = new Binding("Name");



            List<WorkType> workTypes = await db.GetWorkTypesAsync();
            List<WorkType> workTypeArray = new List<WorkType>();
            foreach (var item in workTypes)
            {
                workTypeArray.Add(item);
            }
            workTypeArray.OrderBy(x => x.Code);
            workTypePicker.ItemsSource = workTypeArray;
            workTypePicker.ItemDisplayBinding = new Binding("Code_And_Description");


        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Start_Clicked(object sender, EventArgs e)
        {
            HandleElements();
            if (workType == null)
            {
                await DisplayAlert("Fejl!", "Du skal en arbejdstype", "OK");
            }
            else
            {
                CheckForRunningRecLine();

                ResourcePerson resourcePerson = null;
                List<ResourcePerson> rl = await db.GetResourcePersonsAsync();
                resourcePerson = rl.Where(x => x.Name == gd.User.Name).FirstOrDefault();
                SetRecLineValues(resourcePerson.No);

                await db.SaveJobRecLineAsync(recLine);
                await Navigation.PopModalAsync();
            }
            HandleElements();
        }

        private async void CheckForRunningRecLine()
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
        }

        private void SetRecLineValues(string no)
        {
            recLine.TimeRegGUID = Guid.NewGuid();
            recLine.New = true;
            recLine.TaskNo = taskGlobal.no.ToString();
            recLine.Start_Time = DateTime.UtcNow;
            recLine.TaskGUID = taskGlobal.TaskGUID;
            recLine.Posting_Date = DateTime.Today;

            recLine.Resource_Person = no;
            if (resourceMachine != null)
            {
                recLine.Resource_Machine = resourceMachine.No;
            }
            recLine.Work_Type = workType.Code;
            recLine.IsRunning = true;
        }

        private void HandleElements()
        {
            //if (done.IsEnabled)
            //{
            //    datePicker.IsEnabled = false;
            //    workTypeButton.IsEnabled = false;
            //    descriptionEntry.IsEnabled = false;
            //    amount.IsEnabled = false;
            //    done.IsEnabled = false;
            //    cancel.IsEnabled = false;
            //}
            //else
            //{
            //    datePicker.IsEnabled = true;
            //    workTypeButton.IsEnabled = true;
            //    descriptionEntry.IsEnabled = true;
            //    amount.IsEnabled = true;
            //    done.IsEnabled = true;
            //    cancel.IsEnabled = true;
            //}
        }
    }
}
