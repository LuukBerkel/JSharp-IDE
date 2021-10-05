using JSharp_IDE.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class UserSignUpViewModel : INotifyPropertyChanged
    {
        private string mHostname;
        public string Hostname
        {
            get
            {
                if (mHostname == null)
                {
                    mHostname = "Hostname";
                }
                return mHostname;
            }

            set
            {
                mHostname = value;
                OnPropertyChanged("Hostname");
            }
        }

        private string mPort;
        public string Port
        {
            get
            {
                if (mPort == null)
                {
                    mPort = "6969";
                }
                return mPort;
            }

            set
            {
                mPort = value;
                OnPropertyChanged("Port");
            }
        }

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

        private RelayCommand mSignUpCommand;
        public ICommand SignUpCommand
        {
            get
            {
                if (mSignUpCommand == null)
                {
                    mSignUpCommand = new RelayCommand(param =>
                    {
                        if (mProjectName.Length > 0)
                        {
                            Connection c = Connection.GetConnection(mHostname, 6969);
                            c.SendCommand(JSONCommand.SignUp());
                            c.SendCommand(JSONCommand.JoinProject(mProjectName));
                        }
                    },
                    param => true);
                }

                return mSignUpCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
