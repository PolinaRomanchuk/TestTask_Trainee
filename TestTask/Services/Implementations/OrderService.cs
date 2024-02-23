using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        protected ApplicationDbContext _dbcontext;
        const int QUANTITYOFPRODUCTS = 10;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Task<Order> GetOrder()
        {
            return Task.Run(GetOrderWithTheLargestTotalCost);
        }

        public Task<List<Order>> GetOrders()
        {
            return Task.Run(GetOrdersWichQuantityOfProductsMoreThanTen);
        }

        private Order GetOrderWithTheLargestTotalCost()
        {
            var allOrders = _dbcontext.Orders.Include(x => x.User);

            var orderId = allOrders
                .Select(x => new { id = x.Id, totalcost = x.Quantity * x.Price }).ToList()
                .MaxBy(x => x.totalcost).id;

            return allOrders.Where(x => x.Id == orderId).FirstOrDefault();
        }

        private List<Order> GetOrdersWichQuantityOfProductsMoreThanTen()
        {
            return _dbcontext.Orders.Include(x => x.User).Where(x => x.Quantity > QUANTITYOFPRODUCTS).ToList();
        }

    }
}
