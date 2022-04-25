using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer musicMixer;

    private Slider slider;
    private string parameterName = "MasterVolume";

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { OpdaterVolume(); });
    }
    
    void OpdaterVolume()
    {
        musicMixer.SetFloat(parameterName, Mathf.Lerp(-80f, 0f, Mathf.Log10(slider.value) + 1));
    }
}