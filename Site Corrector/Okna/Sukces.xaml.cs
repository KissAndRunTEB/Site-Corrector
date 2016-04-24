using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Site_Corrector.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy Sukces.xaml
    /// </summary>
    public partial class Sukces : MetroWindow
    {
        string link="";

        string opis = "";

        Projekt projekt = new Projekt();

        public Sukces()
        {
            InitializeComponent();
        }
        

        public string Link
        {
            get
            {
                return link;
            }

            set
            {
                link = value;
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

        public Projekt Projekt
        {
            get
            {
                return projekt;
            }

            set
            {
                projekt = value;
            }
        }

        public void pokaz()
        {
            etykieta_link.Text = this.Link;
            etykieta_opis.Text = this.Opis;

            etykieta_kopie.Text = "Lub kliknij Tutaj aby przejść do kopii zapasowych.";

            this.ShowDialog();
        }



        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void etykieta_link_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(this.Link);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void etykieta_kopie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(Projekt.Folder_projektu);
        }
    }
}
