using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopPanel : MonoBehaviour
{
    [SerializeField] public OneSkinItem[] oneSkinItemsCS;
    private bool[] isTestLcokData = {true, true, true, false, false, true, false, false};  
    private bool[] isTestBuyData = {true, false, false, false, false, true, false, false}; 
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
    }

    private void Setting(){
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            oneSkinItemsCS[i].ChangeIndex(i);
        }
    }

    private void CheckLockOrUnLockItems(){
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            if(isTestLcokData[i] == true){
                oneSkinItemsCS[i].ChangeUnLockState();
            }
        }
    }

    private void CheckBuyItems(){
        for(int i=0; i < oneSkinItemsCS.Length; i++){
            if(isTestBuyData[i] == true){
                oneSkinItemsCS[i].ChangeBuyState();
            }
        }
    }

    private void CheckLastSelectItem(int num){
        oneSkinItemsCS[num].OnBuyButton();
    }
}
