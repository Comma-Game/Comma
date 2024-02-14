using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void StartSceneMove(){
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    private IEnumerator LoadAsynSceneCoroutine(){
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
            if(isImage && imgTime > 3){
                // imgNum += 1;
                // if(imgNum >= sprites.Length){
                //     imgNum = 0;
                // }
                imgNum = (imgNum + 1) % sprites.Length;
                image.sprite = sprites[imgNum];
                imgTime = 0;
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
