using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Site_Corrector
{
         public enum StatusPliku
        {
            znaleziony,
            pobrany,
            problem,
            zoptymalizowany,
            wgrany
        }

        [ValueConversion(typeof(StatusPliku), typeof(Color))]
        public class StatusPlikuToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (!(value is StatusPliku))
                {
                    //powinno zamiast tego zgłaszać wyjątek
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    StatusPliku sprawdzany = (StatusPliku)value;
                    if (sprawdzany == StatusPliku.znaleziony)
                    {
                        return new SolidColorBrush(Colors.Gray);
                    }
                    else if (sprawdzany == StatusPliku.pobrany)
                    {
                        return new SolidColorBrush(Colors.Black);
                    }
                    else if (sprawdzany == StatusPliku.zoptymalizowany)
                    {
                        return new SolidColorBrush(Colors.Green);
                    }
                    else if (sprawdzany == StatusPliku.problem)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else if (sprawdzany == StatusPliku.wgrany)
                    {
                        return new SolidColorBrush(Colors.Blue);
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
                return StatusPliku.pobrany;
            }
        }

    public class Plik : INotifyPropertyChanged
    {
        string nazwa;
        string typ;
        int procent_pobrania;
        string adres;
        string lokalizacja_na_serwerze;
        long rozmiar;
        long rozmiar_po_kompresji;

        public static int zlicz_rozmiar_plikow(ObservableCollection<Plik> pliki)
        {
            long suma = 0;
            foreach (Plik p in pliki)
            {
                suma += p.Rozmiar;
            }

            int wynik =(int)Math.Ceiling((suma/1024f));

            return wynik;
        }

        public static int zlicz_rozmiar_plikow_po_kompresji(ObservableCollection<Plik> pliki)
        {
            long suma = 0;
            foreach (Plik p in pliki)
            {
                suma += p.Rozmiar_po_kompresji;
            }

            int wynik = (int)Math.Ceiling((suma / 1024f) );

            return wynik;
        }

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

        public string Typ
        {
            get
            {
                return typ;
            }

            set
            {
                typ = value;
                OnPropertyChanged("Typ");
            }
        }

        public int Procent_pobrania
        {
            get
            {
                return procent_pobrania;
            }

            set
            {
                procent_pobrania = value;
                OnPropertyChanged("Procent_pobrania");
            }
        }

        public long Rozmiar
        {
            get { return rozmiar; }
            set
            {
                rozmiar = value;

                OnPropertyChanged("Rozmiar");
            }
        }

        public string RozmiarNapis
        {
            get {
                if (rozmiar == 0)
                {
                    return " - ";
                }

                return (Math.Ceiling(rozmiar / 1024f) ).ToString()+" KB"; }
            set
            {
               // rozmiar = value;

                OnPropertyChanged("Rozmiar");
            }
        }

        public string RozmiarPoKompresjiNapis
        {
            get {
                if(rozmiar_po_kompresji==0)
                {
                    return " - ";
                }

                return (Math.Ceiling(rozmiar_po_kompresji / 1024f) ).ToString() + " KB"; }
            set
            {
                //rozmiar_po_kompresji = value;

                OnPropertyChanged("RozmiarPoKompresjiNapis");
            }
        }

        public string RozmiarStatystykaNapis
        {
            get
            {
                return "Przed: "+ RozmiarNapis +", "+ "Po: "+ RozmiarPoKompresjiNapis;
            }
            set
            {
                //rozmiar_po_kompresji = value;

                OnPropertyChanged("RozmiarStatystykaNapis");
            }
        }

        public string ProcentRozmiar
        {
            get {
                if (rozmiar_po_kompresji == 0 && rozmiar == 0)
                {
                    return "Zysk: ...";
                }
                double t = (int)rozmiar_po_kompresji *100.0 / rozmiar;
                double t2 = Math.Floor(t);

               // int zmiana = (int)(t2);
                return "Zysk: " + (100-t2).ToString()+"%";

            }
            set
            {
               // rozmiar = value;

                OnPropertyChanged("ProcentRozmiar");
            }
        }


        public long Rozmiar_po_kompresji
        {
            get { return rozmiar_po_kompresji; }
            set
            {
                rozmiar_po_kompresji = value;

                OnPropertyChanged("Rozmiar_po_kompresji");
            }
        }

        public string AdresNaDysku
        {
            get { return adres; }
            set
            {
                adres = value;


                OnPropertyChanged("AdresNaDysku");
            }

        }

        public string LokalizacjaNaSerwerze
        {
            get
            {
                return lokalizacja_na_serwerze;
            }

            set
            {
                lokalizacja_na_serwerze = value;
                OnPropertyChanged("LokalizacjaNaSerwerze");
            }
        }


        StatusPliku status_Pliku;

        public StatusPliku Status_Pliku
        {
            get { return status_Pliku; }
            set { status_Pliku = value; OnPropertyChanged("Status_Pliku"); }
        }


        string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); }
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

    }
}
