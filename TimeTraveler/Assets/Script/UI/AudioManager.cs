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
    [SerializeField] public AudioSource GameSceneBGM;

    [Header("SFX")]
    [SerializeField] public AudioSource MesteryBoxBuySFX;
    [SerializeField] public AudioSource GameButtonClick;
    [SerializeField] public AudioSource GameButtonNoClick;
    [SerializeField] public AudioSource PortalSFX;
    [SerializeField] public AudioSource ItemUpgradeSFX;
    [SerializeField] public AudioSource GameOverSFX;
    [SerializeField] public AudioSource DamgeSFX;
    [SerializeField] public AudioSource SkillSFX;
    [SerializeField] public AudioSource StoryUnLockSFX;
    [SerializeField] public AudioSource GetPieceSFX;

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
    
    public void Start(){
        float sfxSound = SaveLoadManager.Instance.GetSfxSound();
        float bgmSound = SaveLoadManager.Instance.GetBgmSound();
        //Debug.Log("sfxSound : "+ sfxSound + " bgmSound : " + bgmSound);
        SettingSounds(sfxSound, bgmSound);
    }

    private void SettingSounds(float sfxSound, float bgmSound){
        if(sfxSound > 0) sfxSound = 0;
        if(sfxSound < -40) sfxSound = -40f;
        if(sfxSound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sfxSound);
        sfxSlider.value = sfxSound;

        if(bgmSound > 0) bgmSound = 0;
        if(bgmSound < -40) bgmSound = -40f;
        if(bgmSound == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", bgmSound);
        bgmSlider.value = bgmSound;
    }

    public void SFXAudioControl()
    {
        float sound = sfxSlider.value;
        if(sound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound);
        SaveLoadManager.Instance.SetSfxSound(sound);
        SaveLoadManager.Instance.SaveData();
    }

    public void BGMAudioControl()
    {
        float sound = bgmSlider.value;
        if(sound == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", sound);
        SaveLoadManager.Instance.SetBgmSound(sound);
        SaveLoadManager.Instance.SaveData();
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

    public void PlayGameButtonNoClick(){
        GameButtonNoClick.Play();
    }

    public void PlayPortalSFX(){
        PortalSFX.Play();
    }

    public void PlayItemUpgradeSFX(){
        ItemUpgradeSFX.Play();
    }

    public void PlayGameOverSFX(){
        GameOverSFX.Play();
    }

    public void PlayDamgeSFX(){
        DamgeSFX.Play();
    }

    public void PlaySkillSFX(){
        SkillSFX.Play();
    }

    public void PlayStoryUnLockSFX(){
        StoryUnLockSFX.Play();
    }

    public void PlayGetPieceSFX(){
        GetPieceSFX.Play();
    }
}
