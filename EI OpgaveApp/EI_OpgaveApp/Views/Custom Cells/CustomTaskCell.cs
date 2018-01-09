using EI_OpgaveApp.Models;
using EI_OpgaveApp.Views.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Custom_Cells
{
    public class CustomTaskCell : ViewCell
    {
        Color color = Color.Default;
        public CustomTaskCell()
        {
            //SetColor();

            Label plannedDateLabel = new Label()
            {
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Label noLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label sales_order_NoLabel = new Label();
            Label customerLabel = new Label();



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
                    new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                }
            };

            mainGrid.Children.Add(noLabel, 0, 0);
            mainGrid.Children.Add(plannedDateLabel, 3, 0);
            mainGrid.Children.Add(sales_order_NoLabel, 1, 0);
            mainGrid.Children.Add(customerLabel, 1, 1);

            Grid.SetRowSpan(noLabel, 2);
            Grid.SetRowSpan(plannedDateLabel, 2);
            Grid.SetColumnSpan(customerLabel, 2);
            Grid.SetColumnSpan(sales_order_NoLabel, 2);

            mainGrid.BackgroundColor = color;
            View = mainGrid;

            noLabel.SetBinding<MaintenanceTask>(Label.TextProperty, i => i.no);
            plannedDateLabel.SetBinding(Label.TextProperty, new Binding("Shipment_Date", converter: new DateTimeToDateConverter(true)));
            sales_order_NoLabel.SetBinding<MaintenanceTask>(Label.TextProperty, i => i.Sales_Order_Or_Text);
            customerLabel.SetBinding<MaintenanceTask>(Label.TextProperty, i => i.CustomerName);
          
            mainGrid.SetBinding(Label.BackgroundColorProperty, new Binding("status", converter: new MaintenanceTaskRowColor()));


            if (Device.RuntimePlatform.Equals("iOS"))
            {
                mainGrid.Margin = 0;
            }
            else
            {
                mainGrid.Margin = 10;
            }
            //MakeCustomCell();
            CreateMenu();
        }


        private void CreateMenu()
        {
            var pdfAction = new MenuItem { Text = "Vis dokument" };
            pdfAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            pdfAction.Clicked += (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                MaintenanceTask _task = (MaintenanceTask)mi.CommandParameter;
                MaintenancePage mp = new MaintenancePage();
                mp.ShowPDF(_task);
            };

            var doneAction = new MenuItem { Text = "Udført" };
            doneAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            doneAction.Clicked += (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                MaintenanceTask _task = (MaintenanceTask)mi.CommandParameter;

                MaintenancePage mp = new MaintenancePage();
                mp.SetDone(_task);

            };
            var mapAction = new MenuItem { Text = "Kort" };
            mapAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            mapAction.Clicked += (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                MaintenanceTask _task = (MaintenanceTask)mi.CommandParameter;
                MaintenancePage mp = new MaintenancePage();
                mp.ShowOnMap(_task);
            };
            var StopTimeRegAction = new MenuItem { Text = "Stop" };
            StopTimeRegAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            StopTimeRegAction.Clicked += async (sender, e) =>
            {
                MaintenanceDetail md = new MaintenanceDetail(new MaintenanceTask());
                md.StopCurrentJobRec();
            };
            //ContextActions.Add(pdfAction);
            //ContextActions.Add(mapAction);
            ContextActions.Add(doneAction);
            ContextActions.Add(StopTimeRegAction);
        }

        void OnMore(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            //Do something here... e.g. Navigation.pushAsync(new specialPage(item.commandParameter));
            //page.DisplayAlert("More Context Action", item.CommandParameter + " more context action", 	"OK");
        }

    }
}