using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class DatBanViewModel : BaseViewModel
    {
        private readonly HoaDonRepository  _hdRepo  = new();
        private readonly BanRepository     _banRepo = new();
        private readonly KhachHangRepository _khRepo = new();

        private ObservableCollection<LichSuDatBan> _danhSach = new();
        private ObservableCollection<Ban>           _danhSachBan = new();
        private LichSuDatBan _editItem = new();
        private string _thongBao = "";
        private bool _isFormOpen;

        public ObservableCollection<LichSuDatBan> DanhSach    { get => _danhSach;    set => SetProperty(ref _danhSach, value); }
        public ObservableCollection<Ban>           DanhSachBan { get => _danhSachBan; set => SetProperty(ref _danhSachBan, value); }
        public LichSuDatBan EditItem   { get => _editItem;  set => SetProperty(ref _editItem, value); }
        public string ThongBao         { get => _thongBao;  set => SetProperty(ref _thongBao, value); }
        public bool   IsFormOpen       { get => _isFormOpen;set => SetProperty(ref _isFormOpen, value); }

        public ICommand ThemCommand { get; }
        public ICommand LuuCommand  { get; }
        public ICommand HuyCommand  { get; }
        public ICommand LoadCommand { get; }

        public DatBanViewModel()
        {
            ThemCommand = new RelayCommand(_ => { EditItem = new LichSuDatBan { NgayDat = DateTime.Now, SoNguoi = 2, TrangThai = "Đặt trước" }; IsFormOpen = true; ThongBao = ""; });
            LuuCommand  = new RelayCommand(_ => Luu(), _ => IsFormOpen);
            HuyCommand  = new RelayCommand(_ => { IsFormOpen = false; ThongBao = ""; });
            LoadCommand = new RelayCommand(_ => Load());
            Load();
        }

        public void Load()
        {
            DanhSachBan = new ObservableCollection<Ban>(_banRepo.GetAll().Where(b => b.TrangThai == "Trống" || b.TrangThai == "Đã Đặt").ToList());
            DanhSach    = new ObservableCollection<LichSuDatBan>(_hdRepo.GetLichSuDatBan());
        }

        private void Luu()
        {
            if (string.IsNullOrWhiteSpace(EditItem.TenKhachDat)) { ThongBao = "Vui long nhap ten khach."; return; }
            if (string.IsNullOrWhiteSpace(EditItem.SoDienThoai)) { ThongBao = "Vui long nhap so dien thoai."; return; }
            if (EditItem.MaBan == 0)                              { ThongBao = "Vui long chon ban."; return; }

            // Lookup KH by SDT
            var kh = _khRepo.GetBySoDienThoai(EditItem.SoDienThoai);
            EditItem.MaKhachHang = kh?.MaKhachHang;
            EditItem.MaNguoiTao  = Helpers.SessionManager.CurrentUser?.MaNguoiDung ?? 1;

            _hdRepo.TaoDatBan(EditItem);
            _banRepo.CapNhatTrangThai(EditItem.MaBan, "Đã đặt");
            ThongBao   = "Dat ban thanh cong!";
            IsFormOpen = false;
            Load();
        }
    }

}
