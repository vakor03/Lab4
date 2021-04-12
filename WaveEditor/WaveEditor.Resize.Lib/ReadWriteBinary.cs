using System;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class ReadWriteBinary
    {
        private AudioProcessor _audioProcessor;
        private string _inputPath;
        private string _outputPath;

        public ReadWriteBinary(AudioProcessor audioProcessor, string inputPath, string outputPath)
        {
            _audioProcessor = audioProcessor;
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
                _audioProcessor.addRiffHeader(chunkID,chunkSize,format);
                
                //SUBCHUNK1 
                Int32 subchunk1Id = binaryReader.ReadInt32();
                Int32 subchunk1Size = binaryReader.ReadInt32();
                Int32 audioFormat = binaryReader.ReadInt16();
                Int16 numChannels = binaryReader.ReadInt16();
                Int32 sampleRate = binaryReader.ReadInt32();
                Int32 byteRate = binaryReader.ReadInt32();
                Int16 blockAlign = binaryReader.ReadInt16();
                Int16 bitsPerSample = binaryReader.ReadInt16();
                _audioProcessor.addSubchunk1(subchunk1Id, subchunk1Size, audioFormat, numChannels, sampleRate, byteRate, blockAlign, bitsPerSample);
                
                //SUBCHUNK2
                Int32 subchunk2Id = binaryReader.ReadInt32();
                Int32 subchunk2Size = binaryReader.ReadInt32();
                byte[] data = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
                _audioProcessor.addSubchunk2(subchunk2Id, subchunk2Size, data);
                
            }
        }
    }
}