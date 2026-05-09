using System.ComponentModel.DataAnnotations;

namespace Day18.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string DisplayName => string.IsNullOrEmpty(FullName) ? Username : FullName;
    }
}