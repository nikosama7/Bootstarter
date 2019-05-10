namespace Bootstarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commnets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InspirationWord = c.String(nullable: false),
                        CommentTime = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdeaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ideas", t => t.IdeaId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.IdeaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "IdeaId", "dbo.Ideas");
            DropIndex("dbo.Comments", new[] { "IdeaId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Comments");
        }
    }
}
