using JSharp_IDE.Network;
using JSharp_IDE.Utils;
using JSharp_IDE.View;
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
using ToastNotifications.Messages;

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
                                //Remove the out directory from the array.
                                filePaths = filePaths.Where(value => 
                                !value.Contains(Path.DirectorySeparatorChar + "out" + Path.DirectorySeparatorChar)).ToArray();
                                // Store every filepath with the data of that file
                                Network.NetworkFile[] files = new NetworkFile[filePaths.Length];

                                for (int i = 0; i < filePaths.Length; i++)
                                {
                                    string path = Project.GetLocalPath(filePaths[i]);
                                    files[i] = new NetworkFile(path, File.ReadAllBytes(filePaths[i]));
                                }

                                if (File.Exists(Path.Combine(Project.ProjectDirectory, "users.txt")))
                                {
                                    string[] usernames = System.IO.File.ReadAllLines(Path.Combine(Project.ProjectDirectory, "users.txt"));

                                    Connection c = Connection.GetConnection(Settings.GetServerAddress(), port);
                                    c.SendCommand(JSONCommand.Login());
                                    c.SetErrorQueu("Login error....");
                                    c.SendCommand(JSONCommand.HostProject(mProjectName, usernames, files));
                                    c.SetErrorQueu("Project error....");
                                }

                                Project.notifier.ShowInformation("The project is hosting.");
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
                            Project.SignInToProject();
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
