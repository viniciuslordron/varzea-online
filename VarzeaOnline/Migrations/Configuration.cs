using System.Data.Entity;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace VarzeaOnline.Migrations
{
    using System.Data.Entity.Migrations;
    using VarzeaOnline.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VarzeaOnline.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VarzeaOnline.Data.ApplicationDbContext context)
        {
            // Inglaterra - Premier League
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Manchester City", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Arsenal", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Liverpool", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Manchester United", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Chelsea", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Tottenham", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Newcastle", Liga = "Premier League", Pais = "Inglaterra" },
                new Time { Nome = "Aston Villa", Liga = "Premier League", Pais = "Inglaterra" }
            );

            // Espanha - La Liga
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Real Madrid", Liga = "La Liga", Pais = "Espanha" },
                new Time { Nome = "Barcelona", Liga = "La Liga", Pais = "Espanha" },
                new Time { Nome = "Atletico de Madrid", Liga = "La Liga", Pais = "Espanha" },
                new Time { Nome = "Real Sociedad", Liga = "La Liga", Pais = "Espanha" },
                new Time { Nome = "Athletic Bilbao", Liga = "La Liga", Pais = "Espanha" }
            );

            // Alemanha - Bundesliga
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Bayern de Munique", Liga = "Bundesliga", Pais = "Alemanha" },
                new Time { Nome = "Borussia Dortmund", Liga = "Bundesliga", Pais = "Alemanha" },
                new Time { Nome = "Bayer Leverkusen", Liga = "Bundesliga", Pais = "Alemanha" },
                new Time { Nome = "RB Leipzig", Liga = "Bundesliga", Pais = "Alemanha" }
            );

            // Italia - Serie A
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Inter de Milao", Liga = "Serie A", Pais = "Italia" },
                new Time { Nome = "Milan", Liga = "Serie A", Pais = "Italia" },
                new Time { Nome = "Juventus", Liga = "Serie A", Pais = "Italia" },
                new Time { Nome = "Napoli", Liga = "Serie A", Pais = "Italia" },
                new Time { Nome = "Roma", Liga = "Serie A", Pais = "Italia" }
            );

            // Franca - Ligue 1
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "PSG", Liga = "Ligue 1", Pais = "Franca" },
                new Time { Nome = "Marseille", Liga = "Ligue 1", Pais = "Franca" },
                new Time { Nome = "Lyon", Liga = "Ligue 1", Pais = "Franca" }
            );

            // Portugal
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Benfica", Liga = "Liga Portugal", Pais = "Portugal" },
                new Time { Nome = "Porto", Liga = "Liga Portugal", Pais = "Portugal" },
                new Time { Nome = "Sporting", Liga = "Liga Portugal", Pais = "Portugal" }
            );

            // Brasil - Brasileirao
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Flamengo", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Palmeiras", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Sao Paulo", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Corinthians", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Santos", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Gremio", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Internacional", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Atletico Mineiro", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Fluminense", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Vasco", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Botafogo", Liga = "Brasileirao", Pais = "Brasil" },
                new Time { Nome = "Cruzeiro", Liga = "Brasileirao", Pais = "Brasil" }
            );

            // Argentina
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Boca Juniors", Liga = "Liga Argentina", Pais = "Argentina" },
                new Time { Nome = "River Plate", Liga = "Liga Argentina", Pais = "Argentina" }
            );

            // Selecoes
            context.Times.AddOrUpdate(t => t.Nome,
                new Time { Nome = "Brasil", Liga = "Selecao", Pais = "Brasil" },
                new Time { Nome = "Argentina", Liga = "Selecao", Pais = "Argentina" },
                new Time { Nome = "Franca", Liga = "Selecao", Pais = "Franca" },
                new Time { Nome = "Alemanha", Liga = "Selecao", Pais = "Alemanha" },
                new Time { Nome = "Espanha", Liga = "Selecao", Pais = "Espanha" },
                new Time { Nome = "Inglaterra", Liga = "Selecao", Pais = "Inglaterra" },
                new Time { Nome = "Portugal", Liga = "Selecao", Pais = "Portugal" },
                new Time { Nome = "Italia", Liga = "Selecao", Pais = "Italia" },
                new Time { Nome = "Holanda", Liga = "Selecao", Pais = "Holanda" },
                new Time { Nome = "Belgica", Liga = "Selecao", Pais = "Belgica" }
            );

            // Usuario Admin padrao
            context.Usuarios.AddOrUpdate(u => u.Email,
                new Usuario { Nome = "Admin", Email = "admin@fifa.com", Senha = "123" }
            );
        }
    }
}
