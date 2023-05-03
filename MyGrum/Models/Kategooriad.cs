using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MyGrum.Models
{ 
    [Table("Kategooria")]
    public class Kategooriad
    {
        [PrimaryKey, AutoIncrement]
        public int KategooriaID { get; set; }

        [MaxLength(15)]
        public string Kategooria { get; set; }

        [MaxLength(15)]
        public string Pilt { get; set; }
    }
}
