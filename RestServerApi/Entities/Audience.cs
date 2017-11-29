using System.ComponentModel.DataAnnotations;

namespace RestServerApi.Entities
{
    public class Audience
    {
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(500)]
        [Required]
        public string Name { get; set; }
    }
}