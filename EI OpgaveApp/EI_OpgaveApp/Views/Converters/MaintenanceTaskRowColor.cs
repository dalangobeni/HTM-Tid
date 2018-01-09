using System;
using System.Globalization;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Converters
{
    class MaintenanceTaskRowColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Color.FromRgb(205, 201, 201);
            if (value is string)
            {
                string s = (string)value;
                if (s == "Completed")
                {
                    color = Color.FromRgb(135, 206, 250);
                }
                //if (s == "Released")
                //{
                //    color = Color.FromRgb(205, 201, 201);
                //}
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
