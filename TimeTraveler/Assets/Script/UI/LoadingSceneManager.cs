using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] public Slider slider;
    private float time = 0;

    [SerializeField] public GameObject loadingPanel;
    [SerializeField] public GameObject turtorialPanel;
    [SerializeField] public bool isImage;
    [SerializeField] public Image image;
    [SerializeField] public Sprite[] sprites;
    private int imgNum = 0;
    private float imgTime = 0;

    public void StartSceneMove(){
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    private IEnumerator LoadAsynSceneCoroutine(){
        time = 0;
        imgTime = 0;
        imgNum = 0;
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;
        if(isImage == false) loadingPanel.SetActive(true);
        else turtorialPanel.SetActive(true);

        while(!operation.isDone){
            time += Time.time;
            imgTime += Time.time;
            //slider.value = time / 1000f;
            //Debug.Log(imgTime);
            if(time > 10000){
                operation.allowSceneActivation = true;
            }
            if(isImage){
                if(imgTime > 3000){
                    imgNum += 1;
                    if(imgNum >= sprites.Length){
                        imgNum = 0;
                    }
                    image.sprite = sprites[imgNum];
                    imgTime = 0;
                }
            }
            //Debug.Log(time);
            yield return null;
        }
    }
}
