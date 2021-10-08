using JSharp_IDE.Network;
using JSharp_IDE.Utils;
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
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private RelayCommand mApplyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                if (mApplyCommand == null)
                {
                    mApplyCommand = new RelayCommand(param =>
                    {
                        Settings.UpdateUsername(mUsername);
                        Settings.UpdatePassword(mPassword);
                        Settings.UpdateServerAddress(mAddress);
                        Settings.UpdateServerPort(mPort);
                        Settings.UpdateJavaDir(mJavaDir);
                    },
                    param => true);
                }
                return mApplyCommand;
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
                        Settings.UpdateUsername(mUsername);
                        Settings.UpdatePassword(mPassword);

                        Connection c = Connection.GetConnection(Settings.GetServerAddress(), int.Parse(Settings.GetServerPort()));
                        c.SendCommand(JSONCommand.SignUp());
                    },
                    param => true);
                }
                return mSignUpCommand;
            }
        }

        private string mUsername;
        public string UserName
        {
            get
            {
                if (mUsername == null)
                {
                    mUsername = Settings.GetUsername();
                }
                return mUsername;
            }

            set
            {
                mUsername = value;
                Debug.WriteLine("Username changed");
                OnPropertyChanged("UserName");
            }
        }

        private string mPassword;
        public string Password
        {
            get
            {
                if (mPassword == null)
                {
                    mPassword = Settings.GetPassword();
                }
                return mPassword;
            }

            set
            {
                mPassword = value;
                OnPropertyChanged("Password");
            }
        }

        private string mAddress;
        public string Address
        {
            get
            {
                if (mAddress == null)
                {
                    mAddress = Settings.GetServerAddress();
                }
                return mAddress;
            }

            set
            {
                mAddress = value;
                OnPropertyChanged("Address");
            }
        }

        private string mPort;
        public string Port
        {
            get
            {
                if (mPort == null)
                {
                    mPort = Settings.GetServerPort();
                }
                return mPort;
            }

            set
            {
                mPort = value;
                OnPropertyChanged("Port");
            }
        }

        private string mJavaDir;
        public string JavaDir
        {
            get
            {
                if (mJavaDir == null)
                {
                    mJavaDir = Settings.GetJavaBin();
                }
                return mJavaDir;
            }

            set
            {
                mJavaDir = value;
                OnPropertyChanged("JavaDir");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
