using System;
using UnityEngine;

public class PitchDetector
{
    private int _minLag;
    private int _maxLag;
    private int _sampleLookbackNumber;
    
    private AudioClip _audioClip;

    public PitchDetector(float minPitch, float maxPitch, float lookbackTimeToCalculatePitch, AudioClip clip)
    {
        _audioClip = clip;
        _sampleLookbackNumber = (int)( lookbackTimeToCalculatePitch * AudioSettings.outputSampleRate);
        
        _minLag = (int) (AudioSettings.outputSampleRate / maxPitch);
        _maxLag = (int) (AudioSettings.outputSampleRate / minPitch);
    }

    public float GetPitchAtSamplePosition(int currentSamplePositionInClip)
    {
        int startingPositionInSamples = currentSamplePositionInClip - _sampleLookbackNumber;

        if (startingPositionInSamples < 0)
        {
            throw new ArgumentException("not enough sample data to find the pitch");
        }

        float[] samples = new float[_sampleLookbackNumber];
        _audioClip.GetData(samples, startingPositionInSamples);

        return GetPitch(samples);
    }

    private float GetPitch(float[] data)
    {
        float maxAutoCorr = 0;
        int optimalLag = 1;
        
        for (int lag = _minLag; lag < _maxLag; lag++)
        {
            float autoCorr = 0f;
            for (int i = 0; i < data.Length - lag; i++)
            {
                autoCorr += (data[i] * data[i + lag]);
            }

            if (autoCorr > maxAutoCorr)
            {
                maxAutoCorr = autoCorr;
                optimalLag = lag;
            }
        }
        
        if (maxAutoCorr > 4f)
        {
            return (float) AudioSettings.outputSampleRate / (float) optimalLag;
        }
        
        throw new ArgumentException("No clear pitch detected");
    }
}