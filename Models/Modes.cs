namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHashed { get; set; }
    }
    public class UserDTO
    {
        [Required(ErrorMessage = "Name should not be empty")]
        public string? Nickname { get; set; }

        [Required(ErrorMessage = "Email should not be empty"), EmailAddress(ErrorMessage = "Wrong email format")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Password should not be empty")]
        public string? Password { get; set; }
    }
    public class Note
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime dateCreated = DateTime.Now;
        public int UserId { get; set; }
    }
    public class NoteDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}