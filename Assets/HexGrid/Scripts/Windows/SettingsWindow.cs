using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : BasicWindow
{
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _effectsSlider;
    [SerializeField]
    private Toggle _muteToggle;

    public override void Start()
    {
        base.Start();
        GetSettings();
        AddListeners();
    }

    public override void OpenWindow()
    {
        transform.localScale = Vector3.zero;
        base.OpenWindow();
        transform.DOScale(Vector3.one, 0.5f);
    }

    private void GetSettings()
    {
        _musicSlider.value = GetSliderValue(AudioManager.Instance.GetSavedVolume(AudioManager.Instance.musicKey));
        _effectsSlider.value = GetSliderValue(AudioManager.Instance.GetSavedVolume(AudioManager.Instance.effectsKey));
        _muteToggle.isOn = AudioManager.Instance.IsMuted();
    }

    private float GetSliderValue(float value)
    {
        return Mathf.Pow(10, value / 20);
    }

    private void AddListeners()
    {
        _musicSlider.onValueChanged.AddListener(delegate { MusicVolumeChanged(); });
        _effectsSlider.onValueChanged.AddListener(delegate { EffectsVolumeChanged(); });
        _muteToggle.onValueChanged.AddListener(Mute);
    }

    private void MusicVolumeChanged()
    {
        AudioManager.Instance.SetVolume(AudioManager.Instance.musicKey, _musicSlider.value);
        AudioManager.Instance.PlaySound(Sounds.Slider);
    }

    private void EffectsVolumeChanged()
    {
        AudioManager.Instance.SetVolume(AudioManager.Instance.effectsKey, _effectsSlider.value);
        AudioManager.Instance.PlaySound(Sounds.Slider);
    }

    private void Mute(bool isMute)
    {
        AudioManager.Instance.SetMuteState(isMute);
    }
}
