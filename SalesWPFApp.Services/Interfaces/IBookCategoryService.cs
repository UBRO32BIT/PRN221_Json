using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Services.Interfaces
{
    public interface IBookCategoryService
    {
        public void AddCategory(BookCategory category);
        public BookCategory GetCategoryById(int id);
        public List<BookCategory> GetAllCategories();
        public void UpdateCategory(BookCategory category);
        public void DeleteCategory(int id);
    }
}
