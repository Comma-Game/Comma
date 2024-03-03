using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopPanel : MonoBehaviour
{
    [SerializeField] public GameObject newImg;
    [SerializeField] public OneSkinItem[] oneSkinItemsCS;
    private bool[] isTestLockData = {false, false, false, false, false, false, false, false, false, false};  
    private bool[] isTestBuyData = {false, false, false, false, false, false, false, false, false, false}; 
    private int currentSelectButton = 0;

    void Start()
    {
        // 세팅 (인덱스 배정)
        Setting();
        // 잠금 또는 풀린 걸린 아이템 확인
        CheckLockOrUnLockItems();
        // 구매한 아이템 아닌 아이템 확인
        CheckBuyItems();
        // 마지막으로 선택했던 아이템 설정
        CheckLastSelectItem(currentSelectButton);
        // 알림 이미지 띄워주기
        CheckNewImg();
    }

    public void SelectItem(int index){
        SaveLoadManager.Instance.SetCurrentSkinItem(index);
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            if(i != index){
                if(isTestBuyData[i] == true){ // 구매된 아이템만 상태 바뀌기
                    oneSkinItemsCS[i].OffBuyButton();
                }
            }
        }
    }

    public void BuyItem(int index){
        // 데이터 저장 필요
        isTestBuyData[index] = true;
        SaveLoadManager.Instance.SetIsBuySkinItem(index); 
    }

    private void Setting(){
        currentSelectButton = SaveLoadManager.Instance.GetCurrentSkinItem(); 
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            oneSkinItemsCS[i].ChangeIndex(i);
        }
    }

    private void CheckLockOrUnLockItems(){
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            if(isTestLockData[i] == true){
                oneSkinItemsCS[i].ChangeUnLockState();
            }
            if(i >= 0 && i <= 4){
                oneSkinItemsCS[i].ChangeUnLockState();
            }
            if(i == 5){// 광고 플레이 30 횟수
                if(SaveLoadManager.Instance.GetShowAdsCount() >= 30){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 6){ // 광고 플레이 50 횟수
                if(SaveLoadManager.Instance.GetShowAdsCount() >= 50){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 7){ // 광고 플레이 30 횟수
                if(SaveLoadManager.Instance.GetShowAdsCount() >= 100){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 8){ // 모든 스토리 읽은 후 업적 해금
                if(SaveLoadManager.Instance.GetUnlockedConcept() >= 9){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 9){ // 최초 5000점 달성 시 업적 해금
                // if(SaveLoadManager.Instance.GetHighScore() >= 5000){
                //     oneSkinItemsCS[i].ChangeUnLockState();
                // }
            }
        }
    }

    private void CheckBuyItems(){
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            if(SaveLoadManager.Instance.GetIsBuySkinItem(i) == true){
                isTestBuyData[i] = SaveLoadManager.Instance.GetIsBuySkinItem(i);
                oneSkinItemsCS[i].ChangeBuyState();
            }
        }
    }

    private void CheckLastSelectItem(int num){
        oneSkinItemsCS[num].OnBuyButton();
    }

    private void CheckNewImg(){
        if(SaveLoadManager.Instance.GetHighScore() >= 5000){
            if(isTestBuyData[9] == false){
                // 알림 이미지 띄워주기
                newImg.SetActive(true);
            }
        }
    }

    public void CheckOverScore(){
        if(SaveLoadManager.Instance.GetHighScore() >= 5000){
            oneSkinItemsCS[9].ChangeUnLockState();
            BuyItem(9);
            oneSkinItemsCS[9].ChangeBuyState();
            newImg.SetActive(false);
        }
    }
}
