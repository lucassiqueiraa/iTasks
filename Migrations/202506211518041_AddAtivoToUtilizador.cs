namespace iTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtivoToUtilizador : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Utilizadores", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Utilizadores", "Ativo");
        }
    }
}
