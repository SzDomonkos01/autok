using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autok
{
    public class Jarmu
    {
        public string? rendszam { get; set; }
        public string? marka { get; set; }
        public DateOnly? forgalmi { get; set; }
        public DateOnly? biztositas { get; set; }
        public int? evjarat { get; set; }
        public string? motorszam { get; set; }
        public string? alvazszam { get; set; }
        public DateOnly? forgalombah { get; set; }
        public string? model { get; set; }
    }

}
