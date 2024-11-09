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
    public class OrderDAO
    {
        private static OrderDAO? _instance;
        private readonly string _jsonFilePath = "orders.json";

        public static OrderDAO Instance => _instance ??= new OrderDAO();

        private OrderDAO()
        {
            // Initialize JSON file if it does not exist
            if (!File.Exists(_jsonFilePath))
            {
                File.WriteAllText(_jsonFilePath, "[]");
            }
        }

        // Helper method to read orders from the JSON file
        private List<Order> ReadOrdersFromFile()
        {
            var jsonData = File.ReadAllText(_jsonFilePath);
            return JsonSerializer.Deserialize<List<Order>>(jsonData) ?? new List<Order>();
        }

        // Helper method to write orders to the JSON file
        private void WriteOrdersToFile(List<Order> orders)
        {
            var jsonData = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonData);
        }

        // Create a new order with order details
        public Order CreateOrder(Order order)
        {
            var orders = ReadOrdersFromFile();

            // Auto-generate OrderId if not set
            order.OrderId = orders.Any() ? orders.Max(o => o.OrderId) + 1 : 1;
            orders.Add(order);

            WriteOrdersToFile(orders);
            return order;
        }

        // Retrieve an order by its ID, including related OrderDetails and Books
        public Order? GetOrderById(int orderId)
        {
            var orders = ReadOrdersFromFile();
            return orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        // Get all orders
        public List<Order> GetAllOrders()
        {
            return ReadOrdersFromFile();
        }

        // Update an existing order
        public bool UpdateOrder(Order updatedOrder)
        {
            var orders = ReadOrdersFromFile();
            var index = orders.FindIndex(o => o.OrderId == updatedOrder.OrderId);

            if (index != -1)
            {
                orders[index] = updatedOrder;
                WriteOrdersToFile(orders);
                return true;
            }
            return false;
        }

        // Delete an order by its ID
        public bool DeleteOrder(int orderId)
        {
            var orders = ReadOrdersFromFile();
            var orderToRemove = orders.SingleOrDefault(o => o.OrderId == orderId);

            if (orderToRemove != null)
            {
                orders.Remove(orderToRemove);
                WriteOrdersToFile(orders);
                return true;
            }
            return false;
        }
    }
}
