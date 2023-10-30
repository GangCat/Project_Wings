using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EAudioPlayer
{
    NONE = -1,
    PLAYER_AUDIO,
    BACKGROUND_AUDIO
}

public enum EPlayerAudio
{
    NONE = -1,
    FIRST_AUDIO,
    SECOND_AUDIO,
    THIRD_AUDIO
}

public enum EBackgroundAudio
{
    NONE = -1,
    BACKGROUND_AUDIO,
    BACKGROUND_AMBIENCE,
    BACKGROUND_EFFECT
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
                PlayPlayerAudio(EPlayerAudio.SECOND_AUDIO);
            });
    }

    public void PlayPlayerAudio(EPlayerAudio _playerAudio)
    {
        arrAudioPlayer[(int)EAudioPlayer.PLAYER_AUDIO].PlayAudio(_playerAudio);
    }

    public void PlayBackgroundAudio(EBackgroundAudio _backgroundAudio)
    {
        arrAudioPlayer[(int)EAudioPlayer.BACKGROUND_AUDIO].PlayAudio(_backgroundAudio);
    }

    [SerializeField]
    private AudioPlayer[] arrAudioPlayer = null;

    [SerializeField]
    private Button[] buttons = null;
}
