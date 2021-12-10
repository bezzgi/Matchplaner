using Microsoft.EntityFrameworkCore;
using Matchplaner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Helpers
{
    public class DbMatchplaner : DbContext
    {
        public DbMatchplaner(DbContextOptions<DbMatchplaner> options) : base(options)
        {
            
        }

        public DbSet<Mannschaft> Mannschaft { set; get; }

        public DbSet<Qualifikation> Qualifikation { set; get; }

        public DbSet<Match> Match { set; get; }

        public DbSet<Benutzer> Benutzer { set; get; }

        public DbSet<BhasM> Benutzer_Has_Mannschaft { set; get; }

        public DbSet<BhasQ> Benutzer_Has_Qualifikation { set; get; }

        public DbSet<MhasM> Match_Has_Mannschaft { set; get; }
    }
}
