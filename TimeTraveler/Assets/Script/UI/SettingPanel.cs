using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요

public class SettingPanel : MonoBehaviour
{
    [SerializeField] public GameObject settingPanel;
    [SerializeField] public bool isGameScene;

    public void OnSettingPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        settingPanel.SetActive(isActive);
        if(isGameScene){
            PlayGameManager.Instance.PauseGame(); //일시정지
        }
    }

    public void ClickReturnButton(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("ClickReturnButton");
        if(isGameScene){
            PlayGameManager.Instance.ResumeGame(); //재개
        }
    }

    public void ClickQuitButton(){
        PlayGameManager.Instance.ResumeGame(); //재개
        AudioManager.Instance.PlayGameButtonClick();
        SceneManager.LoadScene("MainScene");
    }
}
