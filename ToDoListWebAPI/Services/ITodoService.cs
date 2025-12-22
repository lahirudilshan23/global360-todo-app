using ToDoListWebAPI.Models;

namespace ToDoListWebAPI.Services
{
    public interface ITodoService
    {
        IEnumerable<In_TodoItem> GetAll();
        int Add(In_TodoItem input);
        void Update(In_TodoItem input);
        void Delete(int id);
    }
}
