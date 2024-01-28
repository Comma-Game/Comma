using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashShopPanel : MonoBehaviour
{
    [SerializeField] public MainSceneController mainSceneController;

    public void BuyCoin1(){
        Debug.Log("BuyCoin1");
        mainSceneController.PlusCoin(1000);
    }

    public void BuyCoin2(){
        Debug.Log("BuyCoin2");
        mainSceneController.PlusCoin(5000);
    }

    public void BuyCoin3(){
        Debug.Log("BuyCoin3");
        mainSceneController.PlusCoin(15000);
    }

    public void BuyCoin4(){
        Debug.Log("BuyCoin4");
        mainSceneController.PlusCoin(30000);
    }

    public void BuyADFree(){
        Debug.Log("BuyADFree");
    }
}
