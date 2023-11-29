using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Site_Corrector.Program;

namespace Site_Corrector.Konta
{
    [Serializable]
    public class UstawieniaProgramu
    {
       public UserSC uzytkownik = new UserSC("-", "-");
        public Jezyki lang;

        public static UstawieniaProgramu obiekt()
        {
            return (App.Current.Resources["ustawienia_programu"] as UstawieniaProgramu);
        }

        public static void zapisz()
        {
            UstawieniaProgramu ustawienia = obiekt();


            XmlWriterSettings sledzone = new XmlWriterSettings();
            sledzone.Indent = true; //oddziela nowa linia

            XmlWriter plik_ustawien = XmlWriter.Create(Adresy.ustawienia + "\\Ustawienia.xml", sledzone);

            plik_ustawien.WriteStartDocument();
            plik_ustawien.WriteComment("Ustawienia programu");

            plik_ustawien.WriteStartElement("glowne");

            plik_ustawien.WriteStartElement("uzytkownik");
            plik_ustawien.WriteAttributeString("login", ustawienia.uzytkownik.Login);
            plik_ustawien.WriteAttributeString("haslo", ustawienia.uzytkownik.Haslo);
            plik_ustawien.WriteAttributeString("jezyk", ustawienia.lang.ToString());
            plik_ustawien.WriteEndElement();

            plik_ustawien.WriteEndElement();

            plik_ustawien.Flush();
            plik_ustawien.Close();

        }

        public static void wczytaj()
        {
            UstawieniaProgramu ustawienia = obiekt();

            FileInfo plik_z_lista = new FileInfo(Adresy.ustawienia + "\\Ustawienia.xml");
            if (plik_z_lista.Exists)
            {
                XmlDocument dokument_xml = new XmlDocument(); dokument_xml.Load(Adresy.ustawienia + "\\Ustawienia.xml");

                XmlNodeList lista_w_seriali = dokument_xml.GetElementsByTagName("uzytkownik");

                string login = (string)wartosc_atrybutu_jesli_istnieje(lista_w_seriali[0], "login");
                string haslo = (string)wartosc_atrybutu_jesli_istnieje(lista_w_seriali[0], "haslo");
                string jezyk = (string)wartosc_atrybutu_jesli_istnieje(lista_w_seriali[0], "jezyk");

            ustawienia.uzytkownik = new UserSC(login, haslo);
            ustawienia.lang = Jezyki.Angielski;
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
                    return true;
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

