using BusinessObject.Entities;
using SalesWPFApp.DAOs;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using SalesWPFApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService()
        {
            _bookRepository = new BookRepository();
        }
        public void AddBook(Book book)
            => _bookRepository.AddBook(book);

        public void DeleteBook(int id)
            => _bookRepository.DeleteBook(id);

        public List<Book> GetAllBooks()
            => _bookRepository.GetAllBooks();

        public Book? GetBookById(int id)
            => _bookRepository.GetBookById(id);

        public void UpdateBook(Book book)
            => _bookRepository.UpdateBook(book);

        public List<Book> GetBookByCategoryId(int categoryId)
            => _bookRepository.GetBookByCategoryId(categoryId);
    }
}
