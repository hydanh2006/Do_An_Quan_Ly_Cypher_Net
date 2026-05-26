using QuanLyTiemNet.Models;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyTiemNet.Views
{
    public partial class DialogInHoaDon : Window
    {
        public DialogInHoaDon(LichSuNapTien giaoDich)
        {
            InitializeComponent();

            // Gắn dữ liệu của dòng vừa chọn vào giao diện hóa đơn
            this.DataContext = giaoDich;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại chọn máy in của Windows
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                // Ra lệnh in ĐÚNG CÁI KHUNG CÓ TÊN LÀ "KhuVucIn" TRÊN MÀN HÌNH
                printDialog.PrintVisual(KhuVucIn, "Hóa Đơn Tiệm Net");

                MessageBox.Show("Đã gửi lệnh in thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // In xong tự đóng cửa sổ
            }
        }
    }
}