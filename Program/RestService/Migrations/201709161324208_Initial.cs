namespace RestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Signatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isOrginal = c.Boolean(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Strokes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isScaled = c.Boolean(nullable: false),
                        SignatureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signatures", t => t.SignatureId, cascadeDelete: true)
                .Index(t => t.SignatureId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Pressure = c.Single(nullable: false),
                        Timestamp = c.Long(nullable: false),
                        StrokeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Strokes", t => t.StrokeId, cascadeDelete: true)
                .Index(t => t.StrokeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signatures", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Strokes", "SignatureId", "dbo.Signatures");
            DropForeignKey("dbo.Points", "StrokeId", "dbo.Strokes");
            DropIndex("dbo.Points", new[] { "StrokeId" });
            DropIndex("dbo.Strokes", new[] { "SignatureId" });
            DropIndex("dbo.Signatures", new[] { "AuthorId" });
            DropTable("dbo.Points");
            DropTable("dbo.Strokes");
            DropTable("dbo.Signatures");
            DropTable("dbo.Authors");
        }
    }
}
