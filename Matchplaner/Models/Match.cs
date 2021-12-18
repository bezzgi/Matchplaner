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

        [Required(ErrorMessage = "Hallenname ist erforderlich!")]
        [MaxLength(40)]
        public string hallenname { get; set; }

        [Required(ErrorMessage = "Ort ist erforderlich!")]
        [MaxLength(30)]
        public string ort { get; set; }

        [Required(ErrorMessage = "Datum ist erforderlich!")]
        [DataType(DataType.Date, ErrorMessage = "Datum ist ungültig!")]
        public DateTime? datum { get; set; }

        [Required(ErrorMessage = "Uhrzeit ist erforderlich!")]
        [DataType(DataType.Time, ErrorMessage = "Uhrzeit ist ungültig!")]
        public DateTime? uhrzeit { get; set; }
    }
}
