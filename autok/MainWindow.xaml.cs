using System.Buffers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace autok;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{

    public Felhasznalo AktivFelhasznalo { get; set; }

    public ObservableCollection<Jarmu>? Jarmuvek { get; set; }

    public ObservableCollection<string> JarmuModels { get; set; }

    public SearchFilters Filters { get; set; } = new SearchFilters();

    public Jarmu SelectedJarmu { get; set; }

    private ObservableCollection<Jarmu>? jarmuvekToShow;
    public ObservableCollection<Jarmu>? JarmuvekToShow
    {
        get { return jarmuvekToShow; }
        set { jarmuvekToShow = value; OnPropertyChanged(nameof(JarmuvekToShow)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string tulajdonsagNev)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(tulajdonsagNev));
    }


    public MainWindow()
    {
        var loginWindow = new LoginWindow();
        bool? sikeres = loginWindow.ShowDialog();

        if (sikeres != true)
        {
            Application.Current.Shutdown();
            return;
        }

        AktivFelhasznalo = loginWindow.BelepettFelhasznalo;

        InitializeComponent();
        this.DataContext = this;
        FileRead();
        EllenorizLejaratokat();
        JarmuModels = new(Jarmuvek.Select(x => x.marka).Distinct().Order());
        JarmuModels.Insert(0, "");

        EllenorizLejaratokat();
    }

    private void FileRead()
    {
        string jsonStr = File.ReadAllText("jarmuvek.json");
        var osszes = JsonSerializer.Deserialize<ObservableCollection<Jarmu>>(jsonStr);
        Jarmuvek = new(osszes.Where(j => j.tulajEmail == AktivFelhasznalo.Email));
        JarmuvekToShow = new ObservableCollection<Jarmu>(Jarmuvek);
    }

    private void EllenorizLejaratokat()
    {
        foreach (var jarmu in JarmuvekToShow!)
        {
            if (string.IsNullOrEmpty(jarmu.tulajEmail)) continue;

            EllenorizDatum(jarmu.forgalmi, jarmu.tulajEmail, jarmu.rendszam!, "forgalmi");
            EllenorizDatum(jarmu.biztositas, jarmu.tulajEmail, jarmu.rendszam!, "biztosítás");
        }
    }

    private void EllenorizDatum(DateOnly? datum, string email, string rendszam, string tipus)
    {
        if (!datum.HasValue) return;

        var hatralevoNap = (datum.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).TotalDays;

        if (hatralevoNap <= 0)
        {
            EmailSender.Send(email, $"{tipus} lejárt", $"Figyelem! A(z) {rendszam} rendszámú jármű {tipus} érvényessége lejárt.");
        }
        else if (hatralevoNap <= 30)
        {
            EmailSender.Send(email, $"{tipus} hamarosan lejár", $"Figyelem! A(z) {rendszam} rendszámú jármű {tipus} érvényessége kevesebb mint 1 hónap múlva lejár.");
        }
        else if (hatralevoNap <= 60)
        {
            EmailSender.Send(email, $"{tipus} lejárat közeleg", $"Emlékeztető: A(z) {rendszam} rendszámú jármű {tipus} érvényessége kevesebb mint 2 hónap múlva lejár.");
        }
    }


    private void search_BTN_Click(object sender, RoutedEventArgs e)
    {
        DateOnly ma = DateOnly.FromDateTime(DateTime.Today);

        JarmuvekToShow = new(Jarmuvek.Where(jar =>
            (string.IsNullOrEmpty(Filters.SelectedMarka) || jar.marka == Filters.SelectedMarka) &&
            (!jar.evjarat.HasValue || jar.evjarat >= (int)Filters.SelectedEvjarat) &&
            (!Filters.ForgalmiLejart || (jar.forgalmi.HasValue && jar.forgalmi.Value < ma)) &&
            (!Filters.BiztositasLejart || (jar.biztositas.HasValue && jar.biztositas.Value < ma)) &&
            (string.IsNullOrEmpty(Filters.RendszamSearch) || (jar.rendszam != null && jar.rendszam.Contains(Filters.RendszamSearch, StringComparison.OrdinalIgnoreCase)))
        ));
    }

    private void JarmuAdd(string jarmuModelStr)
    {
        if (!JarmuModels.Contains(jarmuModelStr))
        {
            JarmuModels.Add(jarmuModelStr);
        }
    }
    private void new_BTN_Click(object sender, RoutedEventArgs e)
    {
        DataWindow dataWindow = new DataWindow(new Jarmu() { evjarat = Jarmuvek.Max(x => x.evjarat) + 1 });
        dataWindow.ShowDialog();
        if(dataWindow.DialogResult == true)
        {
            Jarmuvek.Add(dataWindow.Jarmu);
            JarmuAdd(dataWindow.Jarmu.evjarat.ToString());
            search_BTN_Click(sender, e);
        }
    }

    private void del_BTN_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedJarmu != null)
        {
            MessageBoxResult result = MessageBox.Show($"Biztosan törölni kívánja a(z) {SelectedJarmu.marka} - {SelectedJarmu.model} modellű autót?", "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Jarmuvek.Remove(SelectedJarmu);
                JarmuvekToShow.Remove(SelectedJarmu);
            }
            return;
        }
        MessageBox.Show("Válassza ki a törlendő elemet!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void mod_BTN_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedJarmu != null)
        {
            int index = Jarmuvek.IndexOf(Jarmuvek.FirstOrDefault(x => x.evjarat == SelectedJarmu.evjarat));
            Jarmu tmpJarmu = new Jarmu()
            {
                rendszam = SelectedJarmu.rendszam,
                marka = SelectedJarmu.marka,
                forgalmi = SelectedJarmu.forgalmi,
                biztositas = SelectedJarmu.biztositas,
                evjarat = SelectedJarmu.evjarat,
                motorszam = SelectedJarmu.motorszam,
                alvazszam = SelectedJarmu.alvazszam,
                forgalombah = SelectedJarmu.forgalombah,
                model = SelectedJarmu.model,
            };
            DataWindow dataWindow = new DataWindow(tmpJarmu);
            dataWindow.ShowDialog();
            if (dataWindow.DialogResult == true)
            {
                Jarmuvek[index] = dataWindow.Jarmu;
                JarmuAdd(dataWindow.Jarmu.evjarat.ToString());
                search_BTN_Click(sender, e);
            }
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        string jsonStr = JsonSerializer.Serialize(Jarmuvek);
        File.WriteAllText("jarmuvekjson", jsonStr);
    }

    private void esc_BTN_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}