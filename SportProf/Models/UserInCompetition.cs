using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportProf.Models
{
    public class UserInCompetition
    {
        public int Id { get; set; }

        public int? LocationId { get; set; }
        public string UserId { get; set; }
        public int Result { get; set; }
        public int Pulse { get; set; }

        public virtual Location Location { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}