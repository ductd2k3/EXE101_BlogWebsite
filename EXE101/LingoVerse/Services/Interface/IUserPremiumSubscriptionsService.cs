using LingoVerse.Models;

namespace LingoVerse.Services.Interface
{
    public interface IUserPremiumSubscriptionsService : IGenericService<UserPremiumSubscription>
    {
        Task<UserPremiumSubscription> GetByUserId(int userId);
    }
}
