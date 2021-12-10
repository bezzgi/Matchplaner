using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class BhasQ
    {
        [Key]
        [ReadOnly(true)]

        public int benutzer_id_benutzer { get; set; }

        public int qualifikation_id_qualifikation { get; set; }
    }
}
