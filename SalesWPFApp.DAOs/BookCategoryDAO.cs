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
    public class BookCategoryDAO
    {
        private static BookCategoryDAO? _instance;
        private readonly string _jsonFilePath = "bookCategories.json";

        public static BookCategoryDAO Instance => _instance ??= new BookCategoryDAO();

        private BookCategoryDAO()
        {
            try
            {
                if (!File.Exists(_jsonFilePath))
                {
                    File.WriteAllText(_jsonFilePath, "[]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing file: {ex.Message}");
                throw;
            }
        }

        private List<BookCategory> ReadCategoriesFromFile()
        {
            try
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                return JsonSerializer.Deserialize<List<BookCategory>>(jsonData) ?? new List<BookCategory>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading categories from file: {ex.Message}");
                return new List<BookCategory>();
            }
        }

        private void WriteCategoriesToFile(List<BookCategory> categories)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing categories to file: {ex.Message}");
            }
        }

        public BookCategory? GetBookCategoryById(int id)
        {
            try
            {
                return ReadCategoriesFromFile().SingleOrDefault(c => c.CategoryId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving category by ID: {ex.Message}");
                return null;
            }
        }

        public List<BookCategory> GetAllCategories()
        {
            try
            {
                return ReadCategoriesFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all categories: {ex.Message}");
                return new List<BookCategory>();
            }
        }

        public void AddBookCategory(BookCategory category)
        {
            try
            {
                var categories = ReadCategoriesFromFile();

                category.CategoryId = categories.Any() ? categories.Max(c => c.CategoryId) + 1 : 1;

                categories.Add(category);
                WriteCategoriesToFile(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
            }
        }

        public void UpdateBookCategory(BookCategory category)
        {
            try
            {
                var categories = ReadCategoriesFromFile();
                var index = categories.FindIndex(c => c.CategoryId == category.CategoryId);

                if (index != -1)
                {
                    categories[index].Name = category.Name;
                    WriteCategoriesToFile(categories);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
            }
        }

        public void DeleteBookCategory(int id)
        {
            try
            {
                var categories = ReadCategoriesFromFile();
                var categoryToRemove = categories.SingleOrDefault(c => c.CategoryId == id);

                if (categoryToRemove != null)
                {
                    categories.Remove(categoryToRemove);
                    WriteCategoriesToFile(categories);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
            }
        }
    }
}
