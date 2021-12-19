using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class Logger
    {
        [Key]
        [ReadOnly(true)]
        public int id_log { get; set; }

        [ForeignKey("id_besitzer")]
        public int fk_benutzer_id { get; set; }

        public string logging { get; set; }

        public DateTime zeit { get; set; }
    }
}
