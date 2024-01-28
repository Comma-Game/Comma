using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartShopPanel : MonoBehaviour
{
    [SerializeField] public HeartPanel HeartPanelCS;
    [SerializeField] public MainSceneController mainSceneController;

    public void BuyOneHeart(){
        HeartPanelCS.AddHearts(1);
        mainSceneController.MinusCoin(15000);
    }

    public void BuyFiveHeart(){
        HeartPanelCS.AddFullHearts();
    }
}
