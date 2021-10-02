﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string javaPath = @"C:\Program Files\Java\jdk1.8.0_261\bin";

        private string mDebugWindow;
        public string DebugWindow
        {
            get
            {
                if (mDebugWindow == null)
                {
                    mDebugWindow = javaPath;
                }

                return mDebugWindow;
            }

            set
            {
                mDebugWindow = value;
                OnPropertyChanged("DebugWindow");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private RelayCommand mRunCommand;
        public ICommand RunCommand
        {
            get
            {
                if (mRunCommand == null)
                {
                    mRunCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            Compiler compiler = new Compiler(javaPath);
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"), 
                                Path.Combine(Project.ProjectDirectory, "src"), 
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"));
                            compiler.Execute(
                                Path.Combine(Project.ProjectDirectory, "out"),
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"),
                                Project.ProjectName);
                            DebugWindow = "";
                        }
                        catch (Exception ex)
                        {
                            DebugWindow += ("\n{0}f", ex.Message);
                        }
                    },
                    param => true);
                }
                return mRunCommand;
            }
        }

        private RelayCommand mCompileCommand;
        public ICommand CompileCommand
        {
            get
            {
                if (mCompileCommand == null)
                {
                    mCompileCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin"); 
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"),
                                 Path.Combine(Project.ProjectDirectory, "src"),
                                 Path.Combine(Project.ProjectDirectory, "lib"),
                                 Path.Combine(Project.ProjectDirectory, "res"));
                            DebugWindow = "";
                        }
                        catch (Exception ex)
                        {
                            DebugWindow += ("\n{0}f", ex.Message);
                        }
                    },
                    param => true);
                }
                return mCompileCommand;
            }
        }
    }
}
