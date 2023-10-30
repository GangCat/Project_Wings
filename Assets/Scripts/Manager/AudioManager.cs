using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EAudioPlayer
{
    NONE = -1,
    PLAYER_AUDIO,
    BACKGROUND_AUDIO,
    EFFECT_AUDIO
}

public enum EPlayerAudio
{
    NONE = -1,
    PLAYER_FIRST,
    PLAYER_SECOND,
    PLAYER_THIRD
}

public enum EBackgroundAudio
{
    NONE = -1,
    BACKGROUND_AUDIO,
    BACKGROUND_AMBIENCE,
    BACKGROUND_EFFECT
}

public enum EEffectAudio
{
    NONE = -1,
    EFFECT_FIRST,
    EFFECT_SECOND,
    EFFECT_THIRD
}

public class AudioManager : MonoBehaviour
{
    public void Init()
    {
        DontDestroyOnLoad(this);

        buttons[0].onClick.AddListener(
            () =>
            {
                PlayBackgroundAudio(EBackgroundAudio.BACKGROUND_EFFECT);
            });

        buttons[1].onClick.AddListener(
            () =>
            {
                PlayPlayerAudio(EPlayerAudio.PLAYER_SECOND);
            });

        buttons[2].onClick.AddListener(
            () =>
            {
                PlayEffectAudio(EEffectAudio.EFFECT_THIRD);
            });

        foreach (AudioPlayer AP in arrAudioPlayer)
            AP.Init();
    }

    public void PlayPlayerAudio(EPlayerAudio _playerAudio)
    {
        arrAudioPlayer[(int)EAudioPlayer.PLAYER_AUDIO].PlayAudio(_playerAudio);
    }

    public void PlayBackgroundAudio(EBackgroundAudio _backgroundAudio)
    {
        arrAudioPlayer[(int)EAudioPlayer.BACKGROUND_AUDIO].PlayAudio(_backgroundAudio);
    }

    public void PlayEffectAudio(EEffectAudio _effectAudio)
    {
        arrAudioPlayer[(int)EAudioPlayer.EFFECT_AUDIO].PlayAudio(_effectAudio);
    }

    [SerializeField]
    private AudioPlayer[] arrAudioPlayer = null;

    [SerializeField]
    private Button[] buttons = null;
}
