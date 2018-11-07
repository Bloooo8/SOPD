namespace SOPD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reviews", "UserID");
            AddForeignKey("dbo.Reviews", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "UserID" });
            DropColumn("dbo.Reviews", "UserID");
        }
    }
}
