using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VarzeaOnline.Models
{
    public class Partida
    {
        [Key]
        public int IdPartida { get; set; }

        public int Rodada { get; set; }

        public int? GolsCasa { get; set; }
        public int? GolsFora { get; set; }

        public bool ResultadoRegistrado { get; set; }

        public DateTime DataPartida { get; set; }

        [ForeignKey("Campeonato")]
        public int IdCampeonato { get; set; }
        public virtual Campeonato Campeonato { get; set; }

        [ForeignKey("JogadorCasa")]
        public int IdJogadorCasa { get; set; }
        public virtual Jogador JogadorCasa { get; set; }

        [ForeignKey("JogadorFora")]
        public int IdJogadorFora { get; set; }
        public virtual Jogador JogadorFora { get; set; }
    }
}