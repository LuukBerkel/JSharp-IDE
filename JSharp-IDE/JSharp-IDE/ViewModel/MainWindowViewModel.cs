using JSharp_IDE;
using JSharp_IDE.Utils;
using JSharp_IDE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Debug window is the console output on the bottom of the GUI.
        /// </summary>
        private string mDebugWindow;
        public string DebugWindow
        {
            get
            {
                if (mDebugWindow == null)
                {
                    mDebugWindow = Settings.GetJavaBin();
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

        /// <summary>
        /// Builds and then runs the code.
        /// </summary>
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
                            SaveAllOpenedFiles();
                            Compiler compiler = new Compiler(Settings.GetJavaBin(), this);
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"), 
                                Path.Combine(Project.ProjectDirectory, "src"), 
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"));
                            compiler.Execute(
                                Path.Combine(Project.ProjectDirectory, "out"),
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"),
                                compiler.MainSearcher(Path.Combine(Project.ProjectDirectory, "src")));
                            DebugWindow = "";
                        }
                        catch (Exception ex)
                        {
                            DebugWindow += ("\n"+ ex.Message);
                        }
                    },
                    param => true);
                }
                return mRunCommand;
            }
        }

        /// <summary>
        /// Only builds the code.
        /// </summary>
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
                            SaveAllOpenedFiles();
                            DebugWindow = "Started build";
                            Compiler compiler = new Compiler(Settings.GetJavaBin(), this); 
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"),
                                 Path.Combine(Project.ProjectDirectory, "src"),
                                 Path.Combine(Project.ProjectDirectory, "lib"),
                                 Path.Combine(Project.ProjectDirectory, "res"));
                            DebugWindow = "Build completed";
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

        /// <summary>
        /// Saves all files to the disk that are opened in the editor.
        /// </summary>
        public static void SaveAllOpenedFiles()
        {
            foreach (TabItem item in MainWindow.CodePanels.Items)
            {
                item.Dispatcher.Invoke(() =>
                {
                    RichTextBox rtb = item.Content as RichTextBox;

                    string[] lines = new string[rtb.Document.Blocks.Count];
                    for (int i = 0; i < rtb.Document.Blocks.Count; i++)
                    {
                        lines[i] = new TextRange(rtb.Document.Blocks.ElementAt(i).ContentStart, rtb.Document.Blocks.ElementAt(i).ContentEnd).Text;
                    }
                    Debug.WriteLine("Saved files");
                    File.WriteAllLines(Path.Combine(Project.ProjectDirectory, item.Tag.ToString()), lines);
                });
            }
        }

        /// <summary>
        /// Save all the opened files.
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
                        SaveAllOpenedFiles();
                    },
                    param => true);
                }
                return mSaveCommand;
            }
        }
    }
}
