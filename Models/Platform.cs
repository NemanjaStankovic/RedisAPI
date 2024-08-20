using System.ComponentModel.DataAnnotations;

namespace RedisAPI.Models
{
    public class Platform
    {
        [Required]
        public string Id { get; set; } = $"url:{Guid.NewGuid().ToString()}:id";

        [Required]
        public string Name {get; set;} = String.Empty;
    }
}