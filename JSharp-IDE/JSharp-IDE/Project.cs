using JSharp_IDE.View;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JSharp_IDE
{
    public class Project
    {
        public static string ProjectDirectory { get; set; }
        public static string ProjectName { get; set; }

        /// <summary>
        /// Creates a new project with the correct file structure.
        /// </summary>
        public static void CreateNewProject()
        {
            string path = OpenFolderDialog();
            if (path != null)
            {
                Directory.CreateDirectory(Path.Combine(path, "src"));
                Directory.CreateDirectory(Path.Combine(path, "out"));
                Directory.CreateDirectory(Path.Combine(path, "lib"));
                Directory.CreateDirectory(Path.Combine(path, "res"));
                File.WriteAllText(Path.Combine(path, "src", "Main.java"), 
                    $"public class Main {{" +
                    $"{Environment.NewLine}    public static void main(String[] args) {{" +
                    $"{Environment.NewLine}        //Write your code here" +
                    $"{Environment.NewLine}    }}" +
                    $"{Environment.NewLine}}}");
                File.WriteAllText(Path.Combine(path, "users.txt"), "");
                UpdateTreeView(path);
            }
        }

        /// <summary>
        /// Add a file to the project
        /// </summary>
        public static void AddFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = "Class";
            saveFileDialog.DefaultExt = ".java";
            saveFileDialog.Filter = "Java classes|*.java";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, $"public class {Path.GetFileNameWithoutExtension(saveFileDialog.FileName)} {{{Environment.NewLine}{Environment.NewLine}}}");
                UpdateTreeView(ProjectDirectory);
            }
        }

        public static void OpenFileToEdit()
        {
            //Get the current selected item
            TreeViewItem treeViewItem = ProjectHierarchyView.ProjectHierarchyTree.SelectedItem as TreeViewItem;
            if (treeViewItem == null) { return; }
            string path = treeViewItem.Tag.ToString();

            //If the file is already opened, focus on that tab and exit this method.
            foreach (TabItem item in MainWindow.CodePanels.Items)
            {
                if (item.Tag.ToString() == path)
                {
                    item.Focus();
                    return;
                }
            }

            if (File.Exists(path))
            {
                RichTextBoxView rtbv = new RichTextBoxView();
                TabItem tabItem = new TabItem();
                //Create stack panel
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                //Close button
                Button closeButton = new Button();
                closeButton.Content = "X";
                closeButton.Click += (x, y) => MainWindow.CodePanels.Items.RemoveAt(MainWindow.CodePanels.SelectedIndex);
                //Label
                Label label = new Label();
                label.Content = treeViewItem.Header.ToString();
                //Add to stackpanel
                sp.Children.Add(label);
                sp.Children.Add(closeButton);
                tabItem.Header = sp;
                tabItem.Tag = path;

                FlowDocument doc = new FlowDocument();

                foreach (string line in File.ReadAllLines(path))
                {
                    Run run = new Run(line);
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(run);
                    p.Margin = new Thickness(0);
                    doc.Blocks.Add(p);
                }

                rtbv.RichTextBox.Document = doc;
                tabItem.Content = rtbv.RichTextBox;
                tabItem.IsSelected = true;
                tabItem.Focus();
                MainWindow.CodePanels.Items.Add(tabItem);

                Task.Run(async () =>
                {
                    await TextFormatter.OnTextPasted(rtbv.RichTextBox);
                });
            }
        }

        /// <summary>
        /// This method will update the treeview to display the current file hierarchy.
        /// !! This will also update current project dir in 
        /// 
        /// !!
        /// </summary>
        /// <param name="projectPath"></param>
        public static void UpdateTreeView(string projectPath)
        {
            ProjectDirectory = projectPath;
            Debug.WriteLine($"Current project path: {projectPath}");
            TreeView treeView = ProjectHierarchyView.ProjectHierarchyTree;
            if (treeView != null)
            {
                if (projectPath != null)
                {
                    treeView.Dispatcher.Invoke(() =>
                    {
                        treeView.Items.Clear();
                        var rootDirectoryInfo = new DirectoryInfo(projectPath);
                        var rootNode = CreateDirectoryNode(rootDirectoryInfo);
                        treeView.Items.Add(rootNode);
                    });
                }
            }
        }

        /// <summary>
        /// Recursive method that will get all files and folders from a certain path.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
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
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine($"No access to folder \n {e.Message}");
                }
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = file.Name;
                item.Tag = file.FullName;
                directoryNode.Items.Add(item);
            }

            return directoryNode;
        }

        /// <summary>
        /// Open a folder chooser dialog to select a project directory.
        /// </summary>
        /// <returns></returns>
        public static string OpenFolderDialog()
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            bool? success = dlg.ShowDialog();
            string path = null;
            if (success == true)
            {
                path = dlg.SelectedPath;
            }

            return path;
        }

        public static void DeleteFile()
        {
            //Get the current selected item
            TreeViewItem treeViewItem = ProjectHierarchyView.ProjectHierarchyTree.SelectedItem as TreeViewItem;
            if (treeViewItem == null) { return; }
            string path = treeViewItem.Tag.ToString();
            try
            {
                if (File.Exists(path)) {
                    MessageBoxResult mr = MessageBox.Show($"Are you sure you want to delete {new DirectoryInfo(path).Name}", "JSharp IDE", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        File.Delete(path);
                        ProjectHierarchyView.ProjectHierarchyTree.Items.Remove(treeViewItem);
                        Project.UpdateTreeView(Project.ProjectDirectory);
                        foreach (TabItem item in MainWindow.CodePanels.Items)
                        {
                            if (item.Tag == treeViewItem.Tag)
                            {
                                MainWindow.CodePanels.Items.Remove(item);
                                return;
                            }
                        }
                    }
                }
            } catch (Exception)
            {
                Debug.WriteLine($"File not found {path}");
            }
        }
    }
}