using System;
using System.Windows;
using WPF_Shell_Access_NET;
using WPF_Shell_Access_NET5._0.Models;
using WPF_Shell_Access_NET5._0.ViewModels;

namespace PWA_AccessLocal_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            //Custominitialize();
        }

       



    }
}
