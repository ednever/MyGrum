using System;
using System.Collections.Generic;
using System.Text;

namespace MyGrum.Models
{
    public class Soogiajad
    {
        public int SoogiaegID { get; set; }
        public string Soogiaeg { get; set; }
        public string Pilt { get; set; }
        public Soogiajad() { }
        public Soogiajad(int SoogiaegID, string Soogiaeg, string Pilt)
        {
            this.SoogiaegID = SoogiaegID;
            this.Soogiaeg = Soogiaeg;
            this.Pilt = Pilt;
        }
    }
}
