using UnityEngine.UI;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
   [SerializeField] VolumeSliderAudioSource audioSource;

    AudioController _audioController;
    Slider _slider;
   
    private void Start()
    {
        _audioController = GameObject.FindGameObjectWithTag("Core").GetComponent<AudioController>();
        _slider = GetComponent<Slider>();
        _slider.value = PlayerPrefs.GetFloat(audioSource.ToString());
        _slider.onValueChanged.AddListener(val => _audioController.ChangeVolume(val, audioSource));

    }
}
