using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class QuanLyNguoiDungViewModel : BaseViewModel
    {
        private readonly NguoiDungRepository _repo = new();
        private ObservableCollection<NguoiDung> _danhSach = new();
        private ObservableCollection<VaiTro>    _danhSachVaiTro = new();
        private NguoiDung? _selected;
        private NguoiDung  _editItem = new();
        private string _keyword = "";
        private bool _isEditMode;
        private string _thongBao = "";
        private string _matKhauMoi = "";

        public ObservableCollection<NguoiDung> DanhSach       { get => _danhSach;       set => SetProperty(ref _danhSach, value); }
        public ObservableCollection<VaiTro>    DanhSachVaiTro  { get => _danhSachVaiTro; set => SetProperty(ref _danhSachVaiTro, value); }
        public NguoiDung? Selected  { get => _selected;  set { SetProperty(ref _selected, value); OnPropertyChanged(nameof(HasSelected)); } }
        public NguoiDung  EditItem  { get => _editItem;  set => SetProperty(ref _editItem, value); }
        public string Keyword       { get => _keyword;   set { SetProperty(ref _keyword, value); Search(); } }
        public bool IsEditMode      { get => _isEditMode; set => SetProperty(ref _isEditMode, value); }
        public string ThongBao      { get => _thongBao;  set => SetProperty(ref _thongBao, value); }
        public string MatKhauMoi    { get => _matKhauMoi;set => SetProperty(ref _matKhauMoi, value); }
        public bool HasSelected     => Selected != null;

        public ICommand SearchCommand    { get; }
        public ICommand ThemCommand      { get; }
        public ICommand SuaCommand       { get; }
        public ICommand KhoaCommand      { get; }
        public ICommand LuuCommand       { get; }
        public ICommand HuyCommand       { get; }
        public ICommand DoiMatKhauCommand{ get; }

        public QuanLyNguoiDungViewModel()
        {
            SearchCommand     = new RelayCommand(_ => Search());
            ThemCommand       = new RelayCommand(_ => BatDauThem());
            SuaCommand        = new RelayCommand(_ => BatDauSua(),  _ => HasSelected);
            KhoaCommand       = new RelayCommand(_ => KhoaTK(),     _ => HasSelected);
            LuuCommand        = new RelayCommand(_ => Luu(),         _ => IsEditMode);
            HuyCommand        = new RelayCommand(_ => Huy(),         _ => IsEditMode);
            DoiMatKhauCommand = new RelayCommand(_ => DoiMatKhau(),  _ => HasSelected);
            DanhSachVaiTro = new ObservableCollection<VaiTro>(_repo.GetAllVaiTro());
            Search();
        }

        private void Search()
            => DanhSach = new ObservableCollection<NguoiDung>(_repo.GetAll(Keyword));

        private void BatDauThem()
        {
            EditItem   = new NguoiDung { TrangThai = true, MaVaiTro = 3 };
            MatKhauMoi = "";
            IsEditMode = true;
            ThongBao   = "";
        }

        private void BatDauSua()
        {
            if (Selected == null) return;
            EditItem = new NguoiDung
            {
                MaNguoiDung = Selected.MaNguoiDung,
                TenDangNhap = Selected.TenDangNhap,
                HoTen       = Selected.HoTen,
                Email       = Selected.Email,
                SoDienThoai = Selected.SoDienThoai,
                MaVaiTro    = Selected.MaVaiTro,
                TrangThai   = Selected.TrangThai,
            };
            MatKhauMoi = "";
            IsEditMode = true;
            ThongBao   = "";
        }

        private void Luu()
        {
            if (string.IsNullOrWhiteSpace(EditItem.TenDangNhap)) { ThongBao = "Vui long nhap ten dang nhap."; return; }
            if (string.IsNullOrWhiteSpace(EditItem.HoTen))       { ThongBao = "Vui long nhap ho ten."; return; }
            if (_repo.CheckTrungTenDangNhap(EditItem.TenDangNhap, EditItem.MaNguoiDung))
                { ThongBao = "Ten dang nhap da ton tai."; return; }

            if (EditItem.MaNguoiDung == 0)
            {
                if (string.IsNullOrWhiteSpace(MatKhauMoi)) { ThongBao = "Vui long nhap mat khau."; return; }
                EditItem.MatKhauHash = HashHelper.SHA256Hash(MatKhauMoi);
                _repo.Add(EditItem);
            }
            else
            {
                _repo.Update(EditItem);
                if (!string.IsNullOrWhiteSpace(MatKhauMoi))
                    _repo.DoiMatKhau(EditItem.MaNguoiDung, HashHelper.SHA256Hash(MatKhauMoi));
            }
            ThongBao   = "Luu thanh cong!";
            IsEditMode = false;
            Search();
        }

        private void Huy() { IsEditMode = false; ThongBao = ""; }

        private void KhoaTK()
        {
            if (Selected == null || Selected.MaNguoiDung == SessionManager.CurrentUser?.MaNguoiDung)
                { ThongBao = "Khong the khoa tai khoan dang dung."; return; }
            var newStatus = !Selected.TrangThai;
            _repo.KhoaTK(Selected.MaNguoiDung, newStatus);
            ThongBao = newStatus ? "Da mo khoa tai khoan." : "Da khoa tai khoan.";
            Search();
        }

        private void DoiMatKhau()
        {
            if (Selected == null || string.IsNullOrWhiteSpace(MatKhauMoi)) return;
            _repo.DoiMatKhau(Selected.MaNguoiDung, HashHelper.SHA256Hash(MatKhauMoi));
            ThongBao = "Doi mat khau thanh cong!";
        }
    }
}
