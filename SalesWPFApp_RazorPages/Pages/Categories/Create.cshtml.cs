using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesWPFApp.Services.Interfaces;

namespace SalesWPFApp_RazorPages.Pages.Categories
{
    [Authorize(Roles = "ADMIN")]
    public class CreateModel : PageModel
    {
        private readonly IBookCategoryService _categoryRepository;

        [BindProperty]
        public BookCategory category { get; set; }


        public CreateModel(IBookCategoryService categoryRepository)
        {
            _categoryRepository = categoryRepository;
            category = new();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _categoryRepository.AddCategory(category);
            return RedirectToPage("Index");
        }
    }
}
