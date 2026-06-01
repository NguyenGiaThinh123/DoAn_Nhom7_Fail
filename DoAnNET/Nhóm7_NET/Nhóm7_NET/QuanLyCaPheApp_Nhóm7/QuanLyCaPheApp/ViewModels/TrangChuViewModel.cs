using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace QuanLyCaPheApp.ViewModels
{
    public class TrangChuViewModel : BaseViewModel
    {
        private readonly BanRepository      _banRepo = new();
        private readonly ThongKeRepository  _tkRepo  = new();

        private List<Ban> _allBans = new();

        // ── Properties ──────────────────────────────────────────────
        private ObservableCollection<Ban> _danhSachBan = new();
        public  ObservableCollection<Ban> DanhSachBan
        {
            get => _danhSachBan;
            set => SetProperty(ref _danhSachBan, value);
        }

        private string _filterKhuVuc = "Tất cả";
        public  string FilterKhuVuc
        {
            get => _filterKhuVuc;
            set { if (SetProperty(ref _filterKhuVuc, value)) ApplyFilter(); }
        }

        public List<string> DanhSachKhuVuc { get; private set; } = new() { "Tất cả" };

        private string _doanhThuText = "0 đ";
        public  string DoanhThuText  { get => _doanhThuText; set => SetProperty(ref _doanhThuText, value); }

        private int _soHoaDonHomNay;
        public  int SoHoaDonHomNay   { get => _soHoaDonHomNay; set => SetProperty(ref _soHoaDonHomNay, value); }

        private int _soBanDangDung;
        public  int SoBanDangDung    { get => _soBanDangDung; set => SetProperty(ref _soBanDangDung, value); }

        private int _soKhachHangMoi;
        public  int SoKhachHangMoi   { get => _soKhachHangMoi; set => SetProperty(ref _soKhachHangMoi, value); }

        // Callback khi bấm chọn bàn
        public Action<Ban>? BanDuocChon { get; set; }

        // ── Commands ────────────────────────────────────────────────
        public ICommand RefreshCommand { get; }
        public ICommand ChonBanCommand { get; }

        public TrangChuViewModel()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            ChonBanCommand = new RelayCommand(p => { if (p is Ban ban) BanDuocChon?.Invoke(ban); });
        }

        public void Load()
        {
            try
            {
                // Lấy danh sách bàn
                _allBans = _banRepo.GetAll();

                // Lấy khu vực (nếu có)
                var kvList = new List<string> { "Tất cả" };
                try { kvList.AddRange(_banRepo.GetDanhSachKhuVuc().Where(kv => !string.IsNullOrEmpty(kv))); }
                catch { }
                DanhSachKhuVuc = kvList;
                OnPropertyChanged(nameof(DanhSachKhuVuc));

                ApplyFilter();
            }
            catch { _allBans = new(); }

            // Thống kê (không crash nếu DB lỗi)
            try { SoBanDangDung = _allBans.Count(b =>
                      b.TrangThai == "Đang dùng" || b.TrangThai == "Dang dung"); }
            catch { SoBanDangDung = 0; }

            try
            {
                var dt = _tkRepo.GetDoanhThuHomNay();
                DoanhThuText = $"{dt:N0} đ";
            }
            catch { DoanhThuText = "0 đ"; }

            try { SoHoaDonHomNay = _tkRepo.GetSoHoaDonHomNay(); }
            catch { SoHoaDonHomNay = 0; }

            try { SoKhachHangMoi = _tkRepo.GetSoKhachHangMoi(); }
            catch { SoKhachHangMoi = 0; }
        }

        private void ApplyFilter()
        {
            try
            {
                var filtered = (FilterKhuVuc == "Tất cả" || string.IsNullOrEmpty(FilterKhuVuc))
                    ? _allBans
                    : _allBans.Where(b => b.TenKhuVuc == FilterKhuVuc).ToList();

                // MauTrangThai là computed property trên Ban model - tự tính tự động
                DanhSachBan = new ObservableCollection<Ban>(filtered);
            }
            catch
            {
                DanhSachBan = new ObservableCollection<Ban>();
            }
        }
    }
}
