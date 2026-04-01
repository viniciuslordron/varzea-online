using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VarzeaOnline.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100)]
        public string Senha { get; set; }

        public virtual ICollection<Campeonato> Campeonatos { get; set; }
    }
}