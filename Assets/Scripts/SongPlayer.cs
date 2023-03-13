using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SongPlayer : MonoBehaviour
{
    [SerializeField] private float volume = 1f;
    private AudioSource _audioSource;

    private const float FADE_OUT_TIME = 0.1f;
    
    public void LoadSong(AudioClip clip)
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = volume;
        // Debug.Log("web " + webFilePath);
        _audioSource.clip = clip;
            // Resources.Load<AudioClip>("Audio/bach");
        
        _audioSource.Play();
    }

    public float GetPlaybackPosition()
    {
        return _audioSource.time;
    }
    
    public void TurnVolumeUp()
    {
        StartCoroutine(StartFade(_audioSource, FADE_OUT_TIME, volume));
    }
    
    public void TurnVolumeOff()
    {
        StartCoroutine(StartFade(_audioSource, FADE_OUT_TIME, 0));
    }
    
    private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
