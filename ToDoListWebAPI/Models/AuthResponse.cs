namespace ToDoListWebAPI.Models
{
    public class AuthResponse
    {
        public AuthToken AccessToken { get; set; } = new();
        public AuthToken RefreshToken { get; set; } = new();
    }
}
