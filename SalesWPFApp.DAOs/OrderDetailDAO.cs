using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class OrderDetailDAO
    {
        private readonly FstoreContext _context;

        public OrderDetailDAO(FstoreContext context)
        {
            _context = context;
        }

        // Retrieve OrderDetail by OrderId and BookId
        public OrderDetail? GetOrderDetail(int orderId, int bookId)
        {
            return _context.OrderDetails
                .Include(od => od.Book)
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderId == orderId && od.BookId == bookId);
        }
    }
}
