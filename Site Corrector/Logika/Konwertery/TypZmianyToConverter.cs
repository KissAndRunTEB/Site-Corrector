using Site_Corrector.Logika.Modele;
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
    [ValueConversion(typeof(TypZmiany), typeof(Color))]
    public class TypZmianyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TypZmiany))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                TypZmiany sprawdzany = (TypZmiany)value;
                if (sprawdzany == TypZmiany.PoprawkaBledu)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 255, 255, 0));
                }
                else if (sprawdzany == TypZmiany.PoprawkaStabilnosci)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 0, 255, 0));//zielony
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
            return TypZmiany.NowaFunkcja;
        }
    }



    [ValueConversion(typeof(TypZmiany), typeof(string))]
    public class TypZmianyToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                //HACK: powinno zamiast tego zgłaszać wyjątek
                return "1111111111";
            }
            else
            {
                TypZmiany sprawdzany = (TypZmiany)value;
                if (sprawdzany == TypZmiany.PoprawkaBledu)
                {
                    return "Poprawka błędu";
                }
                else if (sprawdzany == TypZmiany.PoprawkaStabilnosci)
                {
                    return "Poprawka stabilności";
                }
                else
                {
                    return "22222222222";
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
            return TypZmiany.NowaFunkcja;
        }
    }
}
