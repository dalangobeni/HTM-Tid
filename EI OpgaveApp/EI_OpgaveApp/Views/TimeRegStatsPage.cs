using EI_OpgaveApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EI_OpgaveApp.Views
{
    public class TimeRegStatsPage : ContentPage
    {
        Grid statGrid;

        Label userLabel;
        Label monthLabel;
        Label weekLabel;
        Label mondayLabel;
        Label tuesdayLabel;
        Label wednesdayLabel;
        Label thursdayLabel;
        Label fridayLabel;
        Label saturdayLabel;
        Label sundayLabel;
        Label billableLabel;
        Label monthValueLabel;
        Label weekValueLabel;
        Label mondayValueLabel;
        Label tuesdayValueLabel;
        Label wednesdayValueLabel;
        Label thursdayValueLabel;
        Label fridayValueLabel;
        Label saturdaValueyLabel;
        Label sundayValueLabel;
        Label billableValueLabel;

        Color buttonColor = Color.FromRgb(135, 206, 250);

        TimeRegStat statGlobal;

        Button backButton;
        string month;
        public TimeRegStatsPage(TimeRegStat stat)
        {
            statGlobal = stat;
            backButton = new Button { Text = "Tilbage", BackgroundColor = Color.White, TextColor = buttonColor, VerticalOptions = LayoutOptions.EndAndExpand };
            backButton.Clicked += BackButton_Clicked; ;
            month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(stat.Month);
            month = UppercaseFirst(month);
            BackgroundColor = Color.FromRgb(135, 206, 250);

            MakeLabels();
            MakeGrid();
            Content = new StackLayout
            {
                Children = {
                    statGrid,
                    backButton
                }
            };
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void MakeLabels()
        {
            userLabel = new Label()
            {
                Text = statGlobal.User,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };
            monthLabel = new Label()
            {
                Text = month,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            weekLabel = new Label()
            {
                Text = "Uge " + statGlobal.Week.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            mondayLabel = new Label()
            {
                Text = "Mandag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            tuesdayLabel = new Label()
            {
                Text = "Tirsdag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            wednesdayLabel = new Label()
            {
                Text = "Onsdag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            thursdayLabel = new Label()
            {
                Text = "Torsdag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            fridayLabel = new Label()
            {
                Text = "Fredag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            saturdayLabel = new Label()
            {
                Text = "Lørdag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            sundayLabel = new Label()
            {
                Text = "Søndag",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            monthValueLabel = new Label()
            {
                Text = statGlobal.MonthValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            weekValueLabel = new Label()
            {
                Text = statGlobal.WeekValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            mondayValueLabel = new Label()
            {
                Text = statGlobal.MondayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            tuesdayValueLabel = new Label()
            {
                Text = statGlobal.TuesdayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            wednesdayValueLabel = new Label()
            {
                Text = statGlobal.WednesdayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            thursdayValueLabel = new Label()
            {
                Text = statGlobal.ThursdayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            fridayValueLabel = new Label()
            {
                Text = statGlobal.FridayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            saturdaValueyLabel = new Label()
            {
                Text = statGlobal.SaturdayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            sundayValueLabel = new Label()
            {
                Text = statGlobal.SundayValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            billableLabel = new Label()
            {
                Text = "Fakturerbare timer (måned)",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
            billableValueLabel = new Label()
            {
                Text = statGlobal.BillableHoursValue.ToString(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.White,
                TextColor = Color.White,
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 14,
                //FontAttributes = FontAttributes.Bold
            };
        }

        private void MakeGrid()
        {
            statGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                }
            };
            statGrid.Children.Add(userLabel, 0, 0);
            Grid.SetColumnSpan(userLabel, 2);

            statGrid.Children.Add(monthLabel, 0, 1);
            statGrid.Children.Add(monthValueLabel, 1, 1);
            statGrid.Children.Add(weekLabel, 0, 2);
            statGrid.Children.Add(weekValueLabel, 1, 2);
            statGrid.Children.Add(mondayLabel, 0, 3);
            statGrid.Children.Add(mondayValueLabel, 1, 3);
            statGrid.Children.Add(tuesdayLabel, 0, 4);
            statGrid.Children.Add(tuesdayValueLabel, 1, 4);
            statGrid.Children.Add(wednesdayLabel, 0, 5);
            statGrid.Children.Add(wednesdayValueLabel, 1, 5);
            statGrid.Children.Add(thursdayLabel, 0, 6);
            statGrid.Children.Add(thursdayValueLabel, 1, 6);
            statGrid.Children.Add(fridayLabel, 0, 7);
            statGrid.Children.Add(fridayValueLabel, 1, 7);
            statGrid.Children.Add(saturdayLabel, 0, 8);
            statGrid.Children.Add(saturdaValueyLabel, 1, 8);
            statGrid.Children.Add(sundayLabel, 0, 9);
            statGrid.Children.Add(sundayValueLabel, 1, 9);
            statGrid.Children.Add(billableLabel, 0, 10);
            statGrid.Children.Add(billableValueLabel, 1, 10);


            statGrid.BackgroundColor = Color.FromRgb(135, 206, 250);
            statGrid.Padding = 20;
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}