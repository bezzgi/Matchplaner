using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class Match
    {
        [Key]
        [ReadOnly(true)]
        public int id_match { get; set; }

        public string hallenname { get; set; }

        public string ort { get; set; }

        [DataType(DataType.Date)]
        public DateTime datum { get; set; }

        [DataType(DataType.Time)]
        public DateTime uhrzeit { get; set; }
    }
}
