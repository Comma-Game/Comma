using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartShopPanel : MonoBehaviour
{
    [SerializeField] public HeartPanel HeartPanelCS;
    [SerializeField] public MainSceneController mainSceneController;

    public void BuyOneHeart(){
        AudioManager.Instance.PlayGameButtonClick();
        if(HeartPanelCS.GetCurrentHearts() < 5){
            HeartPanelCS.AddHearts(1);
            mainSceneController.MinusCoin(15000);
        }
    }

    public void BuyFiveHeart(){
        AudioManager.Instance.PlayGameButtonClick();
        if(HeartPanelCS.GetCurrentHearts() < 5){
            HeartPanelCS.AddFullHearts();
        }
    }
}
