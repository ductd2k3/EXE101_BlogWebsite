using LingoVerse.Models;

namespace LingoVerse.Services.Interface
{
    public interface IUserService : IGenericService<User>
    {
        Task<bool> RegisterAsync(User user);
        Task<User> LoginAsync(User user);
    }
}
