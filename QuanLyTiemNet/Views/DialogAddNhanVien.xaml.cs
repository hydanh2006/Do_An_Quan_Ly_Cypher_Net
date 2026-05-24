using QuanLyTiemNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogAddNhanVien : Window
    {
        public NhanVien NewNhanVien { get; set; }
        public List<TaiKhoan> ListTaiKhoan { get; set; }

        public DialogAddNhanVien()
        {
            InitializeComponent();
            NewNhanVien = new NhanVien { NgayVaoLam = DateTime.Now, TrangThai = "Đang làm việc", MucLuong = 0 };
            LoadData();
            this.DataContext = this;
        }

        private void LoadData()
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                ListTaiKhoan = db.TaiKhoans.ToList();
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
            if (string.IsNullOrEmpty(NewNhanVien.HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    db.NhanViens.Add(NewNhanVien);
                    db.SaveChanges();
                }
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