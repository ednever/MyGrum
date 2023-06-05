using System;
using System.Collections.Generic;
using System.Text;

namespace MyGrum.Models
{
    public class Retseptid
    {
        public int RetseptID { get; set; }
        public string Retsept { get; set; }
        public string Pilt { get; set; }
        public int SoogiaegID { get; set; }
        public string Nimekiri { get; set; }
        public string Kirjeldus { get; set; }
        public Retseptid() { }
        public Retseptid(int RetseptID, string Retsept, string Pilt, int SoogiaegID, string Nimekiri, string Kirjeldus)
        {
            this.RetseptID = RetseptID;
            this.Retsept = Retsept;
            this.Pilt = Pilt;
            this.SoogiaegID = SoogiaegID;
            this.Nimekiri = Nimekiri;
            this.Kirjeldus = Kirjeldus;
        }
    }
}
