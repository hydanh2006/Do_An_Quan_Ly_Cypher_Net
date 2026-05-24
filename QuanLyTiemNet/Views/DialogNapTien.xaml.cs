using QuanLyTiemNet.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogNapTien : Window
    {
        private int _maTaiKhoanCachNap;

        public DialogNapTien(TaiKhoan tk)
        {
            InitializeComponent();
            _maTaiKhoanCachNap = tk.MaTaiKhoan;
            txtTenKhach.Text = $"Đang nạp cho: {tk.TenDangNhap} ({tk.HoTen})";
            txtSoTienNap.Focus();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        private void BtnHuy_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; this.Close(); }

        private void BtnNap_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtSoTienNap.Text, out decimal soTien) || soTien <= 0)
            {
                MessageBox.Show("Số tiền nạp không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    var tkDb = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == _maTaiKhoanCachNap);
                    if (tkDb != null)
                    {
                        tkDb.SoDu = (tkDb.SoDu ?? 0) + soTien;
                        db.SaveChanges();
                    }
                }
                MessageBox.Show($"Nạp thành công {soTien:N0} VNĐ!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}