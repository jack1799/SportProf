namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Competition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "ApplicationUserId", c => c.Int());
            AddColumn("dbo.Competitions", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Competitions", "ApplicationUser_Id");
            AddForeignKey("dbo.Competitions", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Competitions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Competitions", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Competitions", "ApplicationUser_Id");
            DropColumn("dbo.Competitions", "ApplicationUserId");
        }
    }
}
