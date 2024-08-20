using System.ComponentModel.DataAnnotations;

namespace RedisAPI.Models
{
    public class User 
    { 
        [Key]
        public int Id {get; set;}

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set;}

        public string? Wishlist { get; set; }
    }
}