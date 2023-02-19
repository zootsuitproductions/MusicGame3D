using System.Collections;
using UnityEngine;
public static class FadeAudioSource {
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
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
    
    public static IEnumerator FadeOutThenPlayNote(AudioSource audioSource, float duration, MidiPlayer midiPlayer, Note note)
    {
        
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }
        audioSource.Pause();
        note.Play(midiPlayer);
        yield break;
    }
    
    public static IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 1f, currentTime / duration);
            yield return null;
        }
        audioSource.UnPause();
        yield break;
    }
}