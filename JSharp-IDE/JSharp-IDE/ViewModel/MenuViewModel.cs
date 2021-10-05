using JSharp_IDE;
using JSharp_IDE.View;
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
    class MenuViewModel
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
                        sv.Show();
                    },
                    param => true);
                }
                return mSettingsCommand;
            }
        }
    }
}
