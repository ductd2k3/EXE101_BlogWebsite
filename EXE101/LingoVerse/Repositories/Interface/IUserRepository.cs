using LingoVerse.Models;

namespace LingoVerse.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> RegisterAsync(User user);
        Task<User> LoginAsync(User user);
    }
}
