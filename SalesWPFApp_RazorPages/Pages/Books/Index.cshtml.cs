using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages
{
    [Authorize(Roles = "ADMIN")]
    public class BooksModel : PageModel
    {
        private readonly IBookService _bookRepository;

        public List<Book> Books { get; set; } = new List<Book>();

        public BooksModel(IBookService bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void OnGet()
        {
            Books = _bookRepository.GetAllBooks();
        }
    }
}
