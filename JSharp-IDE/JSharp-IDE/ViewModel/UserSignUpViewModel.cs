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
                        if (mProjectName.Length > 0)
                        {
                            Connection c = Connection.GetConnection(Settings.GetServerAddress(), 6969);
                            c.SendCommand(JSONCommand.Login());
                            c.SendCommand(JSONCommand.HostProject(mProjectName, new string[] { "hardcoded" }, new Network.File[] { new Network.File(Path.Combine(Directory.GetCurrentDirectory(), "Main.java"), "joemama") }));
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
