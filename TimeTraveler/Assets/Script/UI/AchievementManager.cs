using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [Header("Story Panels")]
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject[] Buttons;
    [SerializeField] public GameObject[] stroyPanels;

    [SerializeField] public StoryPanel[] stroyPanelsCS;
    [SerializeField] public TextMeshProUGUI highScoreText;
    private int currentUnlockedConceptNum = 0;
    private List<UnlockStoryInfo> currentUnlockedMemory = null;

    public void Start(){
        currentUnlockedConceptNum = SaveLoadManager.Instance.GetUnlockedConcept();
        currentUnlockedMemory = SaveLoadManager.Instance.GetUnlockedMemory();
        //Debug.Log("currentUnlockedConceptNum : " + currentUnlockedConceptNum);
        SettingAchievement(currentUnlockedConceptNum);
        SettingStroy(currentUnlockedConceptNum);
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

    private void DisablePanels(){
        for(int i=0; i < stroyPanels.Length; i++){
            stroyPanels[i].SetActive(false);
        }
	}
}
