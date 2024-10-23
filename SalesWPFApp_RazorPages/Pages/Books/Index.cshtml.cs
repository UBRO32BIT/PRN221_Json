using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesWPFApp.Repositories.Interfaces;
using BusinessObject.Entities;

namespace SalesWPFApp_RazorPages.Pages
{
    public class BooksModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public List<Book> Books { get; set; } = new List<Book>();

        public BooksModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void OnGet()
        {
            Books = _bookRepository.GetAllBooks();
        }
    }
}
