using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;

namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class OrderRepository : BHLRepositoryBase<ProductRepository>, IOrderRepository
    {
        private IBHLDbContext Context;
        public OrderRepository(IServiceProvider serviceProvider, IBHLDbContext context) : base(serviceProvider)
        {
            Context = context;
        }

        public async Task<string> CreateOrderProduct(OrderEntity request, CancellationToken cancellationToken)
        {
            
            var user = await Context.Users.FindAsync(request.UserId);
            if(user == null)
            {
                return "Người dùng không tồn tại";
            }
       

            await Context.Orders.AddAsync(request);
            await Context.SaveChangesAsync(cancellationToken);

            return "Thêm đơn hàng thành công";
        }

        public async Task<IEnumerable<OrderEntity>> GetOrderProduct()
        {

            return await Context.Orders.Include(o => o.Status)
            .ToListAsync();
        }
    }
}
