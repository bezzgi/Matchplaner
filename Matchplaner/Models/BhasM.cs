using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class BhasM
    {
        [Key]
        [ReadOnly(true)]

        public int benutzer_id_benutzer { get; set; }

        public int mannschaft_id_mannschaft { get; set; }
    }
}
