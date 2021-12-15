using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Matchplaner.Helpers;

namespace Matchplaner.Models
{
    public class Benutzer
    {
        [Key]
        [ReadOnly(true)]
        public int id_benutzer { get; set; }

        [Required(ErrorMessage = "Vorname ist erforderlich.")]
        public string vorname { get; set; }

        [Required(ErrorMessage = "Nachname ist erforderlich.")]
        public string nachname { get; set; }

        [Required(ErrorMessage = "Benutzername ist erforderlich.")]
        public string benutzername { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich.")]
        [DataType(DataType.Password)]
        public string passwort { get; set; }

        [Required]
        [ReadOnly(true)]
        public int fk_mannschaft_id { get; set; }

        public int is_schiedsrichter { get; set; }

        public int is_spieler { get; set; }

        public int is_punkteschreiber { get; set; }

        public int admin { get; set; }

        [ForeignKey("benutzer_id_benutzer")]
        public List<MhasB> Match_Has_Benutzer { get; set; }
    }
}
