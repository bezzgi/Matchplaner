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

        public List<Match> Matches = new List<Match>();

        public Mannschaft Mannschaft = new Mannschaft();

        public List<Mannschaft> Mannschaften = new List<Mannschaft>();

        public Benutzer Benutzer = new Benutzer();

        public List<Benutzer> Spieler = new List<Benutzer>();

        public List<Benutzer> Schiedsrichter = new List<Benutzer>();

        public List<Benutzer> Punkteschreiber = new List<Benutzer>();

        public MhasB MhasB = new MhasB();
    }
}
