using Lit.Test.API.Models;

namespace Lit.Test.API.Services
{
    public class TodoService : ITodoService
    {
        private List<TodoItem> _items;

        public TodoService()
        {
            _items = new List<TodoItem>
            {
                new TodoItem { Id = 1, Name = "Mow the grass", IsComplete = false },
                new TodoItem { Id = 2, Name = "Wash the car", IsComplete = false },
                new TodoItem { Id = 3, Name = "Take out the rubbish", IsComplete = false }
            };
        }

        public void AddTodoItem(TodoAddItem item)
        {
            _items.Add(new TodoItem 
            { 
                Id = _items.Count + 1,
                Name = item.Name,
                IsComplete = false
            });
        }

        public void ToggleCompleted(int itemId)
        {
            var item = _items.First(_items => _items.Id == itemId);
            item.IsComplete = !item.IsComplete;
        }

        public List<TodoItem> GetTodoItems()
        {
            return _items;
        }
    }
}
