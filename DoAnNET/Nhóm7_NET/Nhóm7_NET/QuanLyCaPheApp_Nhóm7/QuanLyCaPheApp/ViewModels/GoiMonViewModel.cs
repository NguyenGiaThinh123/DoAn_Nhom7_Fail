using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class GoiMonViewModel : BaseViewModel
    {
        private readonly HoaDonRepository  _hdRepo  = new();
        private readonly SanPhamRepository _spRepo  = new();
        private readonly BanRepository     _banRepo = new();

        private ObservableCollection<SanPham>       _menuItems = new();
        private ObservableCollection<ChiTietHoaDon> _gioHang   = new();
        private ObservableCollection<LoaiSanPham>   _danhSachLoai = new();
        private ObservableCollection<Ban>            _danhSachBan  = new();
        private SanPham?  _selectedSP;
        private Ban?      _selectedBan;
        private HoaDon?   _currentHD;
        private string    _keyword = "";
        private int       _filterLoai;
        private decimal   _tongTien;
        private string    _thongBao = "";

        public ObservableCollection<SanPham>       MenuItems    { get => _menuItems;    set => SetProperty(ref _menuItems,    value); }
        public ObservableCollection<ChiTietHoaDon> GioHang      { get => _gioHang;      set => SetProperty(ref _gioHang,      value); }
        public ObservableCollection<LoaiSanPham>   DanhSachLoai { get => _danhSachLoai; set => SetProperty(ref _danhSachLoai, value); }
        public ObservableCollection<Ban>           DanhSachBan  { get => _danhSachBan;  set => SetProperty(ref _danhSachBan,  value); }
        public SanPham? SelectedSP   { get => _selectedSP;  set => SetProperty(ref _selectedSP, value); }
        public Ban?     SelectedBan  { get => _selectedBan; set { SetProperty(ref _selectedBan, value); OnBanChanged(); } }
        public HoaDon?  CurrentHD    { get => _currentHD;   set => SetProperty(ref _currentHD, value); }
        public string   Keyword      { get => _keyword;     set { SetProperty(ref _keyword, value); SearchMenu(); } }
        public int      FilterLoai   { get => _filterLoai;  set { SetProperty(ref _filterLoai, value); SearchMenu(); } }
        public decimal  TongTien     { get => _tongTien;    set { SetProperty(ref _tongTien, value); OnPropertyChanged(nameof(TongTienText)); } }
        public string   ThongBao     { get => _thongBao;    set => SetProperty(ref _thongBao, value); }
        public string   TongTienText => $"{TongTien:N0} d";
        public bool     CoHoaDon     => CurrentHD != null;
        public string   TenBanHienTai => CurrentHD != null ? SelectedBan?.TenBan ?? "" : "Chua chon ban";

        public ICommand ThemVaoGioCommand { get; }
        public ICommand XoaKhoiGioCommand { get; }
        public ICommand TaoHoaDonCommand  { get; }
        public ICommand RefreshMenuCommand{ get; }

        public GoiMonViewModel()
        {
            ThemVaoGioCommand = new RelayCommand(_ => ThemVaoGio(),  _ => SelectedSP != null && SelectedBan != null);
            XoaKhoiGioCommand = new RelayCommand(p => XoaKhoiGio(p as ChiTietHoaDon));
            TaoHoaDonCommand  = new RelayCommand(_ => TaoHoaDon(),   _ => SelectedBan != null && GioHang.Count > 0);
            RefreshMenuCommand = new RelayCommand(_ => Load());
            Load();
        }

        public void Load()
        {
            DanhSachLoai = new ObservableCollection<LoaiSanPham>(
                new List<LoaiSanPham> { new() { MaLoai = 0, TenLoai = "-- Tat ca --" } }
                    .Concat(_spRepo.GetAllLoai(true)));
            DanhSachBan = new ObservableCollection<Ban>(_banRepo.GetAll());
            SearchMenu();
        }

        public void SetBan(Ban ban)
        {
            SelectedBan = DanhSachBan.FirstOrDefault(b => b.MaBan == ban.MaBan) ?? ban;
        }

        private void SearchMenu()
            => MenuItems = new ObservableCollection<SanPham>(
                _spRepo.GetAll(Keyword, FilterLoai, true));

        private void OnBanChanged()
        {
            if (SelectedBan == null) return;
            var hd = _hdRepo.GetDangGoiByBan(SelectedBan.MaBan);
            CurrentHD = hd;
            GioHang   = hd != null
                ? new ObservableCollection<ChiTietHoaDon>(hd.ChiTiet)
                : new();
            TinhTong();
            OnPropertyChanged(nameof(CoHoaDon));
            OnPropertyChanged(nameof(TenBanHienTai));
        }

        private void ThemVaoGio()
        {
            if (SelectedSP == null || SelectedBan == null) return;
            var giaHienTai = SelectedSP.GiaSauGiam;
            // Check if already in cart
            var existing = GioHang.FirstOrDefault(g => g.MaSanPham == SelectedSP.MaSanPham);
            if (existing != null)
            {
                existing.SoLuong++;
                existing.ThanhTien = existing.DonGia * existing.SoLuong * (1 - existing.PhanTramGiam / 100);
                if (CurrentHD != null)
                    _hdRepo.CapNhatSoLuong(existing.MaChiTiet, existing.SoLuong, existing.ThanhTien);
            }
            else
            {
                var ct = new ChiTietHoaDon
                {
                    MaHoaDon     = CurrentHD?.MaHoaDon ?? 0,
                    MaSanPham    = SelectedSP.MaSanPham,
                    TenSanPham   = SelectedSP.TenSanPham,
                    DonGia       = giaHienTai,
                    SoLuong      = 1,
                    PhanTramGiam = SelectedSP.PhanTramGiam,
                    ThanhTien    = giaHienTai,
                    TrangThaiMon = "ChoPha",
                };

                if (CurrentHD == null) TaoHoaDonNhanh();
                if (CurrentHD != null)
                {
                    ct.MaHoaDon  = CurrentHD.MaHoaDon;
                    ct.MaChiTiet = _hdRepo.ThemChiTiet(ct);
                }
                GioHang.Add(ct);
            }
            TinhTong();
            if (CurrentHD != null)
                _hdRepo.CapNhatTongTien(CurrentHD.MaHoaDon, TongTien, TongTien);
            OnPropertyChanged(nameof(GioHang));
            ThongBao = $"Da them: {SelectedSP.TenSanPham}";
        }

        private void TaoHoaDonNhanh()
        {
            if (SelectedBan == null || Helpers.SessionManager.CurrentUser == null) return;
            var hd = new HoaDon
            {
                MaBan       = SelectedBan.MaBan,
                MaNguoiTao  = Helpers.SessionManager.CurrentUser.MaNguoiDung,
                LoaiHoaDon  = "TraSau",
                TrangThai   = "DangGoi",
            };
            hd.MaHoaDon = _hdRepo.TaoHoaDon(hd);
            CurrentHD = hd;
            _banRepo.CapNhatTrangThai(SelectedBan.MaBan, "Dang dung");
            OnPropertyChanged(nameof(CoHoaDon));
        }

        private void TaoHoaDon() => TaoHoaDonNhanh();

        private void XoaKhoiGio(ChiTietHoaDon? ct)
        {
            if (ct == null) return;
            if (ct.MaChiTiet > 0) _hdRepo.XoaChiTiet(ct.MaChiTiet);
            GioHang.Remove(ct);
            TinhTong();
            if (CurrentHD != null)
                _hdRepo.CapNhatTongTien(CurrentHD.MaHoaDon, TongTien, TongTien);
        }

        private void TinhTong()
            => TongTien = GioHang.Sum(g => g.ThanhTien);
    }
}
