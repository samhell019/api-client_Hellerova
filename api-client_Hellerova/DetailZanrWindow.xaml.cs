﻿using System;
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
using System.Windows.Shapes;

namespace api_client_Hellerova
{
    /// <summary>
    /// Interakční logika pro DetailZanrWindow.xaml
    /// </summary>
    public partial class DetailZanrWindow : Window
    {
        public DetailZanrWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}