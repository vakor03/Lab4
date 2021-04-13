using System;
using System.IO;

namespace WaveEditor.Resize.Lib
{
    public class AudioProcessor
    {
        private Int16 _numChannels;
        private int _bytePerSample;
        private byte[] _data;
        private double _scale;
        private byte[][] channels;
        private byte[][] changedCh;

        public AudioProcessor(short numChannels, short bitsPerSample, byte[] data, double scale)
        {
            _numChannels = numChannels;
            _bytePerSample = bitsPerSample / 8;
            _data = data;
            _scale = scale;
        }

        public byte[] ScaleTrack()
        {
            channels = new byte[_numChannels][];
            changedCh = new byte[_numChannels][];
            int oneChLen = _data.Length / _numChannels;


            for (int i = 0; i < _numChannels; i++)
            {
                channels[i] = new byte[oneChLen];
                changedCh[i] = new byte[Convert.ToInt32(oneChLen * _scale)];
            }

            for (int i = 0; i < _data.Length / _bytePerSample / _numChannels; i++)
            {
                for (int k = 0; k < _numChannels; k++)
                {
                    for (int j = 0; j < _bytePerSample; j++)
                    {
                        channels[k][i*_bytePerSample + j] = _data[i*_numChannels*_bytePerSample+k*_bytePerSample+j];
                    }
                }
            }

            double step = 1 / _scale;
            for (double i = 0; i < oneChLen / _bytePerSample; i += step)
            {
                if (i % 1 == 0)
                {
                    for (int j = 0; j < _numChannels; j++)
                    {
                        for (int k = 0; k < _bytePerSample; k++)
                        {
                            changedCh[j][Convert.ToInt32(i/step)*_bytePerSample + k] = channels[j][Convert.ToInt32(i)*_bytePerSample + k];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < _numChannels; j++)
                    {
                        for (int k = 0; k < _bytePerSample; k++)
                        {
                            changedCh[j][Convert.ToInt32(i / step)*_bytePerSample + k] = Interpolate(i, j, k);
                        }
                    }
                }
            }

            byte[] newData = new byte[Convert.ToInt32(oneChLen * _scale * _numChannels)];
            for (int i = 0; i < oneChLen * _scale / _bytePerSample; i++)
            {
                for (int j = 0; j < _numChannels; j++)
                {
                    for (int k = 0; k < _bytePerSample; k++)
                    {
                        newData[i * _numChannels * _bytePerSample + j * _numChannels + k] =
                            changedCh[j][i * _bytePerSample + k];
                    }
                }
            }


            return newData;
        }

        private byte Interpolate(double x, int channel, int currByte)
        {
            int x0 = Convert.ToInt32(Math.Floor(x));
            int x1 = Convert.ToInt32(Math.Ceiling(x));
            double y0 = Convert.ToDouble(channels[channel][x0 + currByte]);
            double y1 = Convert.ToDouble(channels[channel][x1 + currByte]);
            double result = y0 + (x - x0) * ((y1 - y0) / (x1 - x0));
            return Convert.ToByte(result);
        }
    }
}