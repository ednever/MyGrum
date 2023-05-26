using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Text;

namespace MyGrum.Models
{
    public class Tooted
    {
        public int TooteID { get; set; }
        public string Toote { get; set; }
        public string Pilt { get; set; }
        public int KategooriaID { get; set; }
        public Tooted(){}
        public Tooted(int TooteID, string Toote, string Pilt, int KategooriaID)
        {
            this.TooteID = TooteID;
            this.Toote = Toote;
            this.Pilt = Pilt;
            this.KategooriaID = KategooriaID;
        }
    }
}