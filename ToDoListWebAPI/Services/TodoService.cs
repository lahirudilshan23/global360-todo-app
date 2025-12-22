using ToDoListWebAPI.Models;

namespace ToDoListWebAPI.Services
{
    public class TodoService : ITodoService
    {
        // Static in-memory storage
        private static readonly List<In_TodoItem> _todos = [];
        private static int _nextId = 1;
        private static readonly object _lock = new();

        public IEnumerable<In_TodoItem> GetAll()
        {
            lock (_lock)
            {
                return _todos.ToList();
            }
        }

        public int Add(In_TodoItem input)
        {
            input.Id = GetNextId();

            lock (_lock)
            {
                _todos.Add(input);
            }

            return input.Id;
        }

        public void Update(In_TodoItem input)
        {
            lock (_lock)
            {
                var todo = _todos.FirstOrDefault(t => t.Id == input.Id);
                if (todo != null)
                {
                    todo.Title = input.Title;
                }
            }
        }

        public void Delete(int id)
        {
            lock (_lock)
            {
                var todo = _todos.FirstOrDefault(t => t.Id == id);
                if (todo != null)
                {
                    _todos.Remove(todo);
                }
            }
        }

        private static int GetNextId()
        {
            lock (_lock)
            {
                return _nextId++;
            }
        }
    }
}
