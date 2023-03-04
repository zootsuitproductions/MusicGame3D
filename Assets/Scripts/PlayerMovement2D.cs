using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private AudioSource reducedNoiseAudioSource;
    private string _clipName; //used to check when the clip data is updated with new microphone input
    
    private float _minPitch = 130f;
    private float _maxPitch = 260f;
    private int _pitchShift = 0;

    private float _holdNoteTimeRequired = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _clipName = reducedNoiseAudioSource.clip.name;
    }

    public void SetPitchShift(int shift)
    {
        _pitchShift = shift;
    }
    
    
    
    public void SetPitchRange(int min, int max)
    {
        _minPitch = Note.MidiNoteToHertz(min);
        _maxPitch = Note.MidiNoteToHertz(max);
    }

    //Check if the output clip of the noise reduction mic has been updated
    // (the name changes whenever it is updated)
    private bool ClipUpdated()
    {
        return reducedNoiseAudioSource.clip.name != _clipName;
    }

    void Update()
    {
        if (ClipUpdated())
        {
            _clipName = reducedNoiseAudioSource.clip.name;
            float pitch = ClipPitchDetector.GetPitch(reducedNoiseAudioSource.clip, _minPitch, _maxPitch);
            float x = Note.HertzToMidiNoteValue(pitch) - _pitchShift;
            
            if (pitch != -1)
                transform.localPosition = new Vector3(x, transform.position.y, 0);
        }
    }
}
