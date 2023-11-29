using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Site_Corrector.Logika.Konwertery
{

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class CzyMaUstawieniaToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return Visibility.Collapsed;
            }
            else
            {
                bool sprawdzany = (bool)value;
                if (sprawdzany == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //brak
            return false;
        }

    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class CzyMoznaWylaczycToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return Visibility.Collapsed;
            }
            else
            {
                bool sprawdzany = (bool)value;
                if (sprawdzany == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //brak
            return false;
        }

    }

    [ValueConversion(typeof(StatusProjektu), typeof(Color))]
    public class StatusProjektuToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusProjektu))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                StatusProjektu sprawdzany = (StatusProjektu)value;
                if (sprawdzany == StatusProjektu.domyslny)
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
                }
                else if ((sprawdzany == StatusProjektu.rozpoczeto))
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
                }
                else if ((sprawdzany == StatusProjektu.zoptymalizowano))
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));//zielony
                }
                else if ((sprawdzany == StatusProjektu.blad))
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));//czerwony
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
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
            return StatusProjektu.domyslny;
        }
    }
}
