using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingImgPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image frontImg;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject frontImgObj;
    
    public void Run(){
        // 5초 동안 알파 값을 변경하는 코루틴 시작
        StartCoroutine(FadeAlphaOverTime(5f));
    }

    IEnumerator FadeAlphaOverTime(float duration)
    {
        // 시작 시간
        float startTime = Time.time;
        // 현재 알파 값
        float startAlpha = frontImg.color.a;
        // 목표 알파 값
        float targetAlpha = 0f;

        while (Time.time < startTime + duration)
        {
            // 현재 시간에서 시작 시간을 뺀 비율 계산
            float t = (Time.time - startTime) / duration;
            // 알파 값을 보간하여 설정
            Color newColor = frontImg.color;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            frontImg.color = newColor;

            // 한 프레임씩 기다림
            yield return null;
        }

        // 최종적으로 알파 값을 목표 알파 값으로 설정
        Color finalColor = frontImg.color;
        finalColor.a = targetAlpha;
        frontImg.color = finalColor;
        text.SetActive(false);
        frontImgObj.SetActive(false);
    }
}
