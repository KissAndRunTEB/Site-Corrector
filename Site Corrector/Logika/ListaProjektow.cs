using Biblioteka;
using Site_Corrector.Konta;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Site_Corrector.Logika
{
    [Serializable]
    public class ListaProjektow: ObservableCollection<Projekt>, INotifyPropertyChanged
    {
        static public void wyczysc()
        {
            (App.Current.Resources["lista_projektow"] as ListaProjektow).Clear();
        }
        static public void dodaj(Projekt s)
        {
            System.IO.Directory.CreateDirectory(s.Folder_projektu);
            System.IO.Directory.CreateDirectory(s.Folder_projektu_oryginal);
            System.IO.Directory.CreateDirectory(s.Folder_projektu_po_optymalizacji);

            (App.Current.Resources["lista_projektow"] as ListaProjektow).Add(s);
        }

        public static void usun(Projekt s)
        {
            System.IO.Directory.Delete(s.Folder_projektu,true);

            (App.Current.Resources["lista_projektow"] as ListaProjektow).Remove(s);
        }
        public static void zapisz()
        {
            ListaProjektow lista = (App.Current.Resources["lista_projektow"] as ListaProjektow);

            XmlWriterSettings sledzone = new XmlWriterSettings();
            sledzone.Indent = true; //oddziela nowa linia

            XmlWriter plik_ustawien = XmlWriter.Create(Adresy.ustawienia + "\\ListaProjektow.xml", sledzone);

            plik_ustawien.WriteStartDocument();
            plik_ustawien.WriteComment("Lista projektów dodanych przez użytkownika");

            plik_ustawien.WriteStartElement("lista_projektow");

            foreach (Projekt projekt in lista)
            {
                plik_ustawien.WriteStartElement("projekt");
                plik_ustawien.WriteAttributeString("nazwa", projekt.Nazwa);
                plik_ustawien.WriteAttributeString("adresStrony", projekt.AdresStrony);
                plik_ustawien.WriteAttributeString("slowaKluczowe", projekt.SlowaKluczowe);

                plik_ustawien.WriteAttributeString("adresSerwera", projekt.AdresSerwera);
                plik_ustawien.WriteAttributeString("uzytkownikFTP", projekt.UzytkownikFTP);
                plik_ustawien.WriteAttributeString("hasloFTP", Szyfrowanie.Encrypt(projekt.HasloFTP, Program.sol));

                plik_ustawien.WriteAttributeString("serwerBazy", projekt.SerwerBazy);
                plik_ustawien.WriteAttributeString("nazwaBazy", projekt.NazwaBazy);
                plik_ustawien.WriteAttributeString("uzytkownikBazy", projekt.UzytkownikBazy);
                plik_ustawien.WriteAttributeString("hasloBazy", Szyfrowanie.Encrypt(projekt.HasloBazy, Program.sol));

                plik_ustawien.WriteAttributeString("dataPobrania", projekt.data_pobrania);
                plik_ustawien.WriteAttributeString("dataOptymalizacji", projekt.data_optymalizacji);


                plik_ustawien.WriteAttributeString("czycss", projekt.ustawienia.czy_css.ToString());
                plik_ustawien.WriteAttributeString("czydeflate", projekt.ustawienia.czy_deflate.ToString());

                plik_ustawien.WriteAttributeString("czyexpires", projekt.ustawienia.czy_expires.ToString());
                plik_ustawien.WriteAttributeString("czyhtaccess", projekt.ustawienia.czy_htaccess.ToString());

                plik_ustawien.WriteAttributeString("czyhtml", projekt.ustawienia.czy_html.ToString());
                plik_ustawien.WriteAttributeString("czyjpg", projekt.ustawienia.czy_jpg.ToString());

                plik_ustawien.WriteAttributeString("czyjs", projekt.ustawienia.czy_js.ToString());
                plik_ustawien.WriteAttributeString("czypng", projekt.ustawienia.czy_png.ToString());

                plik_ustawien.WriteAttributeString("jakoscJPG", projekt.ustawienia.jakosc_JPG.ToString());
                plik_ustawien.WriteAttributeString("jakoscPNG", projekt.ustawienia.jakosc_PNG.ToString());

                plik_ustawien.WriteAttributeString("jakiepliki", projekt.ustawienia.jakie_pliki.ToString());




                plik_ustawien.WriteEndElement();
            }

            plik_ustawien.WriteEndElement();

            plik_ustawien.Flush();
            plik_ustawien.Close();

        }

        public static void wczytaj()
        {
            ListaProjektow.wyczysc();

            ListaProjektow lista = new ListaProjektow();

            FileInfo plik_z_lista = new FileInfo(Adresy.ustawienia + "\\ListaProjektow.xml");
            if (plik_z_lista.Exists)
            {
                XmlDocument dokument_xml = new XmlDocument(); dokument_xml.Load(Adresy.ustawienia + "\\ListaProjektow.xml");
                XmlNodeList lista_w_seriali = dokument_xml.GetElementsByTagName("projekt");


                foreach (XmlNode projekt_z_pliku in lista_w_seriali)
                {
                    XmlNodeList lista_w_sezonow = projekt_z_pliku.ChildNodes;

                    string nazwa = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "nazwa");
                    string adresStrony = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "adresStrony");
                    string slowaKluczowe = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "slowaKluczowe");

                    string adresSerwera = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "adresSerwera");
                    string uzytkownikFTP = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "uzytkownikFTP");
                    string hasloFTP =Szyfrowanie.Decrypt((string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "hasloFTP"), Program.sol);

                    string serwerBazy = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "serwerBazy");
                    string nazwaBazy = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "nazwaBazy");
                    string uzytkownikBazy = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "uzytkownikBazy");
                    string hasloBazy = Szyfrowanie.Decrypt((string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "hasloBazy"), Program.sol);


                    string data_pobrania = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "dataPobrania");
                    string data_optymalizacji = (string)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "dataOptymalizacji");

                    //ustawienia projektu
                    int jakoscJPG = (int)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "jakoscJPG", "Int32");
         int jakoscPNG = (int)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "jakoscPNG", "Int32");

         bool czyjpg = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czyjpg", "Boolean");
         bool czypng = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czypng", "Boolean");

                    bool czycss = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czycss", "Boolean");
                    bool czyjs = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czyjs", "Boolean");

                    bool czyhtml = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czyhtml", "Boolean");

                    bool czyhtaccess = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czyhtaccess", "Boolean");

                    bool czydeflate = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czydeflate", "Boolean");
                    bool czyexpires = (bool)wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "czyexpires", "Boolean");




                    Ustawienia.PlikiDoPobrania jakiepliki = (Ustawienia.PlikiDoPobrania)(wartosc_atrybutu_jesli_istnieje(projekt_z_pliku, "jakiepliki", "PlikiDoPobrania"));


                    Ustawienia ustawienia = new Ustawienia();
                    ustawienia.czy_css = czycss;
                    ustawienia.czy_deflate = czydeflate;
                    ustawienia.czy_expires = czyexpires;
                    ustawienia.czy_htaccess = czyhtaccess;
                    ustawienia.czy_html = czyhtml;
                    ustawienia.czy_jpg = czyjpg;
                    ustawienia.czy_js = czyjs;
                    ustawienia.czy_png = czypng;
                    ustawienia.jakie_pliki = jakiepliki;
                    ustawienia.jakosc_JPG = jakoscJPG;
                    ustawienia.jakosc_PNG = jakoscPNG;


                    

                    //bool ulubiony = (bool)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "ulubiony", "Boolean");

                    //StatusSerialu status = (StatusSerialu)(wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "status", "StatusSerialu"));

                    Projekt projekt = new Projekt();
                    projekt.Nazwa = nazwa;
                    projekt.AdresStrony = adresStrony;
                    projekt.SlowaKluczowe = slowaKluczowe;

                    projekt.AdresSerwera = adresSerwera;
                    projekt.UzytkownikFTP = uzytkownikFTP;
                    projekt.HasloFTP = hasloFTP;

                    projekt.DataPobrania = data_pobrania;
                    projekt.DataOptymalizacji = data_optymalizacji;

                    projekt.SerwerBazy = serwerBazy;
                    projekt.NazwaBazy = nazwaBazy;
                    projekt.UzytkownikBazy = uzytkownikBazy;
                    projekt.HasloBazy = hasloBazy;

                    projekt.ustawienia = ustawienia;

                    //serial = ustaw_status(serial);

                    ListaProjektow.dodaj(projekt);
                }
            }
        }

        private static object wartosc_atrybutu_jesli_istnieje(XmlNode serial_z_pliku, string atrybut, string typ = "")
        {
            if (serial_z_pliku.Attributes[atrybut] != null)
            {
                string wartosc_atrybutu = serial_z_pliku.Attributes[atrybut].InnerText;
                if (typ == "")
                {
                    return wartosc_atrybutu;
                }
                else if (typ == "Int32")
                {
                    int wynik;
                    if (Int32.TryParse(wartosc_atrybutu, out wynik))
                    {
                        return wynik;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (typ == "Boolean")
                {
                    bool wynik;
                    if (Boolean.TryParse(wartosc_atrybutu, out wynik))
                    {
                        return wynik;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (typ == "PlikiDoPobrania")
                {
                    return (Ustawienia.PlikiDoPobrania)Enum.Parse(typeof(Ustawienia.PlikiDoPobrania), wartosc_atrybutu);
                }
            }
            else
            {
                if (typ == "Int32")
                {
                    return 0;
                }
                if (typ == "Boolean")
                {
                    return false;
                }
                if (typ == "PlikiDoPobrania")
                {
                    return Ustawienia.PlikiDoPobrania.niezbedne;
                }
                return "";
            }

            return "";
        }



    }
}
