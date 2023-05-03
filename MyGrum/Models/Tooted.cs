using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using SQLite;
//using System.ComponentModel.DataAnnotations.Schema;

namespace MyGrum.Models
{
    [Table("Tooted")]
    public class Tooted
    {
        [PrimaryKey, AutoIncrement]
        public int TooteID { get; set; }

        [MaxLength(15)]
        public string Toote { get; set; }

        [MaxLength(15)]
        public string Pilt { get; set; }

        //[ForeignKey(typeof(Kategooriad))]
        public int KategooriaID { get; set; }
    }
}
