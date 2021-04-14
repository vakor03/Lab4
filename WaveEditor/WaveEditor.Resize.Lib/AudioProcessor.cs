﻿using System;
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
            DataToChannels();

            for (int i = 0; i < _numChannels; i++)
            {
                ScaleChannel(i);
            }

            byte[] newData = new byte[changedCh[0].Length * _numChannels];
            for (int i = 0; i < changedCh[0].Length / _bytePerSample; i++)
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

        private void DataToChannels()
        {
            channels = new byte[_numChannels][];
            changedCh = new byte[_numChannels][];
            int oneChLen = _data.Length / _numChannels;


            for (int i = 0; i < _numChannels; i++)
            {
                channels[i] = new byte[oneChLen];
            }

            for (int i = 0; i < _data.Length / _bytePerSample / _numChannels; i++)
            {
                for (int k = 0; k < _numChannels; k++)
                {
                    for (int j = 0; j < _bytePerSample; j++)
                    {
                        channels[k][i * _bytePerSample + j] =
                            _data[i * _numChannels * _bytePerSample + k * _bytePerSample + j];
                    }
                }
            }
        }
        private void ScaleChannel(int channel)
        {
            int inputSamples = channels[channel].Length / _bytePerSample;
            int outputSamples = (int) (_scale * inputSamples);

            changedCh[channel] = new byte[outputSamples * _bytePerSample];

            for (int i = 0; i < changedCh[channel].Length - _bytePerSample; i += _bytePerSample)
            {
                double placeInInput = Interpolate(0, inputSamples - 1, 0, outputSamples - 1, i / _bytePerSample);

                int x0 = Convert.ToInt32(Math.Floor(placeInInput));
                int x1 = x0 + 1;

                byte[] currentSample = new byte[_bytePerSample];

                for (int k = 0; k < _bytePerSample; k++)
                {
                    currentSample[k] = Convert.ToByte(Interpolate(
                        channels[channel][x0 * _bytePerSample + k],
                        channels[channel][x0 * _bytePerSample + k], x0, x1,
                        placeInInput));
                }
                Array.Copy(currentSample, 0, changedCh[channel], i, _bytePerSample);
            }
        }

        private double Interpolate(double y0, double y1, double x0, double x1, double x)
        {
            return (int) (y0 + (y1 - y0) * (x - x0) / (x1 - x0));
        }
    }
}