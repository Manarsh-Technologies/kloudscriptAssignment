using System.ComponentModel.DataAnnotations;

namespace kloudscript.Test.API.Entity
{
    public class CacheMemoryEntity
    {
        [Required]
        public string CacheKey { get; set; } = "";
        [Required]
        public int ExpirationTime { get; set; }
        [Required]
        public string CacheValue { get; set; } = "";
    }
}
