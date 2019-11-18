namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompetitionTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetitionTypes", t => t.CompetitionTypeId, cascadeDelete: true)
                .Index(t => t.CompetitionTypeId);
            
            CreateTable(
                "dbo.CompetitionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descryption = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        Name = c.String(),
                        UserCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.UserInCompetitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.LocationId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.AspNetUsers", "CompetitionCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInCompetitions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInCompetitions", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes");
            DropIndex("dbo.UserInCompetitions", new[] { "User_Id" });
            DropIndex("dbo.UserInCompetitions", new[] { "LocationId" });
            DropIndex("dbo.Locations", new[] { "CompetitionId" });
            DropIndex("dbo.Competitions", new[] { "CompetitionTypeId" });
            DropColumn("dbo.AspNetUsers", "CompetitionCount");
            DropTable("dbo.UserInCompetitions");
            DropTable("dbo.Locations");
            DropTable("dbo.CompetitionTypes");
            DropTable("dbo.Competitions");
        }
    }
}
