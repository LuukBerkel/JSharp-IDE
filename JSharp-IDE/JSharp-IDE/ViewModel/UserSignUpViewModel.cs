using JSharp_IDE.Network;
using JSharp_IDE.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class UserSignUpViewModel : INotifyPropertyChanged
    {
        private string mProjectName;
        public string ProjectName
        {
            get
            {
                if (mProjectName == null)
                {
                    mProjectName = "Project name";
                }
                return mProjectName;
            }

            set
            {
                mProjectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        private RelayCommand mHostCommand;
        public ICommand HostCommand
        {
            get
            {
                if (mHostCommand == null)
                {
                    mHostCommand = new RelayCommand(param =>
                    {
                        if (Project.ProjectDirectory == null)
                        {
                            MessageBox.Show("No valid project loaded.", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        if (mProjectName.Length > 0 && Directory.Exists(Project.ProjectDirectory))
                        {
                            int port;
                            if (int.TryParse(Settings.GetServerPort(), out port))
                            {
                                //Get every file path from the project
                                string[] filePaths = Directory.GetFiles(Project.ProjectDirectory, "*.*", SearchOption.AllDirectories);
                                // Store every filepath with the data of that file
                                Network.File[] files = new Network.File[filePaths.Length];

                                for (int i = 0; i < filePaths.Length; i++)
                                {
                                    files[i] = new Network.File(filePaths[i], System.IO.File.ReadAllBytes(filePaths[i]));
                                }

                                

                                Connection c = Connection.GetConnection(Settings.GetServerAddress(), port);
                                c.SendCommand(JSONCommand.Login());
                                c.SendCommand(JSONCommand.HostProject(mProjectName, new string[] { "hardcodedUserName" }, files ));
                            }
                        }
                    },
                    param => true);
                }
                return mHostCommand;
            }
        }

        private RelayCommand mJoinCommand;
        public ICommand JoinCommand
        {
            get
            {
                if (mJoinCommand == null)
                {
                    mJoinCommand = new RelayCommand(param =>
                    {
                        if (mProjectName.Length > 0)
                        {
                            Connection c = Connection.GetConnection(Settings.GetServerAddress(), 6969);
                            c.SendCommand(JSONCommand.Login());
                            c.SendCommand(JSONCommand.JoinProject(mProjectName));
                        }
                    },
                    param => true);
                }
                return mJoinCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
