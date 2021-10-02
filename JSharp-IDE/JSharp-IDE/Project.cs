using JSharp_IDE.View;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                UpdateTreeView(path);
            }
        }

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

        /// <summary>
        /// This method will update the treeview to display the current file hierarchy.
        /// !! This will also update current project dir in Compiler !!
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
                Trace.WriteLine(file.FullName);
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