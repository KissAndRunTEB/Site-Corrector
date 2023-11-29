using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Site_Corrector.Logika.Konwertery
{
    [ValueConversion(typeof(StatusPliku), typeof(Color))]
    public class StatusPlikuToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusPliku))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                StatusPliku sprawdzany = (StatusPliku)value;
                if (sprawdzany == StatusPliku.znaleziony)
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                else if (sprawdzany == StatusPliku.pobrany)
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else if (sprawdzany == StatusPliku.zoptymalizowany)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (sprawdzany == StatusPliku.problem)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else if (sprawdzany == StatusPliku.wgrany)
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Color c = (Color)value;
            //if (c == Colors.Gray)
            //{
            //    return Status.niedostepny;
            //}
            //else
            //{
            //    return Status.niedostepny;
            //}
            return StatusPliku.pobrany;
        }
    }

}
