using api_client_Hellerova.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace api_client_Hellerova
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        internal MainWindow()
        {
            InitializeComponent();
            vm = (MainViewModel)DataContext;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Beruska != null)
            {
                EditWindow editWindow = new EditWindow();
                editWindow.DataContext = vm;
                vm.EditedFilm = vm.Beruska;
                editWindow.ShowDialog();
            }
        }
    }
}
