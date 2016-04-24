using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    public class Internet
    {
        public static bool czy_polaczenie()
        {
            bool internet = true;
            string adres_www = "www.google.com";

            try
            {
                System.Net.IPHostEntry wejscie = System.Net.Dns.GetHostEntry(adres_www);
            }
            catch (WebException)
            {
                internet = false;
            }
            catch (SocketException)
            {
                internet = false;
            }

            return internet;
        }

        public static void idz_do_strony(string adres)
        {
            if (!adres.Contains("www."))
            {
                adres = "www." + adres;
            }
            if (!adres.Contains("http://"))
            {
                adres = "http://" + adres;
            }

            Process.Start(adres);
        }

        public static HtmlDocument stworz_dokument_HTML(string tresc)
        {
            HtmlDocument dokument = new HtmlDocument();

            if (tresc != null & tresc != "")
            {
                dokument.OptionFixNestedTags = true;

                dokument.LoadHtml(tresc);

                if (dokument.ParseErrors != null && dokument.ParseErrors.Count() > 0)
                {
                    //dokument = null;
                }
            }
            else
            {
                dokument = null;
            }

            return dokument;
        }

        public static string pobierz_strone(string adres_www)
        {
            string tresc = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adres_www);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            tresc = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                tresc = null;
            }

            return tresc;
        }



        #region Maile

        public static void wyślij_maila_na_scapp(string tytul, string wiadomosc, string od_kogo, string od_mail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(od_mail, od_kogo);
            message.To.Add(new MailAddress("scapp@gmail.com"));
            message.Subject = tytul;
            message.Body = wiadomosc;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;


            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


            smtp.EnableSsl = true;
            smtp.Port = 587;



            smtp.Credentials = new NetworkCredential("aplikacja.serialomaniak@gmail.com", "ser.5glee1");

            smtp.Send(message);




        }

        #endregion
    }
}
