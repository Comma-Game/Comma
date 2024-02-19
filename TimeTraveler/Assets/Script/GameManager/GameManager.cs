using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance; //유일성 보장
    public static GameManager Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    public static void Init_Instance()
    {
        if (!_instance)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            _instance = gameManager.GetComponent<GameManager>();

            DontDestroyOnLoad(gameManager);
        }
    }

    bool _gameMode;

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    void Init()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        _gameMode = false;
    }

    public void SetGameScene() { _gameMode = true; }
    public void SetTutorialScene() { _gameMode = false; }
    public bool GetGameMode() { return _gameMode; }
}
