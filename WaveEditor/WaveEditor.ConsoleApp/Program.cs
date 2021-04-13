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
            string input = @"../../../../Audio/5.wav";
            string output = @"../../../../Audio/6.wav";
            double scale = 2;
            ReadWriteBinary s = new ReadWriteBinary(scale,input, output);
            s.Read();
            AudioProcessor audioProcessor = new AudioProcessor(s.NumChannels, s.BitsPerSample, s.Data, s.Scale);
            byte[] newData = audioProcessor.ScaleTrack();
            s.Write(newData);
        }
    }
}