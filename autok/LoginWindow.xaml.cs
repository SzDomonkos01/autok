using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public Felhasznalo BelepettFelhasznalo { get; private set; }

        private List<Felhasznalo> felhasznalok;

        private LoginViewModel viewModel;

        public LoginWindow()
        {
            InitializeComponent();

            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
            string json = File.ReadAllText("felhasznalok.json");
            felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(json);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            viewModel.Jelszo = pb.Password;
        }

        private void belepes_BTN_Click(object sender, RoutedEventArgs e)
        {
            var felh = felhasznalok.FirstOrDefault(f =>
               f.Email == viewModel.Email && f.Jelszo == viewModel.Jelszo);

            if (felh != null)
            {
                BelepettFelhasznalo = felh;
                DialogResult = true;
            }
            else
            {
                hiba_TB.Text = "Hibás email vagy jelszó!";
            }
        }

        private void regisztracio_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Email) || string.IsNullOrWhiteSpace(viewModel.Jelszo))
            {
                hiba_TB.Text = "Az email és a jelszó megadása kötelező.";
                return;
            }

            if (felhasznalok.Any(f => f.Email == viewModel.Email))
            {
                hiba_TB.Text = "Ez az email már regisztrálva van.";
                return;
            }

            Felhasznalo ujFelhasznalo = new Felhasznalo
            {
                Email = viewModel.Email,
                Jelszo = viewModel.Jelszo
            };

            felhasznalok.Add(ujFelhasznalo);
            File.WriteAllText("felhasznalok.json", JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true }));

            hiba_TB.Text = "Sikeres regisztráció! Most már bejelentkezhetsz.";
        }
    }
}
