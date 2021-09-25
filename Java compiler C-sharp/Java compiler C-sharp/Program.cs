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
                compiler.Compile(@"C:\Users\berke\Downloads\", "helloworld.java");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
