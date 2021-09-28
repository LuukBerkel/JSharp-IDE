using System;

namespace Java_compiler_C_sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Compiler compiler = new Compiler(@"C:\Program Files\Java\jdk1.8.0_261\bin");

            try
            {
                compiler.Compile(@"D:\CompileTest\out", @"D:\CompileTest\src", @"D:\CompileTest\lib", @"D:\CompileTest\res");
                compiler.Execute(@"D:\CompileTest\out\", @"D:\CompileTest\lib", @"D:\CompileTest\res", @"GUI.GUI");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
