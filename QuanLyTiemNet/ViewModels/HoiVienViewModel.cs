using QuanLyTiemNet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.ViewModels
{
    internal class HoiVienViewModel : BaseViewModel
    {
        private ObservableCollection<TaiKhoan> _danhSachHoiVien;
        public ObservableCollection<TaiKhoan> DanhSachHoiVien
        {
            get => _danhSachHoiVien;
            set { _danhSachHoiVien = value; OnPropertyChanged(nameof(DanhSachHoiVien)); }
        }

        public ICommand AddHoiVienCommand { get; set; }
        public ICommand NapTienCommand { get; set; }
        public ICommand SuaHoiVienCommand { get; set; }
        public ICommand XoaHoiVienCommand { get; set; }
        public ICommand InTongBillNgayCommand { get; set; }

        public HoiVienViewModel()
        {
            LoadDanhSachHoiVien();
            AddHoiVienCommand = new RelayCommand((p) =>
            {
                var dialog = new Views.DialogDKHoiVien();
                if (dialog.ShowDialog() == true) LoadDanhSachHoiVien();
            });

            NapTienCommand = new RelayCommand((p) =>
            {
                if (p is TaiKhoan tkDaChon)
                {
                    var dialog = new Views.DialogNapTien(tkDaChon);
                    if (dialog.ShowDialog() == true) LoadDanhSachHoiVien();
                }
            });

            XoaHoiVienCommand = new RelayCommand((p) =>
            {
                if (p is TaiKhoan tkDaChon)
                {
                    var result = MessageBox.Show($"Xóa vĩnh viễn tài khoản [{tkDaChon.TenDangNhap}]?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var db = new QuanLyTiemNetEntities1())
                            {
                                var tkDb = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == tkDaChon.MaTaiKhoan);
                                if (tkDb != null)
                                {
                                    db.TaiKhoans.Remove(tkDb);
                                    db.SaveChanges();
                                    LoadDanhSachHoiVien();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa tài khoản này vì nó đang liên kết với dữ liệu khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });

            SuaHoiVienCommand = new RelayCommand((p) =>
            {
                MessageBox.Show("Bạn có thể copy form Thêm Hội Viên để làm form Sửa tương tự như cách làm ở Tab Nhân Sự nhé!", "Hướng dẫn");
            });

            InTongBillNgayCommand = new RelayCommand((p) =>
            {
                if (p is TaiKhoan khachHangDaChon)
                {
                    using (var db = new QuanLyTiemNetEntities1())
                    {
                        DateTime ngayHomNay = DateTime.Now.Date;
                        DateTime ngayMai = ngayHomNay.AddDays(1);

                        var dsGiaoDichTrongNgay = db.LichSuNapTiens
                            .Include("TaiKhoan1")
                            .Where(x => x.MaTaiKhoanKhach == khachHangDaChon.MaTaiKhoan
                                     && x.ThoiGianNap >= ngayHomNay
                                     && x.ThoiGianNap < ngayMai)
                            .OrderBy(x => x.ThoiGianNap)
                            .ToList();

                        if (dsGiaoDichTrongNgay.Count == 0)
                        {
                            MessageBox.Show($"Hôm nay hội viên [{khachHangDaChon.TenDangNhap}] không có giao dịch nào để in!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        var dialog = new Views.DialogInHoaDonTong(khachHangDaChon, dsGiaoDichTrongNgay);
                        dialog.ShowDialog();
                    }
                }
            });

        }

        public void LoadDanhSachHoiVien()
        {
            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    var listHV = db.TaiKhoans.Where(x => x.VaiTro == 1).ToList();
                    int stt = 1;
                    foreach (var hv in listHV)
                    {
                        hv.STT = stt++;
                    }

                    DanhSachHoiVien = new ObservableCollection<TaiKhoan>(listHV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }
    }
}