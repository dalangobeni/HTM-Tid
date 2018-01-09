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
    class JobRecLineDetail : ContentPage
    {
        List<DetailModel> list = new List<DetailModel>();
        ListView lv;

        public JobRecLineDetail(JobRecLine c)
        {
            //if (c.Job_No != null)
            //{
            //    DetailModel jobNoModel = new DetailModel()
            //    {
            //        type = "Sagsnummer",
            //        value = c.Job_No.ToString()
            //    };
            //    list.Add(jobNoModel);
            //}
            //if (c.Journal_Template_Name != null)
            //{
            //    DetailModel journalTemplateNameModel = new DetailModel()
            //    {
            //        type = "Journal Template Name",
            //        value = c.Journal_Template_Name
            //    };
            //    list.Add(journalTemplateNameModel);
            //}
            if (c.Start_Time > new DateTime(1950, 1, 1))
            {
                DetailModel postingDateModel = new DetailModel()
                {
                    type = "Registreret dato",
                    value = c.Start_Time.ToString("dd/MM/yyyy")
                };
                list.Add(postingDateModel);
            }
            //if (c.Type != null)
            //{
            //    DetailModel typeModel = new DetailModel()
            //    {
            //        type = "Type",
            //        value = c.Type
            //    };
            //    list.Add(typeModel);
            //}
            //if (c.No != null)
            //{
            //    DetailModel noModel = new DetailModel()
            //    {
            //        type = "Nummer",
            //        value = c.No.ToString()
            //    };
            //    list.Add(noModel);
            //}
            //if (c.Description != null)
            //{
            //    DetailModel descriptionModel = new DetailModel()
            //    {
            //        type = "Beskrivelse",
            //        value = c.Description
            //    };
            //    list.Add(descriptionModel);
            //}
            //DetailModel quantityModel = new DetailModel()
            //{
            //    type = "Antal",
            //    value = c.Quantity.ToString()
            //};
            //list.Add(quantityModel);
            //if (c.Unit_of_Measure_Code != null)
            //{
            //    DetailModel unitOfMeasureCodeModel = new DetailModel()
            //    {
            //        type = "Enhedskode",
            //        value = c.Unit_of_Measure_Code
            //    };
            //    list.Add(unitOfMeasureCodeModel);
            //}
            //if (c.Work_Type_Code != null)
            //{
            //    JobRecLineDetailModel workTypeCodeModel = new JobRecLineDetailModel()
            //    {
            //        type = "Arbejdstype",
            //        value = c.Work_Type_Code
            //    };
            //    list.Add(workTypeCodeModel);
            //}
            //if (c.Journal_Batch_Name != null)
            //{
            //    DetailModel journalBatchNameModel = new DetailModel()
            //    {
            //        type = "Batch Name",
            //        value = c.Journal_Batch_Name
            //    };
            //    list.Add(journalBatchNameModel);
            //}
            //if (c.Job_Task_No != null)
            //{
            //    DetailModel jobTaskNoModel = new DetailModel()
            //    {
            //        type = "Opgavenummer",
            //        value = c.Job_Task_No
            //    };
            //    list.Add(jobTaskNoModel);
            //}
            if (c.TaskNo != null)
            {
                DetailModel maintenanceTaskNoModel = new DetailModel()
                {
                    type = "Vedligeholdsopgavenummer",
                    value = c.TaskNo
                };
                list.Add(maintenanceTaskNoModel);
            }
            if (c.Status != null)
            {
                DetailModel statusModel = new DetailModel()
                {
                    type = "Status",
                    value = c.Status
                };
                list.Add(statusModel);
            }
            if (c.Work_Type != null)
            {
                DetailModel workTypeModel = new DetailModel()
                {
                    type = "Arbejdstype",
                    value = c.Work_Type
                };
                list.Add(workTypeModel);
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
