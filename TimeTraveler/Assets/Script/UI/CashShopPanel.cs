using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashShopPanel : MonoBehaviour
{
    [SerializeField] public MainSceneController mainSceneController;
    [SerializeField] public bool isActive = false;

    public void BuyCoin1(){
        if(isActive){
            AudioManager.Instance.PlayGameButtonClick();
            Debug.Log("BuyCoin1");
            mainSceneController.PlusCoin(15000);
        }else{
            AudioManager.Instance.PlayGameButtonNoClick();
        }
    }

    public void BuyCoin2(){
        if(isActive){
             AudioManager.Instance.PlayGameButtonClick();
            Debug.Log("BuyCoin2");
            mainSceneController.PlusCoin(45000);
        }
        else{
            AudioManager.Instance.PlayGameButtonNoClick();
        }
    }

    public void BuyCoin3(){
        if(isActive){
            AudioManager.Instance.PlayGameButtonClick();
            Debug.Log("BuyCoin3");
            mainSceneController.PlusCoin(100000);
        }
        else{
            AudioManager.Instance.PlayGameButtonNoClick();
        }
    }

    public void BuyCoin4(){
        if(isActive){
            AudioManager.Instance.PlayGameButtonClick();
            Debug.Log("BuyCoin4");
            mainSceneController.PlusCoin(150000);
        }
        else{
            AudioManager.Instance.PlayGameButtonNoClick();
        }
    }

    public void BuyADFree(){
        if(isActive){
            AudioManager.Instance.PlayGameButtonClick();
            SaveLoadManager.Instance.SetIsBuyAd(true);
            SaveLoadManager.Instance.SaveData();
            Debug.Log("BuyADFree");
        }
        else{
            AudioManager.Instance.PlayGameButtonNoClick();
        }
    }
}
