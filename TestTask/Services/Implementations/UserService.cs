using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        protected ApplicationDbContext _dbcontext;
        public UserService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<User> GetUser()
        {
            return Task.Run(GetUserWithTheMostOrders);
        }

        public Task<List<User>> GetUsers()
        {
            return Task.Run(GetInaсtiveUsers);
        }

        private User GetUserWithTheMostOrders()
        {
            var allOrders = _dbcontext.Orders.Include(x => x.User);

            var userId = allOrders.GroupBy(x => x.User.Id, x => x.Quantity)
                .Select(x => new { id = x.Key, quantity = x.Sum() }).ToList()
                .MaxBy(x => x.quantity).id;

            return _dbcontext.Users.Include(x => x.Orders).Where(x => x.Id == userId).FirstOrDefault();
        }

        private List<User> GetInaсtiveUsers()
        {
            return _dbcontext.Users.Include(x => x.Orders).Where(x => x.Status == Enums.UserStatus.Inactive).ToList();
        }
    }
}
