using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameManager : MonoBehaviour
{
    static PlayGameManager _instance; //유일성 보장
    public static PlayGameManager Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    static int _score;
    public static int Score
    {
        get
        {
            return _score;
        }
    }

    bool _scoreBuff;
    Coroutine _coroutine;
    Player _player;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _score = 0;
        _scoreBuff = false;
    }

    void Start()
    {
        Init_Instance();
        Init();
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
                gameManager.AddComponent<PlayGameManager>();
                gameManager.AddComponent<UseItem>();
            }

            _instance = gameManager.GetComponent<PlayGameManager>();
        }
    }

    void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        ResumeGame();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
    }

    public void EndGame()
    {
        if (_scoreBuff) _score *= 2;
        EnableGameOverUI();
    }

    void EnableGameOverUI()
    {
        CanvasController.Instance.OpenGameOverPanel(true);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_score);

        SaveLoadManager.Instance.PlusCoin(_score);
        SaveLoadManager.Instance.SetHighScore(_score);
        SaveLoadManager.Instance.SaveData();
    }

    public void ScoreUp(int value)
    {
        _score += value;
        CanvasController.Instance.ChangeScoreText(_score);
    }

    public void SetScoreBuff()
    {
        _scoreBuff = true;
    }

    public int ScorePerTime()
    {
        int conceptCount = (StageController.Instance.GetStageCount() - 1) / 3;
        return 10 + (conceptCount >= 5 ? 10 : conceptCount * 2);
    }

    public void PauseGame() { Time.timeScale = 0f; } //게임 일시정지

    public void ResumeGame() { Time.timeScale = 1f; } //게임 재개

    IEnumerator ScoreTime()
    {
        while (true)
        {
            _score += ScorePerTime();

            CanvasController.Instance.ChangeScoreText(_score);
            CanvasController.Instance.ChangeState(StageController.Instance.GetStageCount());
            _player.TimeDamage();
            _player.ChargeEnergy();

            yield return new WaitForSeconds(1f);
        }
    }
}
