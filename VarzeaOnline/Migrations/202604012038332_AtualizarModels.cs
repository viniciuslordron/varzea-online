namespace VarzeaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizarModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classificacao", "Jogos", c => c.Int(nullable: false));
            AddColumn("dbo.Partida", "ResultadoRegistrado", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Partida", "GolsCasa", c => c.Int());
            AlterColumn("dbo.Partida", "GolsFora", c => c.Int());
            CreateIndex("dbo.Partida", "IdJogadorCasa");
            CreateIndex("dbo.Partida", "IdJogadorFora");
            AddForeignKey("dbo.Partida", "IdJogadorCasa", "dbo.Jogador", "IdJogador");
            AddForeignKey("dbo.Partida", "IdJogadorFora", "dbo.Jogador", "IdJogador");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Partida", "IdJogadorFora", "dbo.Jogador");
            DropForeignKey("dbo.Partida", "IdJogadorCasa", "dbo.Jogador");
            DropIndex("dbo.Partida", new[] { "IdJogadorFora" });
            DropIndex("dbo.Partida", new[] { "IdJogadorCasa" });
            AlterColumn("dbo.Partida", "GolsFora", c => c.Int(nullable: false));
            AlterColumn("dbo.Partida", "GolsCasa", c => c.Int(nullable: false));
            DropColumn("dbo.Partida", "ResultadoRegistrado");
            DropColumn("dbo.Classificacao", "Jogos");
        }
    }
}
