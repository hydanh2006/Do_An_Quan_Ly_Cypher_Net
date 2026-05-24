using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyTiemNet.ViewModels
{
    internal class MainViewModel:BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView"); 
            }
        }
        public MayTramViewModel MayTramVM { get; set; }
        public NhanVienViewModel NhanVienVM { get; set; }
        public CangTinViewModel CangTinVM { get; set; }

        public HoiVienViewModel HoiVienVM { get; set; }

        public ICommand SwitchViewCommand { get; set; }

        public System.Windows.Visibility HienThiAdmin
        {
            get
            {
                if (Models.PhienDangNhap.NhanVienHienTai != null && Models.PhienDangNhap.NhanVienHienTai.VaiTro == 0)
                {
                    return System.Windows.Visibility.Visible;
                }
                return System.Windows.Visibility.Collapsed;
            }
        }
        public MainViewModel()
        {
            // Khởi tạo các ViewModel con
            MayTramVM = new MayTramViewModel();
            NhanVienVM = new NhanVienViewModel();
            CangTinVM = new CangTinViewModel();
            HoiVienVM = new HoiVienViewModel();

            // Màn hình mặc định khi vừa mở App lên là Sơ đồ máy trạm
            CurrentView = MayTramVM;

            SwitchViewCommand = new RelayCommand((parameter) =>
            {
                string viewName = parameter as string;
                if (viewName == null) return;

                switch (viewName)
                {
                    case "MayTram":
                        CurrentView = MayTramVM;
                        break;
                    case "NhanVien":
                        CurrentView = NhanVienVM;
                        break;
                    case "CangTin":
                        CurrentView = CangTinVM;
                        break;
                    case "HoiVien":
                        CurrentView = HoiVienVM;
                        break;
                }
            });
        }
    }
}
