using QuanLyTiemNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogEditNhanVien : Window
    {
        public NhanVien EditingNhanVien { get; set; }
        public List<TaiKhoan> ListTaiKhoan { get; set; }
        public DialogEditNhanVien(NhanVien targetNhanVien)
        {
            InitializeComponent();
            EditingNhanVien = new NhanVien
            {
                MaNhanVien = targetNhanVien.MaNhanVien,
                MaTaiKhoan = targetNhanVien.MaTaiKhoan,
                HoTen = targetNhanVien.HoTen,
                ChucVu = targetNhanVien.ChucVu,
                SoDienThoai = targetNhanVien.SoDienThoai,
                CaLamViec = targetNhanVien.CaLamViec,
                MucLuong = targetNhanVien.MucLuong,
                NgayVaoLam = targetNhanVien.NgayVaoLam,
                TrangThai = targetNhanVien.TrangThai
            };

            LoadDanhSachTaiKhoan();
            this.DataContext = this;
        }

        private void LoadDanhSachTaiKhoan()
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                ListTaiKhoan = db.TaiKhoans.Where(t => t.VaiTro == 0 || t.VaiTro == 2).ToList();
            }
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
            if (string.IsNullOrWhiteSpace(EditingNhanVien.HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    var nvTrongDb = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == EditingNhanVien.MaNhanVien);
                    if (nvTrongDb != null)
                    {
                        nvTrongDb.HoTen = EditingNhanVien.HoTen;
                        nvTrongDb.MaTaiKhoan = EditingNhanVien.MaTaiKhoan;
                        nvTrongDb.ChucVu = EditingNhanVien.ChucVu;
                        nvTrongDb.SoDienThoai = EditingNhanVien.SoDienThoai;
                        nvTrongDb.CaLamViec = EditingNhanVien.CaLamViec;
                        nvTrongDb.MucLuong = EditingNhanVien.MucLuong;
                        nvTrongDb.NgayVaoLam = EditingNhanVien.NgayVaoLam;
                        nvTrongDb.TrangThai = EditingNhanVien.TrangThai;

                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu thay đổi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}