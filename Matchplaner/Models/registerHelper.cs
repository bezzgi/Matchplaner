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
    public class registerHelper
    {
        public Benutzer Benutzer = new Benutzer();

        public List<Mannschaft> Mannschaften = new List<Mannschaft>();

        public List<Qualifikation> Qualifikationen = new List<Qualifikation>();
    }
}
