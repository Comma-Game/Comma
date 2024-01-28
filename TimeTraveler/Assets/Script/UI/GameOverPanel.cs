using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 필요

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI coinText;

    public void openGameOverPanel(bool isActive){
        Debug.Log("openGameOverPanel");
        mainPanel.SetActive(isActive);
    }

    public void ChangeScoreText(float score){
        scoreText.text = "Score : " + score.ToString();
    }

    public void ChangeCoinText(float coin){
        coinText.text = "Coin : " + coin.ToString();
    }

    public void Get2Coin(){
        //AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("Get2Coin");
        SceneManager.LoadScene("MainScene");
        // 코인 두 배 획득해서 얻는 코드
    }

    public void acceptButton(){
        //AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("acceptButton");
        SceneManager.LoadScene("MainScene");
        // 메인 화면 돌아가기
    }
}
