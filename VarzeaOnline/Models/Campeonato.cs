using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VarzeaOnline.Models
{
    public class Campeonato
    {
        [Key]
        public int IdCampeonato { get; set; }
        [Required(ErrorMessage = "O nome do campeonato é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome do Campeonato")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O tipo do campeonato é obrigatório")]
        [Display(Name = "Tipo de Campeonato")]
        public string TipoCampeonato { get; set; }
        [Required(ErrorMessage = "A quantidade de jogadores é obrigatória")]
        [Display(Name = "Quantidade de Jogadores")]
        public int QuantidadeJogadores { get; set; }
        public int RodadaAtual { get; set; }
        public int TotalRodadas { get; set; }
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }
        public string Status { get; set; }
        // Chave estrangeira
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        // Relacionamentos
        public virtual ICollection<Jogador> Jogadores { get; set; }
        public virtual ICollection<Partida> Partidas { get; set; }
        public virtual ICollection<Classificacao> Classificacoes { get; set; }
    }
}