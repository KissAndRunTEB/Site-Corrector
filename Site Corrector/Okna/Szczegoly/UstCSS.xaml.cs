using Grafika;
using MahApps.Metro.Controls;
using Site_Corrector.Logika;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Site_Corrector.Okna.Szczegoly
{
    /// <summary>
    /// Logika interakcji dla klasy UstCSS.xaml
    /// </summary>
    public partial class UstCSS : MetroWindow
    {
        public UstCSS()
        {
            InitializeComponent();
        }

        public Projekt projekt = new Projekt();

        public Etap etap = new Etap();

        public ObservableCollection<Etap> etapy = new ObservableCollection<Etap>();

        public void pokaz()
        {
            this.nazwa.Text = "Ustawienia dla " + projekt.Nazwa + " - " + etap.Nazwa;

            ustawienia_na_wybory_uzytkownika();

            this.ShowDialog();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            wybory_na_ustawienia_uzytkownika();


            ListaProjektow.zapisz();

            this.Close();
        }

        #region Ustawienia

        public void ustawienia_na_wybory_uzytkownika()
        {
            Grafika.WlaczWylaczUI.ustaw(projekt.ustawienia.czy_css, pole_czy_css);
        }


        public void wybory_na_ustawienia_uzytkownika()
        {
            bool czy_wlaczony = WlaczWylaczUI.odczytaj(pole_czy_css);

            projekt.ustawienia.czy_css = czy_wlaczony;

            etapy[2].Status = czy_wlaczony ? StatusEtapu.dostepny : StatusEtapu.wylaczony;

        }

        #endregion


    }
}
