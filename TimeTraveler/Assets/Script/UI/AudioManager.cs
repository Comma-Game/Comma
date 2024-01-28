using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioMixer masterMixer;
    [SerializeField] public Slider sfxSlider;
    [SerializeField] public Slider bgmSlider;

    [Header("BGM")]
    [SerializeField] public AudioSource MainSceneBGM;

    [Header("SFX")]
    [SerializeField] public AudioSource MesteryBoxBuySFX;
    [SerializeField] public AudioSource GameButtonClick;
    [SerializeField] public AudioSource MainScenePortal;

    private static AudioManager instance;
    // singleton
    public static AudioManager Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

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

    /////////////////////////////////////////////////////////////////////////////
    //// BGM

    /////////////////////////////////////////////////////////////////////////////
    //// SFX

    public void PlayMesteryBoxBuySFX(){
        MesteryBoxBuySFX.Play();
    }

    public void PlayGameButtonClick(){
        GameButtonClick.Play();
    }

    public void PlayMainScenePortal(){
        MainScenePortal.Play();
    }
}
