using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector
{
    public class Ustawienia
    {
        public int jakosc_JPG = 100;
        public int jakosc_PNG = 5;

        public bool czy_jpg = true;
        public bool czy_png = true;

        public bool czy_css = true;
        public bool czy_js = true;

        public bool czy_html = true;

        public bool czy_htaccess = true;

        public bool czy_deflate = true;
        public bool czy_expires = true;



        public PlikiDoPobrania jakie_pliki = PlikiDoPobrania.wszystkie;

        public enum PlikiDoPobrania
        {
            wszystkie,
            niezbedne
        }

        public List<string> lista_rozszerzen_niezbednych()
        {
            List<string> lista = new List<string>();

            if(this.czy_jpg)
            {
                lista.Add(".JPG");
            }

            if (this.czy_png)
            {
                lista.Add(".PNG");
            }

            if (this.czy_css)
            {
                lista.Add(".CSS");
            }

            if (this.czy_js)
            {
                lista.Add(".JS");
            }

            if (this.czy_html)
            {
                lista.Add(".HTML");
            }

            if (this.czy_htaccess)
            {
                lista.Add(".HTACCESS");
            }

            return lista;
        }
    }
}
