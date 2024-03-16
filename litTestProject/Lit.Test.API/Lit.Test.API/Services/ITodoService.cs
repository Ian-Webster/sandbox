using Lit.Test.API.Models;

namespace Lit.Test.API.Services
{
    public interface ITodoService
    {
        List<TodoItem> GetTodoItems();

        void AddTodoItem(TodoAddItem item);

        void ToggleCompleted(int itemId);
    }
}
