using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Site_Corrector
{
    class Serwer
    {
        string ftpBase;
        string uzytkownik;
        string haslo;

        public List<string> lista_plikow = new List<string>();

        public Serwer(string _adres, string _uzytkownik, string _haslo)
        {
            this.ftpBase = _adres;
            this.uzytkownik = _uzytkownik;
            this.haslo = _haslo;
        }

        private static readonly object _syncObject = new object();
        public bool sprawdz_czy_mozna_polaczyc()
        {
            lock (_syncObject)
            {
                bool wynik = false;
                try
                {
                    FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(this.ftpBase);
                    reqFTP.Credentials = new NetworkCredential(uzytkownik, haslo);
                    reqFTP.EnableSsl = false;
                    reqFTP.KeepAlive = true;
                    reqFTP.UseBinary = true;
                    reqFTP.UsePassive = true;
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    wynik = true;
                }
                catch (Exception ex)
                {
                    Program.loguj("Sprawdzono, że nie poprawnie wpisany adres serwera lub dane logowania" + ex.ToString());
                    wynik = false;
                    return wynik;
                }

                return wynik;
            }
        }

        public int sprawdz_czy_mozna_polaczyc_z_kodem_bledu()
        {
            lock (_syncObject)
            {
                int wynik = 0;
                try
                {
                    FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(this.ftpBase);
                    reqFTP.Credentials = new NetworkCredential(uzytkownik, haslo);
                    reqFTP.EnableSsl = false;
                    reqFTP.KeepAlive = true;
                    reqFTP.UseBinary = true;
                    reqFTP.UsePassive = true;
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    wynik = 1;
                }
                catch (Exception ex)
                {
                    Program.loguj("Sprawdzono, że nie poprawnie wpisany adres serwera lub dane logowania" + ex.ToString());
                    wynik = 0;

                    if(ex.Message== "Nie można połączyć się z serwerem zdalnym")
                    {
                        wynik = -2;
                    }

                    if(ex.Message == "Serwer zdalny zwrócił błąd: (530) Niezalogowany.")
                    {
                        wynik = -3;
                    }

                    return wynik;
                }

                return wynik;
            }
        }


        public bool sprawdz_czy_cos_jest()
        {
            bool wynik = false;
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(this.ftpBase);
                reqFTP.Credentials = new NetworkCredential(uzytkownik, haslo);
                reqFTP.EnableSsl = false;
                reqFTP.KeepAlive = true;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = true;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                List<string> list = new List<string>();
                List<string> files = new List<string>();

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string odpowiedz = reader.ReadToEnd();

                    list = odczyta_z_serwerowej_odpowiedzi(odpowiedz);
                }
                
                if(list.Count()>0)
                {
                    wynik= true;
                }
                else
                {
                    wynik= false;
                }
            }
            catch (Exception ex)
            {
                Program.loguj("Nie można wylistować zwartości folderu na serwerze."+ex.ToString());
                wynik = false;
            }

            return wynik;
        }


        //Gets all files in a given FTP address.  RECURSIVE METHOD:
        public void pobierzListePlikow(Projekt projekt, string ftpAddress, ObservableCollection<Plik> pliki, ListBox box, Etap etap)
        {
            string uri = ftpAddress;
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
            reqFTP.Credentials = new NetworkCredential(uzytkownik, haslo);
            reqFTP.EnableSsl = false;
            reqFTP.KeepAlive = true;
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = true;
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;


            List<string> list = new List<string>();
            List<string> files = new List<string>();

            using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string odpowiedz = reader.ReadToEnd();

                list = odczyta_z_serwerowej_odpowiedzi(odpowiedz);
            }

            int licznik_plikow = 0;

            //nowe podejscie
            foreach (string line in list)
            {
                string regex =
              @"^" +                          //# Start of line
              @"(?<dir>[\-ld])" +             //# File size          
              @"(?<permission>[\-rwx]{9})" +  //# Whitespace          \n
              @"\s+" +                        //# Whitespace          \n
              @"(?<filecode>\d+)" +
              @"\s+" +                        //# Whitespace          \n
              @"(?<owner>\w+)" +
              @"\s+" +                        //# Whitespace          \n
              @"(?<group>\w+)" +
              @"\s+" +                        //# Whitespace          \n
              @"(?<size>\d+)" +
              @"\s+" +                        //# Whitespace          \n
              @"(?<month>\w{3})" +            //# Month (3 letters)   \n
              @"\s+" +                        //# Whitespace          \n
              @"(?<day>\d{1,2})" +            //# Day (1 or 2 digits) \n
              @"\s+" +                        //# Whitespace          \n
              @"(?<timeyear>[\d:]{4,5})" +    //# Time or year        \n
              @"\s+" +                        //# Whitespace          \n
              @"(?<filename>(.*))" +          //# Filename            \n
              @"$";                           //# End of line

                //podejscie nr 2

                string podejscie2 = @"



                                                ";

                var split = new Regex(regex).Match(line);

                bool czy_sie_udalo = split.Success;

                if(!czy_sie_udalo)
                {
                    split = new Regex(@"^(?<dir>[d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(?<filename>.+?)\s?$",
    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).Match(line);
                    czy_sie_udalo = split.Success;
                }

                string dir = split.Groups["dir"].ToString();
                string filename = split.Groups["filename"].ToString();

                //jezeli nie jest . lub ..
                if (!(filename == "."|| filename == ".."))
                {

                    bool isDirectory = !string.IsNullOrWhiteSpace(dir) && dir.Equals("d", StringComparison.OrdinalIgnoreCase);


                    //do testow
                    //string cos = split.Groups["dir"].ToString();
                    //string permission = split.Groups["permission"].ToString();
                    //string filecode = split.Groups["filecode"].ToString();
                    //string owner = split.Groups["owner"].ToString();
                    //string group = split.Groups["group"].ToString();

                    //string size = split.Groups["size"].ToString();



                    var parentDirectory = "";

                    if (!isDirectory)
                    {
                        string rozszerzenie = Path.GetExtension(filename);
                        //jezeli nie jest plikiem wideo
                        //MP4, 3GP, ASF, AVI, DV, DVD, FLV, MKV, MOV, MPG, OGG, SMV, SVCD, TS, WMV, VCD
                        //i nie jest plikiem .PDF
                        if (ustawienie_pozwala(projekt, rozszerzenie))
                        {
                            lista_plikow.Add(ftpAddress + filename);

                            licznik_plikow++;

                            Plik nowy = new Plik();
                            nowy.Nazwa = filename;
                            nowy.Typ = rozszerzenie;
                            nowy.Procent_pobrania = 0;

                            box.Dispatcher.BeginInvoke(new Action(delegate ()
                            {
                                pliki.Add(nowy);
                            }));
                        }
                    }
                    else //jest folderem
                    {
                        //If the filename has no extension, then it is just a folder. 
                        //Run this method again as a recursion of the original:
                        parentDirectory += filename + "/";
                        try
                        {
                            pobierzListePlikow(projekt, ftpAddress + parentDirectory, pliki, box, etap);

                            etap.Opis = "Trwa pobieranie z: " + ftpAddress + parentDirectory;

                            System.IO.Directory.CreateDirectory(projekt.Folder_projektu_oryginal + "\\" + (sama_sciezka(ftpAddress + parentDirectory)).Replace('/', '\\'));
                        }
                        catch (Exception)
                        {
                            //throw;
                        }
                    }

                }
            }



            //Loop through the resulting file names.
            //foreach (var fileName in files)
            //{
            //        var parentDirectory = "";

            //    //If the filename has an extension, then it actually is 
            //    //a file and should be added to 'fnl'.        
            //    bool czy_folder = (fileName.IndexOf("<DIR>", 0) == -1);


            //if (czy_folder)
            //{
            //    string rozszerzenie = Path.GetExtension(fileName);
            //    //jezeli nie jest plikiem wideo
            //    //MP4, 3GP, ASF, AVI, DV, DVD, FLV, MKV, MOV, MPG, OGG, SMV, SVCD, TS, WMV, VCD
            //    //i nie jest plikiem .PDF
            //    if (rozszerzenie.ToUpper()!=".MP4"&&
            //        rozszerzenie.ToUpper() != ".3GP" && 
            //        rozszerzenie.ToUpper() != ".ASF" &&
            //        rozszerzenie.ToUpper() != ".AVI" &&
            //        rozszerzenie.ToUpper() != ".DVD" &&
            //        rozszerzenie.ToUpper() != ".FLV" &&
            //        rozszerzenie.ToUpper() != ".MKV" &&
            //        rozszerzenie.ToUpper() != ".MOV" &&
            //        rozszerzenie.ToUpper() != ".MPG" &&
            //        rozszerzenie.ToUpper() != ".OGG" &&
            //        rozszerzenie.ToUpper() != ".SMV" &&
            //        rozszerzenie.ToUpper() != ".SVCD" &&
            //        rozszerzenie.ToUpper() != ".WMV" &&
            //        rozszerzenie.ToUpper() != ".VCD" &&

            //        rozszerzenie.ToUpper() != ".PDF"
            //        )
            //    {
            //        lista_plikow.Add(ftpAddress + fileName);

            //        licznik_plikow++;

            //        Plik nowy = new Plik();
            //        nowy.Nazwa = fileName;
            //        nowy.Typ = rozszerzenie;
            //        nowy.Procent_pobrania = 0;

            //        box.Dispatcher.BeginInvoke(new Action(delegate ()
            //         {
            //             pliki.Add(nowy);
            //         }));
            //    }
            //}
            //else //jest folderem
            //{
            //    //If the filename has no extension, then it is just a folder. 
            //    //Run this method again as a recursion of the original:
            //    parentDirectory += fileName + "/";
            //    try
            //    {
            //        pobierzListePlikow(projekt, ftpAddress + parentDirectory, pliki, box);

            //        System.IO.Directory.CreateDirectory(projekt.Folder_projektu_oryginal + "\\" + (sama_sciezka(ftpAddress + parentDirectory)).Replace('/', '\\'));
            //    }
            //    catch (Exception)
            //    {
            //        //throw;
            //    }
            //}                
            //}
        
        }

        private bool ustawienie_pozwala(Projekt projekt, string rozszerzenie)
        {
            if (projekt.ustawienia.jakie_pliki == Ustawienia.PlikiDoPobrania.wszystkie)
            {
                return true;
            }
            else
            {
                string roz = rozszerzenie.ToUpper();

                if (projekt.ustawienia.lista_rozszerzen_niezbednych().Any<string>(x=>x== roz))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool czy_pdf(string rozszerzenie)
        {
            return rozszerzenie.ToUpper() == ".PDF";
        }

        private static bool czy_wideo(string rozszerzenie)
        {
            string temp = rozszerzenie.ToUpper();

            return temp == ".MP4" ||
                                    temp == ".3GP" ||
                                    temp == ".ASF" ||
                                   temp == ".AVI" ||
                                  temp == ".DVD" ||
                                   temp == ".FLV" ||
                                   temp == ".MKV" ||
                                   temp == ".MOV" ||
                                  temp == ".MPG" ||
                                temp == ".OGG" ||
                                   temp == ".SMV" ||
                           temp == ".SVCD" ||
                                 temp == ".WMV" ||
                          temp == ".VCD";
        }

        private List<string> odczyta_z_serwerowej_odpowiedzi(string odpowiedz)
        {
            List<string> lista = new List<string>();

            if (odpowiedz.Contains("\r\n"))
            {
                lista.AddRange(odpowiedz.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                lista.AddRange(odpowiedz.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
            }

            lista.RemoveAll(x => x.StartsWith("total "));

            return lista;
        }

        public string sama_sciezka(string s)
        {
            return s.Replace(ftpBase, "");
        }

        public void pobierzPlik(string ftpfilepath, string inputfilepath)
        {
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential(uzytkownik,haslo);
                byte[] fileData = request.DownloadData(ftpfilepath);


                    using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
            }
        }

        public void wgrajPlik(Projekt projekt, Serwer s, Plik p,Etap etap, int proba_nr=0)
        {

            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(s.ftpBase + p.LokalizacjaNaSerwerze);
                
                /* Log in to the FTP Server with the User Name and Password Provided */
                request.Credentials = new NetworkCredential(s.uzytkownik, s.haslo);
                /* When in doubt, use these options */
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // Copy the contents of the file to the request stream.
                //StreamReader sourceStream = new StreamReader(p.AdresNaDysku);
                byte[] fileContents = File.ReadAllBytes(p.AdresNaDysku);

                //sourceStream.Close();

                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                //Program.loguj("Wgranie pliku "+p.Nazwa+" status: "+response.StatusDescription);

                response.Close();

                etap.Status = StatusEtapu.rozpoczety;
            }
            catch (Exception ex) {
                Program.loguj(ex.ToString());

                //ponawianie
                if (proba_nr < Program.ile_razy_ponawiac_polaczenie)
                {
                    proba_nr++;
                    etap.Status = StatusEtapu.offline;
                    Thread.Sleep(Program.co_ile_ponawiac_polaczenie * 1000);
                    //po minucie sprobuj jeszcze raz;
                    wgrajPlik(projekt, s, p, etap, proba_nr);
                }
                else
                {
                    etap.Status = StatusEtapu.blad;
                }

                Program.loguj("Ponawianie wgrywania...");
            }
        }

        public void dopisz_komunikat(ListBox lista, string komunikat)
        {
            lista.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    lista.Items.Add(komunikat);
                }));
        }

     

    }
}
