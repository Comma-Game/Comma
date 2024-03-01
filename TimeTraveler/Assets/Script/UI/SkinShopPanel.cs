using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopPanel : MonoBehaviour
{
    [SerializeField] public OneSkinItem[] oneSkinItemsCS;
    private bool[] isTestLockData = {true, true, true, true, false, false, false, false};  
    private bool[] isTestBuyData = {false, false, false, false, false, false, false, false}; 
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
            if(i == 4){ // 업적 공룡까지
                if(SaveLoadManager.Instance.GetUnlockedConcept() >= 4){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 5){ // 모든 업적
                if(SaveLoadManager.Instance.GetUnlockedConcept() >= 9){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 6){ // 광고 플레이 횟수
                if(SaveLoadManager.Instance.GetShowAdsCount() >= 20){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
            }
            if(i == 7){ // 게임 플레이 횟수
                if(SaveLoadManager.Instance.GetPlayCount() >= 100){
                    oneSkinItemsCS[i].ChangeUnLockState();
                }
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
}
