using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUiThread.Utilities;

namespace WpfUiThread.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        MainWindowViewModel _viewModel;
        MainWindow _mainWindow;

        public Page1()
        {
            InitializeComponent();
            Loaded += Page1_Loaded;
        }

        void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = UIHelper.FindVisualParent<MainWindow>(this);
            if (_mainWindow != null)
            {
                _viewModel = _mainWindow.DataContext as MainWindowViewModel;
            }
        }

        private void UpdateFreezeButton_Click(object sender, RoutedEventArgs e)
        {            
            _viewModel.UpdateFreezeButton();

            ResetDemo(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateThreadButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateThreadButton();

            ResetDemo(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateCrashButton_Click(object sender, RoutedEventArgs e)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            new Thread(() =>
            {
                try
                {
                    _viewModel.UpdateDatabase();

                    _mainWindow.Message.Text = "Database Updated";

                }
                catch (Exception exception)
                {
                    _viewModel.ErrorMessage = "APPCRASH " + exception.Message + Environment.NewLine + exception.StackTrace;
                }

                ResetDemo(scheduler);

            }).Start();
        }

        private void ResetDemo(TaskScheduler scheduler)
        {
            Task.Delay(4000).ContinueWith(
                prevTask =>
                {
                    _viewModel.Reset();

                    // note: Assigning directly to the text label would
                    //       have broken the binding, and this line restores it.
                    _mainWindow.Message.SetBinding(TextBlock.TextProperty, new Binding("Message") { Source = _viewModel });
                }
            , scheduler);
        }

        private void UpdateNoCrashButton_Click(object sender, RoutedEventArgs e)
        {
            // so that demo can clean up
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            new Thread(() =>
            {
                _viewModel.UpdateDatabase();

                Dispatcher.Invoke(delegate
                {
                    _mainWindow.Message.Text = "Database Updated";
                    ResetDemo(scheduler);
                });

            }).Start();
        }

        private void NavigatePage2Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new Page2 { DataContext = _viewModel };
            NavigationService.Navigate(page);
        }

    }

}
