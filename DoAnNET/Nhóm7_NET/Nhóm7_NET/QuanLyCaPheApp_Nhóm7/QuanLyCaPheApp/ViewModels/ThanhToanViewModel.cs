using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class ThanhToanViewModel : BaseViewModel
    {
        private readonly HoaDonRepository  _hdRepo  = new();
        private readonly KhachHangRepository _khRepo = new();
        private readonly TichDiemRepository _tdRepo  = new();
        private readonly BanRepository      _banRepo = new();

        private ObservableCollection<HoaDon>    _danhSachHD = new();
        private ObservableCollection<KhuyenMai> _danhSachKM = new();
        private HoaDon?    _selected;
        private KhachHang? _khachHang;
        private KhuyenMai? _selectedKM;
        private string  _maCode = "";
        private decimal _tienKhachDua;
        private string  _phuongThuc = "TienMat";
        private int     _diemDung;
        private string  _thongBao = "";

        public ObservableCollection<HoaDon>    DanhSachHD { get => _danhSachHD; set => SetProperty(ref _danhSachHD, value); }
        public ObservableCollection<KhuyenMai> DanhSachKM { get => _danhSachKM; set => SetProperty(ref _danhSachKM, value); }
        public HoaDon?    Selected     { get => _selected;    set { SetProperty(ref _selected,    value); OnHoaDonChanged(); } }
        public KhachHang? KhachHang    { get => _khachHang;   set { SetProperty(ref _khachHang,   value); TinhToan(); } }
        public KhuyenMai? SelectedKM   { get => _selectedKM;  set { SetProperty(ref _selectedKM,  value); TinhToan(); } }
        public string     MaCode       { get => _maCode;      set => SetProperty(ref _maCode, value); }
        public decimal    TienKhachDua { get => _tienKhachDua;set { SetProperty(ref _tienKhachDua, value); OnPropertyChanged(nameof(TienThua)); } }
        public string     PhuongThuc   { get => _phuongThuc;  set => SetProperty(ref _phuongThuc, value); }
        public int        DiemDung     { get => _diemDung;    set { SetProperty(ref _diemDung, value); TinhToan(); } }
        public string     ThongBao     { get => _thongBao;    set => SetProperty(ref _thongBao, value); }

        public decimal TongTamTinh    => Selected?.TongTamTinh ?? 0;
        public decimal SoTienGiam     { get; private set; }
        public decimal GiamTuDiem     => DiemDung / 10 * 1000m; // 100 diem = 10.000d
        public decimal TongThanhToan  { get; private set; }
        public decimal TienThua       => TienKhachDua - TongThanhToan;
        public int     DiemToiDa      => KhachHang?.TongDiemTichLuy ?? 0;
        public string  TenKhach       => KhachHang?.HoTen ?? "(Khach le)";
        public bool    HasHoaDon      => Selected != null;

        public List<string> PhuongThucList { get; } = ["TienMat", "ChuyenKhoan", "TheNganHang"];

        public ICommand LoadCommand       { get; }
        public ICommand ApDungMaCommand   { get; }
        public ICommand TimKhachCommand   { get; }
        public ICommand ThanhToanCommand  { get; }
        public ICommand HuyHoaDonCommand  { get; }

        public ThanhToanViewModel()
        {
            LoadCommand      = new RelayCommand(_ => Load());
            ApDungMaCommand  = new RelayCommand(_ => ApDungMaCode());
            TimKhachCommand  = new RelayCommand(p => TimKhach(p?.ToString() ?? ""));
            ThanhToanCommand = new RelayCommand(_ => ThucHienThanhToan(), _ => HasHoaDon && TienKhachDua >= TongThanhToan);
            HuyHoaDonCommand = new RelayCommand(_ => HuyHoaDon(),         _ => HasHoaDon);
            Load();
        }

        public void Load()
        {
            DanhSachHD = new ObservableCollection<HoaDon>(_hdRepo.GetAll("DangGoi"));
            DanhSachKM = new ObservableCollection<KhuyenMai>(_hdRepo.GetKhuyenMaiConHieuLuc());
        }

        private void OnHoaDonChanged()
        {
            KhachHang  = null;
            SelectedKM = null;
            DiemDung   = 0;
            TienKhachDua = 0;
            if (Selected?.MaKhachHang.HasValue == true)
                KhachHang = _khRepo.GetById(Selected.MaKhachHang.Value);
            TinhToan();
            OnPropertyChanged(nameof(HasHoaDon));
            OnPropertyChanged(nameof(TenKhach));
        }

        private void TinhToan()
        {
            if (Selected == null) return;
            decimal giam = 0;
            if (SelectedKM != null)
            {
                if (TongTamTinh >= SelectedKM.GiaTriDonToiThieu)
                {
                    giam = SelectedKM.LoaiGiam == "PhanTram"
                        ? TongTamTinh * SelectedKM.GiaTriGiam / 100
                        : SelectedKM.GiaTriGiam;
                    if (SelectedKM.GiamToiDa.HasValue && giam > SelectedKM.GiamToiDa.Value)
                        giam = SelectedKM.GiamToiDa.Value;
                }
            }
            giam += GiamTuDiem;
            SoTienGiam    = giam;
            TongThanhToan = Math.Max(0, TongTamTinh - giam);
            OnPropertyChanged(nameof(SoTienGiam));
            OnPropertyChanged(nameof(TongThanhToan));
            OnPropertyChanged(nameof(TongTamTinh));
            OnPropertyChanged(nameof(GiamTuDiem));
            OnPropertyChanged(nameof(TienThua));
        }

        private void ApDungMaCode()
        {
            if (string.IsNullOrWhiteSpace(MaCode)) return;
            var km = DanhSachKM.FirstOrDefault(k => k.MaCode?.ToUpper() == MaCode.ToUpper());
            if (km == null) ThongBao = "Ma code khong hop le hoac het hieu luc.";
            else { SelectedKM = km; ThongBao = $"AP dung: {km.TenKhuyenMai}"; }
        }

        private void TimKhach(string sdt)
        {
            KhachHang = string.IsNullOrWhiteSpace(sdt) ? null : _khRepo.GetBySoDienThoai(sdt);
            if (!string.IsNullOrWhiteSpace(sdt) && KhachHang == null)
                ThongBao = "Khong tim thay khach hang.";
            else if (KhachHang != null)
                ThongBao = $"Tim thay: {KhachHang.HoTen} - {KhachHang.TongDiemTichLuy} diem";
            OnPropertyChanged(nameof(TenKhach));
            OnPropertyChanged(nameof(DiemToiDa));
            TinhToan();
        }

        private void ThucHienThanhToan()
        {
            if (Selected == null || SessionManager.CurrentUser == null) return;
            _hdRepo.ThanhToan(Selected.MaHoaDon, TongThanhToan, SoTienGiam,
                TienKhachDua, TienThua, PhuongThuc, DiemDung);
            _banRepo.CapNhatTrangThai(Selected.MaBan, "Trong");

            if (KhachHang != null)
            {
                int diemTich = (int)(TongThanhToan / 1000);
                _khRepo.CapNhatTichLuy(KhachHang.MaKhachHang, TongThanhToan, diemTich);
                if (DiemDung > 0)
                {
                    _khRepo.TruDiem(KhachHang.MaKhachHang, DiemDung);
                    _tdRepo.GhiDoiDiem(KhachHang.MaKhachHang, Selected.MaHoaDon, DiemDung,
                        KhachHang.TongDiemTichLuy - DiemDung, SessionManager.CurrentUser.MaNguoiDung);
                }
                int soMoi = KhachHang.TongDiemTichLuy + diemTich - DiemDung;
                _tdRepo.GhiTichDiem(KhachHang.MaKhachHang, Selected.MaHoaDon, diemTich,
                    soMoi, $"Tich diem HD #{Selected.MaHoaDon}", SessionManager.CurrentUser.MaNguoiDung);
            }

            ThongBao = $"Thanh toan thanh cong! Tien thua: {TienThua:N0}d";
            Load();
            Selected = null;
        }

        private void HuyHoaDon()
        {
            if (Selected == null) return;
            _hdRepo.HuyHoaDon(Selected.MaHoaDon);
            _banRepo.CapNhatTrangThai(Selected.MaBan, "Trong");
            ThongBao = "Da huy hoa don.";
            Load();
        }
    }
}
