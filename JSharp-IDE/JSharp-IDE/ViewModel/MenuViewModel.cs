using JSharp_IDE;
using JSharp_IDE.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class MenuViewModel
    {
        /// <summary>
        /// Create a new project
        /// </summary>
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
        
        /// <summary>
        /// Open a project
        /// </summary>
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
        
        /// <summary>
        /// Save all the opened files from the project.
        /// </summary>
        private RelayCommand mSaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (mSaveCommand == null)
                {
                    mSaveCommand = new RelayCommand(param =>
                    {
                        MainWindowViewModel.SaveAllOpenedFiles();
                    },
                    param => true);
                }
                return mSaveCommand;
            }
        }

        /// <summary>
        /// Open the settings window.
        /// </summary>
        private RelayCommand mSettingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                if (mSettingsCommand == null)
                {
                    mSettingsCommand = new RelayCommand(param =>
                    {
                        SettingsView sv = new SettingsView();
                        sv.Width = 400;
                        sv.Height = 220;
                        sv.Title = "JSharp IDE - Settings";
                        sv.ResizeMode = ResizeMode.NoResize;
                        sv.Show();
                    },
                    param => true);
                }
                return mSettingsCommand;
            }
        }
    }
}
