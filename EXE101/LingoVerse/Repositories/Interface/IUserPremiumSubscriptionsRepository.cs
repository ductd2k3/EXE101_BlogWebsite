using LingoVerse.Models;

namespace LingoVerse.Repositories.Interface
{
    public interface IUserPremiumSubscriptionsRepository : IGenericRepository<UserPremiumSubscription>
    {
        Task<UserPremiumSubscription> GetByUserId (int userId);
    }
}
