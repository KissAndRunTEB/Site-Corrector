using MahApps.Metro.Controls;
using Site_Corrector.Logika;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Site_Corrector.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy DodajProjekt.xaml
    /// </summary>
    public partial class DodajProjekt : MetroWindow
    {
        public DodajProjekt()
        {
            InitializeComponent();

            //podpowiedzi            
            pole_adres_strony.Text = "http://google.pl";
            pole_adres_serwera.Text = "ftp://google.pl/www/";
        }
        

        public void sprawdz_poprawnosc_danych()
        {
            bool mozna_dodac = true;

            if (!czy_pola_wymagane_uzupelnione())
            {
                mozna_dodac = false;
                komunkat_o_bledzie.Content = "Wszystkie pola obowiązkowe muszą być uzupełnione";


            }
            else
            {
                //nazwa nie moze byc za dluga, ani zawierac znakow specjalnych
                if (pole_nazwa_projektu.Text.Length > 20)
                {
                    mozna_dodac = false;
                    komunkat_o_bledzie.Content = "Nazwa projektu może mieć maksymalnie 20 znaków.";
                }
                else
                {
                    if ((!pole_adres_serwera.Text.EndsWith("/"))||(!pole_adres_serwera.Text.StartsWith("ftp://")))
                    {
                        mozna_dodac = false;
                        komunkat_o_bledzie.Content = "Adres serwera musi zaczynać się od ftp:// i kończyć się znakiem / i wskazywać folder główny na który kieruje domena.";
                    }
                    else
                    {

                        if (!(pole_nazwa_projektu.Text.All(c => Char.IsLetterOrDigit(c) || c == ' ')))
                        {
                            mozna_dodac = false;
                            komunkat_o_bledzie.Content = "Nazwa projektu nie może zawierać znaków specjalnych.";
                        }
                        else
                        {
                            if (!(pole_slowa_kluczowe.Text.All(c => Char.IsLetterOrDigit(c) || c == ' ' || c == ',')))
                            {
                                mozna_dodac = false;
                                komunkat_o_bledzie.Content = "Słowa kluczowe rozdzielamy przecinkiem.";
                            }
                            else
                            {
                                //conajmniej jedno slowo kluczowe
                                List<string> slowa = pole_slowa_kluczowe.Text.Split(',').ToList<string>();
                                if (slowa.Count < 1)
                                {
                                    mozna_dodac = false;
                                    komunkat_o_bledzie.Content = "Musisz dodać conajmniej jedno słowo kluczowe.";
                                }
                                else
                                {
                                    //adres www poprawny
                                    Uri uriResult;
                                    bool result = Uri.TryCreate(pole_adres_strony.Text, UriKind.Absolute, out uriResult)
                                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                                    if (!result)
                                    {
                                        mozna_dodac = false;
                                        komunkat_o_bledzie.Content = "Wpisany adres strony www musi być poprawny.";
                                    }
                                }
                            }

                        }
                    }
                }
            }


                    Serwer test = new Serwer(pole_adres_serwera.Text, pole_uzytkownik_serwera.Text, pole_haslo_serwera.Text);

            Thread w = new Thread(new ThreadStart(delegate ()
            {
                //jesli dane wygladaja ok sprawdzam polaczenie z serwerem
                if (mozna_dodac)
                {
                    komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        komunkat_o_bledzie.Content = "Łączenie z sererem FTP ...";
                    }));

                    //polaczenie z ftp
                    int kod_polaczenia = test.sprawdz_czy_mozna_polaczyc_z_kodem_bledu();
                    if (kod_polaczenia!=1)
                    {
                        mozna_dodac = false;

                        if (kod_polaczenia==0)
                        {
                            komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                komunkat_o_bledzie.Content = "Nie udało się połączyć z sererem FTP";
                            }));
                        }
                        else if (kod_polaczenia == -2)
                        {
                            komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                komunkat_o_bledzie.Content = "Nie można połączyć się z serwerem zdalnym";
                            }));
                        }
                        else if (kod_polaczenia == -3)
                        {
                            komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                komunkat_o_bledzie.Content = "Zły login lub hasło";
                            }));
                        }
                        else if (kod_polaczenia == -4)
                        {
                            komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                            {
                                komunkat_o_bledzie.Content = "Nie udało się połączyć z sererem FTP";
                            }));
                        }
                    }

                    if (!test.sprawdz_czy_cos_jest())
                    {
                        mozna_dodac = false;

                        komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            komunkat_o_bledzie.Content = "Nie znaleziono na serwerze FTP plików ani folderów, wybierz właściwy folder.";
                        }));
                    }

                    komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        komunkat_o_bledzie.Content = "";
                    }));


                }

                if (mozna_dodac)
                {
                    komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        przycisk_dodaj_projekt.IsEnabled = true;
                        komunkat_o_bledzie.Content = "";
                    }));
                }
                else
                {
                    komunkat_o_bledzie.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        przycisk_dodaj_projekt.IsEnabled = false;
                    }));
                }
            }));

            w.Start();
        }

        private bool czy_pola_wymagane_uzupelnione()
        {
            return (pole_nazwa_projektu.Text != "") &&
            (pole_adres_strony.Text != "") &&
            (pole_adres_serwera.Text != "") &&
            (pole_uzytkownik_serwera.Text != "") &&
            (pole_haslo_serwera.Text != "") &&
             (pole_slowa_kluczowe.Text != "");
        }

        private void pole_jakiekolwiek_zmienione_TextChanged(object sender, TextChangedEventArgs e)
        {
            sprawdz_poprawnosc_danych();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Projekt p = new Projekt();
            p.Nazwa = pole_nazwa_projektu.Text;
            p.AdresStrony = pole_adres_strony.Text;
            p.SlowaKluczowe = pole_slowa_kluczowe.Text;

            p.AdresSerwera = pole_adres_serwera.Text;
            p.UzytkownikFTP = pole_uzytkownik_serwera.Text;
            p.HasloFTP = pole_haslo_serwera.Text;

            p.SerwerBazy = pole_serwer_bazy.Text;
            p.NazwaBazy = pole_nazwa_bazy.Text;
            p.UzytkownikBazy = pole_uzytkownik_bazy.Text;
            p.HasloBazy = pole_haslo_bazy.Text;


            ListaProjektow.dodaj(p);

            ListaProjektow.zapisz();

            this.Close();


        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void pole_adres_serwera_GotFocus(object sender, RoutedEventArgs e)
        {
            if(pole_adres_serwera.Text== "ftp://google.pl/www/")
            {
                pole_adres_serwera.Text = "";
            }
        }

        private void pole_adres_strony_GotFocus(object sender, RoutedEventArgs e)
        {

            if (pole_adres_strony.Text == "http://google.pl")
            {
                pole_adres_strony.Text = "";
            }
            
        }
    }
}
