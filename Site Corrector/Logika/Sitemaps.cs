using Site_Corrector.Logika.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using X.Web.Sitemap;

namespace Site_Corrector.Logika
{
    class Sitemaps
    {
        string folder;
        string www;
        Sitemap sitemap = new Sitemap();

        public Sitemaps(string folder, string www, Linki linki)
        {
            this.folder = folder;
            this.www = www;

            foreach(Link l in linki.Linki_ok)
            {
                if(l.Www.AbsoluteUri.EndsWith(".html") || l.Www.AbsoluteUri.EndsWith(".php") || l.Www.AbsoluteUri.EndsWith(".aspx") || l.Www.AbsoluteUri.EndsWith("/"))
                {
                Sitemap.Add(CreateUrl(l.Www, l.Glebokosc));
                }
                else if(!(l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count()-1]=='.'|| l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 2] == '.' || l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 3] == '.' || l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 4] == '.') || l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 5] == '.' || l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 6] == '.' || l.Www.AbsoluteUri[l.Www.AbsoluteUri.Count() - 7] == '.')
                {
                    Sitemap.Add(CreateUrl(l.Www, l.Glebokosc));
                }
            }
        }


        public void zapisz()
        {
            //Save sitemap structure to file
            Sitemap.Save(this.folder + @"\sitemap.xml");

            //Split a large list into pieces and store in a directory
           // Sitemap.SaveToDirectory(this.folder + @"\sitemaps");

            //Get xml-content of file
            //sitemap.ToXml();
        }

        public string Folder
        {
            get
            {
                return folder;
            }

            set
            {
                folder = value;
            }
        }

        public Sitemap Sitemap
        {
            get
            {
                return sitemap;
            }

            set
            {
                sitemap = value;
            }
        }

        public string Www
        {
            get
            {
                return www;
            }

            set
            {
                www = value;
            }
        }

        private static Url CreateUrl(Uri url, int glebokosc)
        {
            double priorytet = 0.1;
            switch (glebokosc)
            {
                case 0:
                    priorytet = 1.0;
                    break;
                case 1:
                    priorytet = 1.0;
                    break;
                case 2:
                    priorytet = 0.9;
                    break;
                case 3:
                    priorytet = 0.8;
                    break;
                case 4:
                    priorytet = 0.7;
                    break;
                case 5:
                    priorytet = 0.6;
                    break;
                case 6:
                    priorytet = 0.5;
                    break;
                case 7:
                    priorytet = 0.4;
                    break;
                case 8:
                    priorytet = 0.3;
                    break;
                case 9:
                    priorytet = 0.2;
                    break;
                default:
                    priorytet = 0.1;
                    break;
            }

            return new Url
            {
                ChangeFrequency = ChangeFrequency.Daily,
                Location = url.AbsoluteUri,
                Priority = priorytet,
                TimeStamp = DateTime.Now
            };
        }


    }


}