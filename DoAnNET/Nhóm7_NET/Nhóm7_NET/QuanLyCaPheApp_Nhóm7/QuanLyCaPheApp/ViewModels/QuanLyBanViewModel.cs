using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class QuanLyBanViewModel : BaseViewModel
    {
        private readonly BanRepository _repo = new();
        private ObservableCollection<Ban> _danhSach = new();
        public ObservableCollection<Ban> DanhSach { get => _danhSach; set => SetProperty(ref _danhSach, value); }
        public ICommand LoadCommand { get; }
        public QuanLyBanViewModel()
        {
            LoadCommand = new RelayCommand(_ => Load());
            Load();
        }
        public void Load() => DanhSach = new ObservableCollection<Ban>(_repo.GetAll());
    }
}
