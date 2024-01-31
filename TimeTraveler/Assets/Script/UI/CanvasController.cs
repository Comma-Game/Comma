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
        blurPanelCS = blurPanel.GetComponent<BlurPanel>();
        messagePanelCS = messagePanel.GetComponent<NotiMessagePanel>();
        speedPanelCS = speedPanel.GetComponent<SpeedPanel>();
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

    public void ChangeBuffImage(int num){
        playerStatPanelCS.ChangeBuffImage(num);
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

    public void ChangeResultStageText(float stage){
        gameOverPanelCS.ChangeStageText(stage);
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Damge Panel

    public void OpenDamgePanel(bool isActive){
        damgePanel.SetActive(isActive);
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Message Panel

    public void OnMessagePanel(){
        Debug.Log("OnMessagePanel");
        messagePanelCS.StartMove();
    }

    /// ///////////////////////////////////////////////////////////////////
    /// Speed Panel

    public void OnSpeedPanel(bool isActive){
        speedPanelCS.OnSpeedPanel(isActive);
    }

    public void ChangeSpeedColor(int num){
        speedPanelCS.ChangeSpeedColor(num);
    }

    public void ChangeSpeed(float num){
        speedPanelCS.ChangeSpeed(num);
    }
}