using System.ComponentModel.DataAnnotations;

namespace VarzeaOnline.Models
{
    public class Time
    {
        [Key]
        public int IdTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Liga { get; set; }

        [StringLength(50)]
        public string Pais { get; set; }
    }
}