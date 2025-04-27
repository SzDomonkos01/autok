using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace autok
{
    public class Lejarat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly datum)
            {
                DateOnly ma = DateOnly.FromDateTime(DateTime.Today);
                int kulonbseg = datum.DayNumber - ma.DayNumber;

                if (kulonbseg < 0)
                {
                    return Brushes.Red;
                }
                else if (kulonbseg <= 30)
                {
                    return Brushes.Orange;
                }
                else if (kulonbseg <= 60)
                {
                    return Brushes.Yellow;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
