using System;
using System.IO;
using System.Text;
using WaveEditor.Resize.Lib;

namespace WaveEditor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string input = @"../../../../Audio/eightGrade.wav";
            string output = @"";
            ReadWriteBinary s = new ReadWriteBinary(input, output);
            s.Read();
        }
    }
}