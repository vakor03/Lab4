using System;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class AudioProcessor
    {
        // RIFFHEADER
        private Int32 _chunkID;
        private Int32 _chunkSize;
        private Int32 _format;
                
        //SUBCHUNK1 
        private Int32 _subchunk1Id;
        private Int32 _subchunk1Size;
        private Int32 _audioFormat;
        private Int16 _numChannels;
        private Int32 _sampleRate;
        private Int32 _byteRate;
        private Int16 _blockAlign;
        private Int16 _bitsPerSample;
                
        //SUBCHUNK2
        private Int32 _subchunk2Id;
        private Int32 _subchunk2Size;
        private byte[] _data;

        public void addRiffHeader(int chunkId, int chunkSize, int format)
        {
            _chunkID = chunkId;
            _chunkSize = chunkSize;
            _format = format;
            
        }
        public void addSubchunk1(int subchunk1Id, int subchunk1Size, int audioFormat, short numChannels, int sampleRate, int byteRate, short blockAlign, short bitsPerSample)
        {
            _subchunk1Id = subchunk1Id;
            _subchunk1Size = subchunk1Size;
            _audioFormat = audioFormat;
            _numChannels = numChannels;
            _sampleRate = sampleRate;
            _byteRate = byteRate;
            _blockAlign = blockAlign;
            _bitsPerSample = bitsPerSample;
        }
        public void addSubchunk2(int subchunk2Id, int subchunk2Size, byte[] data)
        {
            _subchunk2Id = subchunk2Id;
            _subchunk2Size = subchunk2Size;
            _data = data;
        }

        public void Write()
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(@"../../../../Audio/6.wav", FileMode.OpenOrCreate)))
            {
                bw.Write(_chunkID);
                 _chunkSize = 4 + (8 + _subchunk1Size) + (8 + _subchunk2Size * 2);
                bw.Write(_chunkSize);
                // bw.Write(_format);
                // Console.WriteLine(_numChannels);
            }
        }
    }
}