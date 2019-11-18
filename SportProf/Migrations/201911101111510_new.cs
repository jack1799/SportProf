namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes");
            DropForeignKey("dbo.Locations", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.UserInCompetitions", "LocationId", "dbo.Locations");
            DropIndex("dbo.Competitions", new[] { "CompetitionTypeId" });
            DropIndex("dbo.Locations", new[] { "CompetitionId" });
            DropIndex("dbo.UserInCompetitions", new[] { "LocationId" });
            AlterColumn("dbo.Competitions", "CompetitionTypeId", c => c.Int());
            AlterColumn("dbo.Locations", "CompetitionId", c => c.Int());
            AlterColumn("dbo.UserInCompetitions", "LocationId", c => c.Int());
            AlterColumn("dbo.UserInCompetitions", "UserId", c => c.Int());
            CreateIndex("dbo.Competitions", "CompetitionTypeId");
            CreateIndex("dbo.Locations", "CompetitionId");
            CreateIndex("dbo.UserInCompetitions", "LocationId");
            AddForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes", "Id");
            AddForeignKey("dbo.Locations", "CompetitionId", "dbo.Competitions", "Id");
            AddForeignKey("dbo.UserInCompetitions", "LocationId", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInCompetitions", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes");
            DropIndex("dbo.UserInCompetitions", new[] { "LocationId" });
            DropIndex("dbo.Locations", new[] { "CompetitionId" });
            DropIndex("dbo.Competitions", new[] { "CompetitionTypeId" });
            AlterColumn("dbo.UserInCompetitions", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInCompetitions", "LocationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "CompetitionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Competitions", "CompetitionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserInCompetitions", "LocationId");
            CreateIndex("dbo.Locations", "CompetitionId");
            CreateIndex("dbo.Competitions", "CompetitionTypeId");
            AddForeignKey("dbo.UserInCompetitions", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "CompetitionId", "dbo.Competitions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Competitions", "CompetitionTypeId", "dbo.CompetitionTypes", "Id", cascadeDelete: true);
        }
    }
}
