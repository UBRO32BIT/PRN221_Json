using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Books
{
    [Authorize(Roles = "ADMIN")]
    public class DeleteModel : PageModel
    {
        private readonly IBookService _bookRepository;

        [BindProperty]
        public Book book { get; set; }

        public DeleteModel(IBookService bookRepository)
        {
            _bookRepository = bookRepository;
            book = new();
        }

        public IActionResult OnGet(int id)
        {
            book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (book == null || book.BookId <= 0)
            {
                return NotFound();
            }

            _bookRepository.DeleteBook(book.BookId);

            return RedirectToPage("./Index");
        }
    }
}