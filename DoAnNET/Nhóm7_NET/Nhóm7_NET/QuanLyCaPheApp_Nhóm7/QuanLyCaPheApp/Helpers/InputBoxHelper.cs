using System.Windows;
using System.Windows.Controls;

namespace QuanLyCaPheApp.Helpers
{
    public static class InputBoxHelper
    {
        public static string? Show(string prompt, string title = "Nhập thông tin", string defaultValue = "")
        {
            var dialog = new Window
            {
                Title = title,
                Width = 380,
                Height = 170,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = SystemColors.WindowBrush
            };

            var grid = new Grid { Margin = new Thickness(16) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var label = new TextBlock
            {
                Text = prompt,
                Margin = new Thickness(0, 0, 0, 8),
                FontSize = 13
            };
            Grid.SetRow(label, 0);

            var textBox = new TextBox
            {
                Text = defaultValue,
                Height = 34,
                Padding = new Thickness(8, 6, 8, 6),
                FontSize = 13,
                Margin = new Thickness(0, 0, 0, 12)
            };
            Grid.SetRow(textBox, 1);
            textBox.SelectAll();
            textBox.Focus();

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            Grid.SetRow(btnPanel, 2);

            var btnOk = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 32,
                IsDefault = true,
                Margin = new Thickness(0, 0, 8, 0)
            };
            btnOk.Click += (s, e) => dialog.DialogResult = true;

            var btnCancel = new Button
            {
                Content = "Hủy",
                Width = 80,
                Height = 32,
                IsCancel = true
            };

            btnPanel.Children.Add(btnOk);
            btnPanel.Children.Add(btnCancel);

            grid.Children.Add(label);
            grid.Children.Add(textBox);
            grid.Children.Add(btnPanel);
            dialog.Content = grid;
            dialog.Loaded += (s, e) => { textBox.Focus(); textBox.SelectAll(); };

            bool? result = dialog.ShowDialog();
            return result == true ? textBox.Text.Trim() : null;
        }
    }
}
