using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    public class ProjectHierarchyViewModel
    {
        public static TreeViewItem ParentItem { get; set; }

        private RelayCommand mAddNewFile;
        public ICommand NewFile
        {
            get
            {
                if (mAddNewFile == null)
                {
                    mAddNewFile = new RelayCommand(param =>
                    {
                        Project.AddFile();
                    },
                    param => true);
                }
                return mAddNewFile;
            }
        }

        private RelayCommand mOpenCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (mOpenCommand == null)
                {
                    mOpenCommand = new RelayCommand(param =>
                    {
                        Project.UpdateTreeView(Project.OpenFolderDialog());
                    },
                    param => true);
                }
                return mOpenCommand;
            }
        }
    }
}
