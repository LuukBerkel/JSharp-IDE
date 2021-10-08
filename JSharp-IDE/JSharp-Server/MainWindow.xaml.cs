﻿using JSharp_Server.Comms;
using JSharp_Server.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace JSharp_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow main;

        public MainWindow()
        {
            InitializeComponent();
            Server server = new Server(System.Net.IPAddress.Any, 6969);
            server.Start();
            Debug.WriteLine("Starting server");
            main = this;
            
        }


        /// <summary>
        /// My dirty little fix still work to do to fix.....
        /// </summary>
        /// <param name="collection"></param>
        public static void SetLisview(ObservableCollection<Project> collection)
        {
            ListView list = main.FindName("list") as ListView;
            /*main.Dispatcher.Invoke((Action)(() => { main.list.Items.Clear(); }));
            main.Dispatcher.Invoke((Action)(() => {
                main.list.Items.Add(collection); }));*/
            list.Dispatcher.Invoke(() => list.Items.Refresh());
        }

        
    }
}
