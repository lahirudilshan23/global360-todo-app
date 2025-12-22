namespace ToDoListWebAPI.Models
{
    public class AuthToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
