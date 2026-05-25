using QuanLyTiemNet.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.Views
{
    public partial class DialogEditDichVu : Window
    {
        public DichVu EditingDichVu { get; set; }

        public DialogEditDichVu(DichVu targetDichVu)
        {
            InitializeComponent();

            // Nhận diện dữ liệu cũ và copy ra biến tạm để edit
            EditingDichVu = new DichVu
            {
                MaDichVu = targetDichVu.MaDichVu,
                TenDichVu = targetDichVu.TenDichVu,
                PhanLoai = targetDichVu.PhanLoai,
                DonGia = targetDichVu.DonGia,
                TonKho = targetDichVu.TonKho
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
            if (string.IsNullOrWhiteSpace(EditingDichVu.TenDichVu) || string.IsNullOrWhiteSpace(EditingDichVu.PhanLoai))
            {
                MessageBox.Show("Vui lòng nhập tên món và phân loại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    // Lấy món ăn từ DB dựa trên mã
                    var dvTrongDb = db.DichVus.FirstOrDefault(x => x.MaDichVu == EditingDichVu.MaDichVu);
                    if (dvTrongDb != null)
                    {
                        // Cập nhật các trường
                        dvTrongDb.TenDichVu = EditingDichVu.TenDichVu;
                        dvTrongDb.PhanLoai = EditingDichVu.PhanLoai;
                        dvTrongDb.DonGia = EditingDichVu.DonGia;
                        dvTrongDb.TonKho = EditingDichVu.TonKho;

                        db.SaveChanges(); // Lưu vào SQL
                    }
                }

                MessageBox.Show("Cập nhật dịch vụ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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