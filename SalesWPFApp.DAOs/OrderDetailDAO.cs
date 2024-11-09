using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO? _instance;
        private readonly string _jsonFilePath = "orderDetails.json";

        public static OrderDetailDAO Instance => _instance ??= new OrderDetailDAO();

        private OrderDetailDAO()
        {
            // Initialize JSON file if it does not exist
            if (!File.Exists(_jsonFilePath))
            {
                File.WriteAllText(_jsonFilePath, "[]");
            }
        }

        // Helper method to read order details from the JSON file
        private List<OrderDetail> ReadOrderDetailsFromFile()
        {
            var jsonData = File.ReadAllText(_jsonFilePath);
            return JsonSerializer.Deserialize<List<OrderDetail>>(jsonData) ?? new List<OrderDetail>();
        }

        // Helper method to write order details to the JSON file
        private void WriteOrderDetailsToFile(List<OrderDetail> orderDetails)
        {
            var jsonData = JsonSerializer.Serialize(orderDetails, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonData);
        }

        // Retrieve OrderDetail by OrderId and BookId
        public OrderDetail? GetOrderDetail(int orderId, int bookId)
        {
            var orderDetails = ReadOrderDetailsFromFile();
            return orderDetails.FirstOrDefault(od => od.OrderId == orderId && od.BookId == bookId);
        }

        // Get all OrderDetails for a specific order
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = ReadOrderDetailsFromFile();
            return orderDetails.Where(od => od.OrderId == orderId).ToList();
        }

        // Add a new OrderDetail
        public void AddOrderDetail(OrderDetail orderDetail)
        {
            var orderDetails = ReadOrderDetailsFromFile();

            // Add the new OrderDetail
            orderDetails.Add(orderDetail);

            WriteOrderDetailsToFile(orderDetails);
        }

        // Update an existing OrderDetail
        public void UpdateOrderDetail(OrderDetail updatedOrderDetail)
        {
            var orderDetails = ReadOrderDetailsFromFile();
            var index = orderDetails.FindIndex(od => od.OrderId == updatedOrderDetail.OrderId && od.BookId == updatedOrderDetail.BookId);

            if (index != -1)
            {
                orderDetails[index] = updatedOrderDetail;
                WriteOrderDetailsToFile(orderDetails);
            }
        }

        // Delete an OrderDetail by OrderId and BookId
        public bool DeleteOrderDetail(int orderId, int bookId)
        {
            var orderDetails = ReadOrderDetailsFromFile();
            var orderDetailToRemove = orderDetails.SingleOrDefault(od => od.OrderId == orderId && od.BookId == bookId);

            if (orderDetailToRemove != null)
            {
                orderDetails.Remove(orderDetailToRemove);
                WriteOrderDetailsToFile(orderDetails);
                return true;
            }
            return false;
        }
    }
}
