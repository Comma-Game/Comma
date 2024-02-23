using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HeartShopPanel : MonoBehaviour
{
    [SerializeField] public HeartPanel HeartPanelCS;
    [SerializeField] public MainSceneController mainSceneController;
    [SerializeField] public NotiMessagePanel notiMessagePanel;
    [SerializeField] public TextMeshProUGUI ADCountText;
    private int currentRemainADCount = 0;

    void Start()
    {
        LoadData();
    }

    void Update()
    {
        if(currentRemainADCount != SaveLoadManager.Instance.GetRemainAdsCount()){
            currentRemainADCount = SaveLoadManager.Instance.GetRemainAdsCount();
            ChangeADCountText(currentRemainADCount);
        }
    }

    private void LoadData()
    {
        // 날자가 달라졌으면
        if(IsDateChanged(DateTime.Now , SaveLoadManager.Instance.GetExitTime())){
            Debug.Log("RemainAdsCount IsDateChanged ResetRemainAdsCount");
            SaveLoadManager.Instance.ResetRemainAdsCount(); //남은 광고 횟수 초기화
        }
        currentRemainADCount = SaveLoadManager.Instance.GetRemainAdsCount();
        ChangeADCountText(currentRemainADCount);
    }

    private bool IsDateChanged(DateTime currnetTime, DateTime lastTime)
    {
        bool isChanged = lastTime.Date != currnetTime.Date;
        return isChanged;
    }

    private void ChangeADCountText(int num){
        Debug.Log("ChangeADCountText : "+ num);
        if(ADCountText == null) return;
        ADCountText.text = num.ToString() + " / 3";
    }

    public void BuyOneHeart(){
        AudioManager.Instance.PlayGameButtonClick();
        if(SaveLoadManager.Instance.GetCoin() >= 15000){
            HeartPanelCS.AddHearts(1);
            mainSceneController.MinusCoin(15000);
        }else{
            AudioManager.Instance.PlayGameButtonNoClick();
            notiMessagePanel.StartMove();
        }
    }

    public void BuyFiveHeart(){
        AudioManager.Instance.PlayGameButtonClick();
        // if() 오늘치 썼는지 아닌지 확인
        // 하트가 5개 이상이 아니고
        if(HeartPanelCS.GetCurrentHearts() < 5){
            // 광고 무료 구매했는지 확인
            if(SaveLoadManager.Instance.GetIsBuyAd() == false){
                Debug.Log("BuyFiveHeart AdmobReward");
                // 광고 띄우기
                GoogleAdMob.Instance.LoadAd(0, 0);
            }
        }
    }
}
