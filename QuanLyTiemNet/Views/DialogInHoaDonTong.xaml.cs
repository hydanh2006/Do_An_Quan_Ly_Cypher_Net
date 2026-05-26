using QuanLyTiemNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyTiemNet.Views
{
    public partial class DialogInHoaDonTong : Window
    {
        public DialogInHoaDonTong(TaiKhoan khach, List<LichSuNapTien> dsGiaoDich)
        {
            InitializeComponent();

            // 1. Điền thông tin khách và ngày in lên hóa đơn
            txtTenKhach.Text = $"Tài khoản: {khach.TenDangNhap} ({khach.HoTen})";
            txtNgayIn.Text = $"Ngày áp dụng: {DateTime.Now.ToString("dd/MM/yyyy")}";

            // 2. Đổ danh sách vào DataGrid trong hóa đơn
            dgChiTietGiaoDich.ItemsSource = dsGiaoDich;

            // 3. Tính tổng số tiền thu của khách này trong ngày hôm nay
            decimal tongTien = dsGiaoDich.Sum(x => x.SoTienNap);
            txtTongTien.Text = string.Format("{0:N0} VNĐ", tongTien);
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Thực hiện lệnh in đúng vùng thiết kế tờ hóa đơn tổng
                printDialog.PrintVisual(KhuVucIn, "Hóa Đơn Tổng Ngày");
                MessageBox.Show("Đã in hóa đơn tổng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}