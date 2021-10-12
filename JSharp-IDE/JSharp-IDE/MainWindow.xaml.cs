using JSharp_IDE.Network;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace JSharp_IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TabControl CodePanels;
        public static bool Running = true;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            CodePanels = TabControl;
            Closing += Stop;
        }

        public void Stop(object sender, CancelEventArgs e)
        {
/*            if (sender.GetType() == this.GetType())
            {
                Connection c = Connection.Instance;
                if (c != null)
                {
                    c.Stop();
                }
                Running = false;
            }*/
            Environment.Exit(Environment.ExitCode);
        }
    }
}
