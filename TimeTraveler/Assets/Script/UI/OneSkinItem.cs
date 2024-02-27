using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OneSkinItem : MonoBehaviour
{
    private int index = 0;
    [SerializeField] public Image backImg;
    [SerializeField] public SkinShopPanel skinShopPanel;
    [SerializeField] public GameObject lockImg;
    [SerializeField] public Button buyButton;
    [SerializeField] public TextMeshProUGUI buyText;
    private MainSceneController mainSceneController;
    private int getCoin = 0;
    private bool isBuy = false;
    private Color ChangeColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 회색 및 알파값을 0.5로 설정
    private Color OriginColor = new Color(1, 1, 1, 0); 

    private void Start(){
        mainSceneController = GameObject.Find("MainSceneController").GetComponent<MainSceneController>();
    }

    /// ////////////////////////////////////////////////////
    // 버튼 상태 4가지
    // 1. 잠금
    // 2. 잠금이 풀려서 구매 가능 상태
    // 3. 구매한 상태
    // 4. 선택한 상태

    public void ChangeBuyState(){
        buyText.text = "장착하기";
        isBuy = true;
    }

    public void ChangeUnLockState(){
        lockImg.SetActive(false);
    }

    public void ClickBuyButton(){
        if(isBuy == true){ // 이미 구매 상태
            OnBuyButton(); // 창작 상태로 바꾸기
            SendSelectData(); 
            AudioManager.Instance.PlayGameButtonClick();
        }else{
            if(SaveLoadManager.Instance.GetCoin() >= getCoin){
                // 구매했음으로 바꿔주기
                mainSceneController.MinusCoin(getCoin);
                ChangeBuyState();
                SendBuyData();
            }else{ // 구매하지 못함
                AudioManager.Instance.PlayGameButtonNoClick();
            }
        }
    }

    public void ChangeGetCoin(int num){
        getCoin = num;
    }

    public void ChangeIndex(int num){
        index = num;
    }

    public void OnBuyButton(){
        buyText.text = "장착됨";
        ColorBlock colors = buyButton.colors;
        colors.normalColor = ChangeColor;
        buyButton.colors = colors;

        backImg.color = new Color(1.0f, 0.5f, 0.0f);
    }

    public void OffBuyButton(){
        buyText.text = "장착하기";
        ColorBlock colors = buyButton.colors;
        colors.normalColor = OriginColor;
        buyButton.colors = colors;

        backImg.color = new Color(1.0f, 1.0f, 1.0f);
    }

    /// ////////////////////////////////////////////////////

    private void SendSelectData(){
        skinShopPanel.SelectItem(index);
    }

    private void SendBuyData(){
        skinShopPanel.BuyItem(index);
    }
}
