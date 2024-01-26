using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class GameData
{
    public int high_score;
    public int coin;
    public float upgrade_hp;
    public int upgrade_jelly;
    public float upgrade_energy;
    public int heart;
    public string exit_time;
    public int buff;

    public GameData()
    {
        high_score = 0;
        coin = 0;
        upgrade_hp = 0;
        upgrade_jelly = 0;
        upgrade_energy = 0;
        heart = 5;
        exit_time = "";
        buff = 0;
    }
}

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private GameData _gameData;
    public GameData GameData
    {
        get
        {
            if(_gameData == null)
            {
                _gameData = LoadData();
                SaveData();
            }

            return _gameData;
        }
    }

    private static SaveLoadManager _instance;
    // singleton
    public static SaveLoadManager Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    static void Init_Instance()
    {
        // 인스턴스가 없으면 생성
        if (!_instance)
        {
            // 만약 Scene에 GameManager가 없으면 새로 생성

            GameObject gameData = new GameObject();
            gameData.name = "SaveLoadManager";
            gameData.AddComponent<SaveLoadManager>();
            _instance = gameData.GetComponent<SaveLoadManager>();
            
            DontDestroyOnLoad(gameData);
        }
    }

    private void Awake()
    {
        // Application.persistentDataPath는 각 플랫폼에 따라 저장될 수 있는 영구적인 데이터 경로를 제공합니다.
        savePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }

    public void SaveGameData(int score, int coin, float upgrade_hp, int upgrade_jelly, float upgrade_energy)
    {
        _gameData.high_score = score;
        _gameData.coin = coin;
        _gameData.upgrade_hp = upgrade_hp;
        _gameData.upgrade_jelly = upgrade_jelly;
        _gameData.upgrade_energy = upgrade_energy;
    }

    private GameData LoadData()
    {
        Debug.Log(savePath);
        if (File.Exists(savePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(savePath);

            // JSON 데이터를 클래스로 변환
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

            //Debug.Log("Player Coin : " + loadedData.coin);
            //Debug.Log("upgrade_hp : " + loadedData.upgrade_hp);
            //Debug.Log("score : " + loadedData.high_score);

            return loadedData;
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();

            return _gameData;
        }
    }

    public void UpgradeHP() { GameData.upgrade_hp++; } //호출 시 upgrade_hp 1 증가
    public void UpgradeEnergy() { GameData.upgrade_energy++; } //호출 시 upgrade_energy 1 증가
    public void UpgradeJelly() { GameData.upgrade_jelly++; } //호출 시 upgrade_jelly 1 증가
    public float GetUpgradeHP() { return GameData.upgrade_hp; } //upgrade_hp 반환
    public float GetUpgradeEnergy() { return GameData.upgrade_energy; } //upgrade_energy 반환
    public int GetUpgradeJelly() { return GameData.upgrade_jelly; } //upgrade_jelly 반환
    public void PlusCoin(int coin) { GameData.coin += coin; } //Coin 더하기
    public void MinusCoin(int coin) { GameData.coin -= coin; } //Coin 빼기
    public int GetCoin() { return GameData.coin; } //DB에 있는 Coin 반환
    public void SetHighScore(int high_score) { GameData.high_score = GameData.high_score > high_score ? GameData.high_score : high_score; } //최대 점수 설정
    public int GetHighScore() { return GameData.high_score; } //최대 점수 반환
    public int GetHeart() { return GameData.heart; }
    public DateTime GetExitTime() { return Convert.ToDateTime(GameData.exit_time); }
    public int GetBuff() { return GameData.buff; }
    public void SubtractHeart() { GameData.heart--; }
    public void SetExitTime() { GameData.exit_time = DateTime.Now.ToString(); }
    public void SetBuff(int buff) { GameData.buff = buff; }

    void SaveData()
    {
        // 데이터를 JSON 형식으로 변환
        string jsonData = JsonUtility.ToJson(_gameData);
        
        // JSON 데이터를 파일에 쓰기
        File.WriteAllText(savePath, jsonData);
        Debug.Log("저장 완료");
    }

    private void OnApplicationQuit()
    {
        SetExitTime();
        SaveData();
    }
}
