using MahApps.Metro.Controls;
using Site_Corrector.Logika;
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
    /// Logika interakcji dla klasy DodajProjektSzybko.xaml
    /// </summary>
    public partial class DodajProjektSzybko : MetroWindow
    {
        public DodajProjektSzybko()
        {
            InitializeComponent();

            //podpowiedzi            
            pole_adres_strony.Text = "http://google.pl";

        }




        public bool sprawdz_poprawnosc_danych()
        {
            bool mozna_dodac = true;

            if (!czy_pola_wymagane_uzupelnione())
            {
                mozna_dodac = false;
                komunkat_o_bledzie.Content = "All required fields has to be filled.";
            }
            else
            {
                //nazwa nie moze byc za dluga, ani zawierac znakow specjalnych
                if (pole_nazwa_projektu.Text.Length > 20)
                {
                    mozna_dodac = false;
                    komunkat_o_bledzie.Content = "Project name has to be up to 20 characters lenght.";
                }
                else
                {
                        if (!(pole_nazwa_projektu.Text.All(c => Char.IsLetterOrDigit(c) || c == ' ')))
                        {
                            mozna_dodac = false;
                            komunkat_o_bledzie.Content = "Project name can not contains special characters.";
                        }
                        else
                        {
                            if (!(pole_slowa_kluczowe.Text.All(c => Char.IsLetterOrDigit(c) || c == ' ' || c == ',')))
                            {
                                mozna_dodac = false;
                                komunkat_o_bledzie.Content = "Keyword need to be seperated by coma.";
                            }
                            else
                            {
                                //conajmniej jedno slowo kluczowe
                                List<string> slowa = pole_slowa_kluczowe.Text.Split(',').ToList<string>();
                                if (slowa.Count < 1)
                                {
                                    mozna_dodac = false;
                                    komunkat_o_bledzie.Content = "You have to add at least 1 keyword.";
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
                                        komunkat_o_bledzie.Content = "Website address is not correct.";
                                    }
                                    else
                                {
                                    mozna_dodac = true;
                                    komunkat_o_bledzie.Content = "";
                                }
                                }
                            }
                    }
                }
            }

            if (mozna_dodac)
            {
                    przycisk_dodaj_projekt.IsEnabled = true;
                    komunkat_o_bledzie.Content = "";
            }

            return mozna_dodac;
        }

        private bool czy_pola_wymagane_uzupelnione()
        {
            return (pole_nazwa_projektu.Text != "") &&
            (pole_adres_strony.Text != "") &&
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


            p.AdresSerwera = "";
            p.UzytkownikFTP = "";
            p.HasloFTP = "";

            p.SerwerBazy = "";
            p.NazwaBazy = "";
            p.UzytkownikBazy = "";
            p.HasloBazy = "";



            ListaProjektow.dodaj(p);

            ListaProjektow.zapisz();

            this.Close();


        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
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
