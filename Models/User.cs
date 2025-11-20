namespace Lab5_ChristianMamani.Models
{
    public class User
    {
        public int Id { get; set; }  // PK
        public string Username { get; set; }
        public string Password { get; set; } // Hasheada con BCrypt
        public string Role { get; set; }  // "Admin" o "User"
    }
}