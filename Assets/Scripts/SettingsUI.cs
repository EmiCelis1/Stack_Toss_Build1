using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{
    [Header("SettingsUI")]
    [SerializeField] private Slider _volumeSlider;


    public override void Initialize()
    {
        base.Initialize();
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

    }

    private void OnVolumeChanged(float value)
    {
        Debug.Log($"Volume changed to: {value}");
        // Here you can add code to actually change the game's volume
    }
}