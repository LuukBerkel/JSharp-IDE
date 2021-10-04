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
                            SaveAllOpenedFiles();
                            Compiler compiler = new Compiler(javaPath, this);
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"), 
                                Path.Combine(Project.ProjectDirectory, "src"), 
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"));
                            compiler.Execute(
                                Path.Combine(Project.ProjectDirectory, "out"),
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"),
                                "Main");
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
                            SaveAllOpenedFiles();
                            Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin", this); 
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

        private void SaveAllOpenedFiles()
        {
            foreach (TabItem item in MainWindow.CodePanels.Items)
            {
                RichTextBox rtb = item.Content as RichTextBox;

                string[] lines = new string[rtb.Document.Blocks.Count];
                for (int i = 0; i < rtb.Document.Blocks.Count; i++)
                {
                    lines[i] = new TextRange(rtb.Document.Blocks.ElementAt(i).ContentStart, rtb.Document.Blocks.ElementAt(i).ContentEnd).Text;
                }
                Debug.WriteLine(lines);
                File.WriteAllLines(item.Tag.ToString(), lines);
            }
        }
    }
}
