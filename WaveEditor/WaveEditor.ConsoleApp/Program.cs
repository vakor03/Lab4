using System;
using System.Collections.Generic;
using WaveEditor.Resize.Lib;

namespace WaveEditor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(1.2%1);
            string input = @"C:\Users\Вакор\Desktop\Lab4\WaveEditor\Audio\" +args[0];
            string output = @"C:\Users\Вакор\Desktop\Lab4\WaveEditor\Audio\" +args[1];
            double scale = Double.Parse(args[2]);
            ReadWriteBinary s = new ReadWriteBinary(scale,input, output);
            s.Read();
            AudioProcessor audioProcessor = new AudioProcessor(s.NumChannels, s.BitsPerSample, s.Data, s.Scale);
            byte[] newData = audioProcessor.ScaleTrack();
            s.Write(newData);
        }
    }
}