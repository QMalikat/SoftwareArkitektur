using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }  // Primær nøgle

        public string Email { get; set; } = string.Empty;

        [Column("Password_hash")]  // matcher databasen
        public string PasswordHash { get; set; } = string.Empty;

        [Column("User_type")]
        public int UserType { get; set; } = 0; // fx 0 = normal bruger, 1 = admin
    }
}
