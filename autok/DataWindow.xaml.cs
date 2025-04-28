using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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

namespace autok
{
    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public ObservableCollection<Jarmu>? Jarmuvek { get; set; }
        public ObservableCollection<Jarmu>? JarmuvekToShow;
        public Jarmu Jarmu { get; set; }

        public DataWindow(Jarmu jarmu)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Jarmu = jarmu;
        }

        private bool InputCheck()
        {
            if (Jarmu.rendszam?.Length != 7)
            {
                MessageBox.Show("Érvényes rendszám megadása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(Jarmu.marka))
            {
                MessageBox.Show("Az autó márkájának megadása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Jarmu.forgalmi.HasValue)
            {
                MessageBox.Show("Az autó forgalmi engedélyének kiadási dátumát kötelező megadni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Jarmu.biztositas.HasValue)
            {
                MessageBox.Show("Az autó biztosításának kiadási dátumát kötelező megadni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Jarmu.evjarat > 2025 || Jarmu.evjarat < 1900)
            {
                MessageBox.Show("Az autó gyártási évét kötelező megadni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(Jarmu.motorszam))
            {
                MessageBox.Show("Az autó motorszámának megadása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(Jarmu.alvazszam))
            {
                MessageBox.Show("Az autó alvázszámának megadása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Jarmu.forgalombah.HasValue)
            {
                MessageBox.Show("Az autó forgalomba helyezésének dátumát kötelező megadni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(Jarmu.model))
            {
                MessageBox.Show("Az autó modellének megadása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void save_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (InputCheck())
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void cancel_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}