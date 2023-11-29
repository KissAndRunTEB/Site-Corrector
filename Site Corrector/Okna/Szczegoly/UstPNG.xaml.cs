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
    /// Logika interakcji dla klasy UstPNG.xaml
    /// </summary>
    public partial class UstPNG : MetroWindow
    {
        public UstPNG()
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

            switch (projekt.ustawienia.jakosc_PNG)
            {
                case 5:
                    pole_wyboru_jakosci_png.SelectedIndex = 0;
                    break;
                case 2:
                    pole_wyboru_jakosci_png.SelectedIndex = 1;
                    break;
                case 7:
                    pole_wyboru_jakosci_png.SelectedIndex = 2;
                    break;
                default:
                    pole_wyboru_jakosci_png.SelectedIndex = 0;
                    break;
            }

            

            Grafika.WlaczWylaczUI.ustaw(projekt.ustawienia.czy_png, pole_czy_png);
        }
        

        public void wybory_na_ustawienia_uzytkownika()
        {
            string jakosci_png = (pole_wyboru_jakosci_png.SelectedItem as ComboBoxItem).Content.ToString();

            switch (jakosci_png)
            {
                case "Bezstratna standardowa kompresja":
                    projekt.ustawienia.jakosc_PNG = 5;
                    break;
                case "Bezstratna szybka kompresja":
                    projekt.ustawienia.jakosc_PNG = 2;
                    break;
                case "Bezstratna wolna kompresja":
                    projekt.ustawienia.jakosc_PNG = 7;
                    break;
                default:
                    projekt.ustawienia.jakosc_PNG = 5;
                    break;
            }
           


            bool czy_wlaczony = WlaczWylaczUI.odczytaj(pole_czy_png);

            projekt.ustawienia.czy_png = czy_wlaczony;

            etapy[1].Status = czy_wlaczony ? StatusEtapu.dostepny : StatusEtapu.wylaczony;



        }

        #endregion


    }
}
