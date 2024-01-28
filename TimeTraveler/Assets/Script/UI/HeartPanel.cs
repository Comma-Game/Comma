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
    public float fillCooldownMinutes = 30f; // 하트를 채우는데 걸리는 시간(분)
    private int currentHearts = 0;
    public int GetCurrentHearts() { return currentHearts; }
    private DateTime lastFillTime;
    private int test = 0;
    private int testMobile = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 저장된 데이터 불러오기
        LoadData();
        // timeSinceLastFill 지난 시간
        TimeSpan timeSinceLastFill = DateTime.Now - lastFillTime;
        // 지난 시간 / 1개 채워지는 시간 = 총 채워진 화트 갯수
        int heartsToAdd = Mathf.FloorToInt((float)timeSinceLastFill.TotalMinutes / fillCooldownMinutes);
        if (heartsToAdd > 0)
        {
            AddHearts(heartsToAdd);
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
            test = 29;
        }

        UpdateTimerText(); // 매 프레임마다 타이머 텍스트 업데이트
    }

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
        currentHearts = SaveLoadManager.Instance.GetHeart();
        lastFillTime = SaveLoadManager.Instance.GetExitTime();
        Debug.Log("currentHearts : " + currentHearts);
        Debug.Log("lastFillTime : " + lastFillTime);
        ChangeHeartImg();
        // 저장된 하트 데이터 불러오기
        // currentHearts = PlayerPrefs.GetInt("CurrentHearts", maxHearts);
        // string tempLastFillTime = PlayerPrefs.GetString("LastFillTime", string.Empty);
        // if (!string.IsNullOrEmpty(tempLastFillTime))
        // {
        //     lastFillTime = DateTime.FromBinary(long.Parse(tempLastFillTime));
        // }
        // else
        // {
        //     // 최초 실행 시, 저장된 값이 없을 때
        //     lastFillTime = DateTime.Now;
        // }
        // currentHearts = 5;
    }

    // private void SaveData()
    // {
    //     // 하트 데이터 저장
    //     PlayerPrefs.SetInt("CurrentHearts", currentHearts);
    //     PlayerPrefs.SetString("LastFillTime", lastFillTime.ToBinary().ToString());
    //     PlayerPrefs.Save();
    // }

    public void AddHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = Mathf.Clamp(currentHearts + amount, 0, maxHearts);
        lastFillTime = DateTime.Now;
        //SaveData();
        SaveLoadManager.Instance.SetExitTime();
        ChangeHeartImg();
        SaveLoadManager.Instance.PlusHeart();
    }

    public void AddFullHearts(){
        while(currentHearts < maxHearts){
            AddHearts(1);
        }
    }

    public void MinusHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = Mathf.Clamp(currentHearts - amount, 0, maxHearts);
        lastFillTime = DateTime.Now;
        //SaveData();
        if(currentHearts == 4) SaveLoadManager.Instance.SetExitTime();
        ChangeHeartImg();
        SaveLoadManager.Instance.SubtractHeart();
    }

    private void UpdateTimerText()
    {
        // 현재 하트가 최대 하트가 아닐 경우만 계산
        if (currentHearts < maxHearts)
        {
            TimeSpan timeUntilNextHeart = lastFillTime.AddMinutes(fillCooldownMinutes) - DateTime.Now;
            int minutes = timeUntilNextHeart.Minutes - test;
            if(test != 0) test = 0; // test 
            //Debug.Log(timeUntilNextHeart);
            if(minutes < 1){
                AddHearts(1);
            }

            //timeText.text = string.Format("{0:D2}:{1:D2}",timeUntilNextHeart.Minutes, timeUntilNextHeart.Seconds);
            float time = ((maxHearts-1) - currentHearts) * fillCooldownMinutes + minutes;
            int hour = (int)(time/60);
            int minute = (int)(time%60);
            timeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);
        }
        else
        {
            timeText.text = "Fill";
        }
    }

    public void CheatAddHeart()
    {
        testMobile = testMobile +1;
        if(testMobile >= 5){
            AddHearts(1);
            testMobile = 0;
        }
    }
}
