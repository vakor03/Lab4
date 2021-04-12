using System;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class ReadWriteBinary
    {
        private int _scale;
        private string _inputPath;
        private string _outputPath;

        public ReadWriteBinary(int scale, string inputPath, string outputPath)
        {
            _scale = scale;
            _inputPath = inputPath;
            _outputPath = outputPath;
        }

        public void Read()
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(_inputPath, FileMode.Open, FileAccess.Read)))
            {
                // RIFFHEADER
                Int32 chunkID = binaryReader.ReadInt32();
                Int32 chunkSize = binaryReader.ReadInt32();
                Int32 format = binaryReader.ReadInt32();

                //SUBCHUNK1 
                Int32 subchunk1Id = binaryReader.ReadInt32();
                Int32 subchunk1Size = binaryReader.ReadInt32();
                Int32 audioFormat = binaryReader.ReadInt16();
                Int16 numChannels = binaryReader.ReadInt16();
                Int32 sampleRate = binaryReader.ReadInt32();
                Int32 byteRate = binaryReader.ReadInt32();
                Int16 blockAlign = binaryReader.ReadInt16();
                Int16 bitsPerSample = binaryReader.ReadInt16();

                //SUBCHUNK2
                Int32 subchunk2Id = binaryReader.ReadInt32();
                Int32 subchunk2Size = binaryReader.ReadInt32();
                byte[] data = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
                
                

            }
        }
    }
}