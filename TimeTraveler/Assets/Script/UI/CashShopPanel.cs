using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashShopPanel : MonoBehaviour
{
    [SerializeField] public MainSceneController mainSceneController;

    public void BuyCoin1(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("BuyCoin1");
        mainSceneController.PlusCoin(1000);
    }

    public void BuyCoin2(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("BuyCoin2");
        mainSceneController.PlusCoin(5000);
    }

    public void BuyCoin3(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("BuyCoin3");
        mainSceneController.PlusCoin(15000);
    }

    public void BuyCoin4(){
        AudioManager.Instance.PlayGameButtonClick();
        Debug.Log("BuyCoin4");
        mainSceneController.PlusCoin(30000);
    }

    public void BuyADFree(){
        AudioManager.Instance.PlayGameButtonClick();
        SaveLoadManager.Instance.SetIsBuyAd(true);
        SaveLoadManager.Instance.SaveData();
        Debug.Log("BuyADFree");
    }
}
