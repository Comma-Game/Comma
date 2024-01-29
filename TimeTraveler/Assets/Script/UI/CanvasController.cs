using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 필요

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    public GameObject playerStatPanel;
    private PlayerStatPanel playerStatPanelCS;

    [SerializeField]
    public GameObject gamePanel;
    private GamePanel gamePanelCS;

    [SerializeField]
    public GameObject gameOverPanel;
    private GameOverPanel gameOverPanelCS;

    [SerializeField]
    public GameObject damgePanel;

    [SerializeField]
    public GameObject blurPanel;
    private BlurPanel blurPanelCS;

    [SerializeField]
    public GameObject messagePanel;
    private NotiMessagePanel messagePanelCS;

    [SerializeField]
    public GameObject speedPanel;
    private SpeedPanel speedPanelCS;

    public bool isTest = false;

    private static CanvasController instance;
    // singleton
    public static CanvasController Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<CanvasController>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("CanvasController");
                    instance = obj.AddComponent<CanvasController>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        playerStatPanelCS = playerStatPanel.GetComponent<PlayerStatPanel>();
        gamePanelCS = gamePanel.GetComponent<GamePanel>();
        gameOverPanelCS = gameOverPanel.GetComponent<GameOverPanel>();
        speedPanelCS = speedPanel.GetComponent<SpeedPanel>();
        blurPanelCS = blurPanel.GetComponent<BlurPanel>();
        messagePanelCS = messagePanel.GetComponent<NotiMessagePanel>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("KeyCode D");
            if(damgePanel.activeSelf == true) OpenDamgePanel(false);
            else OpenDamgePanel(true);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("KeyCode O");
            OpenGameOverPanel(true);
        }

        if(isTest){
            ChangeScoreUpText(1.5f);
            ChangeSpeedPanel(0.5f);
            OnMessagePanel();
            isTest = false;
        }
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Player Stat Panel 
    

    public void InitSetting(float maxHp){
        playerStatPanelCS.initSetting(maxHp);
    }

    public void PlayerGetDamgeHP(float damge){
        playerStatPanelCS.TakeDamage(damge);
    }

    public void PlayerRestoreHP(float healAmount){
        playerStatPanelCS.RestoreHealth(healAmount);
    }

    public void PlayerUpEnergy(float Amount){
        playerStatPanelCS.UpEnergy(Amount);
    }

    public void PlayerDownEnergy(float Amount){
        playerStatPanelCS.DownEnergy(Amount);
    }

    public void ChangeScoreText(float score){
        playerStatPanelCS.ChangeScoreText(score);
    }

    public void ChangeScoreUpText(float scoreUp){
        playerStatPanelCS.ChangeScoreUpText(scoreUp);
    }

    public void OnScoreUpImg(bool isActive){
        playerStatPanelCS.OnScoreUpImg(isActive);
    }
    
    /// ///////////////////////////////////////////////////////////////////
    /// Game Panel

    public void ChangeState(float stage){
        gamePanelCS.ChangeStageText(stage);
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Game Over Panel

    public void OpenGameOverPanel(bool isActive){
        gameOverPanelCS.openGameOverPanel(isActive);
    }

    public void ChangeResultScoreText(float score){
        gameOverPanelCS.ChangeScoreText(score);
    }

    public void ChangeResultCoinText(float coin){
        gameOverPanelCS.ChangeCoinText(coin);
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Damge Panel

    public void OpenDamgePanel(bool isActive){
        damgePanel.SetActive(isActive);
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Message Panel

    public void OnMessagePanel(){
        messagePanelCS.StartMove();
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Speed Panel

    public void ChangeSpeedPanel(float num){
        if(num == 0) blurPanel.SetActive(false);
        else {
            blurPanel.SetActive(true);
            blurPanelCS.ChangeBlurNum(num);
        }
    }
}