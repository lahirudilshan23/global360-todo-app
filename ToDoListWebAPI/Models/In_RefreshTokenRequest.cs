namespace ToDoListWebAPI.Models
{
    public class In_RefreshTokenRequest
    {
        public string Username { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
