using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartShopPanel : MonoBehaviour
{
    [SerializeField] public HeartPanel HeartPanelCS;
    [SerializeField] public MainSceneController mainSceneController;
    [SerializeField] public NotiMessagePanel notiMessagePanel;

    public void BuyOneHeart(){
        AudioManager.Instance.PlayGameButtonClick();
        if(SaveLoadManager.Instance.GetCoin() >= 15000){
            HeartPanelCS.AddHearts(1);
            mainSceneController.MinusCoin(15000);
        }else{
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
                // 광고 띄우기
                GoogleAdMob.Instance.LoadRewardedAd(false);
            }
            HeartPanelCS.AddFullHearts();
        }
    }
}
