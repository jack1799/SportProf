using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportProf.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int UserCount { get; set; } = 0;

        public int? CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
    }
}