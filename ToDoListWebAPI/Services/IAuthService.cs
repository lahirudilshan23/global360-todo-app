using ToDoListWebAPI.Models;

namespace ToDoListWebAPI.Services
{
    public interface IAuthService
    {
        AuthResponse Login(string username, string password);
        AuthResponse Refresh(string username, string refreshToken);
    }
}
