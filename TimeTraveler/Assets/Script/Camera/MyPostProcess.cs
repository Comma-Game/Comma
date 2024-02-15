using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MyPostProcess : MonoBehaviour
{
    [SerializeField]
    PostProcessVolume _postProcessVolume;
    AutoExposure _autoExposureLayer;

    void Start()
    {
        _postProcessVolume.profile.TryGetSettings(out _autoExposureLayer);
    }

    public void SetExposure()
    {
        StartCoroutine(StartSetExposure());
    }

    public void ResetExposure()
    {
        StartCoroutine(StartResetExposure());
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
}
