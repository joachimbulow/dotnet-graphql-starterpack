using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Starterpack.Auth.Persistance.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime ExpiresAt { get; set; }

        [Key]
        [Required]
        public Guid UserId { get; set; }
    }
}