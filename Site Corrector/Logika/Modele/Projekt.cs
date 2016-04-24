using System;
using System.Collections.Generic;
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

    public enum StatusProjektu
    {
        domyslny,
        rozpoczeto,
        zoptymalizowano,
        blad
    }

    public class Projekt : INotifyPropertyChanged
    {
        string nazwa;
        string adres_strony;
        string slowa_kluczowe;

        string adres_serwera;
        string uzytkownikFTP;
        string hasloFTP;

        string serwer_bazy;
        string nazwa_bazy;
        string uzytkownik_bazy;
        string haslo_bazy;

        string favicona;

        string podglad;

        public string data_pobrania="";
        public string data_optymalizacji ="";


        public StatusProjektu status = StatusProjektu.domyslny;

        public Ustawienia ustawienia = new Ustawienia();


        public Ustawienia Ustawienia
        {
            get
            {
                return ustawienia;
            }

            set
            {
                ustawienia = value;
                OnPropertyChanged("Ustawienia");
            }
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

        public string AdresStrony
        {
            get
            {
                return adres_strony;
            }

            set
            {
                adres_strony = value;
                OnPropertyChanged("AdresStrony");
            }
        }

        public string AdresSerwera
        {
            get
            {
                return adres_serwera;
            }

            set
            {
                adres_serwera = value;
                OnPropertyChanged("AdresSerwera");
            }
        }

        public string UzytkownikFTP
        {
            get
            {
                return uzytkownikFTP;
            }

            set
            {
                uzytkownikFTP = value;
                OnPropertyChanged("UzytkownikFTP");
            }
        }

        public string HasloFTP
        {
            get
            {
                return hasloFTP;
            }

            set
            {
                hasloFTP = value;
                OnPropertyChanged("hasloFTP");
            }
        }

        public string SlowaKluczowe
        {
            get
            {
                return slowa_kluczowe;
            }

            set
            {
                slowa_kluczowe = value;
                OnPropertyChanged("SlowaKluczowe");
            }
        }

        public string SerwerBazy
        {
            get
            {
                return serwer_bazy;
            }

            set
            {
                serwer_bazy = value;
                OnPropertyChanged("SerwerBazy");
            }
        }

        public string NazwaBazy
        {
            get
            {
                return nazwa_bazy;
            }

            set
            {
                nazwa_bazy = value;
                OnPropertyChanged("NazwaBazy");
            }
        }

        public string UzytkownikBazy
        {
            get
            {
                return uzytkownik_bazy;
            }

            set
            {
                uzytkownik_bazy = value;
                OnPropertyChanged("UzytkownikBazy");
            }
        }

        public string HasloBazy
        {
            get
            {
                return haslo_bazy;
            }

            set
            {
                haslo_bazy = value;
                OnPropertyChanged("HasloBazy");
            }
        }

        public string Favicona
        {
            get
            {
               // return favicona;
               if(favicona!=null&&favicona!="")
                {
                    return favicona;
                }
                return "pack://application:,,,/Site Corrector;component/Okna/Domyslne/Favicon.png";
            }

            set
            {
                favicona = value;
                OnPropertyChanged("Favicona");
            }
        }

        public string Podglad
        {
            get
            {
                // return favicona;
                if (podglad != null && podglad != "")
                {
                    return podglad;
                }
                return "pack://application:,,,/Site Corrector;component/Okna/Domyslne/Favicon.png";
            }

            set
            {
                podglad = value;
                OnPropertyChanged("Podglad");
            }
        }

        public string Folder_projektu
        {
            get
            {
                return Program.gdzie_folder_programu + "\\" + Nazwa;
            }
        }

        public string Folder_projektu_oryginal
        {
            get
            {
                return Program.gdzie_folder_programu + "\\" + Nazwa + "\\Oryginal";
            }
        }

        public string Folder_projektu_po_optymalizacji
        {
            get
            {
                return Program.gdzie_folder_programu + "\\" + Nazwa + "\\Po optymalizacji";
            }
        }

        public string Folder_projektu_oryginal_ZIP
        {
            get
            {
                return Program.gdzie_folder_programu + "\\" + Nazwa + "\\Kopia zapasowa " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".zip";
            }
        }

        public StatusProjektu Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public string KomunikatOStatusie
        {
            get
            {
                if (Status == StatusProjektu.domyslny)
                {
                    return "";
                }
                else if (Status == StatusProjektu.rozpoczeto)
                {
                    return "- rozpoczęto optymalizacje";
                }
                else if (Status == StatusProjektu.zoptymalizowano)
                {
                    return "- zoptymalizowano";
                }
                else if (Status == StatusProjektu.blad)
                {
                    return "- wystąpił błąd";
                }
                else
                {
                    return "";
                }
            }
        }

        public string DataPobrania
        {
            get
            {
                if (data_pobrania == "")
                {
                    return " - ";
                }
                return data_pobrania;
            }

            set
            {
                data_pobrania = value;
                OnPropertyChanged("DataPobrania");
            }
        }

        public string DataOptymalizacji
        {
            get
            {
                if(data_optymalizacji=="")
                {
                    return " - ";
                }
                return data_optymalizacji;
            }

            set
            {
                data_optymalizacji = value;
                OnPropertyChanged("DataOptymalizacji");
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

    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class CzyMaUstawieniaToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return Visibility.Collapsed;
            }
            else
            {
                bool sprawdzany = (bool)value;
                if (sprawdzany == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //brak
            return false;
        }

    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class CzyMoznaWylaczycToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return Visibility.Collapsed;
            }
            else
            {
                bool sprawdzany = (bool)value;
                if (sprawdzany == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //brak
            return false;
        }

    }

    [ValueConversion(typeof(StatusProjektu), typeof(Color))]
    public class StatusProjektuToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StatusProjektu))
            {
                //powinno zamiast tego zgłaszać wyjątek
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                StatusProjektu sprawdzany = (StatusProjektu)value;
                if (sprawdzany == StatusProjektu.domyslny)
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
                }
                else if ((sprawdzany == StatusProjektu.rozpoczeto))
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
                }
                else if ((sprawdzany == StatusProjektu.zoptymalizowano))
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));//zielony
                }
                else if ((sprawdzany == StatusProjektu.blad))
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));//czerwony
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));//jasno szary
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
            return StatusProjektu.domyslny;
        }
    }
}
