using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RedisAPI.Models
{
    public class Igrica
    {
        [Required]
        [NotNull]
        public int Id { get; set; }

        [Required]
        public string? Naziv {get; set;}

        [Required]
        public int Cena { get; set; }

        public bool onSale { get; set; }

        public int? novaCena { get; set; } 
    }
}