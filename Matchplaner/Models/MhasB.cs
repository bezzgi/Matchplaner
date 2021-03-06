using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class MhasB
    {
        [ReadOnly(true)]
        public int match_id_match { get; set; }

        [ForeignKey("match_id_match")]
        public Match Match { get; set; }

        [ReadOnly(true)]
        public int benutzer_id_benutzer { get; set; }

        public Benutzer Benutzer { get; set; }

        public int benutzer_is_schiedsrichter { get; set; }

        public int benutzer_is_punkteschreiber { set; get; }

        public int benutzer_is_spieler { get; set; }
    }
}
