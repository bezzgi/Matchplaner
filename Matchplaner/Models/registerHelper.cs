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
    [Keyless]
    public class registerHelper
    {
        public Benutzer benutzer = new Benutzer();

        public List<Mannschaft> mannschaft = new List<Mannschaft>();

        public List<Qualifikation> qualifikation = new List<Qualifikation>();
    }
}
