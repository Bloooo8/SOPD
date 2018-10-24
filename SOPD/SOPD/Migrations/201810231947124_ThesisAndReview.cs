namespace SOPD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThesisAndReview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ReviewID);
            
            CreateTable(
                "dbo.Theses",
                c => new
                    {
                        ThesisID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorID = c.Int(nullable: false),
                        Descritpion = c.String(),
                        PromoterID = c.Int(nullable: false),
                        ReviewerID = c.Int(nullable: false),
                        ReviewID = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ThesisID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Theses");
            DropTable("dbo.Reviews");
        }
    }
}
