using System;
using System.Collections.Generic;
using System.Text;

namespace MyGrum.Models
{ 
    public class Kategooriad
    {
        public int KategooriaID { get; set; }
        public string Kategooria { get; set; }
        public string Pilt { get; set; }
        public Kategooriad(){}
        public Kategooriad(int KategooriaID, string Kategooria, string Pilt) 
        { 
            this.KategooriaID = KategooriaID;
            this.Kategooria = Kategooria;
            this.Pilt = Pilt;
        }
    }
}
