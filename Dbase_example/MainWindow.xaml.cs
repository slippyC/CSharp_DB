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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dbase_example
{
    /// <summary>
    /// Author:     Christopher Whitehead<cwhitehead73@gmail.com>
    /// Purpose:    C# Dbase Example using "Chinook SQLite DB" with different types of SQL Queries
    ///             SQLite backend used as DB for portability and availability(no DB Server Setup)               
    ///             This DOES NOT incorporate Error Checking
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLConnector mLibrary;

        public MainWindow()
        {
            InitializeComponent();
            this.mLibrary = new SQLConnector();
            xArtists.ItemsSource = this.mLibrary.getArtists();          
        }

        private void xExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
