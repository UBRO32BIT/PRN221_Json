using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class BookCategoryDAO
    {
        private FstoreContext _context;
        private static BookCategoryDAO? _instance;

        // Private constructor to prevent direct instantiation
        private BookCategoryDAO()
        {
            _context = new FstoreContext();
        }

        // Singleton instance
        public static BookCategoryDAO Instance
        {
            get => _instance ?? new BookCategoryDAO();
        }

        // Get BookCategory by Id
        public BookCategory? GetBookCategoryById(int id)
        {
            return _context.BookCategories.SingleOrDefault(c => c.CategoryId == id);
        }

        // Get all BookCategories
        public List<BookCategory> GetAllCategories()
        {
            return _context.BookCategories.ToList();
        }

        // Add new BookCategory
        public void AddBookCategory(BookCategory category)
        {
            _context.BookCategories.Add(category);
            _context.SaveChanges();
        }

        // Update BookCategory
        public void UpdateBookCategory(BookCategory category)
        {
            var updatedCategory = _context.BookCategories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
            if (updatedCategory != null)
            {
                updatedCategory.Name = category.Name;
                _context.SaveChanges();
            }
        }

        // Delete BookCategory
        public void DeleteBookCategory(int id)
        {
            var category = _context.BookCategories.SingleOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                _context.BookCategories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
