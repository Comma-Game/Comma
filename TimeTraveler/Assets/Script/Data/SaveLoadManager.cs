using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public int high_score;
    public int coin;
    public int upgrade_hp;
    public int upgrade_jelly;
    public int upgrade_energy;

    public GameData()
    {
        high_score = 0;
        coin = 0;
        upgrade_hp = 0;
        upgrade_jelly = 0;
        upgrade_energy = 0;
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
        savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    private void Start()
    {
        Init_Instance();
    }

    public void SaveGameData(int score, int coin, int upgrade_hp, int upgrade_jelly, int upgrade_energy)
    {
        _gameData.high_score = score;
        _gameData.coin = coin;
        _gameData.upgrade_hp = upgrade_hp;
        _gameData.upgrade_jelly = upgrade_jelly;
        _gameData.upgrade_energy = upgrade_energy;
    }

    public int GetUpgradeHP()
    {
        return _gameData.upgrade_hp;
    }

    public int GetUpgradeJelly()
    {
        return _gameData.upgrade_jelly;
    }

    public int GetUpgradeEnergy()
    {
        return _gameData.upgrade_energy;
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
        string jsonData = File.ReadAllText(savePath);
        if (File.Exists(jsonData)) SaveData();
    }
}
