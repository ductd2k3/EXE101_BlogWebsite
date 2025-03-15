using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using LingoVerse.Services.Interface;

namespace LingoVerse.Services.Implementation
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<User> LoginAsync(User user)
        {
            return await _repository.LoginAsync(user);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            return await (_repository.RegisterAsync(user));
        }
    }
}
