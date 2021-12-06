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
        public static Benutzer benutzer = new Benutzer();

        public static Mannschaft mannschaft = new Mannschaft();

        public static Qualifikation qualifikation = new Qualifikation();
    }
}
