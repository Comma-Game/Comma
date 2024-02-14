using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstStoryPanel : MonoBehaviour
{
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public Image storyImg;
    [SerializeField] public Sprite[] storySprite;
    [SerializeField] public GameObject leftButton;
    [SerializeField] public GameObject rightButton;
    [SerializeField] public GameObject returnButton;
    private int currentNum = 0;
    [SerializeField] public bool isSaveData;

    public void Reset(){
       currentNum = 0;
       storyImg.sprite = storySprite[currentNum];
       leftButton.SetActive(false);
       returnButton.SetActive(false);
       rightButton.SetActive(true);
    }

    public void ClickLeftButton(){
        AudioManager.Instance.PlayGameButtonClick();
        if(currentNum > 0){
            currentNum -= 1;
            if(currentNum == 0){
                leftButton.SetActive(false);
                returnButton.SetActive(false);
            }else{
                leftButton.SetActive(true);
                rightButton.SetActive(true);
                returnButton.SetActive(false);
            }
            storyImg.sprite = storySprite[currentNum];
        }
    }

    public void ClickRightButton(){
        AudioManager.Instance.PlayGameButtonClick();
        if(currentNum < (storySprite.Length-1)){
            currentNum += 1;
            if(currentNum == (storySprite.Length-1)){
                rightButton.SetActive(false);
                returnButton.SetActive(true);
            }else{
                leftButton.SetActive(true);
                rightButton.SetActive(true);
                returnButton.SetActive(false);
            }
            storyImg.sprite = storySprite[currentNum];
        }
    }

    public void ClickReturnButton(){
        AudioManager.Instance.PlayGameButtonClick();
        if(isSaveData){
            SaveLoadManager.Instance.SetIsGameFirst();
            SaveLoadManager.Instance.SaveData();
        }
    }
}
