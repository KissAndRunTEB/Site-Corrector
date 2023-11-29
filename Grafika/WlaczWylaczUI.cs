using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Grafika
{
    public class WlaczWylaczUI
    {
        static public void ustaw(bool wartosc, ComboBox gdzie)
        {
            switch (wartosc)
            {
                case true:
                    gdzie.SelectedIndex = 0;
                    break;
                case false:
                    gdzie.SelectedIndex = 1;
                    break;
                default:
                    gdzie.SelectedIndex = 0;
                    break;
            }

        }

        static public bool odczytaj(ComboBox skad)
        {
            string wartosc = (skad.SelectedItem as ComboBoxItem).Content.ToString();

            switch (wartosc)
            {
                case "Włącz":
                    return true;
                case "Wyłącz":
                    return false;
                default:
                    return false;
            }
        }
    }
}
