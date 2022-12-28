using System;
using System.IO;
namespace ConsoleApp2
{
    public class Runner
    {
        public static void CreateFile()
        {
            File.Create(@"C:\Users\asulin\Downloads\TestDoc");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
