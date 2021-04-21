using System;
using System.Data;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class ReadWriteBinary
    {
        private double _scale;
        private string _inputPath;
        private string _outputPath;
        private double _loop;
        
        public double Scale => _scale;

        // RIFFHEADER
        private UInt32 _chunkId;
        private UInt32 _chunkSize;
        private UInt32 _format;

        //SUBCHUNK1 
        private UInt32 _subchunk1Id;
        private UInt32 _subchunk1Size;
        private UInt16 _audioFormat;
        private UInt16 _numChannels;

        public UInt16 NumChannels
        {
            get => _numChannels;
            set => _numChannels = value;
        }

        private UInt32 _sampleRate;
        private UInt32 _byteRate;
        private UInt16 _blockAlign;
        private UInt16 _bitsPerSample;

        public UInt16 BitsPerSample
        {
            get => _bitsPerSample;
            set => _bitsPerSample = value;
        }

        //SUBCHUNK2
        private UInt32 _subchunk2Id;
        private UInt32 _subchunk2Size;
        private byte[] _data;

        public byte[] Data
        {
            get => _data;
            set => _data = value;
        }
        
        public ReadWriteBinary(double scale, string inputPath, string outputPath, double loop)
        {
            _scale = scale;
            _inputPath = inputPath;
            _outputPath = outputPath;
            _loop = loop;
        }

        public void Read()
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(_inputPath, FileMode.Open, FileAccess.Read)))
            {
                // RIFFHEADER
                _chunkId = binaryReader.ReadUInt32();
                _chunkSize = binaryReader.ReadUInt32();
                _format = binaryReader.ReadUInt32();

                //SUBCHUNK1 
                _subchunk1Id = binaryReader.ReadUInt32();
                _subchunk1Size = binaryReader.ReadUInt32();
                _audioFormat = binaryReader.ReadUInt16();
                _numChannels = binaryReader.ReadUInt16();
                _sampleRate = binaryReader.ReadUInt32();
                _byteRate = binaryReader.ReadUInt32();
                _blockAlign = binaryReader.ReadUInt16();
                _bitsPerSample = binaryReader.ReadUInt16();

                //SUBCHUNK2
                _subchunk2Id = binaryReader.ReadUInt32();
                _subchunk2Size = binaryReader.ReadUInt32();
                _data = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
            }
        }

        public void Write(byte[] newData)
        {
            Console.WriteLine($"Written result to {_outputPath}");
            using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(_outputPath, FileMode.Create)))
            {
                _subchunk2Size = (uint)(newData.Length * _numChannels * _bitsPerSample / 8);
                _chunkSize = 4 + (8 + _subchunk1Size) + (8 + _subchunk2Size);
                binaryWriter.Write(_chunkId);
                binaryWriter.Write(_chunkSize);
                binaryWriter.Write(_format);
                binaryWriter.Write(_subchunk1Id);
                binaryWriter.Write(_subchunk1Size);
                binaryWriter.Write(_audioFormat);
                binaryWriter.Write(_numChannels);
                binaryWriter.Write(_sampleRate);
                binaryWriter.Write(_byteRate);
                binaryWriter.Write(_blockAlign);
                binaryWriter.Write(_bitsPerSample);
                binaryWriter.Write(_subchunk2Id);
                binaryWriter.Write(_subchunk2Size);
                foreach (var sample in newData)
                {
                    binaryWriter.Write(sample);
                }
            }
        }
    }
}