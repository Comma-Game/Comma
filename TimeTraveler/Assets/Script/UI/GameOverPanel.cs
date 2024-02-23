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

    private float currentGetCoin = 0;
    int _highScore; //최고 스코어 기록을 위한 변수

    private int[] UnLockConceptScore = {0, 0, 0, 5000, 10000, 25000, 40000, 55000, 150000, 250000};
    // {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
    // {10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 1000000}

    public void openGameOverPanel(bool isActive){
        Debug.Log("openGameOverPanel");
        AudioManager.Instance.PlayGameOverSFX();
        mainPanel.SetActive(isActive);
        SaveLoadManager.Instance.PlusPlayCount();
        if(SaveLoadManager.Instance.GetPlayCount()%3 == 0){
            if(SaveLoadManager.Instance.GetIsBuyAd() == false){
                // 광고 띄우기
                GoogleAdMob.Instance.LoadAd(2, 0);
            }
        }
    }

    public void ChangeStageText(float stage){
        stageText.text = "Stage : " + stage.ToString();
    }

    public void ChangeScoreText(float score){
        scoreText.text = "Score : " + score.ToString();
        _highScore = (int)score;
        CheckStoryUnLock(score);
    }

    public void ChangeCoinText(float coin){
        currentGetCoin = coin;
        coinText.text = "Coin : " + coin.ToString();
    }

    public void Get2Coin(){
        if(SaveLoadManager.Instance.GetIsBuyAd() == false){
            // 광고 띄우기
            GoogleAdMob.Instance.LoadAd(1, (int)currentGetCoin);
        }
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("Get2Coin");
        //SceneManager.LoadScene("MainScene");
        // 코인 두 배 획득해서 얻는 코드
        TestConceptButton.Instance.ResetTestConcept();
    }

    public void acceptButton(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("acceptButton");
        SceneManager.LoadScene("MainScene");
        // 메인 화면 돌아가기
        TestConceptButton.Instance.ResetTestConcept();

        SaveLoadManager.Instance.PlusCoin((int)currentGetCoin);
        SaveLoadManager.Instance.SetHighScore(_highScore);
        SaveLoadManager.Instance.SaveData();
    }

    //////////////////////////////////////////
    ///

    private void CheckStoryUnLock(float score){
        int currentUnlockedConceptNum = SaveLoadManager.Instance.GetUnlockedConcept();
        int currentScoreToConceptNum = CheckCurrentScoreToConceptNum((int)score);
        Debug.Log("currentUnlockedConceptNum : " + currentUnlockedConceptNum + " currentScoreToConceptNum : " + currentScoreToConceptNum);
        if(currentScoreToConceptNum > currentUnlockedConceptNum){
            SaveLoadManager.Instance.SetUnlockedConcept(currentUnlockedConceptNum+1);
            SaveLoadManager.Instance.SaveData();
        }
    }

    private int CheckCurrentScoreToConceptNum(int score){
        int num = -1;
        for(int i=0; i<UnLockConceptScore.Length; i++){
            if(UnLockConceptScore[i] <= score){
                num += 1;
            }
        }
        return num;
    }
}
