namespace JwtWebApi.Models
{
        public class UserProfileDto
        {
                public string Username { get; set; } = string.Empty;
                public string? FirstName { get; set; }
                public string? LastName { get; set; }
                public string? Email { get; set; }
        }
}
