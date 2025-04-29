using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autok
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string jelszo;
        public string Jelszo
        {
            get => jelszo;
            set { jelszo = value; OnPropertyChanged(nameof(Jelszo)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
