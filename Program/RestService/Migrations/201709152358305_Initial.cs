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
                        Author_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Strokes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isScaled = c.Boolean(nullable: false),
                        Signature_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signatures", t => t.Signature_Id)
                .Index(t => t.Signature_Id);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        Pressure = c.Single(nullable: false),
                        Timestamp = c.Long(nullable: false),
                        Stroke_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Strokes", t => t.Stroke_Id)
                .Index(t => t.Stroke_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Signatures", "Author_Id", "dbo.Authors");
            DropForeignKey("dbo.Strokes", "Signature_Id", "dbo.Signatures");
            DropForeignKey("dbo.Points", "Stroke_Id", "dbo.Strokes");
            DropIndex("dbo.Points", new[] { "Stroke_Id" });
            DropIndex("dbo.Strokes", new[] { "Signature_Id" });
            DropIndex("dbo.Signatures", new[] { "Author_Id" });
            DropTable("dbo.Points");
            DropTable("dbo.Strokes");
            DropTable("dbo.Signatures");
            DropTable("dbo.Authors");
        }
    }
}
