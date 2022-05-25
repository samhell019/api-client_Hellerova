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
        public MainWindow()
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

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (vm.Beruska2 != null)
            {
                EditZanrWindow editWindow = new EditZanrWindow();
                editWindow.DataContext = vm;
                vm.EditedZanr= vm.Beruska2;
                editWindow.ShowDialog();
            }

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            AddFilmWindow addWindow = new AddFilmWindow();
            addWindow.DataContext = vm;
            vm.NewFilm = new Models.Films();
            addWindow.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            AddZanrWindow addWindow = new AddZanrWindow();
            addWindow.DataContext = vm;
            vm.NewZanr = new Models.Zanrs();
            addWindow.ShowDialog();

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (vm.Beruska != null)
            {
                DetailFilmWindow editWindow = new DetailFilmWindow();
                editWindow.DataContext = vm;
               vm.EditedFilm = vm.Beruska;
                editWindow.ShowDialog();
            }
        }



        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (vm.Beruska != null)
            {
                DetailZanrWindow editWindow = new DetailZanrWindow();
                editWindow.DataContext = vm;
                vm.EditedZanr = vm.Beruska2;
                editWindow.ShowDialog();
            }

        }
    }
}

