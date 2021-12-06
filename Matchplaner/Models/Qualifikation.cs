using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Models
{
    public class Qualifikation
    {
        [Key]
        [ReadOnly(true)]
        public int id_qualifikation { get; set; }

        public string name { get; set; }
    }
}
