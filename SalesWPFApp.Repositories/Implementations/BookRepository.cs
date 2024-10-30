using BusinessObject.Entities;
using SalesWPFApp.DAOs;
using SalesWPFApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        public void AddBook(Book book)
            => BookDAO.Instance.AddBook(book);

        public void DeleteBook(int id)
            => BookDAO.Instance.DeleteBook(id);

        public List<Book> GetAllBooks()
            => BookDAO.Instance.GetAllBooks();

        public Book? GetBookById(int id)
            => BookDAO.Instance.GetBookById(id);

        public void UpdateBook(Book book)
            => BookDAO.Instance.UpdateBook(book);

        public List<Book> GetBookByCategoryId(int categoryId)
            => BookDAO.Instance.GetBooksByCategoryId(categoryId);
    }
}
