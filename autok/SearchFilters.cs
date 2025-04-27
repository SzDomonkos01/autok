using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autok
{
    public class SearchFilters
    {
        private string selectedMarka = "";
        public string SelectedMarka
        {
            get => selectedMarka;
            set { selectedMarka = value; OnPropertyChanged(nameof(SelectedMarka)); }
        }

        private double selectedEvjarat;
        public double SelectedEvjarat
        {
            get => selectedEvjarat;
            set { selectedEvjarat = value; OnPropertyChanged(nameof(SelectedEvjarat)); }
        }

        private bool forgalmiLejart;
        public bool ForgalmiLejart
        {
            get => forgalmiLejart;
            set { forgalmiLejart = value; OnPropertyChanged(nameof(ForgalmiLejart)); }
        }

        private bool biztositasLejart;
        public bool BiztositasLejart
        {
            get => biztositasLejart;
            set { biztositasLejart = value; OnPropertyChanged(nameof(BiztositasLejart)); }
        }

        private string rendszamSearch = "";
        public string RendszamSearch
        {
            get => rendszamSearch;
            set { rendszamSearch = value; OnPropertyChanged(nameof(RendszamSearch)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string tulajdonsagNev) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(tulajdonsagNev));
    }
}
