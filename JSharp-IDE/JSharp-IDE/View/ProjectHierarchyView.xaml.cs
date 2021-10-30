using System;
using System.Collections.Generic;
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

namespace JSharp_IDE.View
{
    /// <summary>
    /// Interaction logic for ProjectHierarchyView.xaml
    /// </summary>
    public partial class ProjectHierarchyView : UserControl
    {
        public static TreeView ProjectHierarchyTree;
        public ProjectHierarchyView()
        {
            InitializeComponent();
            ProjectHierarchyTree = ProjectHierarchyElement;
        }

        private void RightMouseButtonOnItem(object sender, MouseEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            Debug.WriteLine(item.Header);
            if (item != null)
            {
                Debug.WriteLine("Right clicked on item");
                item.Focus();
                item.IsSelected = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Open the double clicked class file in a new rich text box in a TabView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseDoubleClickOnItem(object sender, MouseButtonEventArgs e)
        {
            Project.OpenFileToEdit();
        }
    }
}
