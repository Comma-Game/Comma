using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MyPostProcess : MonoBehaviour
{
    static MyPostProcess _instance;
    public static MyPostProcess Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    static void Init_Instance()
    {
        if(_instance == null) _instance = GameObject.Find("PostProcessVolume").GetComponent<MyPostProcess>();
    }

    private void Start()
    {
        Init_Instance();
    }

    [SerializeField]
    PostProcessVolume _postProcessVolume;
    
    AutoExposure _autoExposureLayer;
    Bloom _bloom;

    public void SetExposure()
    {
        _postProcessVolume.profile.TryGetSettings(out _autoExposureLayer);
        StartCoroutine(StartSetExposure());
    }

    public void ResetExposure()
    {
        StartCoroutine(StartResetExposure());
    }

    public void EnableSkillEffect()
    {
        _postProcessVolume.profile.TryGetSettings(out _bloom);
        StartCoroutine(StartSetBloom());
    }

    public void DisableSkillEffect()
    {
        StopAllCoroutines();
        StartCoroutine(StartResetBloom());
    }

    IEnumerator StartSetExposure()
    {
        while(_autoExposureLayer.keyValue.value <= 100)
        {
            _autoExposureLayer.keyValue.value += 1;
            yield return new WaitForSeconds(0.01f);
        }
        _autoExposureLayer.keyValue.value = 100;

        StageController.Instance.EnableBonusStage();
    }

    IEnumerator StartResetExposure()
    {
        while (_autoExposureLayer.keyValue.value > 1)
        {
            _autoExposureLayer.keyValue.value -= 10;
            yield return new WaitForSeconds(0.01f);
        }
        _autoExposureLayer.keyValue.value = 1;
    }

    IEnumerator StartSetBloom()
    {
        while (_bloom.intensity.value <= 10)
        {
            _bloom.intensity.value += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        _bloom.intensity.value = 10;
    }

    IEnumerator StartResetBloom()
    {
        while (_bloom.intensity.value > 0)
        {
            _bloom.intensity.value -= 0.5f;
            yield return new WaitForSeconds(0.01f);
        }
        _bloom.intensity.value = 0;
    }
}
