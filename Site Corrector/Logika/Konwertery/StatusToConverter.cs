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
    [ValueConversion(typeof(StatusEtapu), typeof(Boolean))]
    public class StatusToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusEtapu))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return true;
            }
            else
            {
                StatusEtapu sprawdzany = (StatusEtapu)value;
                if (sprawdzany == StatusEtapu.dostepny)
                {
                    return true;
                }
                else if ((sprawdzany == StatusEtapu.niedostepny) || (sprawdzany == StatusEtapu.niepotrzebny) || (sprawdzany == StatusEtapu.wylaczony))
                {
                    return false;
                }
                else
                {
                    return true;
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
            return StatusEtapu.dostepny;
        }
    }


    [ValueConversion(typeof(StatusEtapu), typeof(Color))]
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusEtapu))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                StatusEtapu sprawdzany = (StatusEtapu)value;
                if (sprawdzany == StatusEtapu.dostepny)
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
                else if ((sprawdzany == StatusEtapu.niedostepny) || (sprawdzany == StatusEtapu.niepotrzebny) || (sprawdzany == StatusEtapu.wylaczony))
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
                }
                else if (sprawdzany == StatusEtapu.rozpoczety)
                {
                    return new SolidColorBrush(Color.FromArgb(150, 0, 255, 0));//zielony
                }
                else if (sprawdzany == StatusEtapu.offline)
                {
                    return new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));//niebieski
                }
                else if (sprawdzany == StatusEtapu.zakonczony)
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));//jasno zielony
                }
                else if (sprawdzany == StatusEtapu.blad)
                {
                    return new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));//jasno czerwony
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
            return StatusEtapu.niedostepny;
        }
    }
}
