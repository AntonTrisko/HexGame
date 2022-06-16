using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public string musicKey;
    public string effectsKey;
    public string muteKey;
    private bool _isMuted;
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private AudioSource _effectsSource;
    [SerializeField]
    private List<AudioClip> _audioClips;
    [SerializeField]
    private List<AudioClip> _musicList;

    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        SetAudio();
        DontDestroyOnLoad(this);
    }

    private void SetAudio()
    {
        _audioMixer.SetFloat(musicKey, GetSavedVolume(musicKey));
        _audioMixer.SetFloat(effectsKey, GetSavedVolume(effectsKey));
        _isMuted = Convert.ToBoolean(PlayerPrefs.GetInt(muteKey, 0));
        CheckForMute();
    }

    public float GetSavedVolume(string key)
    {
        float sound = 0;
        if (PlayerPrefs.HasKey(key))
        {
            sound = PlayerPrefs.GetFloat(key, 0);
        }
        return sound;
    }

    public void SetVolume(string key,float value)
    {
        Debug.LogError(value);
        _audioMixer.SetFloat(key, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(key, Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }

    public void SetMuteState(bool isMuted)
    {
        _isMuted = isMuted;
        PlayerPrefs.SetInt(muteKey, Convert.ToInt32(isMuted));
        PlayerPrefs.Save();
        CheckForMute();
    }

    private void CheckForMute()
    {
        if (_isMuted)
        {
            _audioMixer.SetFloat(musicKey, -80);
            _audioMixer.SetFloat(effectsKey, -80);
        }
        else
        {
            _audioMixer.SetFloat(musicKey, GetSavedVolume(musicKey));
            _audioMixer.SetFloat(effectsKey, GetSavedVolume(effectsKey));
        }
    }

    public bool IsMuted()
    {
        return _isMuted;
    }

    public void PlaySound(Sounds sound)
    {
        _effectsSource.PlayOneShot(_audioClips[(int)sound]);
    }

    public void PlayMusic(Music music)
    {
        _musicSource.clip = _musicList[(int)music];
        _musicSource.Play();
    }

    public AudioClip GetAudioClip(Sounds sound)
    {
        return _audioClips[(int)sound];
    }
}

public enum Sounds
{
    Button,
    Slider,
    CloseWindow,
    OpenWindow,
    BuyCell,
    Build,
    Upgrade,
    Hire,
    Castle,
    Farm,
    Lambermill,
    Mine,
    Smith,
    NextTurn,
    Victory
}

public enum Music
{
    MenuMusic,
    GameMusic
}