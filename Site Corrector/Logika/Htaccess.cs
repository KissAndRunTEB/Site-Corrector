using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Logika
{
    class Htaccess
    {
        public static void optymalizuj(Projekt projekt, Etap etap, ObservableCollection<Plik> pliki_htaccess)
        {
            Parallel.ForEach(pliki_htaccess, p =>
            {
                p.Status = "Trwa...";

                //obrobka pliku

                List<string> zawartosc_pliku = File.ReadAllLines(p.AdresNaDysku).ToList<string>();



                if (projekt.ustawienia.czy_deflate)
                {
                    //jesli nie ma dodaj
                    if (!zawartosc_pliku.Exists(x => x.Contains("mod_deflate.c")))
                    {
                        zawartosc_pliku.InsertRange(0, formulka_deflate());
                    }

                }

                if (projekt.ustawienia.czy_expires)
                {
                    //jesli nie ma dodaj
                    if (!zawartosc_pliku.Exists(x => x.Contains("mod_expires.c")))
                    {
                        zawartosc_pliku.InsertRange(0, formulka_expires());
                    }
                }

                File.WriteAllLines(p.AdresNaDysku, zawartosc_pliku.ToArray<string>());

                FileInfo fi = new FileInfo(p.AdresNaDysku);
                p.Rozmiar_po_kompresji = fi.Length;

                p.Status = "Po";
                p.Status_Pliku = StatusPliku.zoptymalizowany;

                etap.Liczba_operacji_zrobionych++;
            });
        }



        private static List<string> formulka_deflate()
        {
            List<string> formulka = new List<string>();

            formulka.Add("<IfModule mod_deflate.c>");
            formulka.Add("SetOutputFilter DEFLATE");
            formulka.Add("AddOutputFilterByType DEFLATE text/plain");
            formulka.Add("AddOutputFilterByType DEFLATE text/html");
            formulka.Add("AddOutputFilterByType DEFLATE text/xml");
            formulka.Add("AddOutputFilterByType DEFLATE text/css");
            formulka.Add("AddOutputFilterByType DEFLATE application/xml");
            formulka.Add("AddOutputFilterByType DEFLATE application/xhtml+xml");
            formulka.Add("AddOutputFilterByType DEFLATE application/rss+xml");
            formulka.Add("AddOutputFilterByType DEFLATE application/javascript");
            formulka.Add("AddOutputFilterByType DEFLATE application/x-javascript");
            formulka.Add("</IfModule>");

            return formulka;
        }

        private static List<string> formulka_expires()
        {
            List<string> formulka = new List<string>();
       
            formulka.Add("<IfModule mod_expires.c>");
            formulka.Add("ExpiresActive On");
            formulka.Add("ExpiresByType image/jpg \"access 1 year\"");
            formulka.Add("ExpiresByType image/jpeg \"access 1 year\"");
            formulka.Add("ExpiresByType image/gif \"access 1 year\"");
            formulka.Add("ExpiresByType image/png \"access 1 year\"");
            formulka.Add("ExpiresByType text/css \"access 1 month\"");
            formulka.Add("ExpiresByType text/x-javascript \"access 1 month\"");
            formulka.Add("ExpiresByType image/x-icon \"access 1 month\"");
            formulka.Add("ExpiresDefault \"access 7 days\"");
            formulka.Add("</IfModule>");

            return formulka;
        }
    }
}