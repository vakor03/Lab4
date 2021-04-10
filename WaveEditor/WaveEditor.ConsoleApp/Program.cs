using System;
using WaveEditor.Resize.Lib;

namespace WaveEditor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string input = @"../../../../Audio/5.wav";
            string output = @"../../../../Audio/6.wav";
            AudioProcessor a1 = new AudioProcessor();
            ReadWriteBinary s = new ReadWriteBinary(a1, input, output);
            s.Read();
            a1.Write();
        }
    }
}