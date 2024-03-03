using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider slider;
    private float time = 0;
    public GameObject loadingPanel;
    public GameObject turtorialPanel;
    public bool isImage;
    public Image image;
    public Sprite[] sprites;
    private int imgNum = 0;
    private float imgTime = 0;
    private AsyncOperation operation;
    public TextMeshProUGUI tipText;
    private int[] tipList = {0,1,2,3, 4};
    private int currentTipNum = 0;
    private static readonly System.Random rng = new System.Random();
    private string[] tip_texts = {
                "TIP: 플레이어 주변의 원이 초록색으로 변하면 상태이상 상태이다.",
                "TIP:  맵 종류의 따라 움직이거나 크기가 바뀌는 맵이 존재한다." ,
                "TIP: 파이프맵에서 오염된 물(초록색 물)에 닿으면 상태이상으로 체력이 감소된다.",
                "TIP: 실험실 맵에서 불에 맞으면 상태이상으로 체력이 감소된다.",
                "TIP: 공룡 맵에서 검은(화산) 연기에 맞으면 상태이상으로 체력이 감소된다."
    };

    public void StartSceneMove(){
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    private IEnumerator LoadAsynSceneCoroutine(){
        currentTipNum = 0;
        Array.Sort(tipList, (x, y) => rng.Next(-1, 2));
        time = 0;
        imgTime = 0;
        imgNum = 0;
        operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;
        if(isImage == false) loadingPanel.SetActive(true);
        else turtorialPanel.SetActive(true);

        while(!operation.isDone){
            time += Time.deltaTime;
            imgTime += Time.deltaTime;
            //slider.value = time / 1000f;
            //Debug.Log(imgTime);

            // 9초 후 넘어가기
            if(time > 9){
                operation.allowSceneActivation = true;
            }

            // 3초마다 컷씬 바뀜
            if(isImage && imgTime > 2){
                // imgNum += 1;
                // if(imgNum >= sprites.Length){
                //     imgNum = 0;
                // }
                imgNum = (imgNum + 1) % sprites.Length;
                image.sprite = sprites[imgNum];
                imgTime = 0;

                // 팁 글자 바꾸기
                tipText.text = tip_texts[tipList[currentTipNum]];
                currentTipNum++;
                if(currentTipNum >= tip_texts.Length){
                    currentTipNum = 0;
                }
            }
            //Debug.Log(time);
            yield return null;
        }

        //operation.allowSceneActivation = true;
    }

    // void OnApplicationQuit()
    // {
    //     operation.allowSceneActivation = true;
    //     operation = null; // 비동기 작업 객체 초기화
    // }
}
