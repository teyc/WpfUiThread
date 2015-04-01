using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfUiThread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _viewModel = new MainWindowViewModel();
        }

    }
}
