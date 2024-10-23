using BusinessObject.Entities;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
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
        private IBookCategoryRepository _bookCategoryRepository;
        public BookCategoryManagementWindow()
        {
            InitializeComponent();
            _bookCategoryRepository = new BookCategoryRepository();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _bookCategoryRepository.GetAllCategories();
                BookDataGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                var bookList = _bookCategoryRepository.GetAllCategories();
                BookDataGrid.ItemsSource = bookList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            string searchBook = SearchTextBox.Text.ToLower();
            var allBooks = _bookCategoryRepository.GetAllCategories();
            var results = allBooks.Where(m => m.Name.ToLower().Contains(searchBook));
            BookDataGrid.ItemsSource = results;
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
                _bookCategoryRepository.AddCategory(bookCategory);

                MessageBox.Show("Book added successfully!");

                // Reload books and clear the input fields
                LoadCategories();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
                    _bookCategoryRepository.UpdateCategory(selectedCategory);

                    MessageBox.Show("Book updated successfully!");
                }
                else
                {
                    MessageBox.Show("Please select a bookCategory to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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

                    if (result == MessageBoxResult.Yes)
                    {
                        // Delete the book
                        _bookCategoryRepository.DeleteCategory(selectedBook.CategoryId);
                        MessageBox.Show("Book deleted successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a bookCategory to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
    }
}
