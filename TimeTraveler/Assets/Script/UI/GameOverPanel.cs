using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 필요

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField] public TextMeshProUGUI stageText;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI coinText;

    public void openGameOverPanel(bool isActive){
        Debug.Log("openGameOverPanel");
        AudioManager.Instance.PlayGameOverSFX();
        mainPanel.SetActive(isActive);
    }

    public void ChangeStageText(float stage){
        stageText.text = "Stage : " + stage.ToString();
    }

    public void ChangeScoreText(float score){
        scoreText.text = "Score : " + score.ToString();
    }

    public void ChangeCoinText(float coin){
        coinText.text = "Coin : " + coin.ToString();
    }

    public void Get2Coin(){
        if(SaveLoadManager.Instance.GetIsBuyAd() == false){
            // 광고 띄우기
        }
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("Get2Coin");
        SceneManager.LoadScene("MainScene");
        // 코인 두 배 획득해서 얻는 코드
    }

    public void acceptButton(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("acceptButton");
        SceneManager.LoadScene("MainScene");
        // 메인 화면 돌아가기
    }
}
