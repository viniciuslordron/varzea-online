using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VarzeaOnline.Models
{
    public class Classificacao
    {
        [Key]
        public int IdClassificacao { get; set; }

        public int Jogos { get; set; }
        public int Pontos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }
        public int SaldoGols { get; set; }

        [ForeignKey("Campeonato")]
        public int IdCampeonato { get; set; }
        public virtual Campeonato Campeonato { get; set; }

        [ForeignKey("Jogador")]
        public int IdJogador { get; set; }
        public virtual Jogador Jogador { get; set; }
    }
}