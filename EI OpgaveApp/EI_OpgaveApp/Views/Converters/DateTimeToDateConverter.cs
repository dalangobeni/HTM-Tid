using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Views.Converters
{
    class DateTimeToDateConverter : IValueConverter
    {
        bool _withYear;
        public DateTimeToDateConverter(bool withYear)
        {
            _withYear = withYear;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = new DateTime();
            string time = "-";
            if (value is DateTime)
            {
                dt = (DateTime)value;
                if (dt > new DateTime(1900, 1, 1))
                {
                    if (_withYear)
                    {
                        time = dt.ToString("dd/MM/yy");
                    }
                    else
                    {
                        time = dt.ToString("dd/MM");
                    }
                }
            }

            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
