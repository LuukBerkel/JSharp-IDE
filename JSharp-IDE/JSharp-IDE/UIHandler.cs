using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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

        public void AddFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = "Class";
            saveFileDialog.DefaultExt = ".java";
            saveFileDialog.Filter = "Java classes|*.java";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, $"public class {Path.GetFileNameWithoutExtension(saveFileDialog.FileName)} {{{Environment.NewLine}{Environment.NewLine}}}");
                UpdateTreeView(Compiler.projectPath);
            }
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
            if (path != null)
            {
                Directory.CreateDirectory(Path.Combine(path, "src"));
                Directory.CreateDirectory(Path.Combine(path, "out"));
                Directory.CreateDirectory(Path.Combine(path, "lib"));
                Directory.CreateDirectory(Path.Combine(path, "res"));
                File.WriteAllText(Path.Combine(path, "src", "Main.java"), $"public class Main {{{Environment.NewLine}{Environment.NewLine}}}");
                UpdateTreeView(path);
            }
        }

        /// <summary>
        /// This method will update the treeview to display the current file hierarchy.
        /// !! This will also update current project dir in Compiler !!
        /// </summary>
        /// <param name="projectPath"></param>
        private void UpdateTreeView(string projectPath)
        {
            Compiler.projectPath = projectPath;
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
            directoryNode.Tag = directoryInfo.FullName;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                try
                {
                    directoryNode.Items.Add(CreateDirectoryNode(directory));
                } catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine($"No access to folder \n {e.Message}");
                }
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = file.Name;
                item.Tag = file.FullName;
                Trace.WriteLine(file.FullName);
                directoryNode.Items.Add(item);
            }

            return directoryNode;
        }


        /// <summary>
        /// Open the double clicked class file in a new rich text box in a TabView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="tabControl"></param>
        /// <param name="rtb"></param>
        public void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e, TabControl tabControl, RichTextBox rtb)
        {
            Trace.WriteLine((this.treeView.SelectedItem as TreeViewItem).Tag);
            string path = (this.treeView.SelectedItem as TreeViewItem).Tag.ToString();

            if (File.Exists(path))
            {
                TabItem tab = new TabItem();
                FlowDocument doc = new FlowDocument();

                foreach (string line in File.ReadAllLines(path))
                {
                    Run run = new Run(line);
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(run);
                    p.LineHeight = 2;
                    doc.Blocks.Add(p);
                }

                rtb.Document = doc;
                tab.Content = rtb;
                tab.Header = new DirectoryInfo(path).Name;
                tab.IsSelected = true;
                tab.Focus();
                tabControl.Items.Add(tab);
            }
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

        public void Button_RunCode(object sender, RoutedEventArgs e)
        {
            try
            {
                Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin");
                compiler.Compile(@"C:\TEst\out", @"C:\TEst\src", @"C:\TEst\lib", @"C:\TEst\res");
                compiler.Execute(@"C:\TEst\out", @"C:\TEst\lib", @"C:\TEst\res", "AngryBirds");
            }
            catch (Exception ex)
            {
                
                ErrorWindow window = new ErrorWindow();
                window.setError(ex.Message);
                window.Show();
            }
        }

        public void Button_CompileCode(object sender, RoutedEventArgs e)
        {
            try
            {
                Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin");
                compiler.Compile(@"C:\TEst\out", @"C:\TEst\src", @"C:\TEst\lib", @"C:\TEst\res");
            }
            catch (Exception ex)
            {
                ErrorWindow window = new ErrorWindow();
                window.setError(ex.Message);
                window.Show();
            }
        }
    }
}
