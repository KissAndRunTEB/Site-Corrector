﻿using MahApps.Metro.Controls;
using Site_Corrector.Logika.Modele;
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
using static Site_Corrector.Program;

namespace Site_Corrector.Okna
{
    /// <summary>
    /// Logika interakcji dla klasy Informacje.xaml
    /// </summary>
    public partial class Informacje : MetroWindow
    {
        public Informacje()
        {
            InitializeComponent();

            List<Zmiana> lista = Program.lista_zmian();

            lista_zmian.ItemsSource = lista;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void przycisk_uwagi_Click(object sender, RoutedEventArgs e)
        {
            Zglos okno = new Zglos();
            okno.ShowDialog();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.facebook.com/Site-Corrector-1600185850307419/");


        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.modernapp.pl/");
        }


        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.facebook.com/Site-Corrector-1600185850307419/");
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:scapp.kontakt@gmail.com?Subject=Nowa%20funkcja");
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Process.Start("mailto:scapp.kontakt@gmail.com?Subject=Zgłoszenie%20błędu");
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Zglos okno = new Zglos();
            okno.ShowDialog();
        }
    }
}
