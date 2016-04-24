using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Site_Corrector
{
    public enum StatusEtapu
    {
        dostepny,
        niedostepny,
        rozpoczety,
        zakonczony,
        niepotrzebny,
        offline,
        blad,
        wylaczony
    }

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

    public class Etap : INotifyPropertyChanged
    {
        string nazwa;
        string opis;
        int liczba_operacji_zrobionych;
        int liczba_operacji_wszystkich;

        bool czy_ma_ustawienia = false;
        bool czy_mozna_wylaczyc = false;

        StatusEtapu status = StatusEtapu.niedostepny;


        int procent_postepu;
        bool czy_indeterminate;
        Visibility czy_pokazac_pasek_postepu;

        public string Nazwa
        {
            get
            {
                return nazwa;
            }

            set
            {
                nazwa = value;
                OnPropertyChanged("Nazwa");
            }
        }



        public bool CzyMoznaWylaczyc
        {
            get
            {
                return czy_mozna_wylaczyc;
            }

            set
            {
                czy_mozna_wylaczyc = value;
                OnPropertyChanged("CzyMoznaWylaczyc");
            }
        }

        public bool CzyMaUstawienia
        {
            get
            {
                return czy_ma_ustawienia;
            }

            set
            {
                czy_ma_ustawienia = value;
                OnPropertyChanged("CzyMaUstawienia");
            }
        }

        public string Opis
        {
            get
            {
                return opis;
            }

            set
            {
                opis = value;
                OnPropertyChanged("Opis");
            }
        }

        public int Liczba_operacji_zrobionych
        {
            get
            {
                return liczba_operacji_zrobionych;
            }

            set
            {
                liczba_operacji_zrobionych = value;
                OnPropertyChanged("Liczba_operacji_zrobionych");
                OnPropertyChanged("KomunikatOPostepie");
                OnPropertyChanged("ProcentPostepu");
                OnPropertyChanged("CzyIndeterminate");
                OnPropertyChanged("CzyPokazacPasekPostepu");
            }
        }

        public int Liczba_operacji_wszystkich
        {
            get
            {
                return liczba_operacji_wszystkich;
            }

            set
            {
                liczba_operacji_wszystkich = value;
                OnPropertyChanged("Liczba_operacji_wszystkich");
                OnPropertyChanged("KomunikatOPostepie");
                OnPropertyChanged("ProcentPostepu");
                OnPropertyChanged("CzyIndeterminate");
                OnPropertyChanged("CzyPokazacPasekPostepu");
            }
        }

        public StatusEtapu Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("KomunikatOPostepie");
                OnPropertyChanged("ProcentPostepu");
                OnPropertyChanged("CzyIndeterminate");
                OnPropertyChanged("CzyPokazacPasekPostepu");
            }
        }

        public string KomunikatOPostepie
        {
            get
            {
                if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status == StatusEtapu.rozpoczety)
                {
                    return "Trwa ustalanie...";
                }
                else if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status != StatusEtapu.rozpoczety)
                {
                    return "";
                }
                else if (Status == StatusEtapu.offline)
                {
                    return "Ponawianie...";
                }
                else
                {
                    return Liczba_operacji_zrobionych.ToString() + "/" + Liczba_operacji_wszystkich.ToString();
                }
            }
        }

        public int ProcentPostepu
        {
            get
            {
                if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status == StatusEtapu.rozpoczety)
                {
                    return 0;
                }
                else if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status != StatusEtapu.rozpoczety)
                {
                    return 0;
                }
                else
                {
                    double t = (int)(Liczba_operacji_zrobionych * 100.0 / Liczba_operacji_wszystkich);
                    double t2 = Math.Floor(t);

                    return (int)t2;
                }
            }
            set
            {
                procent_postepu = value;
                OnPropertyChanged("ProcentPostepu");
            }
        }

        public bool CzyIndeterminate
        {
            get
            {
                if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status == StatusEtapu.rozpoczety)
                {
                    return true;//trwa ustalanie
                }
                else if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status != StatusEtapu.rozpoczety)
                {
                    return false;//nie wlaczony jeszcze
                }
                else
                {
                    return false;
                }
            }
            set
            {
                czy_indeterminate = value;
                OnPropertyChanged("CzyIndeterminate");
            }
        }


        public Visibility CzyPokazacPasekPostepu
        {
            get
            {
                if (Status == StatusEtapu.niedostepny || Status == StatusEtapu.niepotrzebny || Status == StatusEtapu.wylaczony || Status == StatusEtapu.dostepny)
                {
                    return Visibility.Hidden;//nie wlaczone lub nie mozna wlaczyc
                }
                else if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status == StatusEtapu.rozpoczety)
                {
                    return Visibility.Visible;//trwa ustalanie
                }
                else if (Liczba_operacji_zrobionych == 0 && Liczba_operacji_wszystkich == 0 && Status != StatusEtapu.rozpoczety)
                {
                    return Visibility.Hidden;//nie wlaczony jeszcze
                }
                else if (Status == StatusEtapu.offline)
                {
                    return Visibility.Visible;//ponawianie
                }
                else if (Status == StatusEtapu.rozpoczety)
                {
                    return Visibility.Visible;//trwa
                }
                else if (Status == StatusEtapu.zakonczony)
                {
                    return Visibility.Visible;//zakonczony
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            set
            {
                czy_pokazac_pasek_postepu = value;
                OnPropertyChanged("CzyPokazacPasekPostepu");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public static ObservableCollection<Etap> przygotuj_pobieranie_etapy()
        {
            ObservableCollection<Etap> lista = new ObservableCollection<Etap>();

            Etap nr00 = new Etap();
            nr00.Nazwa = "Łączenie...";
            nr00.Opis = "Połączenie się z serwerem";
            nr00.Status = StatusEtapu.dostepny;
            nr00.CzyMaUstawienia = true;
            lista.Add(nr00);

            Etap nr0 = new Etap();
            nr0.Nazwa = "Utworzenie projektu";
            nr0.Opis = "Pobieranie plików projektu";
            nr0.Status = StatusEtapu.dostepny;
            lista.Add(nr0);

            Etap nr1 = new Etap();
            nr1.Nazwa = "Kopia zapasowa";
            nr1.Opis = "Wykonanie archiwum zip z plikami sprzed optymalizacji";
            nr1.Status = StatusEtapu.dostepny;
            nr1.CzyMaUstawienia = true;
            lista.Add(nr1);



            return lista;
        }

        public static ObservableCollection<Etap> przygotuj_wgrywanie_etapy()
        {
            ObservableCollection<Etap> lista = new ObservableCollection<Etap>();


            Etap nr100 = new Etap();
            nr100.Nazwa = "Zapisanie zmian";
            nr100.Opis = "Wgranie na serwerem plików po optymalizacji.";
            nr100.Status = StatusEtapu.dostepny;
            lista.Add(nr100);


            return lista;
        }

        public static ObservableCollection<Etap> przygotuj_etapy(Projekt wybrany)
        {
            ObservableCollection<Etap> lista = new ObservableCollection<Etap>();

            Etap nr1 = new Etap();
            nr1.Nazwa = "Kompresja zdjęć JPG";
            nr1.Opis = "Bezstratna lub stratna kompresja plików JPG, zgodnie z ustawieniami poziomu kompresji. Bez usuwania metadanych.";

            nr1.Status = StatusEtapu.dostepny;
            nr1.CzyMaUstawienia = true;
            nr1.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_jpg;

                if (czy == false)
                {
                    nr1.Status = StatusEtapu.wylaczony;
                }
            }

            lista.Add(nr1);

            Etap nr2 = new Etap();
            nr2.Nazwa = "Kompresja grafik PNG";
            nr2.Opis = "Bezstratna kompresja plików PNG, zgodnie z ustawieniami szybkości kompresji. Bez usuwania metadanych.";
            nr2.Status = StatusEtapu.dostepny;
            nr2.CzyMaUstawienia = true;
            nr2.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_png;

                if (czy == false)
                {
                    nr2.Status = StatusEtapu.wylaczony;
                }
            }

            lista.Add(nr2);

            Etap nr3 = new Etap();
            nr3.Nazwa = "Minifikacja plików CSS";
            nr3.Opis = "Zmniejszanie rozmiaru plików CS, między innymi usuwanie komentarzy i niepotrzebnych białych znaków.";
            nr3.Status = StatusEtapu.dostepny;
            //nr3.CzyMaUstawienia = true;
            nr3.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_css;

                if (czy == false)
                {
                    nr3.Status = StatusEtapu.wylaczony;
                }
            }


            lista.Add(nr3);

            Etap nr4 = new Etap();
            nr4.Nazwa = "Minifikacja plików JS";
            nr4.Opis = "Zmniejszanie rozmiaru plików JS, między innymi usuwanie komentarzy i niepotrzebnych białych znaków.";
            nr4.Status = StatusEtapu.dostepny;
            // nr4.CzyMaUstawienia = true;
            nr4.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_js;

                if (czy == false)
                {
                    nr4.Status = StatusEtapu.wylaczony;
                }
            }


            lista.Add(nr4);

            Etap nr5 = new Etap();
            nr5.Nazwa = "Minifikacja plików HTML";
            nr5.Opis = "Zmniejszanie rozmiaru plików HTML, między innymi usuwanie komentarzy i niepotrzebnych białych znaków.";
            nr5.Status = StatusEtapu.dostepny;
            //nr5.CzyMaUstawienia = true;
            nr5.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_html;

                if (czy == false)
                {
                    nr5.Status = StatusEtapu.wylaczony;
                }
            }


            lista.Add(nr5);

            Etap nr6 = new Etap();
            nr6.Nazwa = "Optymalizacja ustawień serwera";
            nr6.Opis = "Optymalizacja pliku .htaccess";
            nr6.Status = StatusEtapu.dostepny;
            nr6.CzyMaUstawienia = true;
            nr6.CzyMoznaWylaczyc = true;

            if (wybrany != null)
            {
                bool czy = wybrany.ustawienia.czy_htaccess;

                if (czy == false)
                {
                    nr6.Status = StatusEtapu.wylaczony;
                }
            }

            lista.Add(nr6);

            //Etap nr7 = new Etap();
            //nr7.Nazwa = "Optymalizacja pliku robots.txt";
            //nr7.Opis = "Optymalizacja pliku robots.txt";
            //lista.Add(nr7);

            //Etap nr8 = new Etap();
            //nr8.Nazwa = "Optymalizacja mapy witryny";
            //nr8.Opis = "Optymalizacja pliku sitemap.xml";
            //lista.Add(nr8);


            //Etap nr9 = new Etap();
            //nr9.Nazwa = "Optymalizacja tagu keywords";
            //nr9.Opis = "Usówania lub wypełnianie tagu kewords w zależności od ustawień";
            //lista.Add(nr9);

            //Etap nr11 = new Etap();
            //nr11.Nazwa = "Optymalizacja linków";
            //nr11.Opis = "Usówanie linków 404 lub wstawianie domyślnych linków w zależności od ustawień";
            //lista.Add(nr11);

            //Etap nr12 = new Etap();
            //nr12.Nazwa = "Optymalizacja tagu zdjęć";
            //nr12.Opis = "Wstawianie domyślnych altów";
            //lista.Add(nr12);

            //Etap nr13 = new Etap();
            //nr13.Nazwa = "Optymalizacja Facebook Open Graph Tags";
            //nr13.Opis = "wkrótce...";
            //lista.Add(nr13);

            //Etap nr14 = new Etap();
            //nr14.Nazwa = "Optymalizacja Twitter Cards Tags";
            //nr14.Opis = "wkrótce...";
            //lista.Add(nr14);

            //Etap nr15 = new Etap();
            //nr15.Nazwa = "Nieproblematyczne naprawy html";
            //nr15.Opis = "Naprawy typowych błędów HTML, które nie spowodują problemów";
            //lista.Add(nr15);

            //Etap nr16 = new Etap();
            //nr16.Nazwa = "Skonfigurowanie viewport";
            //nr16.Opis = "\"...< meta name = viewport content = \"width = device - width, initial - scale = 1\" >";
            //lista.Add(nr16);

            //Etap nr17 = new Etap();
            //nr17.Nazwa = "Wprowadzenie CDN";
            //nr17.Opis = "Zamienie styli CSS i plików JS odpowiednikami z znaznych i damrowych CDN";
            //lista.Add(nr17);

            //Etap nr18 = new Etap();
            //nr18.Nazwa = "Optymalizacja bazy danych";
            //nr18.Opis = "Polecenia ...";
            //lista.Add(nr18);

            return lista;
        }

    }
}
