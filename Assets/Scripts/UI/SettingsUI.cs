using DG.Tweening;
using Dino.Utility.Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using AudioType = Dino.Utility.Audio.AudioType;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sFXSlider;

    public override void Initialize()
    {
        base.Initialize();
        _musicSlider.onValueChanged.AddListener(OnVolumeMusicChanged);
        _sFXSlider.onValueChanged.AddListener(OnVolumeSFXChanged);
    }

    private void OnVolumeMusicChanged(float volume)
    {
        Debug.Log($"Volume changed to: {volume}");
        AudioManager.Instance.UpdateAudioMixerGroupVolume(volume, AudioType.Music);
        // Here you can add code to actually change the game's volume
    }

    private void OnVolumeSFXChanged(float value)
    {
        Debug.Log($"Volume changed to: {value}");
        AudioManager.Instance.UpdateAudioMixerGroupVolume(value, AudioType.SFX);
        // Here you can add code to actually change the game's volume
    }
}