using Biblioteka;
using Site_Corrector.Logika;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Site_Corrector
{
    class Optymalizacja
    {
        static public bool polacz(Projekt projekt, ObservableCollection<Etap> etapy)
        {
            Etap etap = etapy[0];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;
            etap.Liczba_operacji_wszystkich = 3;
            
            //TODO: sprawdzanie polaczenia z serwerem
            if (!Internet.czy_polaczenie())
            {
                etap.Status = StatusEtapu.blad;
                return false;
            }
            else
            {
                //plus za to ze jest polaczenie
                etap.Liczba_operacji_zrobionych = etap.Liczba_operacji_zrobionych + 1;

                //TODO: sprawdzanie polaczenia z baza
                etap.Liczba_operacji_zrobionych = etap.Liczba_operacji_zrobionych + 1;
                //Thread.Sleep(500);

                //TODO: sprawdzanie polaczenia z strona
                etap.Liczba_operacji_zrobionych = etap.Liczba_operacji_zrobionych + 1;
                //Thread.Sleep(500);

                //ze skonczylo
                etap.Status = StatusEtapu.zakonczony;

                return true;
            }
        }

        static public void wgraj(
                        Projekt projekt,
            ObservableCollection<Etap> etapy,
            ObservableCollection<Plik> pliki,
            ListBox lista_plikow,
            int proba_nr=0
            )
        {
            Etap etap = etapy[etapy.Count-1];
            etap.Liczba_operacji_wszystkich = lista_plikow.Items.Count;

            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;


            if (!Internet.czy_polaczenie())
            {
                //ponawianie
                if (proba_nr < Program.ile_razy_ponawiac_polaczenie)
                {
                    proba_nr++;
                    etap.Status = StatusEtapu.offline;
                    Thread.Sleep(30 * 1000);
                    //po minucie sprobuj jeszcze raz;
                    wgraj(projekt, etapy, pliki, lista_plikow,proba_nr);
                }
                else
                {
                    etap.Status = StatusEtapu.blad;

                    projekt.Status = StatusProjektu.blad;
                }
            }
            else
            {
                Serwer ftp = new Serwer(projekt.AdresSerwera, projekt.UzytkownikFTP, projekt.HasloFTP);

                int skopiowanych = 0;
                int wszystkich = lista_plikow.Items.Count;


                etap.Liczba_operacji_wszystkich = wszystkich;

                foreach (Plik p in lista_plikow.Items)
                {
                    ftp.wgrajPlik(projekt, ftp, p, etap);
                    skopiowanych++;

                    lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        etap.Liczba_operacji_zrobionych = skopiowanych;
                    }));

                }

                //ze skonczylo
                etap.Status = StatusEtapu.zakonczony;
            }
        }

        static public void pobierz_strone(
            Projekt projekt,
            ObservableCollection<Etap> etapy,
            ObservableCollection<Plik> pliki,
            ListBox lista_plikow
            )
        {
            Etap etap = etapy[1];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            //czyszczenie starych plikow
            System.IO.Directory.Delete(projekt.Folder_projektu_oryginal, true);
            System.IO.Directory.Delete(projekt.Folder_projektu_po_optymalizacji, true);

            System.IO.Directory.CreateDirectory(projekt.Folder_projektu_oryginal);
            System.IO.Directory.CreateDirectory(projekt.Folder_projektu_po_optymalizacji);

            Serwer ftp = new Serwer(projekt.AdresSerwera, projekt.UzytkownikFTP, projekt.HasloFTP);

            string tymcz_opis = etap.Opis;
            ftp.pobierzListePlikow(projekt, projekt.AdresSerwera, pliki, lista_plikow, etap);
            etap.Opis = tymcz_opis;

            List<string> lista = ftp.lista_plikow;

            int skopiowanych = 0;
            int wszystkich = lista.Count();


            etap.Liczba_operacji_wszystkich = wszystkich;

            foreach (string s in lista)
            {
                    ftp.pobierzPlik(s, projekt.Folder_projektu_oryginal + "\\" + ftp.sama_sciezka(s).Replace('/', '\\').Replace(':',' '));
                    skopiowanych++;

                    lista_plikow.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        etap.Liczba_operacji_zrobionych = skopiowanych;
                    }));
            }

            etap.Status = StatusEtapu.zakonczony;
        }

        static public void etap_kopia_bezpieczenstwa(
            Projekt projekt,
            ObservableCollection<Etap> etapy,
            ObservableCollection<Plik> pliki,
            ListBox lista_plikow
            )
        {
            Etap etap = etapy[2];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;
            etap.Liczba_operacji_wszystkich = 3;


            //skopiowanie plikow do folderu Po optymalizacji
            Foldery.copy(projekt.Folder_projektu_oryginal, projekt.Folder_projektu_po_optymalizacji, true, true);
            etap.Liczba_operacji_zrobionych++;

            //kompresja folderu z oryginalem
            ZipFile.CreateFromDirectory(projekt.Folder_projektu_oryginal, projekt.Folder_projektu_oryginal_ZIP);
            etap.Liczba_operacji_zrobionych++;

            etap.Liczba_operacji_zrobionych++;

            etap.Status = StatusEtapu.zakonczony;
        }

        public static ObservableCollection<Plik> stworzenie_list_plikow(
            Projekt projekt,
            ObservableCollection<Etap> etapy,
            ObservableCollection<Plik> pliki,
            ListBox lista_plikow
            )
        {
            pliki = Foldery.lista_plikow("*", projekt.Folder_projektu_po_optymalizacji);


            ObservableCollection<Plik> pliki_jpg = pliki_o_typie(pliki, ".jpg");
            ObservableCollection< Plik > pliki_png = pliki_o_typie(pliki, ".png");
            ObservableCollection<Plik> pliki_css = pliki_o_typie(pliki, ".css");
            ObservableCollection< Plik > pliki_js = pliki_o_typie(pliki, ".js");
            ObservableCollection<Plik> pliki_html = pliki_o_typie(pliki, ".html");
            ObservableCollection< Plik > pliki_php = pliki_o_typie(pliki, ".php");


            etapy[0].Liczba_operacji_wszystkich = pliki_jpg.Count;
            etapy[1].Liczba_operacji_wszystkich = pliki_png.Count;
            etapy[2].Liczba_operacji_wszystkich = pliki_css.Count;
            etapy[3].Liczba_operacji_wszystkich = pliki_js.Count;
            etapy[4].Liczba_operacji_wszystkich = pliki_html.Count;

            return pliki;
        }

        public static ObservableCollection<Plik> pliki_o_typie(ObservableCollection<Plik> pliki, string typ)
        {
            ObservableCollection<Plik> pliki_o_typie = new ObservableCollection<Plik>();

            foreach (Plik plik in pliki)
            {
                if (plik.Typ == typ)
                {
                    pliki_o_typie.Add(plik);
                }
            }

            return pliki_o_typie;
        }
        public static void etap_kompresja_JPG(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[0];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            if (!projekt.ustawienia.czy_jpg)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {

                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".jpg");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Kompresja.jpg(etap, pliki_do_obrobki, projekt.ustawienia.jakosc_JPG);
                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }

        public static void zaktualizuj_liste_plikow(ObservableCollection<Plik> pliki, ObservableCollection<Plik> lista_nowa)
        {
            foreach (Plik p in lista_nowa)
            {
                Plik do_zmiany = (Plik)pliki.First(x => x.Nazwa == p.Nazwa);

                do_zmiany = p;
            }
        }

        public static void etap_plik_htaccess(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[5];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            if (!projekt.ustawienia.czy_htaccess)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {
                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".htaccess");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Htaccess.optymalizuj(projekt, etap, pliki_do_obrobki);




                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }


        public static void etap_kompresja_PNG(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[1];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            if (!projekt.ustawienia.czy_png)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {

                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".png");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Kompresja.png(projekt, etap, pliki_do_obrobki, projekt.ustawienia.jakosc_PNG);
                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }



        public static void etap_kompresja_CSS(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[2];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;
            if (!projekt.ustawienia.czy_css)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {

                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".css");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Kompresja.css(etap, pliki_do_obrobki);
                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }



        public static void etap_kompresja_JS(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[3];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            if (!projekt.ustawienia.czy_js)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {
                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".js");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Kompresja.js(etap, pliki_do_obrobki);
                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }

        public static void etap_kompresja_HTML(Projekt projekt, ObservableCollection<Etap> etapy, ObservableCollection<Plik> pliki)
        {
            Etap etap = etapy[4];
            //ze trwa
            etap.Status = StatusEtapu.rozpoczety;

            if (!projekt.ustawienia.czy_html)
            {
                etap.Status = StatusEtapu.wylaczony;
            }
            else
            {

                ObservableCollection<Plik> pliki_do_obrobki = pliki_o_typie(pliki, ".html");


                etap.Liczba_operacji_wszystkich = pliki_do_obrobki.Count;

                Kompresja.html(etap, pliki_do_obrobki);
                zaktualizuj_liste_plikow(pliki, pliki_do_obrobki);

                etap.Status = StatusEtapu.zakonczony;
            }
        }


        static public void wgraj_strone()
        {
            //ze trwa

            //ze skonczylo
        }
    }
}
