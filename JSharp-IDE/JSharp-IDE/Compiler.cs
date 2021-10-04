using JSharp_IDE.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JSharp_IDE
{
    class Compiler
    {
        public string CompilerPath { get; set; }
        public MainWindowViewModel mwvm;

        public Compiler(string compilerPath, MainWindowViewModel mwvm)
        {
            if (!Directory.Exists(compilerPath)) throw new Exception("Invalid java systemvariable path");
            this.CompilerPath = compilerPath;
            this.mwvm = mwvm;
        }

        public void Compile(string pathOut, string pathSrc, string pathLib, string pathRes)
        {
            new Thread(() =>
            {
                using (var compileTask = new Process())
                {
                    //Checks
                    if ((!Directory.Exists(pathSrc)) &&
                        (!Directory.Exists(pathOut)) &&
                        (!Directory.Exists(pathLib)) &&
                        (!Directory.Exists(pathRes)))
                        throw new Exception("Invalid file path");

                    //libraries
                    string[] libraries = Directory.GetFiles(pathLib);
                    string libCommand = "";
                    foreach (string lib in libraries)
                    {
                        libCommand = libCommand + lib + ";";
                    }

                    //Resources
                    string resCommand = pathRes + ";";

                    //Files
                    string fileFinderCompiler(string pathFiles)
                    {
                        //If there is something in the directory
                        if (Directory.GetDirectories(pathFiles).Length > 0
                            || Directory.GetFiles(pathFiles).Length > 0)
                        {
                            //Variables
                            string result = "";

                            //Search for files
                            string[] files = Directory.GetFiles(pathFiles);
                            foreach (string file in files)
                            {
                                if (file.Contains(".java"))
                                    result += file + " ";
                            }

                            //Search for direcories
                            string[] directory = Directory.GetDirectories(pathFiles);
                            if (directory.Length > 0)
                            {
                                foreach (string dir in directory)
                                {
                                    result += fileFinderCompiler(dir);
                                }
                            }

                            //Returning list..
                            return result;
                        }

                        //Throw exceptoin when now files are found..
                        throw new Exception("No files found");
                    }


                    //Arguments
                    string compileCommand = @"javac -cp " + libCommand + resCommand + @" ^ " + fileFinderCompiler(pathSrc) + " -d " + pathOut;


                    //Process
                    compileTask.StartInfo.FileName = @"cmd.exe";
                    compileTask.StartInfo.WorkingDirectory = pathOut;
                    compileTask.StartInfo.EnvironmentVariables["Path"] = this.CompilerPath;
                    compileTask.StartInfo.RedirectStandardInput = true;
                    compileTask.StartInfo.RedirectStandardOutput = true;
                    compileTask.StartInfo.RedirectStandardError = true;
                    compileTask.Start();

                    //Execution
                    compileTask.StandardInput.WriteLine(compileCommand);
                    compileTask.StandardInput.Flush();
                    compileTask.StandardInput.Close();

                    //Checking
                    string error = compileTask.StandardError.ReadToEnd();
                    if (error.Length > 0 && error.Contains("error")) throw new Exception(error);

                    //Closing
                    compileTask.WaitForExit();
                    compileTask.Close();
                }
            }).Start();
        }

        public void Execute(string pathOut, string pathLib, string pathRes, string main)
        {
            new Thread(() =>
            {
                using (var executeTask = new Process())
                {
                    //Checks
                    if ((!Directory.Exists(pathOut)) &&
                    (!Directory.Exists(pathLib)) &&
                    (!Directory.Exists(pathRes)))
                        throw new Exception("Invalid file path");

                    //libraries
                    string[] libraries = Directory.GetFiles(pathLib);
                    string libCommand = "";
                    foreach (string lib in libraries)
                    {
                        libCommand = libCommand + lib + ";";
                    }

                    //Resources
                    string resCommand = pathRes + "; ";

                    //Arguments
                    string executeCommand = @"java -classpath " + libCommand + resCommand + main;

                    //Process
                    executeTask.StartInfo.FileName = @"cmd.exe";
                    executeTask.StartInfo.WorkingDirectory = pathOut;
                    executeTask.StartInfo.RedirectStandardInput = true;
                    executeTask.StartInfo.RedirectStandardOutput = true;
                    executeTask.Start();

                    //Execution
                    executeTask.StandardInput.WriteLine(executeCommand);
                    executeTask.StandardInput.Flush();

                    // To avoid deadlocks, always read the output stream first and then wait.
                    new Thread(() => {
                        bool isReading = true;
                        int exitCode = -1;

                        while (isReading)
                        {
                            try
                            {
                                isReading = !executeTask.HasExited;
                                //exitCode = executeTask.ExitCode;
                                string output = executeTask.StandardOutput.ReadLine();
                                Debug.WriteLine(output);
                                this.mwvm.DebugWindow += "\n" + output;
                            } catch (Exception e)
                            {
                                Debug.WriteLine($"Java process finished with exit code {exitCode}");
                                break;
                            }
                        }
                    }).Start();

                //executeTask.StandardInput.Close();
                //executeTask.StandardOutput.Close();
                executeTask.WaitForExit();
                executeTask.Close();
                }
            }).Start();
        }
    }
}
