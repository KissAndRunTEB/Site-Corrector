using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Logika
{
    class ZnacznikiTwitter
    {
        public static void optymalizuj(Projekt projekt, Etap etap, ObservableCollection<Plik> pliki)
        {
            Parallel.ForEach(pliki, p =>
            {
                p.Status = "Trwa...";

                //obrobka pliku

                List<string> zawartosc_pliku = File.ReadAllLines(p.AdresNaDysku).ToList<string>();


                //DZIALAJ TU

                File.WriteAllLines(p.AdresNaDysku, zawartosc_pliku.ToArray<string>());

                FileInfo fi = new FileInfo(p.AdresNaDysku);
                p.Rozmiar_po_kompresji = fi.Length;

                p.Status = "Po";
                p.Status_Pliku = StatusPliku.zoptymalizowany;

                etap.Liczba_operacji_zrobionych++;
            });
        }
    }
}
