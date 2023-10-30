using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EAudioPlayer
{
    NONE = -1,
    PLAYER_AUDIO,
    BACKGROUND_AUDIO,
    EFFECT_AUDIO,
    LENGTH
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
        #region TestAudioManager
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

        buttons[3].onClick.AddListener(
            () =>
            {
                SetBGMVolume(0.8f);
            });

        buttons[4].onClick.AddListener(
            () =>
            {
                SetBGMVolume(0.2f);
            });

        buttons[5].onClick.AddListener(
            () =>
            {
                SetEffectVolume(0.8f);
            });

        buttons[6].onClick.AddListener(
            () =>
            {
                SetEffectVolume(0.2f);
            });

        sliders[0].onValueChanged.AddListener(
            (float _value) =>
            {
                SetBGMVolume(_value * 0.1f);
            });

        sliders[1].onValueChanged.AddListener(
            (float _value) =>
            {
                SetEffectVolume(_value * 0.1f);
            });
        #endregion
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

    public void SetBGMVolume(float _bgmVolume)
    {
        bgmVolume = _bgmVolume;
        ApplyBGMVolume();
    }

    private void ApplyBGMVolume()
    {
        arrAudioPlayer[(int)EAudioPlayer.BACKGROUND_AUDIO].SetVolume(bgmVolume);
    }

    public void SetEffectVolume(float _effectVolume)
    {
        effectVolume = _effectVolume;
        ApplyEffectVolume();
    }

    private void ApplyEffectVolume()
    {
        for(int i = 0; i < (int)EAudioPlayer.LENGTH; ++i)
        {
            if (i.Equals((int)EAudioPlayer.BACKGROUND_AUDIO)) continue;

            arrAudioPlayer[i].SetVolume(effectVolume);
        }
    }

    [SerializeField]
    private AudioPlayer[] arrAudioPlayer = null;
    [SerializeField]
    private Button[] buttons = null;
    [SerializeField]
    private Slider[] sliders = null;

    private float bgmVolume = 1f;
    private float effectVolume = 1f;
}
