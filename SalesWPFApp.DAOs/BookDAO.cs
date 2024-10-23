using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class BookDAO
    {
        private FstoreContext _context;
        private static BookDAO? _instance;
        public BookDAO()
        {
            _context = new FstoreContext();
        }
        public static BookDAO Instance
        {
            get => _instance ?? new BookDAO();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.SingleOrDefault(p => p.BookId == id);
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.Category).ToList();
        }

        public void AddBook(Book book) {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            var updatedBook = _context.Books.SingleOrDefault(m => m.BookId == book.BookId);
            if (updatedBook != null)
            {
                updatedBook.BookName = book.BookName;
                updatedBook.Author = book.Author;
                updatedBook.Publisher = book.Publisher;
                updatedBook.Year = book.Year;
                updatedBook.Description = book.Description;
                updatedBook.ImageUrl = book.ImageUrl;
                updatedBook.UnitPrice = book.UnitPrice;
                updatedBook.UnitsInStock = book.UnitsInStock;
                updatedBook.CategoryId = book.CategoryId;
                _context.SaveChanges();
            }
        }

        public void DeleteBook(int id) {
            var book = _context.Books.SingleOrDefault(p =>p.BookId == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
