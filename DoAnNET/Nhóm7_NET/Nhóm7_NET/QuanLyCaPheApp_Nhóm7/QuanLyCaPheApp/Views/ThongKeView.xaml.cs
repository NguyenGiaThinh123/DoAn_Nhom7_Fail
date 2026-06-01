using System.Windows.Controls;

namespace QuanLyCaPheApp.Views
{
    public partial class ThongKeView : Page
    {
        public ThongKeView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (DataContext is ViewModels.ThongKeDoanhThuViewModel vm)
                    vm.Load();
            };
        }
    }
}
