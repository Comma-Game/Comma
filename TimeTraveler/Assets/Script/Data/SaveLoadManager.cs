using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int playerCoin;
    public int stage1;
    public int stage2;
    public int stage3;
}

public enum DataType
{
    playerCoin,
    stage1,
    stage2,
    stage3
}

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private PlayerData playerData = null;

    private static SaveLoadManager instance;
    // singleton
    public static SaveLoadManager Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<SaveLoadManager>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("SaveLoadManager");
                    instance = obj.AddComponent<SaveLoadManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Application.persistentDataPath는 각 플랫폼에 따라 저장될 수 있는 영구적인 데이터 경로를 제공합니다.
        savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        LoadData();
    }

    public void SaveData(DataType dataType, int num){
        switch(dataType)
        {
            case DataType.playerCoin:
                playerData.playerCoin = num;
                break;
            case DataType.stage1:
                playerData.stage1 = num;
                break;
            case DataType.stage2:
                playerData.stage2 = num;
                break;
            case DataType.stage3:
                playerData.stage3 = num;
                break;
            default:
                break;
        }
        SavePlayerData(playerData);
    }

    // public void SaveData(string propertyName, int num){
    //     typeof(PlayerData).GetProperty(propertyName)?.SetValue(playerData, num);
    //     SavePlayerData(playerData);
    // }

    public void LoadData(){
        // 불러오기 예제
        SaveLoadManager saveLoadManager = GetComponent<SaveLoadManager>();
        PlayerData loadedData = saveLoadManager.LoadPlayerData();
        if (loadedData != null)
        {
            Debug.Log("Player Coin : " + loadedData.playerCoin);
            Debug.Log("stage1 : " + loadedData.stage1);
            Debug.Log("stage2 : " + loadedData.stage2);
            Debug.Log("stage3 : " + loadedData.stage3);
            playerData = loadedData;
        }else{
            var player = new PlayerData();
            player.playerCoin = 0;
            player.stage1 = 0;
            player.stage2 = 0;
            player.stage3 = 0;
            playerData = player;
        }
    }

    private void SavePlayerData(PlayerData data)
    {
        // 데이터를 JSON 형식으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // JSON 데이터를 파일에 쓰기
        File.WriteAllText(savePath, jsonData);
    }

    private PlayerData LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(savePath);

            // JSON 데이터를 클래스로 변환
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonData);

            return loadedData;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
