using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    public class Zmiana
    {
        string numer;
        string tytul;
        string opis;
        TypZmiany typ;

        public string Numer
        {
            get
            {
                return numer;
            }

            set
            {
                numer = value;
            }
        }

        public string Tytul
        {
            get
            {
                return tytul;
            }

            set
            {
                tytul = value;
            }
        }

        public string Opis
        {
            get
            {
                return opis;
            }

            set
            {
                opis = value;
            }
        }

        public TypZmiany Typ
        {
            get
            {
                return typ;
            }

            set
            {
                typ = value;
            }
        }

        public Zmiana(string numer, string tytul, string opis, TypZmiany typ)
        {
            this.Numer = numer;
            this.Tytul = tytul;
            this.Opis = opis;
            this.Typ = typ;
        }
    }

    public enum TypZmiany
    {
        NowaFunkcja,
        PoprawkaStabilnosci,
        PoprawkaBledu,
        InterfejsUzytkownika
    }
}
