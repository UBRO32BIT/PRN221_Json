using BusinessObject.Entities;
using Microsoft.IdentityModel.Tokens;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using SalesWPFApp.Services.Implementations;
using SalesWPFApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for BookCategoryManagementWindow.xaml
    /// </summary>
    public partial class BookCategoryManagementWindow : Window
    {
        private IBookCategoryService _bookCategoryService;
        private IBookService _bookService;
        public BookCategoryManagementWindow()
        {
            InitializeComponent();
            _bookCategoryService = new BookCategoryService();
            _bookService = new BookService();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _bookCategoryService.GetAllCategories();
                BookDataGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields()
        {
            // Clear TextBox inputs
            txtName.Text = string.Empty;
        }

        private void BookDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookList = _bookCategoryService.GetAllCategories();
                BookDataGrid.ItemsSource = bookList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookDataGrid.SelectedItem is BookCategory selectedBook)
            {
                txtName.Text = selectedBook.Name;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchBook = SearchTextBox.Text.ToLower();
                var allBooks = _bookCategoryService.GetAllCategories();
                var results = allBooks.Where(m => m.Name.ToLower().Contains(searchBook));
                BookDataGrid.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchButton_Click(sender, e);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Input validation
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Category Name is required.");
                    return;
                }

                // Creating a new Book object
                BookCategory bookCategory = new BookCategory()
                {
                    Name = txtName.Text,
                };

                // Add the book to the repository
                _bookCategoryService.AddCategory(bookCategory);

                MessageBox.Show("Category added successfully!");

                // Reload books and clear the input fields
                LoadCategories();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a book is selected in the DataGrid
                if (BookDataGrid.SelectedItem is BookCategory selectedCategory)
                {
                    // Validate input fields
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please fill out all fields correctly.");
                        return;
                    }

                    // Update the selected book with new values
                    selectedCategory.Name = txtName.Text;

                    // Save changes in the repository
                    _bookCategoryService.UpdateCategory(selectedCategory);

                    MessageBox.Show("Category updated successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a bookCategory to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Reload the book list and clear the input fields
                LoadCategories();
                ClearInputFields();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a book is selected in the DataGrid
                if (BookDataGrid.SelectedItem is BookCategory selectedBook)
                {
                    // Confirm deletion
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedBook.Name}'?",
                        "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }

                    var books = _bookService.GetBookByCategoryId(selectedBook.CategoryId);
                    if (!books.IsNullOrEmpty())
                    {
                        var bookNames = string.Join("\n", books.Select(b => $"ID: {b.BookId}, Name: {b.BookName}"));
                        MessageBox.Show($"Cannot delete category. The following books are attached to it: \n{bookNames}");
                        return;
                    }
                    // Delete the book
                    _bookCategoryService.DeleteCategory(selectedBook.CategoryId);
                    MessageBox.Show("Book deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a bookCategory to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Reload the book list and clear the input fields
                LoadCategories();
                ClearInputFields();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();

            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown(); // Exit the application
        }
    }
}
