using System;

namespace WaveEditor.Resize.Lib
{
    public class AudioProcessor
    {
        private readonly UInt16 _numChannels;
        private readonly int _bytePerSample;
        private readonly byte[] _data;
        private readonly double _scale;
        private byte[][] _channels;
        private byte[][] _changedCh;
        private double _loop;
        public AudioProcessor(UInt16 numChannels, UInt16 bitsPerSample, byte[] data, double scale, double loop)
        {
            _numChannels = numChannels;
            _bytePerSample = bitsPerSample / 8;
            _data = data;
            _scale = scale;
            _loop = loop;
        }

        public byte[] ScaleTrack()
        {
            Console.Write("Scaling sound file...");
            DataToChannels();

            for (int i = 0; i < _numChannels; i++)
            {
                ScaleChannel(i);
            }
            LoopTrack();
            byte[] newData = new byte[_changedCh[0].Length * _numChannels];
            for (int i = 0; i < _changedCh[0].Length / _bytePerSample; i++)
            {
                for (int j = 0; j < _numChannels; j++)
                {
                    for (int k = 0; k < _bytePerSample; k++)
                    {
                        newData[i * _numChannels * _bytePerSample + j * _numChannels + k] =
                            _changedCh[j][i * _bytePerSample + k];
                    }
                }
            }

            Console.WriteLine("Done!");
            return newData;
        }

        private void LoopTrack()
        {
            for (int i = 0; i < _changedCh.Length; i++)
            {
                long width = (int)(_changedCh[i].Length/_bytePerSample*_loop)*_bytePerSample ;
                byte[] channel = new byte[(int)(_changedCh[i].Length/_bytePerSample*(_loop))*_bytePerSample];
                for (int j = 0; j < Math.Floor(_loop); j++)
                {
                    Array.Copy(_changedCh[i],0, channel, j*_changedCh[i].Length, _changedCh[i].Length);
                }
               Array.Copy(_changedCh[i],0,channel, (int)Math.Floor(_loop)*_changedCh[i].Length ,(int)(_changedCh[i].Length/_bytePerSample*(_loop%1))*_bytePerSample);
                _changedCh[i] = (channel);
            }
        }
        

        private void DataToChannels()
        {
            _channels = new byte[_numChannels][];
            _changedCh = new byte[_numChannels][];
            int oneChLen = _data.Length / _numChannels;


            for (int i = 0; i < _numChannels; i++)
            {
                _channels[i] = new byte[oneChLen];
            }

            for (int i = 0; i < _data.Length / _bytePerSample / _numChannels; i++)
            {
                for (int k = 0; k < _numChannels; k++)
                {
                    for (int j = 0; j < _bytePerSample; j++)
                    {
                        _channels[k][i * _bytePerSample + j] =
                            _data[i * _numChannels * _bytePerSample + k * _bytePerSample + j];
                    }
                }
            }
        }

        private void ScaleChannel(int channel)
        {
            int inputSamples = _channels[channel].Length / _bytePerSample;
            int outputSamples = (int) (_scale * inputSamples);

            _changedCh[channel] = new byte[outputSamples * _bytePerSample];

            for (int i = 0; i < _changedCh[channel].Length - _bytePerSample; i += _bytePerSample)
            {
                double placeInInput = Interpolate(0, inputSamples - 1, 0, outputSamples - 1, i / _bytePerSample);

                int x0 = Convert.ToInt32(Math.Floor(placeInInput));
                int x1 = x0 + 1;

                byte[] currentSample = new byte[_bytePerSample];

                for (int k = 0; k < _bytePerSample; k++)
                {
                    currentSample[k] = Convert.ToByte(Interpolate(
                        _channels[channel][x0 * _bytePerSample + k],
                        _channels[channel][x1 * _bytePerSample + k], x0, x1,
                        placeInInput));
                }

                Array.Copy(currentSample, 0, _changedCh[channel], i, _bytePerSample);
            }
        }

        private double Interpolate(double y0, double y1, double x0, double x1, double x)
        {
            return (int)(y0 + (y1 - y0) * (x - x0) / (x1 - x0));
        }
    }
}