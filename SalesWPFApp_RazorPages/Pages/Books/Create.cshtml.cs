using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWPFApp.Repositories.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCategoryRepository _categoryRepository;

        [BindProperty]
        public Book book { get; set; }
        [BindProperty]
        public List<SelectListItem> categories { get; set; }

        public CreateModel(IBookRepository bookRepository, IBookCategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            book = new();
        }

        public void OnGet()
        {
            // Fetch the categories from the repository and map to SelectListItems
            categories = _categoryRepository.GetAllCategories()
                .Select(category => new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name
                })
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddBook(book);
            }

            return RedirectToPage("Index");
        }
    }
}
