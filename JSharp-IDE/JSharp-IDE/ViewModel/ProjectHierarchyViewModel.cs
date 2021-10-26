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

        /// <summary>
        /// Add a new file to the project.
        /// </summary>
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

        /// <summary>
        /// Delete a file from the project.
        /// </summary>
        private RelayCommand mDeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (mDeleteCommand == null)
                {
                    mDeleteCommand = new RelayCommand(param =>
                    {
                        Project.DeleteFile();
                    },
                    param => true);
                }
                return mDeleteCommand;
            }
        }
    }
}
