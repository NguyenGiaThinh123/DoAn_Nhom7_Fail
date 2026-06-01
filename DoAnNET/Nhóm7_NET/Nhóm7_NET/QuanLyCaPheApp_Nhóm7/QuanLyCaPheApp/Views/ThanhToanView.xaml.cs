using System.Windows.Controls;

namespace QuanLyCaPheApp.Views
{
    public partial class ThanhToanView : Page
    {
        public ThanhToanView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (DataContext is ViewModels.ThanhToanViewModel vm)
                    vm.Load();
            };
        }
    }
}
