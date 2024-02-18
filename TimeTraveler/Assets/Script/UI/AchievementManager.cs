using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [Header("Story Panels")]
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject newAchievementImg;
    [SerializeField] public GameObject[] Buttons;
    [SerializeField] public GameObject[] stroyPanels;
    [SerializeField] public GameObject[] newButtonImg;
    [SerializeField] public StoryPanel[] stroyPanelsCS;
    [SerializeField] public TextMeshProUGUI highScoreText;
    private int currentUnlockedConceptNum = 0;
    private List<UnlockStoryInfo> currentUnlockedMemory = null;

    private static AchievementManager instance;
    // singleton
    public static AchievementManager Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<AchievementManager>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("AchievementManager");
                    instance = obj.AddComponent<AchievementManager>();
                }
            }
            return instance;
        }
    }

    public void Start(){
        currentUnlockedConceptNum = SaveLoadManager.Instance.GetUnlockedConcept();
        currentUnlockedMemory = SaveLoadManager.Instance.GetUnlockedMemory();
        //Debug.Log("currentUnlockedConceptNum : " + currentUnlockedConceptNum);
        SettingAchievement(currentUnlockedConceptNum);
        SettingStroy(currentUnlockedConceptNum);
        SettingNewNoti(currentUnlockedConceptNum);
    }

    public void ClickStory(int number){
        AudioManager.Instance.PlayGameButtonClick();
        //Debug.Log("ClickStory : " + number);
        DisablePanels();
        stroyPanels[number].SetActive(true);
    }

    public void ChangeHighScoreText(int num){
        highScoreText.text = "내 최고 점수 : " + num.ToString();
    }

    public void OnPanel(bool isActive){
        mainPanel.SetActive(isActive);
    }

    // 새로운 업적 상태 표시 해야할지 확인 후 삭제
    public void DeletNewImg(){
        bool checkAllNewStory = false;
        for(int i=0; i < Buttons.Length; i++){
            if(i <= currentUnlockedConceptNum){
                List<bool> newStories = SaveLoadManager.Instance.GetUnOpenedStory(i);
                if(newStories == null) return;
                bool checkNewStory = false;
                for(int j=0; j < newStories.Count; j++){
                    if(newStories[j] == true){
                        checkNewStory = true;
                        checkAllNewStory = true;
                    }
                }
                if(checkNewStory) newButtonImg[i].SetActive(true);
                else newButtonImg[i].SetActive(false);
            }
        }
        if(checkAllNewStory){
            newAchievementImg.SetActive(true);
        }else{
            newAchievementImg.SetActive(false);
        }
    }

    /// ///////////////////////////////////////////////////////////////////

    private void SettingAchievement(int unlockNum){
        for(int i=0; i < Buttons.Length; i++){
            if(i <= unlockNum){
                Buttons[i].SetActive(true);
            }else{
                Buttons[i].SetActive(false);
            }
        }
    }

    private void SettingStroy(int unlockNum){
        for(int i=0; i < Buttons.Length; i++){
            if(i <= unlockNum){
                stroyPanelsCS[i].SettingStory(currentUnlockedMemory[i]);
            }
        }
    }

    private void SettingNewNoti(int unlockNum){
        bool checkAllNewStory = false;
        for(int i=0; i < Buttons.Length; i++){
            if(i <= unlockNum){
                List<bool> newStories = SaveLoadManager.Instance.GetUnOpenedStory(i);
                if(newStories == null) return;
                bool checkNewStory = false;
                for(int j=0; j < newStories.Count; j++){
                    if(newStories[j] == true){
                        checkNewStory = true;
                        stroyPanelsCS[i].SettingNewStory(j, true);
                        checkAllNewStory = true;
                    }
                }
                if(checkNewStory) newButtonImg[i].SetActive(true);
            }
        }
        if(checkAllNewStory){
            newAchievementImg.SetActive(true);
        }
    }

    private void DisablePanels(){
        for(int i=0; i < stroyPanels.Length; i++){
            stroyPanels[i].SetActive(false);
        }
	}
}
