using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWebApp.AuthService.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual User? User { get; set; }
    }
}
