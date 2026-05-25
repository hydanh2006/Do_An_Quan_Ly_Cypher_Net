using QuanLyTiemNet.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogAddDichVu : Window
    {
        public DichVu NewDichVu { get; set; }

        public DialogAddDichVu()
        {
            InitializeComponent();
            NewDichVu = new DichVu { DonGia = 0, TonKho = 0 };
            this.DataContext = this;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        private void BtnHuy_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; this.Close(); }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewDichVu.TenDichVu) || string.IsNullOrWhiteSpace(NewDichVu.PhanLoai))
            {
                MessageBox.Show("Vui lòng nhập tên món và phân loại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    db.DichVus.Add(NewDichVu);
                    db.SaveChanges();
                }
                MessageBox.Show("Thêm dịch vụ thành công!");
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