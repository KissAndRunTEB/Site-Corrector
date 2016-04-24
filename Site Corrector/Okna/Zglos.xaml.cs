using Biblioteka;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Site_Corrector.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy Zglos.xaml
    /// </summary>
    public partial class Zglos : MetroWindow
    {
        public Zglos()
        {
            InitializeComponent();
        }

       
        private void pole_na_wiadomosc_GotFocus(object sender, RoutedEventArgs e)
        {
            pole_na_wiadomosc.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            przycisk_wyslij.IsEnabled = false;

            string wiadomosc = pole_na_wiadomosc.Text;
            string nick = "Tester";
            string mail = "aplikacja.serialomaniak@gmail.com";

            if (Internet.czy_polaczenie())
            {
                Internet.wyślij_maila_na_scapp("Site Corrector, " + nick, wiadomosc, nick, mail);

                pole_na_wiadomosc.Text = "Wysłane, dziękujemy w następnej wersji postaram się wziąść Twoje sugestie pod uwagę.";

                przycisk_wyslij.IsEnabled = true;
            }

            przycisk_wyslij.Content = "Dziękujemy!";
            przycisk_wyslij.IsEnabled = false;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
