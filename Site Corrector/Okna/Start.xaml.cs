using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy Start.xaml
    /// </summary>
    public partial class Start : MetroWindow
    {
        public Start()
        {
            InitializeComponent();
        }

        private void przycisk_start_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
