using System;
using WaveEditor.Resize.Lib;

namespace WaveEditor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(1.2%1);
            string input = @"../../../../Audio/5.wav";
            string output = @"../../../../Audio/6.wav";
            int scale = 1;
            ReadWriteBinary s = new ReadWriteBinary(scale,input, output);
            s.Read();
            
        }
    }
}