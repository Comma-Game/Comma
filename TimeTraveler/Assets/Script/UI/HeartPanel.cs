using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HeartPanel : MonoBehaviour
{
    [SerializeField] public Image[] hearts;
    [SerializeField] public Sprite onHeart;
    [SerializeField] public Sprite offHeart;
    [SerializeField] public TextMeshProUGUI timeText;

    public int maxHearts = 5; // 최대 하트 개수
    private float fillCooldownMinutes = 10f; // 하트를 채우는데 걸리는 시간(분)
    public void SetFillCooldownMinutes(float time) { fillCooldownMinutes = time;}
    private int currentHearts = 0;
    public int GetCurrentHearts() { return currentHearts; }
    private DateTime lastFillTime;
    private int test = 0;
    private int testMobile = 0;
    private int heartsToAdd = 0;

    private static HeartPanel instance;
    // singleton
    public static HeartPanel Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<HeartPanel>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("HeartPanel");
                    instance = obj.AddComponent<HeartPanel>();
                }
            }
            return instance;
        }
    }

    /// ////////////////////////////////////////////////////////////////////
    /// 

    // Start is called before the first frame update
    void Start()
    {
        // 저장된 데이터 불러오기
        LoadData();
        // timeSinceLastFill 지난 시간
        TimeSpan timeSinceLastFill = DateTime.Now - lastFillTime;
        // 지난 시간 / 1개 채워지는 시간 = 총 채워진 화트 갯수
        heartsToAdd = Mathf.FloorToInt((float)timeSinceLastFill.TotalMinutes / fillCooldownMinutes);
        //Debug.Log("heartsToAdd : "+ heartsToAdd);
        if (heartsToAdd > 0)
        {
            if(currentHearts + heartsToAdd > maxHearts) heartsToAdd = maxHearts - currentHearts;
            if(heartsToAdd > 0) AddHearts(heartsToAdd);
        }
        // 업데이트
        UpdateTimerText(); // 시작 시 타이머 텍스트 업데이트
    }

    // Update is called once per frame
    void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MinusHearts(1);
        }
        // test
        if (Input.GetKeyDown(KeyCode.K))
        {
            test = 10;
        }

        UpdateTimerText(); // 매 프레임마다 타이머 텍스트 업데이트
    }

    public void AddHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = currentHearts + amount;
        lastFillTime = DateTime.Now;
        //SaveData();
        SaveLoadManager.Instance.SetExitTime();
        ChangeHeartImg();
        for(var i = 0; i<amount; i++) {
            SaveLoadManager.Instance.PlusHeart();
        }
        SaveLoadManager.Instance.SaveData();
    }

    public void AddFullHearts(){
        while(currentHearts < maxHearts){
            AddHearts(1);
        }
    }

    public void MinusHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = currentHearts - amount;
        lastFillTime = DateTime.Now;
        //SaveData();
        if(currentHearts == 4) SaveLoadManager.Instance.SetExitTime();
        ChangeHeartImg();
        SaveLoadManager.Instance.SubtractHeart();
        SaveLoadManager.Instance.SaveData();
    }

    public void CheatAddHeart()
    {
        testMobile = testMobile +1;
        if(testMobile >= 5){
            AddHearts(1);
            testMobile = 0;
        }
    }

    /// ////////////////////////////////////////////////////////////////////
    /// 

    private void ChangeHeartImg(){
        for(int i=0; i<maxHearts; i++){
            if(i < currentHearts){
                hearts[i].sprite = onHeart;
            }else{
                hearts[i].sprite = offHeart;
            }
        }
    }

    private void LoadData()
    {
        //fillCooldownMinutes = SaveLoadManager.Instance.GetHeartTimeTest();
        currentHearts = SaveLoadManager.Instance.GetHeart();
        lastFillTime = SaveLoadManager.Instance.GetExitTime();
        // Debug.Log("currentHearts : " + currentHearts);
        // Debug.Log("lastFillTime : " + lastFillTime);
        ChangeHeartImg();
    }

    private void UpdateTimerText()
    {
        // 현재 하트가 최대 하트가 아닐 경우만 계산
        if (currentHearts < maxHearts)
        {
            // 마지막 시간 + 총 하트 채워진 시간 - 현재 시간 = 지금 남은 시간 
            //TimeSpan timeUntilNextHeart = lastFillTime.AddMinutes(fillCooldownMinutes*heartsToAdd) - DateTime.Now;
            TimeSpan timeUntilNextHeart = lastFillTime.AddMinutes(fillCooldownMinutes) - DateTime.Now;
            int minutes = timeUntilNextHeart.Minutes;
            int seconds = timeUntilNextHeart.Seconds;

            // test
            minutes -= test;
            if(test != 0) test = 0;

            // 30분 지나면 하트 한 개 채워짐
            if (minutes < 1 && seconds < 1)
            {
                AddHearts(1);
            }

            // float time = ((maxHearts - 1) - currentHearts) * fillCooldownMinutes + minutes;
            // int totalSeconds = (int)(time * 60); // 분을 초로 변환
            // int minute = totalSeconds / 60;
            // int second = totalSeconds % 60;
            timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        else
        {
            int overNum = currentHearts - maxHearts;
            timeText.text = "+ " + overNum;
        }
    }
}
