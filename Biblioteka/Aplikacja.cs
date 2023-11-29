using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    public class Aplikacja
    {
        static public void odwiedz_facebook(string fb)
        {
            Process.Start(fb);
        }

        static public void odwiedz_www(string www)
        {
            Process.Start(www);
        }

        static public void mailuj_nowa_funkcje(string mail)
        {
            Process.Start("mailto:"+mail+"?Subject=New%20sugestion");
        }

        static public void mailuj_blad(string mail)
        {
            Process.Start("mailto:" + mail + "?Subject=Error%20report");
        }
    }
}
