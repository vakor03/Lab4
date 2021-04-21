using System;
using WaveEditor.Resize.Lib;

namespace WaveEditor.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"..\..\..\..\Audio\" +args[0];
            string output = @"..\..\..\..\Audio\" +args[1];
            double scale = Double.Parse(args[2]);
            double loop = Double.Parse(args[3]);
            ReadWriteBinary s = new ReadWriteBinary(scale,input, output, loop);
            s.Read();
            AudioProcessor audioProcessor = new AudioProcessor(s.NumChannels, s.BitsPerSample, s.Data, s.Scale, loop);
            byte[] newData = audioProcessor.ScaleTrack();
            s.Write(newData);
        }
    }
}