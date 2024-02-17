using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryPanel : MonoBehaviour
{
    [SerializeField] public int conceptNum;

    [Header("put Story")]
    [SerializeField] public string mytitleText;
    [SerializeField] public Sprite mystroy1Img;
    [SerializeField] public Sprite mystroy2Img;
    [SerializeField] public Sprite mystroy3Img;
    [SerializeField] public string mystroy1TitleText;
    [SerializeField] public string mystroy2TitleText;
    [SerializeField] public string mystroy3TitleText;
    [SerializeField] public string mystroy1Text;
    [SerializeField] public string mystroy2Text;
    [SerializeField] public string mystroy3Text;

    [Header("Story Panels")]
    [SerializeField] public TextMeshProUGUI titleText;
    [SerializeField] public Image[] stroyImg;    
    [SerializeField] public TextMeshProUGUI[] stroyTitleText;
    [SerializeField] public TextMeshProUGUI[] stroyText;

    private UnlockStoryInfo isStoryLock = null;
    private bool[] isUnlockStroy = {false, false, false}; 
    
    void Start()
    {
        titleText.text = mytitleText;

        stroyImg[0].sprite = mystroy1Img;
        stroyImg[1].sprite = mystroy2Img;
        stroyImg[2].sprite = mystroy3Img;

        // stroyTitleText[0].text = mystroy1TitleText;
        // stroyTitleText[1].text = mystroy2TitleText;
        // stroyTitleText[2].text = mystroy3TitleText;

        stroyText[0].text = mystroy1Text;
        stroyText[1].text = mystroy2Text;
        stroyText[2].text = mystroy3Text;
    }

    public void SettingStory(UnlockStoryInfo currentUnlockedMemory){
        isStoryLock = currentUnlockedMemory;
        for(int i=0; i<3; i++){
            //Debug.Log("SettingStory "+ i + " : " + isStoryLock.CheckStory(i));
            if(isStoryLock.CheckStory(i) == false){
                isUnlockStroy[i] = false;
                stroyImg[i].enabled = false;
                //stroyText[i].enabled = false;
                stroyTitleText[i].text = "Story " + (i+1).ToString() + " - 잠김";
            }else{
                isUnlockStroy[i] = true;
                stroyImg[i].enabled = true;
                //stroyText[i].enabled = true;
                stroyTitleText[i].text = "Story " + (i+1).ToString() + " - 열림";
            }
        }
    }

    public void OnStory(int storyum){
        if(isUnlockStroy[storyum]) StoryManager.Instance.ShowStory(conceptNum, storyum);
        //StoryManager.Instance.ShowStory(conceptNum, storyum);
    }
}
