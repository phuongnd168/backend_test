using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.DTO.Order;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
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

        public async Task<ApiResponse> CreateOrderProduct(CreateOrderProductDTO request, CancellationToken cancellationToken)
        {
            try {
                var user = await Context.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    return ResponseHelper.Error(404, "Người dùng không tồn tại");
                }
                var order = new OrderEntity
                {
                    Products = request.Products,
                    UserId = user.Id,
                    Created_at = DateTime.Now,
                };

                await Context.Orders.AddAsync(order);
                await Context.SaveChangesAsync(cancellationToken);

                return ResponseHelper.Created("Thêm đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }
            
        }

        public async Task<ApiResponse> GetOrderProduct()
        {
            try
            {

                var orders = await Context.Orders.Include(o => o.Status).ToListAsync();
                List<GetListOrderItemDTO> data = new List<GetListOrderItemDTO>();
                foreach (var order in orders)
                {
                    data.Add(new GetListOrderItemDTO
                    {
                        Product = order.Products,
                        StatusName = order.Status.Name,
                        OrderId = order.OrderId,
                        CreatedTime = order.Created_at,
                    });
                }


                return ResponseHelper.Success("Thành công", data);
            }
            catch (Exception ex)
            {
                return ResponseHelper.Error(500, ex.Message);
            }

        
        }
    }
}
