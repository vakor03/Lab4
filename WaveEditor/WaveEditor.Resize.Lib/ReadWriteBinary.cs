using System;
using System.Data;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class ReadWriteBinary
    {
        private int _scale;
        private string _inputPath;
        private string _outputPath;
        
        public int Scale => _scale;

        // RIFFHEADER
        private Int32 _chunkId;
        private Int32 _chunkSize;
        private Int32 _format;

        //SUBCHUNK1 
        private Int32 _subchunk1Id;
        private Int32 _subchunk1Size;
        private Int16 _audioFormat;
        private Int16 _numChannels;

        public short NumChannels
        {
            get => _numChannels;
            set => _numChannels = value;
        }

        private Int32 _sampleRate;
        private Int32 _byteRate;
        private Int16 _blockAlign;
        private Int16 _bitsPerSample;

        public short BitsPerSample
        {
            get => _bitsPerSample;
            set => _bitsPerSample = value;
        }

        //SUBCHUNK2
        private Int32 _subchunk2Id;
        private Int32 _subchunk2Size;
        private byte[] _data;

        public byte[] Data
        {
            get => _data;
            set => _data = value;
        }
        
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
                _chunkId = binaryReader.ReadInt32();
                _chunkSize = binaryReader.ReadInt32();
                _format = binaryReader.ReadInt32();

                //SUBCHUNK1 
                _subchunk1Id = binaryReader.ReadInt32();
                _subchunk1Size = binaryReader.ReadInt32();
                _audioFormat = binaryReader.ReadInt16();
                _numChannels = binaryReader.ReadInt16();
                _sampleRate = binaryReader.ReadInt32();
                _byteRate = binaryReader.ReadInt32();
                _blockAlign = binaryReader.ReadInt16();
                _bitsPerSample = binaryReader.ReadInt16();

                //SUBCHUNK2
                _subchunk2Id = binaryReader.ReadInt32();
                _subchunk2Size = binaryReader.ReadInt32();
                _data = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
            }
        }

        public void Write(byte[] newData)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(_outputPath, FileMode.Create)))
            {
                _subchunk2Size = newData.Length * _numChannels * _bitsPerSample / 8;
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