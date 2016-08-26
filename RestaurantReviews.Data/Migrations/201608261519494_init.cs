namespace RestaurantReviews.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestaurantReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RestaurantId = c.Int(nullable: false),
                        WouldRecommend = c.Boolean(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(unicode: false, storeType: "text"),
                        CreatorId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Email = c.String(nullable: false, maxLength: 200),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_UserEmail");
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RestaurantName = c.String(nullable: false, maxLength: 200),
                        Description = c.String(unicode: false, storeType: "text"),
                        FoodType = c.String(maxLength: 100),
                        ServesAlcohol = c.Boolean(nullable: false),
                        KidFriendly = c.Boolean(nullable: false),
                        Street = c.String(unicode: false, storeType: "text"),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 2),
                        PostalCode = c.String(nullable: false, maxLength: 10),
                        GeoLocation = c.Geography(),
                        CreatorId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.RestaurantName)
                .Index(t => t.City, name: "IX_RestaurantCity")
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RestaurantReviews", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.RestaurantReviews", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.Restaurants", "CreatorId", "dbo.Users");
            DropIndex("dbo.Restaurants", new[] { "CreatorId" });
            DropIndex("dbo.Restaurants", "IX_RestaurantCity");
            DropIndex("dbo.Restaurants", new[] { "RestaurantName" });
            DropIndex("dbo.Users", "IX_UserEmail");
            DropIndex("dbo.RestaurantReviews", new[] { "CreatorId" });
            DropIndex("dbo.RestaurantReviews", new[] { "RestaurantId" });
            DropTable("dbo.Restaurants");
            DropTable("dbo.Users");
            DropTable("dbo.RestaurantReviews");
        }
    }
}
