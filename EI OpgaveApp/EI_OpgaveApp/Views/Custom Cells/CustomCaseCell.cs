using EI_OpgaveApp.Models;
using EI_OpgaveApp.Views.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Custom_Cells
{
    public class CustomCaseCell : ViewCell
    {
        Color color = Color.Default;
        public CustomCaseCell()
        {
            Label type = new Label();
            Label WorkType = new Label();
            Label Date = new Label();


            Label anlægsbeskrivelse = new Label()
            {
                //FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                //HorizontalOptions = LayoutOptions.End
            };
            Image colorImg = new Image()
            {
                VerticalOptions = LayoutOptions.End
            };

            Label done = new Label()
            {
                //HorizontalOptions = LayoutOptions.CenterAndExpand,
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Label image = new Label()
            {
                HorizontalOptions = LayoutOptions.Start,
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Grid mainGrid = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star) }
                }
            };

            mainGrid.Children.Add(image, 1, 0);
            //Grid.SetRowSpan(image, 2);
            mainGrid.Children.Add(WorkType, 2, 0);
            mainGrid.Children.Add(type, 3, 0);
            mainGrid.Children.Add(anlægsbeskrivelse, 0, 0);
            mainGrid.Children.Add(Date, 3, 1);
            //Grid.SetColumnSpan(Date, 4);

            //mainGrid.Children.Add(done, 4, 0);
            //Grid.SetRowSpan(done, 2);
            //Grid.SetColumnSpan(type, 2);
            mainGrid.BackgroundColor = color;
            View = mainGrid;

            //image.SetBinding<JobRecLine>(Label.TextProperty, i => i.Unit_of_Measure_Code);
            WorkType.SetBinding<JobRecLine>(Label.TextProperty, i => i.Work_Type);
            //type.SetBinding<JobRecLine>(Label.TextProperty, i => i.Description);
            //anlægsbeskrivelse.SetBinding<JobRecLine>(Label.TextProperty, i => i.Quantity);
            Date.SetBinding(Label.TextProperty, new Binding("Posting_Date", converter: new DateTimeToDateConverter(false)));
            //done.SetBinding<JobRecLine>(Label.TextProperty, i => i.Journal_Batch_Name);
            mainGrid.BackgroundColor = Color.FromRgb(205, 201, 201);

            if (Device.RuntimePlatform.Equals("iOS"))
            {
                mainGrid.Margin = 0;
            }
            else
            {
                mainGrid.Margin = 10;
            }
            CreateMenu();
        }
        private void CreateMenu()
        {
            var moreAction = new MenuItem { Text = "Rediger" };
            moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            moreAction.Clicked += (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                JobRecLine _recLine = (JobRecLine)mi.CommandParameter;
                MessagingCenter.Send<JobRecLine>(_recLine, "hi");
                //(new JobRecLineUpdateForm(_recLine));

            };

            var SendAction = new MenuItem { Text = "Send" };
            SendAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            SendAction.Clicked += (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                
                JobRecLine _recLine = (JobRecLine)mi.CommandParameter;
                _recLine.Sent = true;
                _recLine.Edited = true;
                App.Database.UpdateJobRecLineAsync(_recLine);
            };
            this.ContextActions.Add(moreAction);
            this.ContextActions.Add(SendAction);
        }
    }
}