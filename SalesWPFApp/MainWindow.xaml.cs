using BusinessObject.Entities;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using SalesWPFApp.Services.Implementations;
using SalesWPFApp.Services.Interfaces;
using System.Net.NetworkInformation;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMemberService _memberService;
        public MainWindow()
        {
            InitializeComponent();
            _memberService = new MemberService();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Do you want to exit the app?", "Exit App!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
                {
                    MessageBox.Show("Please enter email and password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Member user = _memberService.GetMemberByEmailAndPassword(txtEmail.Text, txtPassword.Password);
                if (user == null)
                {
                    MessageBox.Show("Invalid email or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user != null && user.Role.RoleId == 1)
                {
                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your role is forbidden to access this resource!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}