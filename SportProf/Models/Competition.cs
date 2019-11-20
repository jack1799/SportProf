using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportProf.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Status Status { get; set; } = Status.Requests;

        public int? CompetitionTypeId { get; set; } = 0;
        public string ApplicationUserId { get; set; } = "";

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}