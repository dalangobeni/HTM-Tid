using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Converters
{
    class AppErrorStatusToColorConverter : IValueConverter
    {
        Color color;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                switch (value)
                {
                    case "New":
                        color = Color.FromHex("#de3242");
                        break;
                    case "Checked":
                        color = Color.FromHex("#66cdaa");
                        break;
                    default:
                        color = Color.Default;
                        break;
                }
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
