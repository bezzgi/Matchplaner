using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class MhasM
    {
        [ReadOnly(true)]
        public int match_id_match { get; set; }

        [ForeignKey("match_id_match")]
        public Match Match { get; set; }

        [Key]
        [ReadOnly(true)]
        public int mannschaft_id_mannschaft { get; set; }

        [ForeignKey("mannschaft_id_mannschaft")]
        public Mannschaft Mannschaft { set; get; }
    }
}
