using JSharp_IDE.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

                    },
                    param => true);
                }
                return mApplyCommand;
            }
        }

        private string mUsername;
        public string UserName
        {
            get
            {
                if (mUsername == null)
                {
                    Settings.GetUsername();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
