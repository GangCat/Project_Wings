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

    public virtual void PlayAudio<T>(T audioEnum) where T : Enum
    {
        audioIdx = Convert.ToInt32(audioEnum);

        if (audioIdx >= 0 && audioIdx < myAudioClips.Length)
        {
            Debug.Log(audioEnum.GetType());
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
    private AudioClip[] myAudioClips;

    private AudioSource audioSource;
    private int audioIdx = 0;
}
