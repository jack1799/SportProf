using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SportProf.Models
{
    public class dbInitialize : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleUser = new IdentityRole { Name = "user" };
            var roleAdmin = new IdentityRole { Name = "admin" };

            roleManager.Create(roleUser);
            roleManager.Create(roleAdmin);

            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(new ApplicationUser { Email = "111@gmail.com", UserName = "111@gmail.com", CompetitionCount = 20 });
            string password = "Jack1799";
            var result = userManager.Create(users[0], password);
            if (result.Succeeded)
            {
                userManager.AddToRole(users[0].Id, roleUser.Name);
                userManager.AddToRole(users[0].Id, roleAdmin.Name);
            }
            users.Add(new ApplicationUser { Email = "222@gmail.com", UserName = "222@gmail.com", CompetitionCount = 10 });
            result = userManager.Create(users[1], password);
            if (result.Succeeded)
            {
                userManager.AddToRole(users[1].Id, roleUser.Name);
            }
            for (int i = 0; i < 10; i++)
            {
                users.Add(new ApplicationUser { Email = (i + 1) + "@gmail.com", UserName = (i + 1) + "@gmail.com", CompetitionCount = i });
                result = userManager.Create(users[2 + i], password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(users[2 + i].Id, roleUser.Name);
                }
            }

            var competitionTypes = new List<CompetitionType>()
            {
                new CompetitionType() { Name="Race", Descryption="Some text" },
                new CompetitionType() { Name="Race2", Descryption="Some text" }
            };
            var competitions = new List<Models.Competition>()
            {
                new Models.Competition() { Name="Race #1", Description="Some text for race #1", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #2", Description="Some text for race #2", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #3", Description="Some text for race #3", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #4", Description="Some text for race #4", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #5", Description="Some text for race #5", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #6", Description="Some text for race #6", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #7", Description="Some text for race #7", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #8", Description="Some text for race #8", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #9", Description="Some text for race #9", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #10", Description="Some text for race #10", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },

                new Models.Competition() { Name="Race #3", Description="Some text for race #3", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #4", Description="Some text for race #4", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #5", Description="Some text for race #5", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #6", Description="Some text for race #6", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #7", Description="Some text for race #7", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #8", Description="Some text for race #8", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #9", Description="Some text for race #9", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #10", Description="Some text for race #10", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #3", Description="Some text for race #3", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #4", Description="Some text for race #4", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #5", Description="Some text for race #5", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #6", Description="Some text for race #6", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #7", Description="Some text for race #7", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #8", Description="Some text for race #8", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #9", Description="Some text for race #9", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #10", Description="Some text for race #10", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #3", Description="Some text for race #3", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #4", Description="Some text for race #4", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #5", Description="Some text for race #5", CompetitionTypeId=1, ApplicationUser=users[1], ApplicationUserId=users[1].Id },
                new Models.Competition() { Name="Race #6", Description="Some text for race #6", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #7", Description="Some text for race #7", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #8", Description="Some text for race #8", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #9", Description="Some text for race #9", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id },
                new Models.Competition() { Name="Race #10", Description="Some text for race #10", CompetitionTypeId=1, ApplicationUser=users[0], ApplicationUserId=users[0].Id }
            };
            var locations = new List<Location>()
            {
                new Location() { Name="Location #1", UserCount=4, CompetitionId=1, Competition=competitions[0] },
                new Location() { Name="Location #2", UserCount=2, CompetitionId=1, Competition=competitions[0] },
                new Location() { Name="Location #3", UserCount=5, CompetitionId=1, Competition=competitions[0] },
                new Location() { Name="Location #1", UserCount=3, CompetitionId=2, Competition=competitions[1] },
                new Location() { Name="Location #2", UserCount=3, CompetitionId=2, Competition=competitions[1] },
                new Location() { Name="Location #3", UserCount=3, CompetitionId=2, Competition=competitions[1] }
            };
            var requests = new List<Models.Request>()
            {
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[0].Id, ApplicationUser=users[0] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[1].Id, ApplicationUser=users[1] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[2].Id, ApplicationUser=users[2] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[3].Id, ApplicationUser=users[3] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[4].Id, ApplicationUser=users[4] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[5].Id, ApplicationUser=users[5] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[6].Id, ApplicationUser=users[6] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[7].Id, ApplicationUser=users[7] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=1, Competition=competitions[0], ApplicationUserId=users[8].Id, ApplicationUser=users[8] },

                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[0].Id, ApplicationUser=users[0] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[1].Id, ApplicationUser=users[1] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[2].Id, ApplicationUser=users[2] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[3].Id, ApplicationUser=users[3] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[4].Id, ApplicationUser=users[4] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[5].Id, ApplicationUser=users[5] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[6].Id, ApplicationUser=users[6] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[7].Id, ApplicationUser=users[7] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[8].Id, ApplicationUser=users[8] },
                new Models.Request(){ Name="Name", Description="Text", Accepted=true, CompetitionId=2, Competition=competitions[1], ApplicationUserId=users[9].Id, ApplicationUser=users[9] }
            };
            var userInCompetitions = new List<UserInCompetition>()
            {

            };

            competitionTypes.ForEach(ct => context.CompetitionTypes.Add(ct));
            context.SaveChanges();
            competitions.ForEach(c => context.Competitions.Add(c));
            context.SaveChanges();
            locations.ForEach(l => context.Locations.Add(l));
            context.SaveChanges();
            requests.ForEach(r => context.Requests.Add(r));
            context.SaveChanges();
            userInCompetitions.ForEach(uic => context.UserInCompetitions.Add(uic));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}