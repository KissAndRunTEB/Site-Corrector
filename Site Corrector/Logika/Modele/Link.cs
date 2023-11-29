using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Logika.Modele
{
    class Link : INotifyPropertyChanged
    {
        Uri www;
        int glebokosc;


        public Link(string adres, int glebokosc)
        {
            this.Www = new Uri(adres);
            this.Glebokosc = glebokosc;
        }

        public Uri Www
        {
            get
            {
                return www;
            }

            set
            {
                www = value;
                OnPropertyChanged("Www");
            }
        }

        public int Glebokosc
        {
            get
            {
                return glebokosc;
            }

            set
            {
                glebokosc = value;
                OnPropertyChanged("Glebokosc");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
