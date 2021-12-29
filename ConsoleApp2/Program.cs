using System;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Length != 2)
            {
                Console.Out.WriteLine("Incorrect number of arguments.");
            }
            else
            {
                var src = args[0];
                var dest = args[1];
                FileCopier copier = new FileCopier();
                Console.Out.WriteLine("Copied files " + copier.Copy(src, dest));
            }

        }

        
    }
}