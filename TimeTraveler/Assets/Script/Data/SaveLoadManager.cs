using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class UnlockStoryInfo
{
    public List<bool> info, unOpened;

    public UnlockStoryInfo()
    {
        info = new List<bool>();
        unOpened = new List<bool>();
        for (int i = 0; i < 3; i++)
        {
            info.Add(false);
            unOpened.Add(false);
        }
    }

    public void UnlockStory(int index) { info[index] = true; } //스토리 해금 설정
    public bool CheckStory(int index) { return info[index]; } //true 일 때 스토리 해금 상태
    public void SetUnOpenedStory(int index) { unOpened[index] = true; } //스토리 첫 열람 설정
    public void ReadStory(int index) { unOpened[index] = false; } //스토리 읽으면
    public bool GetUnOpenedStory(int index) { return unOpened[index]; } //스토리 첫 열람 상태
}

[System.Serializable]
public class GameData
{
    public int high_score;
    public int coin;
    public List<int> upgrade;
    public int heart;
    public string exit_time;
    public int buff;
    public bool isGameFirst;
    public bool isBuyAd;
    public int unlockedConcept;
    public List<UnlockStoryInfo> unlockedMemory;
    public float bgmSound;
    public float sfxSound;


    public float heartTimetTest;

    public GameData()
    {
        high_score = 0;
        coin = 0;

        upgrade = new List<int>();
        for (int i = 0; i < 3; i++) upgrade.Add(0);

        heart = 5;
        exit_time = DateTime.Now.ToString();
        buff = 0;
        isGameFirst = false;
        isBuyAd = false;
        unlockedConcept = 2;

        //10개의 스테이지
        unlockedMemory = new List<UnlockStoryInfo>();
        for (int i = 0; i < 10; i++)
        {
            unlockedMemory.Add(new UnlockStoryInfo());
        }

        bgmSound = 0;
        sfxSound = 0;


        heartTimetTest = 20f;
    }
}

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private GameData _gameData;
    static bool _isInit = false;

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

    private void OnDestroy()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!Destroy SaveLoadManager!!!!!!!!!!!!!!!!!!!!");
    }

    static void Init_Instance()
    {
        if (_isInit) return;

        // 인스턴스가 없으면 생성
        if (_instance == null)
        {
            GameObject gameData = GameObject.Find("SaveLoadManager");

            // 만약 Scene에 GameManager가 없으면 새로 생성
            if (gameData == null)
            {
                gameData = new GameObject();
                gameData.name = "SaveLoadManager";
                gameData.AddComponent<SaveLoadManager>();
            }

            _instance = gameData.GetComponent<SaveLoadManager>();
            DontDestroyOnLoad(gameData);
        }
    }

    private void Awake()
    {
        // Application.persistentDataPath는 각 플랫폼에 따라 저장될 수 있는 영구적인 데이터 경로를 제공합니다.
        savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        Debug.Log(Application.persistentDataPath);
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
            GameData gameData = new GameData();

            return gameData;
        }
    }

    public void UpgradeHP() { GameData.upgrade[0]++; } //호출 시 upgrade_hp 1 증가
    public void UpgradeEnergy() { GameData.upgrade[1]++; } //호출 시 upgrade_energy 1 증가
    public void UpgradeJelly() { GameData.upgrade[2]++; } //호출 시 upgrade_jelly 1 증가
    public int GetUpgradeHP() { return GameData.upgrade[0]; } //upgrade_hp 반환
    public int GetUpgradeEnergy() { return GameData.upgrade[1]; } //upgrade_energy 반환
    public int GetUpgradeJelly() { return GameData.upgrade[2]; } //upgrade_jelly 반환
    public void PlusCoin(int coin) { GameData.coin += coin; } //Coin 더하기
    public void MinusCoin(int coin) { GameData.coin -= coin; } //Coin 빼기
    public int GetCoin() { return GameData.coin; } //DB에 있는 Coin 반환
    public void SetHighScore(int high_score) { GameData.high_score = GameData.high_score > high_score ? GameData.high_score : high_score; } //최대 점수 설정
    public int GetHighScore() { return GameData.high_score; } //최대 점수 반환
    public int GetHeart() { return GameData.heart; } //하트 반환
    public DateTime GetExitTime() { return Convert.ToDateTime(GameData.exit_time); } //종료 시간 반환
    public int GetBuff() { return GameData.buff; } //버프 인덱스 반환
    public void PlusHeart() { GameData.heart++; } //하트 하나 더하기
    public void SubtractHeart() { GameData.heart--; } //하트 하나 빼기
    public void SetExitTime() { GameData.exit_time = DateTime.Now.ToString(); } //나간 시간 설정
    public void SetBuff(int buff) { GameData.buff = buff; } //버프 인덱스 설정
    public bool GetIsGameFirst() { return GameData.isGameFirst; } //처음 시작 여부 반환
    public void SetIsGameFirst() { GameData.isGameFirst = true; } //처음 시작 완료 설정
    public bool GetIsBuyAd() { return GameData.isBuyAd; } //광고 구매 여부 반환
    public void SetIsBuyAd(bool isBuyAd) { GameData.isBuyAd = isBuyAd; } //광구 구매 설정
    public int GetUnlockedConcept() { return GameData.unlockedConcept; } //열린 Concept 반환
    public void SetUnlockedConcept(int unlock) { GameData.unlockedConcept = unlock; } //Concept 설정
    public List<UnlockStoryInfo> GetUnlockedMemory() { return GameData.unlockedMemory; } //먹은 기억의 조각 반환
    public void SetUnlockedMemory(int concept, int stage) { GameData.unlockedMemory[concept].UnlockStory(stage); } //먹은 기억의 조각 설정
    public void SetBgmSound(float sound) { GameData.bgmSound = sound; } //bgm 소리 저장
    public float GetBgmSound() { return GameData.bgmSound; } //bgm 소리 반환
    public void SetSfxSound(float sound) { GameData.sfxSound = sound; } //sfx 소리 저장
    public float GetSfxSound() { return GameData.sfxSound; } //sfx 소리 반환
    public void SetUnOpenedStory(int concept, int index) { GameData.unlockedMemory[concept].SetUnOpenedStory(index); } //스토리를 열람되지 않은 상태로 설정
    public void ReadStory(int concept, int index) { GameData.unlockedMemory[concept].ReadStory(index); } //스토리를 읽은 상태로 설정
    public List<bool> GetUnOpenedStory(int concept) //해당 컨셉의 스토리 열람 상태를 반환
    {
        List<bool> ret = new List<bool>();
        for (int i = 0; i < 3; i++) ret.Add(GameData.unlockedMemory[concept].GetUnOpenedStory(i));
        return ret;
    }

    public void SetHeartTimeTest(float time) { GameData.heartTimetTest = time; } //하트 테스트 시간 설정
    public float GetHeartTimeTest() { return GameData.heartTimetTest; } //하트 테스트 시간 반환

    public void SaveData()
    {
        // 데이터를 JSON 형식으로 변환
        string jsonData = JsonUtility.ToJson(_gameData);
        
        // JSON 데이터를 파일에 쓰기
        File.WriteAllText(savePath, jsonData);
        Debug.Log("저장 완료");
    }

    void OnApplicationQuit()
    {
        _isInit = true;
        SaveData();
    }
}
