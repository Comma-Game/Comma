using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider sfxSlider;
    public Slider bgmSlider;

    public void SFXAudioControl()
    {
        float sound = sfxSlider.value;
        if(sound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound);
    }

    public void BGMAudioControl()
    {
        float sound = bgmSlider.value;
        if(sound == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", sound);
    }

    // public void ToogleAudioVolume()
    // {
    //     AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    // }
}
