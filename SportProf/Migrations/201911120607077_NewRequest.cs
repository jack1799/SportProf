namespace SportProf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            AddForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Requests", "ApplicationUser_Id");
            DropColumn("dbo.Requests", "ApplicationUserId");
        }
    }
}
