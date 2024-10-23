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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void btnMotorbikeManagement_Click(object sender, RoutedEventArgs e)
        {
            BookManagementWindow motorbikeManagementWindow = new BookManagementWindow();
            motorbikeManagementWindow.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();
            }
        }

        private void btnCustomerManagement_Click(object sender, RoutedEventArgs e)
        {
            BookCategoryManagementWindow categoryManagementWindow = new BookCategoryManagementWindow();
            categoryManagementWindow.Show();
            this.Hide();
        }
    }
}
