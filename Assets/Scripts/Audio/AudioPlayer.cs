using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public virtual void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void PlayAudio(Enum _audioEnum)
    {
        audioIdx = Convert.ToInt32(_audioEnum);

        if (audioIdx >= 0 && audioIdx < myAudioClips.Length)
        {
            Debug.Log(_audioEnum.GetType());
            Debug.Log(audioIdx);
            //audioSource.clip = myAudioClips[audioIndex];
            //audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid player audio index");
        }
    }


    // EPlayerAudio Enum에 대응하는 오디오 클립을 저장할 배열
    [SerializeField]
    protected AudioClip[] myAudioClips;

    protected AudioSource audioSource;
    protected int audioIdx = 0;
}
