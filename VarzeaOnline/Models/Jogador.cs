using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VarzeaOnline.Models
{
    public class Jogador
    {
        [Key]
        public int IdJogador { get; set; }

        [Required(ErrorMessage = "O nome do jogador é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome do Jogador")]
        public string NomeJogador { get; set; }

        // Time escolhido pelo jogador
        [ForeignKey("Time")]
        [Display(Name = "Time")]
        public int IdTime { get; set; }

        public virtual Time Time { get; set; }

        [ForeignKey("Campeonato")]
        public int IdCampeonato { get; set; }

        public virtual Campeonato Campeonato { get; set; }

        public virtual ICollection<Classificacao> Classificacoes { get; set; }
    }
}