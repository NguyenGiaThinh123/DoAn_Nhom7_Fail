using System.Windows.Controls;

namespace QuanLyCaPheApp.Views
{
    public partial class DatBanView : Page
    {
        public DatBanView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (DataContext is ViewModels.DatBanViewModel vm)
                    vm.Load();
            };
        }
    }
}
