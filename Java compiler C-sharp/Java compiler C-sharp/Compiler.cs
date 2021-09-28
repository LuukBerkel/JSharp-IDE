using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Java_compiler_C_sharp
{
    class Compiler
    {
        public string CompilerPath { get; set; }

        public Compiler(string compilerPath)
        {
            if (!Directory.Exists(compilerPath)) throw new Exception("Invalid java systemvariable path");
            this.CompilerPath = compilerPath;
        }


        public void Compile(string pathOut, string pathSrc, string pathLib, string pathRes)
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
                string[] libraries  = Directory.GetFiles(pathLib);
                string libCommand = "";
                foreach(string lib in libraries)
                {
                    libCommand = libCommand + lib + ";";
                }

                //Resouces
                string resCommand = pathRes + ";";
                


                //Arguments
                string compileCommand = @"javac -cp " + libCommand + resCommand + @" ^ " + pathSrc + @"\*.java -d " + pathOut;

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
                if (error.Length > 0) throw new Exception(error);

                //Closing
                compileTask.WaitForExit();
                compileTask.Close();
            }
        }

        public void Execute(string pathOut, string pathLib, string pathRes, string main)
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
                executeTask.Start();

                //Execution
                executeTask.StandardInput.WriteLine(executeCommand);
                executeTask.StandardInput.Flush();
                executeTask.StandardInput.Close();
                executeTask.WaitForExit();
                executeTask.Close();
            }
        }


       
    }
}
