using GKS.Core.IRepositories;
using GKS.Core.IServices;


namespace GKS.Service.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _repository;

        public UserActivityService(IUserActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<int, int>> GetUserMonthlyUsageAsync(int userId, int year, int month)
        {
            return await _repository.GetUserMonthlyUsageAsync(userId, year, month);
        }

        public async Task<Dictionary<int, int>> GetUserYearlyUsageAsync(int userId, int year)
        {
            return await _repository.GetUserYearlyUsageAsync(userId, year);
        }

        public async Task<Dictionary<int, int>> GetYearlyUsageAsync(int year)
        {
            return await _repository.GetYearlyUsageAsync(year);
        }

        public async Task<Dictionary<int, int>> GetMonthlyUsageAsync(int year, int month)
        {
            return await _repository.GetMonthlyUsageAsync(year, month);
        }
    }
}
