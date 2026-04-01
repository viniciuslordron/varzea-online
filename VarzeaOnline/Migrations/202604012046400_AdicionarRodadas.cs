namespace VarzeaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarRodadas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campeonato", "RodadaAtual", c => c.Int(nullable: false));
            AddColumn("dbo.Campeonato", "TotalRodadas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campeonato", "TotalRodadas");
            DropColumn("dbo.Campeonato", "RodadaAtual");
        }
    }
}
