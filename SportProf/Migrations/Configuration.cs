namespace SportProf.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SportProf.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : System.Data.Entity.Migrations.DbMigrationsConfiguration<SportProf.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SportProf.Models.ApplicationDbContext";
        }

        protected override void Seed(SportProf.Models.ApplicationDbContext context)
        {
            
        }
    }
}
