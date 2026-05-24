using QuanLyTiemNet.ViewModels;
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

namespace QuanLyTiemNet.Views
{
    /// <summary>
    /// Interaction logic for LoginWindowView.xaml
    /// </summary>
    public partial class LoginWindowView : Window
    {
        private LoginViewModel _viewModel;
        public LoginWindowView()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Lấy text người dùng gõ vào (Đảm bảo thẻ TextBox tên là txtUsername, thẻ PasswordBox tên là txtPassword)
            string user = txtUsername.Text;
            string pass = txtPassword.Password;

            // Gọi hàm KiemTraDangNhap đã viết ở Bước 1
            bool isThanhCong = _viewModel.KiemTraDangNhap(user, pass);

            if (isThanhCong)
            {
                // Nếu đăng nhập đúng -> Mở màn hình MainWindow
                MainWindow main = new MainWindow();
                main.Show();

                // Đóng màn hình đăng nhập hiện tại
                this.Close();
            }
            else
            {
                // Đăng nhập sai -> Báo lỗi
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Bắt sự kiện khi Click nút Thoát
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
