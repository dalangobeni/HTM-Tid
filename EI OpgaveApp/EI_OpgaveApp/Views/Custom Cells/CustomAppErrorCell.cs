using EI_OpgaveApp.Views.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Custom_Cells
{
    class CustomAppErrorCell : ViewCell
    {
        Color color = Color.Default;

        public CustomAppErrorCell()
        {
            Label descriptionLabel = new Label();
            Label timeLoggedLabel = new Label();

            Grid mainGrid = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                }
            };

            mainGrid.Children.Add(descriptionLabel, 0, 0);
            mainGrid.Children.Add(timeLoggedLabel, 1, 0);
         

            View = mainGrid;

            descriptionLabel.SetBinding(Label.TextProperty, "Description");
            timeLoggedLabel.SetBinding(Label.TextProperty, "Time_Logged", converter: new DateTimeToDateConverter(true));

            mainGrid.SetBinding(Label.BackgroundColorProperty, new Binding("Status", converter: new AppErrorStatusToColorConverter()));
            if (Device.RuntimePlatform.Equals("iOS"))
            {
                mainGrid.Margin = 0;
            }
            else
            {
                mainGrid.Margin = 10;
            }
        }
    }
}