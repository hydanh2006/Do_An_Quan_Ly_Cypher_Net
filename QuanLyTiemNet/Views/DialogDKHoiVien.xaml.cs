using QuanLyTiemNet.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogDKHoiVien : Window
    {
        public TaiKhoan NewTaiKhoan { get; set; }

        public DialogDKHoiVien()
        {
            InitializeComponent();
            NewTaiKhoan = new TaiKhoan
            {
                VaiTro = 1,
                SoDu = 0
            };

            this.DataContext = this;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewTaiKhoan.TenDangNhap) || string.IsNullOrWhiteSpace(NewTaiKhoan.MatKhau))
            {
                MessageBox.Show("Tài khoản và mật khẩu không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    bool daTonTai = db.TaiKhoans.Any(t => t.TenDangNhap == NewTaiKhoan.TenDangNhap);
                    if (daTonTai)
                    {
                        MessageBox.Show("Tên đăng nhập này đã có người sử dụng. Vui lòng chọn tên khác!", "Lỗi trùng lặp", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    db.TaiKhoans.Add(NewTaiKhoan);
                    db.SaveChanges();
                }

                MessageBox.Show("Đăng ký hội viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}