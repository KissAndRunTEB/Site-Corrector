using MahApps.Metro.Controls;
using Site_Corrector.Logika;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Logika interakcji dla klasy UstawieniaEtapu.xaml
    /// </summary>
    public partial class UstawieniaEtapu : MetroWindow
    {
        public UstawieniaEtapu()
        {
            InitializeComponent();
        }

        public Projekt projekt = new Projekt();

        public Etap etap = new Etap();

        public ObservableCollection<Etap> etapy = new ObservableCollection<Etap>();

        public void pokaz()
        {
            this.nazwa.Text = "Ustawienia dla "+projekt.Nazwa +" - "+ etap.Nazwa;

            ustawienia_na_wybory_uzytkownika();

            ukryj_pozostale_zakladki(etap.Nazwa);

            this.ShowDialog();
        }

        public void pokaz_wszystkie()
        {
            this.nazwa.Text = "Ustawienia dla " + projekt.Nazwa;

            ustawienia_na_wybory_uzytkownika();

            this.ShowDialog();
        }

        private void ukryj_pozostale_zakladki(string nazwa)
        {
            foreach (TabItem zakladka in zakladki.Items)
            {
                zakladka.Visibility = Visibility.Collapsed;
            }

            switch (nazwa)
            {
                case "Łączenie...":
                    (zakladki.Items[0] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[0] as TabItem).IsSelected = true;
                    break;
                case "Kompresja zdjęć JPG":
                    (zakladki.Items[1] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[1] as TabItem).IsSelected = true;
                    break;
                case "Kompresja grafik PNG":
                    (zakladki.Items[2] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[2] as TabItem).IsSelected = true;
                    break;

                case "Minifikacja plików CSS":
                    (zakladki.Items[3] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[3] as TabItem).IsSelected = true;
                    break;

                case "Minifikacja plików JS":
                    (zakladki.Items[4] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[4] as TabItem).IsSelected = true;
                    break;

                case "Minifikacja plików HTML":
                    (zakladki.Items[5] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[5] as TabItem).IsSelected = true;
                    break;

                case "Optymalizacja ustawień serwera":
                    (zakladki.Items[6] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[6] as TabItem).IsSelected = true;
                    break;

                case "Kopia zapasowa":
                    (zakladki.Items[7] as TabItem).Visibility = Visibility.Visible;
                    (zakladki.Items[7] as TabItem).IsSelected = true;
                    break;


                default:
                    this.nazwa.Text = "Nie ma ustawień dla tego etapu, poniżej lista wszystkich możliwych ustawień.";

                    foreach (TabItem zakladka in zakladki.Items)
                    {
                        zakladka.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
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

            switch (projekt.ustawienia.jakie_pliki)
            {
                case Ustawienia.PlikiDoPobrania.niezbedne:
                    pole_ktore_pliki.SelectedIndex = 0;
                    break;
                case Ustawienia.PlikiDoPobrania.wszystkie:
                    pole_ktore_pliki.SelectedIndex = 1;
                    break;
                default:
                    pole_ktore_pliki.SelectedIndex = 0;
                    break;
            }

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
                    pole_wyboru_jakosci_jpg.SelectedIndex = 0;
                    break;
            }


            switch (projekt.ustawienia.czy_jpg)
            {
                case true:
                    pole_czy_jpg.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_jpg.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_jpg.SelectedIndex = 0;
                    break;
            }

            //switch (pole_czy_jpg.SelectedItem.ToString())
            //{
            //    case "Włącz":
            //        projekt.ustawienia.czy_jpg = true;
            //        etapy[0].Status = StatusEtapu.dostepny;
            //        break;
            //    case "Wyłącz":
            //        projekt.ustawienia.czy_jpg = false;
            //        etapy[0].Status = StatusEtapu.wylaczony;
            //        break;
            //}


            switch (projekt.ustawienia.czy_png)
            {
                case true:
                    pole_czy_png.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_png.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_png.SelectedIndex = 0;
                    break;
            }

            switch (projekt.ustawienia.czy_css)
            {
                case true:
                    pole_czy_css.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_css.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_css.SelectedIndex = 0;
                    break;
            }

            switch (projekt.ustawienia.czy_js)
            {
                case true:
                    pole_czy_js.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_js.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_js.SelectedIndex = 0;
                    break;
            }

            switch (projekt.ustawienia.czy_html)
            {
                case true:
                    pole_czy_html.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_html.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_html.SelectedIndex = 0;
                    break;
            }

            switch (projekt.ustawienia.czy_htaccess)
            {
                case true:
                    pole_czy_htaccess.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_htaccess.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_htaccess.SelectedIndex = 0;
                    break;
            }


            switch (projekt.ustawienia.czy_deflate)
            {
                case true:
                    pole_czy_deflate.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_deflate.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_deflate.SelectedIndex = 0;
                    break;
            }


            switch (projekt.ustawienia.czy_expires)
            {
                case true:
                    pole_czy_expires.SelectedIndex = 0;
                    break;
                case false:
                    pole_czy_expires.SelectedIndex = 1;
                    break;
                default:
                    pole_czy_expires.SelectedIndex = 0;
                    break;
            }

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

            string ktore_pliki = (pole_ktore_pliki.SelectedItem as ComboBoxItem).Content.ToString();

            switch (ktore_pliki)
            {
                case "Tylko niezbędne":
                    projekt.ustawienia.jakie_pliki = Ustawienia.PlikiDoPobrania.niezbedne;
                    break;
                case "Wszystkie":
                    projekt.ustawienia.jakie_pliki = Ustawienia.PlikiDoPobrania.wszystkie;
                    break;
                default:
                    projekt.ustawienia.jakie_pliki = Ustawienia.PlikiDoPobrania.niezbedne;
                    break;
            }

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

            string czy_jpg = (pole_czy_jpg.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_jpg)
            {
                case "Włącz":
                    projekt.ustawienia.czy_jpg = true;
                    etapy[0].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_jpg = false;
                    etapy[0].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_png = (pole_czy_png.SelectedItem as ComboBoxItem).Content.ToString();


            switch (czy_png)
            {
                case "Włącz":
                    projekt.ustawienia.czy_png = true;
                    etapy[1].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_png = false;
                    etapy[1].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_css = (pole_czy_css.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_css)
            {
                case "Włącz":
                    projekt.ustawienia.czy_css = true;
                    etapy[2].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_css = false;
                    etapy[2].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_js = (pole_czy_js.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_js)
            {
                case "Włącz":
                    projekt.ustawienia.czy_js = true;
                    etapy[3].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_js = false;
                    etapy[3].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_html = (pole_czy_html.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_html)
            {
                case "Włącz":
                    projekt.ustawienia.czy_html = true;
                    etapy[4].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_html = false;
                    etapy[4].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_htaccess = (pole_czy_htaccess.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_htaccess)
            {
                case "Włącz":
                    projekt.ustawienia.czy_htaccess = true;
                    etapy[5].Status = StatusEtapu.dostepny;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_htaccess = false;
                    etapy[5].Status = StatusEtapu.wylaczony;
                    break;
            }

            string czy_deflate = (pole_czy_deflate.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_deflate)
            {
                case "Włącz":
                    projekt.ustawienia.czy_deflate = true;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_deflate = false;
                    break;
            }

            string czy_expires = (pole_czy_expires.SelectedItem as ComboBoxItem).Content.ToString();

            switch (czy_expires)
            {
                case "Włącz":
                    projekt.ustawienia.czy_expires = true;
                    break;
                case "Wyłącz":
                    projekt.ustawienia.czy_expires = false;
                    break;
            }
        }
        #endregion

        
        



        private void TextBlock_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Process.Start(projekt.Folder_projektu);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            wybory_na_ustawienia_uzytkownika();


            ListaProjektow.zapisz();

            this.Close();
        }
    }
}
