using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField]
    private AudioClip[] soundClips;
    [SerializeField, Range(0, 100)]
    private float[] volumes;
    [SerializeField, Range(2,10000)]
    private float[] maxDistance;
    public enum ESounds
    {
        NONE = -1,
        CANNONLAUNCHSOUND,
        CANNONPASSINGSOUND,
        CANNONEXPOSIONSOUND,
        CANNONUNDERWATEREXPLOIONSOUND
    }

    private AudioSource audioSource;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
                //instance = FindAnyObjectByType<SoundManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {

        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }

    }

    public float Volume { get; set; }

    public void Init(GameObject _other)
    {
        AudioSource representAudioSource = GetComponent<AudioSource>();
        if (_other.GetComponent<AudioSource>() == null)
        {
            audioSource = _other.AddComponent<AudioSource>();

        }
        else
        {
            audioSource = _other.GetComponent<AudioSource>();
        }
        audioSource.rolloffMode = representAudioSource.rolloffMode;

    }


    public void PlayAudio(AudioSource _otherAudioSource, int _audioIdx, bool _isLoop = false, bool _canFeelDistance = true)
    {
        if (_canFeelDistance) _otherAudioSource.spatialBlend = 1;
        else _otherAudioSource.spatialBlend = 0;
        _otherAudioSource.Stop(); // 나중에 차이함 보기
        _otherAudioSource.maxDistance = maxDistance[_audioIdx];
        _otherAudioSource.clip = soundClips[_audioIdx];
        _otherAudioSource.volume = Mathf.InverseLerp(0, 100, volumes[_audioIdx]);
        _otherAudioSource.loop = _isLoop;
        _otherAudioSource.Play();
    }
    public void StopAllAudio(AudioSource _otherAudioSource)
    {
        _otherAudioSource.Stop();
    }
    public bool IsPlaying(AudioSource _otherAudioSource)
    {
        return _otherAudioSource.isPlaying;
    }
}