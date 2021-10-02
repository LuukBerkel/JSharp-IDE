﻿using JSharp_IDE.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class FileMenuViewModel
    {
        private RelayCommand mNewCommand;
        public ICommand NewCommand
        {
            get
            {
                if (mNewCommand == null)
                {
                    mNewCommand = new RelayCommand(param =>
                    {
                        Project.CreateNewProject();
                    },
                    param => true);
                }
                return mNewCommand;
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
