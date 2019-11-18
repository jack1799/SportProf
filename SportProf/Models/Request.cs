using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportProf.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Accepted { get; set; }

        public int CompetitionId { get; set; }
        public string ApplicationUserId { get; set; }

        public Competition Competition { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}