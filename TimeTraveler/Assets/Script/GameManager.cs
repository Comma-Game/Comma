using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance; //���ϼ� ����
    public static GameManager Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    SaveLoadManager _gameData;

    void Start()
    {
        Init_Instance();
        _gameData = SaveLoadManager.Instance;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
    }

    static void Init_Instance()
    {
        if (!_instance)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (!gameManager)
            {
                gameManager = new GameObject();
                gameManager.name = "GameManager";
                gameManager.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(gameManager);
            _instance = gameManager.GetComponent<GameManager>();
        }
    }
}
