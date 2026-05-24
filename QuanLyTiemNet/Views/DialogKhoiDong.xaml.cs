using QuanLyTiemNet.Models;
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
    /// Interaction logic for DialogKhoiDong.xaml
    /// </summary>
    public partial class DialogKhoiDong : Window
    {
        private MayTram _mayHienTai;
        public DialogKhoiDong(MayTram may)
        {
            InitializeComponent();
            _mayHienTai = may;
            txtTieuDe.Text = $"Khởi động: {may.TenMay}";

            if (may.TrangThai == 2) // Nếu máy đang có người chơi
            {
                txtTieuDe.Text = $"Quản lý: {may.TenMay}";
                btnKhoiDong.Visibility = Visibility.Collapsed; // Ẩn nút khởi động
                btnTatMay.Visibility = Visibility.Visible;     // Hiện nút tắt máy
                txtSoTien.IsEnabled = false;                   // Khóa ô nhập tiền trước
            }
        }
        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnKhoiDong_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                var mayInDb = db.MayTrams.Find(_mayHienTai.MaMay);
                if (mayInDb != null)
                {
                    mayInDb.TrangThai = 2;

                    int? maTaiKhoanDuocChon = null;
                    if (rdoHoiVien.IsChecked == true)
                    {
                        if (cboHoiVien.SelectedValue == null)
                        {
                            System.Windows.MessageBox.Show("Vui lòng chọn tài khoản hội viên!", "Thông báo");
                            return;
                        }
                        var tkDuocChon = cboHoiVien.SelectedItem as TaiKhoan;
                        if (tkDuocChon != null && tkDuocChon.SoDu <= 0) 
                        {
                            System.Windows.MessageBox.Show("Tài khoản này đã hết tiền! Vui lòng nạp thêm để khởi động máy.",
                                "Cảnh báo an toàn", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            return; 
                        }
                        maTaiKhoanDuocChon = (int)cboHoiVien.SelectedValue;
                    }

                    string chuoiTien = txtSoTien.Text.Replace(".", "").Replace(",", "").Trim();
                    decimal tienTraTruoc = 0;
                    decimal.TryParse(chuoiTien, out tienTraTruoc);

                    var phienMoi = new PhienChoi
                    {
                        MaMay = mayInDb.MaMay,
                        GioBatDau = DateTime.Now,
                        MaTaiKhoan = maTaiKhoanDuocChon,
                        GioKetThuc = null,
                        TienGio = tienTraTruoc
                    };

                    db.PhienChois.Add(phienMoi);
                    db.SaveChanges();
                }
            }
            this.DialogResult = true;
            this.Close();
        }


        private void BtnBaoTri_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                var mayInDb = db.MayTrams.Find(_mayHienTai.MaMay);
                if (mayInDb != null)
                {
                    mayInDb.TrangThai = 3;
                    db.SaveChanges();
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        private void BtnTatMay_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                var mayInDb = db.MayTrams.Find(_mayHienTai.MaMay);
                if (mayInDb != null)
                {
                    mayInDb.TrangThai = 1;
                }
                var phienHienTai = db.PhienChois.FirstOrDefault(p => p.MaMay == _mayHienTai.MaMay && p.GioKetThuc == null);
                if (phienHienTai != null)
                {
                    phienHienTai.GioKetThuc = DateTime.Now;
                    TimeSpan ts = DateTime.Now - phienHienTai.GioBatDau.GetValueOrDefault();
                    phienHienTai.TienGio = (decimal)ts.TotalHours * (mayInDb?.GiaMoiGio ?? 0);

                    if (phienHienTai.MaTaiKhoan != null)
                    {
                        var tkHoiVien = db.TaiKhoans.Find(phienHienTai.MaTaiKhoan);
                        if (tkHoiVien != null)
                        {
                            tkHoiVien.SoDu -= phienHienTai.TienGio;
                        }
                    }

                }

                db.SaveChanges();
            }

            this.DialogResult = true;
            this.Close();
        }


        private void LoadDanhSachHoiVien()
        {
            using (var db = new QuanLyTiemNetEntities1())
            {
                var listHoiVien = db.TaiKhoans.Where(t => t.VaiTro == 1).ToList();
                cboHoiVien.ItemsSource = listHoiVien;
            }
        }

        private void LoaiKhach_Checked(object sender, RoutedEventArgs e)
        {
            if (pnlHoiVien == null) return;

            if (rdoHoiVien.IsChecked == true)
            {
                pnlHoiVien.Visibility = Visibility.Visible;
                LoadDanhSachHoiVien();
            }
            else
            {
                pnlHoiVien.Visibility = Visibility.Collapsed;
            }
        }

        private void CboHoiVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboHoiVien.SelectedItem is TaiKhoan selectedTK)
            {
                txtTenHoiVien.Text = selectedTK.HoTen;
                txtSoDuHoiVien.Text = string.Format("{0:N0} VNĐ", selectedTK.SoDu);
            }
        }
    }
}
