using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Environment;

namespace JSharp_IDE
{
    class UIHandler
    {
        private TreeView treeView;
        public UIHandler(Window window)
        {
        }

        public void CodeTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            Task.Run(async () =>
            {
                await TextFormatter.OnTextPasted(rtb);
            });
        }

        public void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            this.treeView = treeView;
        }

        public void MenuItem_Open(object sender, RoutedEventArgs e)
        {

            UpdateTreeView(OpenFolderDialog());
        }

        /// <summary>
        /// This method will also update the treeview, and create the new project folders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItem_New(object sender, RoutedEventArgs e)
        {
            string path = OpenFolderDialog();
            Directory.CreateDirectory(Path.Combine(path, "src"));
            Directory.CreateDirectory(Path.Combine(path, "out"));
            Directory.CreateDirectory(Path.Combine(path, "lib"));
            Directory.CreateDirectory(Path.Combine(path, "res"));
            UpdateTreeView(path);
        }

        private void UpdateTreeView(string projectPath)
        {
            this.treeView.Dispatcher.Invoke(() => {
                this.treeView.Items.Clear();
                var rootDirectoryInfo = new DirectoryInfo(projectPath);
                var rootNode = CreateDirectoryNode(rootDirectoryInfo);
                this.treeView.Items.Add(rootNode);
            });
        }

        private static TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeViewItem directoryNode = new TreeViewItem();
            directoryNode.Header = directoryInfo.Name;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                try
                {
                    directoryNode.Items.Add(CreateDirectoryNode(directory));
                } catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                directoryNode.Items.Add(new TreeViewItem().Header = file.Name);
            }
            return directoryNode;
        }

        private string OpenFolderDialog()
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.SelectedPath = Environment.SpecialFolder.MyDocuments.ToString();
            dlg.ShowNewFolderButton = true;
            bool? success = dlg.ShowDialog();
            string path = null;
            if (success == true)
            {
                path = dlg.SelectedPath;
            }

            return path;
        }
    }
}
