namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Competitions", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Competitions", new[] { "CompetitionType_Id1" });
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Requests", new[] { "Competition_Id1" });
            DropIndex("dbo.UserInCompetitions", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.UserInCompetitions", new[] { "Location_Id1" });
            DropColumn("dbo.Competitions", "ApplicationUser_Id");
            DropColumn("dbo.Competitions", "CompetitionType_Id");
            DropColumn("dbo.Locations", "Competition_Id");
            DropColumn("dbo.Requests", "ApplicationUser_Id");
            DropColumn("dbo.Requests", "Competition_Id");
            DropColumn("dbo.UserInCompetitions", "ApplicationUser_Id");
            DropColumn("dbo.UserInCompetitions", "Location_Id");
            RenameColumn(table: "dbo.Competitions", name: "ApplicationUser_Id1", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Competitions", name: "CompetitionType_Id1", newName: "CompetitionType_Id");
            RenameColumn(table: "dbo.Locations", name: "Competition_Id1", newName: "Competition_Id");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id1", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Requests", name: "Competition_Id1", newName: "Competition_Id");
            RenameColumn(table: "dbo.UserInCompetitions", name: "ApplicationUser_Id1", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.UserInCompetitions", name: "Location_Id1", newName: "Location_Id");
            RenameIndex(table: "dbo.Locations", name: "IX_Competition_Id1", newName: "IX_Competition_Id");
            AlterColumn("dbo.Competitions", "CompetitionType_Id", c => c.Int());
            AlterColumn("dbo.Competitions", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "Competition_Id", c => c.Int());
            AlterColumn("dbo.Requests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserInCompetitions", "Location_Id", c => c.Int());
            AlterColumn("dbo.UserInCompetitions", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Competitions", "ApplicationUser_Id");
            CreateIndex("dbo.Competitions", "CompetitionType_Id");
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            CreateIndex("dbo.Requests", "Competition_Id");
            CreateIndex("dbo.UserInCompetitions", "ApplicationUser_Id");
            CreateIndex("dbo.UserInCompetitions", "Location_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserInCompetitions", new[] { "Location_Id" });
            DropIndex("dbo.UserInCompetitions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Requests", new[] { "Competition_Id" });
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Competitions", new[] { "CompetitionType_Id" });
            DropIndex("dbo.Competitions", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.UserInCompetitions", "ApplicationUser_Id", c => c.String());
            AlterColumn("dbo.UserInCompetitions", "Location_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "ApplicationUser_Id", c => c.String());
            AlterColumn("dbo.Requests", "Competition_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Competitions", "ApplicationUser_Id", c => c.String());
            AlterColumn("dbo.Competitions", "CompetitionType_Id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Locations", name: "IX_Competition_Id", newName: "IX_Competition_Id1");
            RenameColumn(table: "dbo.UserInCompetitions", name: "Location_Id", newName: "Location_Id1");
            RenameColumn(table: "dbo.UserInCompetitions", name: "ApplicationUser_Id", newName: "ApplicationUser_Id1");
            RenameColumn(table: "dbo.Requests", name: "Competition_Id", newName: "Competition_Id1");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "ApplicationUser_Id1");
            RenameColumn(table: "dbo.Locations", name: "Competition_Id", newName: "Competition_Id1");
            RenameColumn(table: "dbo.Competitions", name: "CompetitionType_Id", newName: "CompetitionType_Id1");
            RenameColumn(table: "dbo.Competitions", name: "ApplicationUser_Id", newName: "ApplicationUser_Id1");
            AddColumn("dbo.UserInCompetitions", "Location_Id", c => c.Int(nullable: false));
            AddColumn("dbo.UserInCompetitions", "ApplicationUser_Id", c => c.String());
            AddColumn("dbo.Requests", "Competition_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "ApplicationUser_Id", c => c.String());
            AddColumn("dbo.Locations", "Competition_Id", c => c.Int());
            AddColumn("dbo.Competitions", "CompetitionType_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Competitions", "ApplicationUser_Id", c => c.String());
            CreateIndex("dbo.UserInCompetitions", "Location_Id1");
            CreateIndex("dbo.UserInCompetitions", "ApplicationUser_Id1");
            CreateIndex("dbo.Requests", "Competition_Id1");
            CreateIndex("dbo.Requests", "ApplicationUser_Id1");
            CreateIndex("dbo.Competitions", "CompetitionType_Id1");
            CreateIndex("dbo.Competitions", "ApplicationUser_Id1");
        }
    }
}
