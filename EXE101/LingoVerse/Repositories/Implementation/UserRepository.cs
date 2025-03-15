using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LingoVerse.Repositories.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly LinguaVerseContext _context;
        public UserRepository(LinguaVerseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(User user)
        {
            var foundUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash);

            if (foundUser == null)
            {
                return null;
            }
            return foundUser;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users
                    .AnyAsync(u => u.Email == user.Email);

                if (existingUser)
                {
                    return false;
                }

                _context.Users.Add(user);

                int rowsAffected = await _context.SaveChangesAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
