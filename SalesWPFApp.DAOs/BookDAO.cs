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
    public class BookDAO
    {
        private static BookDAO? _instance;
        private readonly string _jsonFilePath = "books.json";

        public static BookDAO Instance => _instance ??= new BookDAO();

        private BookDAO()
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

        private List<Book> ReadBooksFromFile()
        {
            try
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                return JsonSerializer.Deserialize<List<Book>>(jsonData) ?? new List<Book>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading books from file: {ex.Message}");
                return new List<Book>();
            }
        }

        private void WriteBooksToFile(List<Book> books)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing books to file: {ex.Message}");
            }
        }

        private Book PopulateBookCategory(Book book)
        {
            try
            {
                var categoryDAO = BookCategoryDAO.Instance;
                var category = categoryDAO.GetBookCategoryById(book.CategoryId);
                if (category != null)
                {
                    book.Category = category;
                }
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error populating book category: {ex.Message}");
                throw;
            }
        }

        public Book? GetBookById(int id)
        {
            try
            {
                var book = ReadBooksFromFile().SingleOrDefault(p => p.BookId == id);
                return book != null ? PopulateBookCategory(book) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving book by ID: {ex.Message}");
                return null;
            }
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                var books = ReadBooksFromFile();
                return books.Select(PopulateBookCategory).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all books: {ex.Message}");
                return new List<Book>();
            }
        }

        public void AddBook(Book book)
        {
            try
            {
                var books = ReadBooksFromFile();
                book.BookId = books.Any() ? books.Max(b => b.BookId) + 1 : 1;
                books.Add(book);
                WriteBooksToFile(books);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding book: {ex.Message}");
            }
        }

        public void UpdateBook(Book book)
        {
            try
            {
                var books = ReadBooksFromFile();
                var index = books.FindIndex(b => b.BookId == book.BookId);

                if (index != -1)
                {
                    books[index] = book;
                    WriteBooksToFile(books);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book: {ex.Message}");
            }
        }

        public void DeleteBook(int id)
        {
            try
            {
                var books = ReadBooksFromFile();
                var bookToRemove = books.SingleOrDefault(b => b.BookId == id);

                if (bookToRemove != null)
                {
                    books.Remove(bookToRemove);
                    WriteBooksToFile(books);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book: {ex.Message}");
            }
        }

        public List<Book> GetBooksByCategoryId(int categoryId)
        {
            try
            {
                var books = ReadBooksFromFile().Where(b => b.CategoryId == categoryId).ToList();
                return books.Select(PopulateBookCategory).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving books by category ID: {ex.Message}");
                return new List<Book>();
            }
        }
    }
}
