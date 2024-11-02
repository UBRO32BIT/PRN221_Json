using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IBookService _bookService;

        public IndexModel(ILogger<IndexModel> logger, IBookService bookRepository)
        {
            _bookService = bookRepository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public List<Book> BookList { get; set; } = new();

        public string? ShoppingMessage { get; set; }

        public void OnGet()
        {
            // Replace this with an actual search method in your repository
            BookList = string.IsNullOrWhiteSpace(Search)
                ? _bookService.GetAllBooks()
                : _bookService.GetAllBooks().Where(m =>
                m.BookName.ToLower().Contains(Search) ||
                m.Author.ToLower().Contains(Search) ||
                m.Description.ToLower().Contains(Search) ||
                m.Publisher.ToLower().Contains(Search)).ToList();
        }
    }
}
