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
        InitializeComponent();
        this.DataContext = this;
        FileRead();
        JarmuModels = new(Jarmuvek.Select(x => x.marka).Distinct().Order());
        JarmuModels.Insert(0, "");
    }

    private void FileRead()
    {
        string jsonStr = File.ReadAllText("jarmuvek.json");
        Jarmuvek = JsonSerializer.Deserialize<ObservableCollection<Jarmu>>(jsonStr);
        JarmuvekToShow = JsonSerializer.Deserialize<ObservableCollection<Jarmu>>(jsonStr);
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
}