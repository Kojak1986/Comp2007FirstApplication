namespace FirstApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        UserId = c.String(nullable: false, maxLength: 128),
                        GameId = c.String(nullable: false, maxLength: 128),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                        EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    })
                .PrimaryKey(t => t.RatingId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GameId);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Email = c.String(),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "GameId", "dbo.Games");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "GameId" });
            DropIndex("dbo.Ratings", new[] { "UserId" });
            //DropTable("dbo.AspNetUsers");
            DropTable("dbo.Ratings");
        }
    }
}
