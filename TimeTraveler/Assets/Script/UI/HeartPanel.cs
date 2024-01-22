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
    private DateTime lastFillTime;

    // Start is called before the first frame update
    void Start()
    {
        // 저장된 데이터 불러오기
        LoadData();
        TimeSpan timeSinceLastFill = DateTime.Now - lastFillTime;
        int heartsToAdd = Mathf.FloorToInt((float)timeSinceLastFill.TotalMinutes / fillCooldownMinutes);
        if (heartsToAdd > 0)
        {
            AddHearts(heartsToAdd);
        }

        UpdateTimerText(); // 시작 시 타이머 텍스트 업데이트
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MinusHearts(5);
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
        // 저장된 하트 데이터 불러오기
        currentHearts = PlayerPrefs.GetInt("CurrentHearts", maxHearts);
        string tempLastFillTime = PlayerPrefs.GetString("LastFillTime", string.Empty);
        if (!string.IsNullOrEmpty(tempLastFillTime))
        {
            lastFillTime = DateTime.FromBinary(long.Parse(tempLastFillTime));
        }
        else
        {
            // 최초 실행 시, 저장된 값이 없을 때
            lastFillTime = DateTime.Now;
        }
    }

    private void SaveData()
    {
        // 하트 데이터 저장
        PlayerPrefs.SetInt("CurrentHearts", currentHearts);
        PlayerPrefs.SetString("LastFillTime", lastFillTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void AddHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = Mathf.Clamp(currentHearts + amount, 0, maxHearts);
        lastFillTime = DateTime.Now;
        SaveData();
        ChangeHeartImg();
    }

    private void MinusHearts(int amount)
    {
        // 하트 추가 및 마지막으로 채운 시간 업데이트
        currentHearts = Mathf.Clamp(currentHearts - amount, 0, maxHearts);
        lastFillTime = DateTime.Now;
        SaveData();
        ChangeHeartImg();
    }

    void UpdateTimerText()
    {
        TimeSpan timeUntilNextHeart = lastFillTime.AddMinutes(fillCooldownMinutes) - DateTime.Now;
        int minutes = timeUntilNextHeart.Minutes;
        if (currentHearts < maxHearts)
        {
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
}
