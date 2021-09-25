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
            CompilerPath = compilerPath;
        }


        public void Compile(string path, string file)
        {

            using (var compileTask = new Process())
            {
                //Checks
                if (!Directory.Exists(CompilerPath)) throw new Exception("Invalid java directory");
                if (!File.Exists(path + @"\" + file)) throw new Exception("Invalid file path");



                //Arguments
                string compileCommand = $"javac {file}";

                //Process
                compileTask.StartInfo.FileName = @"cmd.exe";
                compileTask.StartInfo.WorkingDirectory = path;
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
    }

   


   
}
