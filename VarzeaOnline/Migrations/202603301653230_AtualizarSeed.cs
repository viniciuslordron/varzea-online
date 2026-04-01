namespace VarzeaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizarSeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Time",
                c => new
                    {
                        IdTime = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Liga = c.String(maxLength: 50),
                        Pais = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdTime);
            
            AddColumn("dbo.Campeonato", "QuantidadeJogadores", c => c.Int(nullable: false));
            AddColumn("dbo.Jogador", "IdTime", c => c.Int(nullable: false));
            AlterColumn("dbo.Jogador", "NomeJogador", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Jogador", "IdTime");
            AddForeignKey("dbo.Jogador", "IdTime", "dbo.Time", "IdTime");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jogador", "IdTime", "dbo.Time");
            DropIndex("dbo.Jogador", new[] { "IdTime" });
            AlterColumn("dbo.Jogador", "NomeJogador", c => c.String(nullable: false));
            DropColumn("dbo.Jogador", "IdTime");
            DropColumn("dbo.Campeonato", "QuantidadeJogadores");
            DropTable("dbo.Time");
        }
    }
}
