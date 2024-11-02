using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class OrderDAO
    {
        private readonly FstoreContext _context;

        public OrderDAO(FstoreContext context)
        {
            _context = context;
        }

        // Create a new order with order details
        public Order CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        // Retrieve an order by its ID, including related OrderDetails and Books
        public Order? GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book)
                .FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
