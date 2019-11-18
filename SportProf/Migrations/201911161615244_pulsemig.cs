namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pulsemig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInCompetitions", "Result", c => c.Int(nullable: false));
            AddColumn("dbo.UserInCompetitions", "Pulse", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInCompetitions", "Pulse");
            DropColumn("dbo.UserInCompetitions", "Result");
        }
    }
}
