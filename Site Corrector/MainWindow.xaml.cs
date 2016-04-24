using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Compression;
using Site_Corrector.Okna;
using System.Diagnostics;
using Site_Corrector.Logika;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using System.IO;
using System.Drawing;
using Google.Apis.Services;
using Google.Apis.Pagespeedonline.v2;

namespace Site_Corrector
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 


    //do sitemapy https://github.com/ernado-x/X.Web.Sitemap

    public partial class MainWindow : MetroWindow
    {
        Thread watek;
        Projekt wybrany = new Projekt();

        ObservableCollection<Plik> pliki = new ObservableCollection<Plik>();

        ObservableCollection<Etap> etapy_pobieranie = new ObservableCollection<Etap>();
        ObservableCollection<Etap> etapy = new ObservableCollection<Etap>();
        ObservableCollection<Etap> etapy_wgrywanie = new ObservableCollection<Etap>();

        //polepszenie kodu
        //wykrycie błędów

        //to co wartosciowe z serialomaniaka

        //dzialanie lokalne

        //minimalizacja okna
        //przesowanie okna

        //edycja projektu strony
        //zapisanie ostatnich ustawien i wczytanie

        //licencjonowanie

        //zarządzanie kopiami, kasowanie plikow przed i po kompresji, zostawianie samej kopii

        //cofanie do wersji

        //zmiana lokalizacji przechowywania plikow

        //bledy z analizy kodu

        //polepszenie metryk kodu

        public MainWindow()
        {
            Program.licencjonowanie();

            bool nowy = Program.zainstaluj();
            if (nowy)
            {
                Program.loguj("Zainstalowano poprawnie");
                DodajProjekt okno = new DodajProjekt();
                okno.ShowDialog();
            }

            Program.loguj_start();

            ListaProjektow.wczytaj();


            etapy_pobieranie = Etap.przygotuj_pobieranie_etapy();
            etapy = Etap.przygotuj_etapy(null);
            etapy_wgrywanie = Etap.przygotuj_wgrywanie_etapy();


            //Program.aktualizuj_w_tle();
            //Program.loguj("Zaaktualizowano program");

            //Profil.wczytaj();
            //Program.loguj("Wczytano Profil");
            //Ustawienia.wczytaj();
            //Program.loguj("Wczytano Ustawienia");

            //Program.komunikat_o_aktualizacji_ukonczonej();

            InitializeComponent();
            Program.loguj("Udana inicjalizacja");

            zamienienie_alt_f4_na_minimalizacje_okna();


            //stworz_ikone_w_przyborniku_systemowym();


            Program.loguj("Udane uruchomienie");

            //Program.zapisz_na_serwerze();


            lista_plikow.ItemsSource = pliki;

            if (lista_projektow.Items.Count > 0)
            {

                lista_projektow.SelectedIndex = 0;

                wybrany = lista_projektow.SelectedItem as Projekt;

                etapy = Etap.przygotuj_etapy(wybrany);
            }
            else
            {
                przycisk_start.IsEnabled = false;
                przycisk_pobierz.IsEnabled = false;
                przycisk_optymalizuj.IsEnabled = false;
                przycisk_wgraj.IsEnabled = false;

                etapy = Etap.przygotuj_etapy(null);
            }


            lista_etapow_pobieranie.ItemsSource = etapy_pobieranie;
            lista_etapow.ItemsSource = etapy;
            lista_etapow_wgrywanie.ItemsSource = etapy_wgrywanie;



            zaladuj_slajder();

            zaladuj_zajetosc_dysku();

            zaladuj_favicony();

            //watek_zaladowania_podgladow();

            //obsluga jezykow
            //Title = Site_Corrector.Properties.Resources.Title;


            bool czy_pokazywac = false;


            //lista elementow do ukrycia
            if (czy_pokazywac == false)
            {
                slajder_UC.Visibility = Visibility.Collapsed;
            }

        }

        private void watek_statystyk_PageSpeed_przed()
        {
            Thread watek_statystyk_PS_przed = new Thread(new ThreadStart(zaladuj_statystyki_PageSpeed_przed));
            watek_statystyk_PS_przed.Start();
        }

        private void zaladuj_statystyki_PageSpeed_przed()
        {
            var url = wybrany.AdresStrony;

            PageSpeed strony = new PageSpeed(url);


            etykia_PS_wynik_komp_przed.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                etykia_PS_wynik_komp_przed.Text = strony.Desktop.Wszystko_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed.Text = strony.Smartfony.Wszystko_rozmiar.ToString();


                etykia_PS_wynik_komp_przed_CSS.Text = strony.Desktop.Css_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_CSS.Text = strony.Smartfony.Css_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_JS.Text = strony.Desktop.Js_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_JS.Text = strony.Smartfony.Js_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_HTML.Text = strony.Desktop.Html_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_HTML.Text = strony.Smartfony.Html_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_Tekst.Text = strony.Desktop.Text_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_Tekst.Text = strony.Smartfony.Text_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_Zdjecia.Text = strony.Desktop.Zdjecia_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_Zdjecia.Text = strony.Smartfony.Zdjecia_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_FLASH.Text = strony.Desktop.Flash_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_FLASH.Text = strony.Smartfony.Flash_rozmiar.ToString();

                etykia_PS_wynik_komp_przed_Inne.Text = strony.Desktop.Inne_rozmiar.ToString();
                etykia_PS_wynik_mobile_przed_Inne.Text = strony.Smartfony.Inne_rozmiar.ToString();
            }));

        }

        private void watek_statystyk_PageSpeed_po()
        {
            Thread watek_statystyk_PS_po = new Thread(new ThreadStart(zaladuj_statystyki_PageSpeed_po));
            watek_statystyk_PS_po.Start();
        }

        private void zaladuj_statystyki_PageSpeed_po()
        {
            var url = wybrany.AdresStrony;

            PageSpeed strony = new PageSpeed(url);

            etykia_PS_wynik_komp_po.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                etykia_PS_wynik_komp_po.Text = strony.Desktop.Wszystko_rozmiar.ToString();
                etykia_PS_wynik_mobile_po.Text = strony.Smartfony.Wszystko_rozmiar.ToString();

                etykia_PS_wynik_komp_po_CSS.Text = strony.Desktop.Css_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_CSS.Text = strony.Smartfony.Css_rozmiar.ToString();

                etykia_PS_wynik_komp_po_JS.Text = strony.Desktop.Js_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_JS.Text = strony.Smartfony.Js_rozmiar.ToString();

                etykia_PS_wynik_komp_po_HTML.Text = strony.Desktop.Html_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_HTML.Text = strony.Smartfony.Html_rozmiar.ToString();

                etykia_PS_wynik_komp_po_Tekst.Text = strony.Desktop.Text_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_Tekst.Text = strony.Smartfony.Text_rozmiar.ToString();

                etykia_PS_wynik_komp_po_Zdjecia.Text = strony.Desktop.Zdjecia_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_Zdjecia.Text = strony.Smartfony.Zdjecia_rozmiar.ToString();

                etykia_PS_wynik_komp_po_FLASH.Text = strony.Desktop.Flash_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_FLASH.Text = strony.Smartfony.Flash_rozmiar.ToString();

                etykia_PS_wynik_komp_po_Inne.Text = strony.Desktop.Inne_rozmiar.ToString();
                etykia_PS_wynik_mobile_po_Inne.Text = strony.Smartfony.Inne_rozmiar.ToString();

            }));
        }

        private void watek_zaladowania_podgladow()
        {
            Thread watek_podgladow = new Thread(new ThreadStart(zaladuj_podglad));
            watek_podgladow.SetApartmentState(ApartmentState.STA);
            watek_podgladow.Start();
        }

        private void zaladuj_podglad()
        {
            ListaProjektow lista = (App.Current.Resources["lista_projektow"] as ListaProjektow);

            foreach (Projekt p in lista)
            {
                string adres = p.AdresStrony;

                string adres_zapisu = p.Folder_projektu + "/podglad.png";

                Bitmap thumbnail = PageScreen.GenerateScreenshot(adres, -1, -1);

                thumbnail.Save(adres_zapisu);

                p.Podglad = adres_zapisu;
            }
        }

        private void zaladuj_favicony()
        {
            ListaProjektow lista = (App.Current.Resources["lista_projektow"] as ListaProjektow);

            foreach (Projekt p in lista)
            {
                string folder = p.Folder_projektu_oryginal;

                string[] oDirectories = Directory.GetFiles(folder, "favicon.*", SearchOption.AllDirectories);

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "Favicon.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "logo.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "Logo.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "preview.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "Preview.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "screen.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "Screen.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "view.*", SearchOption.AllDirectories);
                }

                if (oDirectories.Count() == 0)
                {
                    oDirectories = Directory.GetFiles(folder, "View.*", SearchOption.AllDirectories);
                }


                if (oDirectories.Count() > 0)
                {
                    string favicona = oDirectories[0];

                    p.Favicona = favicona;

                    // lista_projektow.items
                }
            }
        }

        private void zaladuj_zajetosc_dysku()
        {
            etykieta_zajetosc_dysku.Content = "w użyciu 0.0GB / 10GB";
        }

        public void zaladuj_slajder()
        {
            slajder_UC.pole_tekstowe.Text = "Przykładowy dłuższy tekst";

            slajder_UC.w_lewo.Visibility = Visibility.Collapsed;
            slajder_UC.w_prawo.Visibility = Visibility.Collapsed;
        }

        public void watek_pobierania()
        {
            watek_statystyk_PageSpeed_przed();


            //Etap 00
            if (Optymalizacja.polacz(wybrany, etapy_pobieranie))
            {
                #region Przygotowanie
                //Etap 0
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow_pobieranie.SelectedIndex = 1;
                }));
                Optymalizacja.pobierz_strone(wybrany, etapy_pobieranie, pliki, lista_plikow);



                //Etap 1
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    etykia_przed_MB.Text = Plik.zlicz_rozmiar_plikow(pliki) + " KB";

                    lista_etapow_pobieranie.SelectedIndex = 2;
                }));

                Optymalizacja.etap_kopia_bezpieczenstwa(wybrany, etapy_pobieranie, pliki, lista_plikow);




                //stworzenie list plikow z podzialem na typy
                pliki = Optymalizacja.stworzenie_list_plikow(wybrany, etapy, pliki, lista_plikow);

                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    etykia_przed_MB.Text = Plik.zlicz_rozmiar_plikow(pliki) + " KB";

                    lista_plikow.ItemsSource = pliki;
                    lista_plikow.Items.Refresh();
                }));
                #endregion

                wybrany.DataPobrania = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            }
            else
            {
                foreach (Etap etap in etapy_pobieranie)
                {
                    etap.Status = StatusEtapu.blad;
                    wybrany.Status = StatusProjektu.blad;
                }
            }

            lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                przycisk_pobierz.Opacity = 1;
                lista_projektow.IsEnabled = true;

            }));



            ListaProjektow.zapisz();
        }

        public void watek_wgrywania()
        {
            //Etap 00
            if (Optymalizacja.polacz(wybrany, etapy_wgrywanie))
            {
                //Etap 100
                Optymalizacja.wgraj(wybrany, etapy_wgrywanie, pliki, lista_plikow);
            }
            else
            {
                foreach (Etap etap in etapy_wgrywanie)
                {
                    etap.Status = StatusEtapu.blad;
                    wybrany.Status = StatusProjektu.blad;
                }
            }

            lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                przycisk_wgraj.Opacity = 1;
                lista_projektow.IsEnabled = true;

                zmiana_jezyka.IsEnabled = true;

                przycisk_start.Opacity = 1;
            }));

            wybrany.DataOptymalizacji = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

            ListaProjektow.zapisz();

            lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Sukces osukcesie = new Sukces();
                osukcesie.Link = wybrany.AdresStrony;
                osukcesie.Opis = "";
                osukcesie.pokaz();
            }));



            watek_statystyk_PageSpeed_po();
        }

        public void watek_etapow()
        {
            //Etap 00
            if (Optymalizacja.polacz(wybrany, etapy))
            {
                //jesli lista pusta zaladuj z folderu
                if (pliki.Count == 0)
                {
                    //stworzenie list plikow z podzialem na typy
                    pliki = Optymalizacja.stworzenie_list_plikow(wybrany, etapy, pliki, lista_plikow);

                    lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        lista_plikow.ItemsSource = pliki;
                        lista_plikow.Items.Refresh();
                    }));
                }


                Optymalizacja.etap_kompresja_JPG(wybrany, etapy, pliki);//etap[3]

                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow.SelectedIndex = lista_etapow_pobieranie.SelectedIndex++;
                }));

                Optymalizacja.etap_kompresja_PNG(wybrany, etapy, pliki);
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow.SelectedIndex = lista_etapow_pobieranie.SelectedIndex++;
                }));

                Optymalizacja.etap_kompresja_CSS(wybrany, etapy, pliki);
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow.SelectedIndex = lista_etapow_pobieranie.SelectedIndex++;
                }));


                Optymalizacja.etap_kompresja_JS(wybrany, etapy, pliki);
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow.SelectedIndex = lista_etapow_pobieranie.SelectedIndex++;
                }));


                Optymalizacja.etap_kompresja_HTML(wybrany, etapy, pliki);
                lista_plikow.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lista_etapow.SelectedIndex = lista_etapow_pobieranie.SelectedIndex++;
                }));


                Optymalizacja.etap_plik_htaccess(wybrany, etapy, pliki);

            }
            else
            {
                foreach (Etap etap in etapy)
                {
                    etap.Status = StatusEtapu.blad;
                    wybrany.Status = StatusProjektu.blad;
                }
            }

            lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                przycisk_optymalizuj.Opacity = 1;
                lista_projektow.IsEnabled = true;


                lista_etapow.IsEnabled = true;

                etykia_po_MB.Text = Plik.zlicz_rozmiar_plikow_po_kompresji(pliki) + " KB";
            }));
        }

        public void watek_start()
        {
            watek_pobierania();

            watek_etapow();

            watek_wgrywania();
        }


        public void zamknij_program(bool zrestartowac = false)
        {
            Program.loguj("Zamykanie programu ...");


            if (watek != null)
            {
                watek.Abort();
            }

            ListaProjektow.zapisz();

            this.Visibility = Visibility.Hidden;

            Program.loguj("... program został zamknięty.");

            if (zrestartowac)
            {
                System.Windows.Forms.Application.Restart();
            }

            System.Windows.Application.Current.Shutdown();
        }

        #region Interfejs programu




        private void lista_projektow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            etykia_przed_MB.Text = " - KB";
            etykia_po_MB.Text = " - KB";

            wybrany = lista_projektow.SelectedItem as Projekt;

            etapy = Etap.przygotuj_etapy(wybrany);

            //   ListaProjektow.wczytaj();

            lista_etapow.ItemsSource = etapy;

            pliki = new ObservableCollection<Plik>();

            lista_plikow.ItemsSource = pliki;

            if (wybrany == null)
            {
                przycisk_pobierz.IsEnabled = false;

                przycisk_optymalizuj.IsEnabled = false;

                przycisk_wgraj.IsEnabled = false;

                przycisk_start.IsEnabled = false;

                etykieta_nazwa_projektu_wybranego.Content = "Dodaj projekt";
            }
            else
            {
                przycisk_pobierz.Opacity = 1;
                przycisk_pobierz.IsEnabled = true;

                przycisk_optymalizuj.Opacity = 1;
                przycisk_optymalizuj.IsEnabled = true;

                przycisk_wgraj.Opacity = 1;
                przycisk_wgraj.IsEnabled = true;

                przycisk_start.Opacity = 1;
                przycisk_start.IsEnabled = true;

                etykieta_nazwa_projektu_wybranego.Content = wybrany.Nazwa;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            zamknij_program();
        }
        private void zamienienie_alt_f4_na_minimalizacje_okna()
        {
            this.KeyDown += MainWindow_KeyDown;
        }


        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
                this.WindowState = WindowState.Minimized;
                this.Hide();
            }
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Informacje okno = new Informacje();
            okno.ShowDialog();
        }

        private void TextBlock_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Informacje okno = new Informacje();
            okno.ShowDialog();
        }


        private void TextBlock_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            if ((lista_projektow.SelectedItem as Projekt) != null)
            {
                Projekt p = (lista_projektow.SelectedItem as Projekt);

                ListaProjektow.usun(p);

                ListaProjektow.zapisz();
            }
        }

        private void TextBlock_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            DodajProjekt okno = new DodajProjekt();

            okno.Closed += Okno_Closed;

            okno.ShowDialog();


        }

        private void Okno_Closed(object sender, EventArgs e)
        {
            lista_projektow.SelectedIndex = lista_projektow.Items.Count - 1;
        }

        #endregion


        private void Ustawienia_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UstawieniaEtapu nowe = new UstawieniaEtapu();
            nowe.etap = (Etap)(sender as StackPanel).DataContext;

            if (nowe.etap.CzyMaUstawienia)
            {
                nowe.etapy = etapy;

                nowe.projekt = wybrany;

                nowe.pokaz();

            }
        }

        //private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Etap etap = (Etap)(sender as Image).DataContext;

        //    switch (etap.Nazwa)
        //    {
        //        case "Kompresja zdjęć JPG":
        //            wybrany.Ustawienia.czy_jpg = (!wybrany.Ustawienia.czy_jpg);

        //            if (wybrany.Ustawienia.czy_jpg)
        //            {
        //                etapy[0].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[0].Status = StatusEtapu.wylaczony;
        //            }


        //            break;
        //        case "Kompresja grafik PNG":

        //            wybrany.Ustawienia.czy_png = (!wybrany.Ustawienia.czy_png);

        //            if (wybrany.Ustawienia.czy_png)
        //            {
        //                etapy[1].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[1].Status = StatusEtapu.wylaczony;
        //            }


        //            break;

        //        case "Minifikacja plików CSS":

        //            wybrany.Ustawienia.czy_css = (!wybrany.Ustawienia.czy_css);
        //            if (wybrany.Ustawienia.czy_css)
        //            {
        //                etapy[2].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[2].Status = StatusEtapu.wylaczony;
        //            }

        //            break;

        //        case "Minifikacja plików JS":

        //            wybrany.Ustawienia.czy_js = (!wybrany.Ustawienia.czy_js);
        //            if (wybrany.Ustawienia.czy_js)
        //            {
        //                etapy[3].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[3].Status = StatusEtapu.wylaczony;
        //            }
        //            break;

        //        case "Minifikacja plików HTML":

        //            wybrany.Ustawienia.czy_html = (!wybrany.Ustawienia.czy_html);
        //            if (wybrany.Ustawienia.czy_html)
        //            {
        //                etapy[4].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[4].Status = StatusEtapu.wylaczony;
        //            }
        //            break;

        //        case "Optymalizacja ustawień serwera":

        //            wybrany.Ustawienia.czy_htaccess = (!wybrany.Ustawienia.czy_htaccess);
        //            if (wybrany.Ustawienia.czy_htaccess)
        //            {
        //                etapy[5].Status = StatusEtapu.dostepny;
        //            }
        //            else
        //            {
        //                etapy[5].Status = StatusEtapu.wylaczony;
        //            }
        //            break;
        //        default:

        //            break;
        //    }

        //    ListaProjektow.zapisz();
        //}


        private void lista_etapow_pobieranie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            if (selector is ListBox)
            {
                (selector as ListBox).ScrollIntoView(selector.SelectedItem);
            }
        }

        private void lista_etapow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            if (selector is ListBox)
            {
                (selector as ListBox).ScrollIntoView(selector.SelectedItem);
            }
        }


        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.facebook.com/Site-Corrector-1600185850307419/");
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:scapp.kontakt@gmail.com?Subject=Nowa%20funkcja");
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:scapp.kontakt@gmail.com?Subject=Zgłoszenie%20błędu");
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                //MainWindowNew nowe = new MainWindowNew();
                // nowe.Show();

                this.Close();
            }
        }

        private void przycisk_start_Click(object sender, MouseButtonEventArgs e)
        {
            etykia_przed_MB.Text = " - KB";
            etykia_po_MB.Text = " - KB";

            Projekt wybrany = lista_projektow.SelectedItem as Projekt;
            if (wybrany == null)
            {
                przycisk_start.Opacity = 0.5;
                przycisk_start.IsEnabled = false;
            }
            else
            {

                przycisk_start.Opacity = 0.5;
                przycisk_start.IsEnabled = false;
                lista_projektow.IsEnabled = false;


                lista_etapow.IsEnabled = false;

                zmiana_jezyka.IsEnabled = false;

                //zgraj pliki
                watek = new Thread(new ThreadStart(watek_start));
                watek.Start();
            }
        }

        private void przycisk_pobierz_MouseDown(object sender, MouseButtonEventArgs e)
        {
            etykia_przed_MB.Text = " - KB";
            etykia_po_MB.Text = " - KB";

            Projekt wybrany = lista_projektow.SelectedItem as Projekt;
            if (wybrany == null)
            {
                przycisk_pobierz.Opacity = 0.5;
                przycisk_pobierz.IsEnabled = false;
            }
            else
            {

                przycisk_pobierz.Opacity = 0.5;
                przycisk_pobierz.IsEnabled = false;
                lista_projektow.IsEnabled = false;

                //zgraj pliki
                watek = new Thread(new ThreadStart(watek_pobierania));
                watek.Start();
            }
        }

        private void przycisk_optymalizuj_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Projekt wybrany = lista_projektow.SelectedItem as Projekt;
            if (wybrany == null)
            {
                przycisk_optymalizuj.Opacity = 0.5;
                przycisk_optymalizuj.IsEnabled = false;
            }
            else
            {

                przycisk_optymalizuj.Opacity = 0.5;
                przycisk_optymalizuj.IsEnabled = false;
                lista_projektow.IsEnabled = false;

                lista_etapow.IsEnabled = false;

                zmiana_jezyka.IsEnabled = false;

                //zgraj pliki
                watek = new Thread(new ThreadStart(watek_etapow));
                watek.Start();
            }
        }

        private void przycisk_wgraj_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Projekt wybrany = lista_projektow.SelectedItem as Projekt;
            if (wybrany == null)
            {
                przycisk_wgraj.Opacity = 0.5;
                przycisk_wgraj.IsEnabled = false;
            }
            else
            {

                przycisk_wgraj.Opacity = 0.5;
                przycisk_wgraj.IsEnabled = false;
                lista_projektow.IsEnabled = false;

                zmiana_jezyka.IsEnabled = false;

                //zgraj pliki
                watek = new Thread(new ThreadStart(watek_wgrywania));
                watek.Start();
            }
        }

        private void etykieta_tryb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (etykieta_tryb.Opacity == 0.2)
            {
                etykieta_tryb.Opacity = 1;

                panel_zawansowane.Visibility = Visibility.Visible;

                panel_plikow.Visibility = Visibility.Visible;
            }
            else
            {
                etykieta_tryb.Opacity = 0.2;


                panel_zawansowane.Visibility = Visibility.Collapsed;

                panel_plikow.Visibility = Visibility.Collapsed;
            }
        }

        private void przycisk_menu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (menu.Visibility == Visibility.Collapsed)
            {
                menu.Visibility = Visibility.Visible;
            }
            else
            {
                menu.Visibility = Visibility.Collapsed;
            }
        }

        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:scapp.kontakt@gmail.com?Subject=Zgłoszenie%20błędu");
        }



        private void Image_MouseDown_6(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            Etap etap = (Etap)(sender as ToggleSwitch).DataContext;
            bool czy_wl_wyl = (bool)(sender as ToggleSwitch).IsChecked;

            switch (etap.Nazwa)
            {
                case "Kompresja zdjęć JPG":

                    wybrany.Ustawienia.czy_jpg = czy_wl_wyl;

                    if (wybrany.Ustawienia.czy_jpg)
                    {
                        etapy[0].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[0].Status = StatusEtapu.wylaczony;
                    }


                    break;
                case "Kompresja grafik PNG":

                    wybrany.Ustawienia.czy_png = czy_wl_wyl;

                    if (wybrany.Ustawienia.czy_png)
                    {
                        etapy[1].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[1].Status = StatusEtapu.wylaczony;
                    }


                    break;

                case "Minifikacja plików CSS":

                    wybrany.Ustawienia.czy_css = czy_wl_wyl;
                    if (wybrany.Ustawienia.czy_css)
                    {
                        etapy[2].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[2].Status = StatusEtapu.wylaczony;
                    }

                    break;

                case "Minifikacja plików JS":

                    wybrany.Ustawienia.czy_js = czy_wl_wyl;
                    if (wybrany.Ustawienia.czy_js)
                    {
                        etapy[3].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[3].Status = StatusEtapu.wylaczony;
                    }
                    break;

                case "Minifikacja plików HTML":

                    wybrany.Ustawienia.czy_html = czy_wl_wyl;
                    if (wybrany.Ustawienia.czy_html)
                    {
                        etapy[4].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[4].Status = StatusEtapu.wylaczony;
                    }
                    break;

                case "Optymalizacja ustawień serwera":

                    wybrany.Ustawienia.czy_htaccess = czy_wl_wyl;
                    if (wybrany.Ustawienia.czy_htaccess)
                    {
                        etapy[5].Status = StatusEtapu.dostepny;
                    }
                    else
                    {
                        etapy[5].Status = StatusEtapu.wylaczony;
                    }
                    break;
                default:

                    break;
            }

            ListaProjektow.zapisz();
        }


        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            Etap etap = (Etap)(sender as ToggleSwitch).DataContext;

            switch (etap.Nazwa)
            {
                case "Kompresja zdjęć JPG":

                    if (etapy[0].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }


                    break;
                case "Kompresja grafik PNG":

                    if (etapy[1].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }


                    break;

                case "Minifikacja plików CSS":

                    if (etapy[2].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }

                    break;

                case "Minifikacja plików JS":

                    if (etapy[3].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }
                    break;

                case "Minifikacja plików HTML":

                    if (etapy[4].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }
                    break;

                case "Optymalizacja ustawień serwera":
                    if (etapy[5].Status == StatusEtapu.wylaczony)
                    {
                        (sender as ToggleSwitch).IsChecked = false;
                    }
                    else
                    {
                        (sender as ToggleSwitch).IsChecked = true;
                    }
                    break;
                default:

                    break;
            }


        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string obecne = Thread.CurrentThread.CurrentUICulture.Name;

            if (obecne == "pl-PL")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl-PL");
            }

            var stare_okno = this;

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();

            stare_okno.Close();


        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UstawieniaEtapu nowe = new UstawieniaEtapu();
            nowe.pokaz_wszystkie();
        }
    }
}
