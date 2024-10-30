using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Services.Interfaces
{
    public interface IBookService
    {
        public void AddBook(Book book);
        public Book GetBookById(int id);
        public List<Book> GetAllBooks();
        public void UpdateBook(Book book);
        public void DeleteBook(int id);
        public List<Book> GetBookByCategoryId(int categoryId);
    }
}
