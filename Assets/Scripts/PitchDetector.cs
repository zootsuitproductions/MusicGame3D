using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//refactor this so that it only detects for the current pitch of the midi, and returns a score on how well they hit the note
public class PitchDetector
{
    private int _minLag;
    private int _maxLag;
    
    private float _minPitch;
    private float _maxPitch;

    private float _requiredNoteHeldTime;
    private int _requiredTimesDetected;
    private float _requiredVolume = 0.05f;

    private int _sampleLookbackNumber;
    
    private AudioClip _audioClip;

    private float[] _pitchOptions = new float[13];
    private float[] _logPitchOptions = new float[13];


    public PitchDetector(float minPitch, float maxPitch, float pitchDetectionRate, float lookbackTimeToCalculatePitch, float requiredNoteHeldTime, float sameNotePercentage, AudioClip clip)
    {
        _pitchOptions = new[]
        {
            130.81f, 138.59f, 146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.65f, 220.00f, 233.08f, 246.94f,
            261.63f
        };

        for (int i = 0; i < _pitchOptions.Length; i++)
        {
            _logPitchOptions[i] = Mathf.Log(_pitchOptions[i], 2);
        }

        _audioClip = clip;

        _maxPitch = maxPitch;
        _minPitch = minPitch;
        _minLag = (int) (AudioSettings.outputSampleRate / maxPitch);
        _maxLag = (int) (AudioSettings.outputSampleRate / minPitch);
        
        _requiredTimesDetected = (int)(requiredNoteHeldTime / pitchDetectionRate);
        _sampleLookbackNumber = (int)( lookbackTimeToCalculatePitch * AudioSettings.outputSampleRate);
        
    }
    public float GetNoInputVolumeLoudness(float time)
    {
        float[] samples = new float[(int) (AudioSettings.outputSampleRate * time)];
        _audioClip.GetData(samples, 0);

        float maxVolume = 0f;
        
        for (int i = 0; i < samples.Length; i++)
        {
            maxVolume = Mathf.Max(samples[i], maxVolume);
        }

        _requiredVolume = maxVolume * 2f;
        return maxVolume;
    }

    private List<float> previousPitchLogs = new List<float>();

    // return an array of sample data if there is enough data 
    private float[] GetSamplesIfEnoughData(int position)
    {
        int startingPositionInSamples = position - _sampleLookbackNumber;

        if (startingPositionInSamples < 0)
        {
            throw new ArgumentException("Not enough sample data to find the pitch.");
        }

        float[] samples = new float[_sampleLookbackNumber];
        _audioClip.GetData(samples, startingPositionInSamples);
        return samples;
    }

    // calculate the average note log over the time window
    private float LookbackTimeAveragePitchLog()
    {
        float LogSum = 0;

        for (int i = 0; i < previousPitchLogs.Count; i++)
        {
            LogSum += previousPitchLogs[i];
        }
        
        return LogSum / previousPitchLogs.Count;
    }

    // Returns true if all of the pitch values read in the required look-back time window
    // are within a semitone range of a given note, supplied as the log of the frequency
    private bool AllSampledPitchesAreWithinNoteRange(float noteLog)
    {
        for (int j = 0; j < previousPitchLogs.Count; j++)
        {
            // check if the note is outside half a semitone of either side of the exact pitch
            if (Mathf.Abs(previousPitchLogs[j] - noteLog) > 0.04166666666f)
            {
                return false;
            }
        }
        return true;
    }

    // add the log of the current pitch to the list of pitch samples within the look-back time,
    // shaving off the oldest pitch if it is no longer within the look-back window. 
    private void AddNoteToLookbackWindow(float currentPitch)
    {
        previousPitchLogs.Add(Mathf.Log(currentPitch, 2));
        
        if (previousPitchLogs.Count > _requiredTimesDetected)
        {
            previousPitchLogs.RemoveAt(0);
        }
    }

    //Get the pitch at the specific sample position in the clip.
    public float GetPitchAtSamplePosition(int currentSamplePositionInClip)
    {
        float[] samples = GetSamplesIfEnoughData(currentSamplePositionInClip);
        float currentPitch = GetPitch(samples, _minPitch, _maxPitch);
        
        AddNoteToLookbackWindow(currentPitch);

        if (previousPitchLogs.Count >= _requiredTimesDetected)
        {
            float averagePitchlog = LookbackTimeAveragePitchLog();
            
            for (int i = 0; i < _logPitchOptions.Length; i++)
            {
                float logPitchOption = _logPitchOptions[i];
                
                if (Mathf.Abs(averagePitchlog - logPitchOption) < 0.025f)
                {
                    if (AllSampledPitchesAreWithinNoteRange(logPitchOption))
                    {
                        return _pitchOptions[i];
                    }
                }
            }
            previousPitchLogs.Clear();
        }
        
        throw new ArgumentException("No clear pitch detected.");
    }

    private bool IsLoudEnough(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] >= _requiredVolume)
            {
                return true;
            }
        }
        return false;
    }

    private float[] AddIntermediarySamples(float[] samples)
    {
        float[] doubled = new float[(samples.Length * 2) - 1];

        for (int i = 0; i < samples.Length - 1; i++)
        {
            int doubledIndex = i * 2;
            doubled[doubledIndex] = samples[i];
            doubled[doubledIndex + 1] = (samples[i] + samples[i + 1]) / 2;
        }

        doubled[doubled.Length - 1] = samples[samples.Length - 1];
        return doubled;
    }

    
    private float GetPitch(float[] data, float minPitch, float maxPitch)
    {   
        // if (!IsLoudEnough(data))
        // {
        //     previousPitchLogs.Clear();
        //     throw new ArgumentException("Not loud enough.");
        // }
        
        // data = AddIntermediarySamples(AddIntermediarySamples(data)); //2 times sample rate
        int sampleExtensionFactor = 1;
        
        int minLag = (int) (sampleExtensionFactor * AudioSettings.outputSampleRate / maxPitch);
        int maxLag = (int) (sampleExtensionFactor * AudioSettings.outputSampleRate / minPitch);

        float maxAutoCorr = 0f;
        int optimalLag = 1;

        for (int lag = minLag; lag < maxLag; lag++)
        {
            float autoCorr = 0f;
            
            for (int i = 0; i < data.Length - lag; i++)
            {
                autoCorr += data[i] * data[i + lag];
            }
            
            if (autoCorr > maxAutoCorr)
            {
                maxAutoCorr = autoCorr;
                optimalLag = lag;
            }
        }

        if (maxAutoCorr > 0.5f)
        {
            return (float) (sampleExtensionFactor * AudioSettings.outputSampleRate) / (float) optimalLag;
        }

        throw new ArgumentException("No clear pitch detected.");


    }
}