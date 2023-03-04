using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClipPitchDetector : MonoBehaviour
{
    //Get the pitch at the specific sample position in the clip.
    public static float GetPitch(AudioClip clip, float minPitch, float maxPitch)
    {

        float[] data = new float[clip.samples];
        clip.GetData(data, 0);
    
        int minLag = (int) (AudioSettings.outputSampleRate / maxPitch);
        int maxLag = (int) (AudioSettings.outputSampleRate / minPitch);

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

        if (maxAutoCorr > 0.1f)
        {
            return (float) (AudioSettings.outputSampleRate) / (float) optimalLag;
        }

        return -1f;
    }
}

