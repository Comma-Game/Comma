using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 필요

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private bool isGameScene;
    [SerializeField] private TextMeshProUGUI haptickText;
    private bool onHaptick = true;
    private bool isTimsStop = false;

    private void Start(){
        if(haptickText != null){
            ChangeHaptickButton(SaveLoadManager.Instance.GetHaptic());
        }
    }

    public void OnSettingPanel(bool isActive){
        AudioManager.Instance.PlayGameButtonClick();
        settingPanel.SetActive(isActive);
        if(isGameScene){
            if(Time.timeScale == 0) isTimsStop = true;
            PlayGameManager.Instance.PauseGame(); //일시정지
        }
    }

    public void ClickReturnButton(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("ClickReturnButton");
        if(isGameScene){
            if(isTimsStop == false) {
                PlayGameManager.Instance.ResumeGame(); //재개
            }
        }
    }

    public void ClickQuitButton(){
        PlayGameManager.Instance.ResumeGame(); //재개
        AudioManager.Instance.PlayGameButtonClick();
        SceneManager.LoadScene("MainScene");
        TestConceptButton.Instance.ResetTestConcept();
    }

    public void ClickOpenURLButton(){
        Application.OpenURL("http://storage.thebackend.io/220f5159a65f36de876c6a7a8113b9dece9c56387cf425473fc67f6a05a6d68f/privacy.html");
    }

    public void ClickHaptickButton(){
        AudioManager.Instance.PlayGameButtonClick();
        if(onHaptick){
            onHaptick = false;
            haptickText.text = "On";
        }else{
            onHaptick = true;
            haptickText.text = "Off";
        }
        SaveLoadManager.Instance.ToggleHaptic();
    }

    public void ChangeHaptickButton(bool isActive){
        if(isActive){
            onHaptick = true;
            haptickText.text = "Off";
        }else{
            onHaptick = false;
            haptickText.text = "On";
        }
    }
}
