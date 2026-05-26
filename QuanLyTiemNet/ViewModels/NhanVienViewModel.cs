using QuanLyTiemNet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyTiemNet.ViewModels
{
    internal class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NhanVien> _danhSachNhanVien;
        public ObservableCollection<NhanVien> DanhSachNhanVien
        {
            get => _danhSachNhanVien;
            set { _danhSachNhanVien = value; OnPropertyChanged(nameof(DanhSachNhanVien)); }
        }

        public Visibility HienThiAdmin
        {
            get => (PhienDangNhap.NhanVienHienTai != null && PhienDangNhap.NhanVienHienTai.VaiTro == 0)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand AddNhanVienCommand { get; set; }
        public ICommand SuaNhanVienCommand { get; set; }
        public ICommand XoaNhanVienCommand { get; set; }

        public NhanVienViewModel()
        {
            LoadDanhSachNhanVien();

            AddNhanVienCommand = new RelayCommand((p) =>
            {
                var dialog = new Views.DialogAddNhanVien();
                if (dialog.ShowDialog() == true)
                {
                    LoadDanhSachNhanVien();
                }
            });

            SuaNhanVienCommand = new RelayCommand((p) =>
            {
                if (p is NhanVien nhanVienDuocChon)
                {
                    var dialog = new Views.DialogEditNhanVien(nhanVienDuocChon);
                    if (dialog.ShowDialog() == true)
                    {
                        LoadDanhSachNhanVien();
                    }
                }
            });

            XoaNhanVienCommand = new RelayCommand((p) =>
            {
                if (p is NhanVien nhanVienDuocChon)
                {
                    var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên [{nhanVienDuocChon.HoTen}] khỏi hệ thống không?",
                        "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var db = new QuanLyTiemNetEntities1())
                            {
                                var nvTrongDb = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == nhanVienDuocChon.MaNhanVien);
                                if (nvTrongDb != null)
                                {
                                    db.NhanViens.Remove(nvTrongDb);
                                    db.SaveChanges();

                                    MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                    LoadDanhSachNhanVien();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa nhân viên này! Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });
        }

        public void LoadDanhSachNhanVien()
        {
            try
            {
                using (var db = new QuanLyTiemNetEntities1())
                {
                    var listNV = db.NhanViens.ToList();

                    int stt = 1;
                    foreach (var nv in listNV)
                    {
                        nv.STT = stt++;
                    }

                    DanhSachNhanVien = new ObservableCollection<NhanVien>(listNV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}