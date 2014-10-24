namespace cicaudittrail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CICAUDITTRAIL.ROLE",
                c => new
                    {
                        ROLEID = c.Decimal(nullable: false, identity: true),
                        NAME = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ROLEID);
            
            CreateTable(
                "CICAUDITTRAIL.USER",
                c => new
                    {
                        USERID = c.Decimal(nullable: false, identity: true),
                        MATRICULE = c.String(nullable: false),
                        NOM = c.String(),
                        PRENOM = c.String(),
                    })
                .PrimaryKey(t => t.USERID);
            
        }
        
        public override void Down()
        {
            DropTable("CICAUDITTRAIL.USER");
            DropTable("CICAUDITTRAIL.ROLE");
        }
    }
}
