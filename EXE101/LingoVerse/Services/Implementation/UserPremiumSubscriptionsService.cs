using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using LingoVerse.Services.Interface;

namespace LingoVerse.Services.Implementation
{
    public class UserPremiumSubscriptionsService : GenericService<UserPremiumSubscription>, IUserPremiumSubscriptionsService
    {
        private readonly IUserPremiumSubscriptionsRepository _repository;
        public UserPremiumSubscriptionsService(IUserPremiumSubscriptionsRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<UserPremiumSubscription> GetByUserId(int userId)
        {
            return await _repository.GetByUserId(userId);
        }
    }
}
