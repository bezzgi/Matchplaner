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
    public class matchHelper
    {
        public Match Match = new Match();

        public List<Mannschaft> Mannschaften = new List<Mannschaft>();
    }
}
