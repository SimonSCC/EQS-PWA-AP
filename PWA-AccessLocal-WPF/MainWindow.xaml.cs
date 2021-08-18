using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Interfaces;
using WPF_Shell_Access_NET5._0.Models;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace PWA_AccessLocal_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDispatcherCaller
    {


        public MainWindow()
        {
            InitializeComponent();
            MainViewModel vm = new MainViewModel();
            vm.UIThreadCaller = this;
            DataContext = vm;
        }

        public void InvokeWithUIThread(ICommand command)
        {
            this.Dispatcher.Invoke(() =>
            {
                command.Execute(null);
            });
        }

        private void ValidateTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Clipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(HyperlinkPWA.Content.ToString());
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_MinimizeClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Button_WindowStateButtonClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Button_CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
