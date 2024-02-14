using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotiMessagePanel : MonoBehaviour
{
    private float moveDistance = 200f; // 이동 거리
    private float mainMoveDistance = 3.5f; // 이동 거리
    private float moveDuration = 2.5f; // 이동에 걸리는 시간
    private float mainMoveDuration = 1f; 
    private float elapsedTime = 0f;
    private bool isMovingUp = true;
    public bool isOn = false;
    private int MoveNum = 0;
    [SerializeField] public bool isMain = false;
    
    void Update()
    {
        if(isOn){
            CheckMove();
        }else{
            this.gameObject.SetActive(false);
        }
    }

    private void CheckMove(){
        if(isMain == true) moveDuration = mainMoveDuration;
        // 일정 시간 동안 Y 축으로 이동
        if (elapsedTime < moveDuration)
        {
            float moveAmount = 0;
            if(isMain == false) moveAmount = (moveDistance / moveDuration) * Time.deltaTime;
            else moveAmount = (mainMoveDistance / moveDuration) * Time.deltaTime;

            if (isMovingUp)
                transform.Translate(Vector3.up * moveAmount);
            else
                transform.Translate(Vector3.down * moveAmount);

            elapsedTime += Time.deltaTime;
        }
        else
        {
            MoveNum += 1;
            // 이동이 끝나면 방향을 바꾸고 시간 초기화
            isMovingUp = !isMovingUp;
            elapsedTime = 0f;
            if(MoveNum == 2){
                isOn = false;
            }
        }
    } 

    public void StartMove(){
        if(isOn == false){
            this.gameObject.SetActive(true);
            isOn = true;
            MoveNum = 0;
            AudioManager.Instance.PlayStoryUnLockSFX();
        }
    }
}
