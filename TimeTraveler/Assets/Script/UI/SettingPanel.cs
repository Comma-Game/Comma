using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요

public class SettingPanel : MonoBehaviour
{
    [SerializeField] public GameObject settingPanel;

    public void OnSettingPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        settingPanel.SetActive(isActive);
    }

    public void ClickReturnButton(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("ClickReturnButton");
    }

    public void ClickQuitButton(){
        AudioManager.Instance.PlayGameButtonClick();
        SceneManager.LoadScene("MainScene");
    }
}
