using System;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class AudioProcessor
    {
        private UInt16 _numChannels;
        private UInt16 _bitsPerSample;
        private byte[] _data;

        public AudioProcessor(ushort numChannels, ushort bitsPerSample, byte[] data)
        {
            _numChannels = numChannels;
            _bitsPerSample = bitsPerSample;
            _data = data;
        }
        
        
        
    }
}