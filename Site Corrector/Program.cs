using Site_Corrector.Konta;
using Site_Corrector.Logika.Modele;
using Site_Corrector.Okna;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Site_Corrector
{
    class Program
    {
        public static string nazwa = "Site Corrector";
        public static string wersja = "0.001";

        //ustawienia
        public static string gdzie_folder_programu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Program.nazwa;
        public static int ile_razy_ponawiac_polaczenie = 5;
        public static int co_ile_ponawiac_polaczenie = 15;
        public static string sol = "sd9ndskof%^3jsid";


        #region Instalacja

        public static bool nowy()
        {
            //jest albo nowa instalacja albo aktualizacja
            DirectoryInfo folder_programu = new DirectoryInfo(Adresy.program);
            if (folder_programu.Exists)
            {
                return false;
            }

            return true;
        }

        public static bool zainstaluj()
        {
            if (nowy())
            {
                tworz_foldery_programu();

                Start okno = new Start();
                okno.ShowDialog();

//TODO: instalacja i aktualizacja

                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Zmiana> lista_zmian()
        {
            List<Zmiana> lista = new List<Zmiana>();


            lista.Add(new Zmiana("0.020", "Nowy wygląd okna głównego", "", TypZmiany.InterfejsUzytkownika));
            lista.Add(new Zmiana("0.020", "Nowy wygląd listy zmian", "", TypZmiany.InterfejsUzytkownika));
            lista.Add(new Zmiana("0.020", "Wstępna obsługa interfejsu EN", "", TypZmiany.InterfejsUzytkownika));
            lista.Add(new Zmiana("0.020", "Profil - Konto testowe", "", TypZmiany.NowaFunkcja));
            lista.Add(new Zmiana("0.020", "Wyświetlanie statystyk PageSpeed Insights", "", TypZmiany.NowaFunkcja));
            lista.Add(new Zmiana("0.020", "Nowy wygląd listy projektów", "", TypZmiany.NowaFunkcja));


            return lista;
        }


        public static void tworz_foldery_programu()
        {
            //tworzenie folderów
            DirectoryInfo folder_programu = new DirectoryInfo(Adresy.program);
            folder_programu.Create();
            DirectoryInfo folder_na_ustawienia = new DirectoryInfo(Adresy.ustawienia);
            folder_na_ustawienia.Create();
            DirectoryInfo folder_na_grafike = new DirectoryInfo(Adresy.grafika);
            folder_na_grafike.Create();
        }

        #endregion

        public static void licencjonowanie()
        {
            bool licencja_wazna = false;


            //DateTime data_konca = new DateTime(2015, 12, 1);
            DateTime data_konca = new DateTime(2016, 5, 30);

            if(data_konca.CompareTo(DateTime.Now)>=0)
            {
                licencja_wazna = true;
            }

            if(!licencja_wazna)
            {
                Program.loguj("Licencja wygasła");

                Program.loguj("Zamykanie programu ...");

                System.Windows.Application.Current.Shutdown();
            }
        }

        #region Logowanie do pliku

        private static readonly object _syncObject = new object();
        public static void loguj(string tekst)
        {
            string nazwa_pliku = Adresy.ustawienia + "\\" + "Log" + ".txt";

            string linia_tekstu = DateTime.Now.ToString("HH:mm:ss") + ((char)9) + tekst;

            //StreamWriter pisanie = File.AppendText(nazwa_pliku);

            //lock (pisanie)
            //{
            //pisanie.Write(DateTime.Now.ToString("HH:mm:ss"));
            //pisanie.Write((char)9);
            //pisanie.WriteLine(tekst);
            //pisanie.WriteLine("");

            //pisanie.Flush();
            //pisanie.Close();
            //}


            lock (_syncObject)
            {
                StreamWriter pisanie = File.AppendText(nazwa_pliku);

                pisanie.WriteLine(linia_tekstu);
                pisanie.WriteLine("");

                pisanie.Flush();

                pisanie.Close();
            }
        }

        public static void loguj_start()
        {
            string nazwa_pliku = Adresy.ustawienia + "//" + "Log" + ".txt";

            lock (_syncObject)
            {
                StreamWriter pisanie = File.AppendText(nazwa_pliku);

                pisanie.WriteLine("----------------------------------------------------");

                pisanie.Write("Uruchomienie " + nazwa + " ");
                pisanie.Write(wersja);
                pisanie.Write((char)9);
                pisanie.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                pisanie.WriteLine("");

                pisanie.Flush();
                pisanie.Close();

            }
        }
        #endregion


        #region Serializacja
        public static void Serializuj_do_XML<T>(T objekt, string nazwa)
        {
            StreamWriter zapisywanie = new StreamWriter(Adresy.ustawienia + "//" + nazwa + ".xml");

            XmlRootAttribute korzen = new XmlRootAttribute();
            korzen.ElementName = nazwa;
            korzen.IsNullable = true;

            XmlSerializer serializator = new XmlSerializer(typeof(T), korzen);
            serializator.Serialize(zapisywanie, objekt);

            zapisywanie.Close();
        }

        public static T Deserializuj_z_XML<T>(string nazwa, string skad = "")
        {
            if (skad == "")
            {
                skad = Adresy.ustawienia;
            }
            StreamReader odczytywanie = new StreamReader(skad + "//" + nazwa + ".xml");

            XmlRootAttribute korzen = new XmlRootAttribute();
            korzen.ElementName = nazwa;
            korzen.IsNullable = true;

            T wynik;
            XmlSerializer deserilizowanie = new XmlSerializer(typeof(T), korzen);
            wynik = (T)deserilizowanie.Deserialize(odczytywanie);

            odczytywanie.Dispose();
            odczytywanie.Close();

            return wynik;
        }
        #endregion

    }
}
