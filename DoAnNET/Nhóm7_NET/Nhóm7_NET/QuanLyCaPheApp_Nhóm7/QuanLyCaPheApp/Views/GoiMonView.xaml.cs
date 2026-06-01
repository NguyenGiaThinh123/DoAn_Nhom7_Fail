using QuanLyCaPheApp.Models;
using QuanLyCaPheApp.ViewModels;
using System.Windows.Controls;

namespace QuanLyCaPheApp.Views
{
    public partial class GoiMonView : Page
    {
        private GoiMonViewModel _vm;

        public GoiMonView()
        {
            InitializeComponent();
            _vm = (GoiMonViewModel)DataContext;
            this.Loaded += (s, e) => _vm.Load();
        }

        public void SetBan(Ban ban)
        {
            _vm.SetBan(ban);
        }
    }
}
