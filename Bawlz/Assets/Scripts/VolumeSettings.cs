using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    public float masterVolume;
    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        float dBVolume = Mathf.Log10(masterVolume) * 20;
        if(dBVolume <= -80f) 
        {
            dBVolume = -80f;
        }
        audioMixer.SetFloat("MasterVolume", dBVolume);
    }


}
