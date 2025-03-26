using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LingoVerse.Repositories.Implementation
{
    public class UserPremiumSubscriptionsRepository : GenericRepository<UserPremiumSubscription>, IUserPremiumSubscriptionsRepository
    {
        private readonly LinguaVerseContext _context;
        public UserPremiumSubscriptionsRepository(LinguaVerseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserPremiumSubscription> GetByUserId(int userId)
        {
            return await _context.UserPremiumSubscriptions.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
