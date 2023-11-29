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
    /// Logika interakcji dla klasy UstJPG.xaml
    /// </summary>
    public partial class UstJPG : MetroWindow
    {
        public UstJPG()
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
            switch (projekt.ustawienia.jakosc_JPG)
            {
                case 100:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 0;
                    break;
                case 90:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 1;
                    break;
                case 80:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 2;
                    break;
                case 70:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 3;
                    break;
                case 60:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 4;
                    break;
                default:
                    pole_wyboru_jakosci_jpg.SelectedIndex = 0;
                    break;
            }

            

            Grafika.WlaczWylaczUI.ustaw(projekt.ustawienia.czy_jpg, pole_czy_jpg);
        }
        

        public void wybory_na_ustawienia_uzytkownika()
        {
            string jakosci_jpg = (pole_wyboru_jakosci_jpg.SelectedItem as ComboBoxItem).Content.ToString();

            switch (jakosci_jpg)
            {
                case "Bezstratna kompresja":
                    projekt.ustawienia.jakosc_JPG = 100;
                    break;
                case "90%":
                    projekt.ustawienia.jakosc_JPG = 90;
                    break;
                case "80%":
                    projekt.ustawienia.jakosc_JPG = 80;
                    break;
                case "70%":
                    projekt.ustawienia.jakosc_JPG = 70;
                    break;
                case "60%":
                    projekt.ustawienia.jakosc_JPG = 60;
                    break;
                default:
                    projekt.ustawienia.jakosc_JPG = 100;
                    break;
            }
            

            bool czy_wlaczony = WlaczWylaczUI.odczytaj(pole_czy_jpg);

            projekt.ustawienia.czy_jpg = czy_wlaczony;

            etapy[0].Status = czy_wlaczony ? StatusEtapu.dostepny : StatusEtapu.wylaczony;
        }

        #endregion


    }
}
