using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JSharp_IDE.ViewModel
{
    class MainWindowViewModel
    {
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
                            Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin");
                            compiler.Compile(Path.Combine(Project.ProjectDirectory, "out"), 
                                Path.Combine(Project.ProjectDirectory, "src"), 
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"));
                            compiler.Execute(
                                Path.Combine(Project.ProjectDirectory, "out"),
                                Path.Combine(Project.ProjectDirectory, "lib"),
                                Path.Combine(Project.ProjectDirectory, "res"),
                                Project.ProjectName);
                        }
                        catch (Exception ex)
                        {

                            ErrorWindow window = new ErrorWindow();
                            window.setError(ex.Message);
                            window.Show();
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
                        }
                        catch (Exception ex)
                        {
                            ErrorWindow window = new ErrorWindow();
                            window.setError(ex.Message);
                            window.Show();
                        }
                    },
                    param => true);
                }
                return mCompileCommand;
            }
        }
    }
}
