using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [Header("Story Panels")]
    [SerializeField] public GameObject[] stroyPanels;
    [SerializeField] public TextMeshProUGUI highScoreText;

    private void DisablePanels(){
        for(int i=0; i < stroyPanels.Length; i++){
            stroyPanels[i].SetActive(false);
        }
	}

    public void ClickStory(int number){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("ClickStory : " + number);
        DisablePanels();
        stroyPanels[number].SetActive(true);
    }

    public void ChangeHighScoreText(int num){
        highScoreText.text = "My High Score : " + num.ToString();
    }
}
