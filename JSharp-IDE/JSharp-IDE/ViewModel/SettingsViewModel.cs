using JSharp_IDE.Network;
using JSharp_IDE.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Apply all values to the settings file.
        /// </summary>
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
        
        /// <summary>
        /// Sign up to the server.
        /// </summary>
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
                        c.SetErrorQueu("Signup failed....");
                    },
                    param => true);
                }
                return mSignUpCommand;
            }
        }

        /// <summary>
        /// Username box
        /// </summary>
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
                OnPropertyChanged("UserName");
            }
        }

        /// <summary>
        /// Password box
        /// </summary>
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
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] bytes = sha.ComputeHash(Encoding.ASCII.GetBytes(value));
                    mPassword = Encoding.ASCII.GetString(bytes);
                    Debug.WriteLine(mPassword);
                }
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// The server ip.
        /// </summary>
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

        /// <summary>
        /// Server port
        /// </summary>
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

        /// <summary>
        /// Java bin directory.
        /// </summary>
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
