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
    Camera _mainCamera;
    SaveLoadManager _saveLoadManager;
    StageController _stageController;

    private void Awake()
    {
        Init_Instance();

        Application.targetFrameRate = 60;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        _score = 0;
        _scoreBuff = false;
    }

    private void OnEnable()
    {
        _saveLoadManager = SaveLoadManager.Instance;
        _stageController = StageController.Instance;
    }

    void Start()
    {
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
        _mainCamera.clearFlags = CameraClearFlags.Nothing;
        
        ResumeGame();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
    }

    public void EndGame()
    {
        if (_scoreBuff) _score += (int)(_score * 0.2f);
       
        EnableGameOverUI();
    }

    void EnableGameOverUI()
    {
        SaveLoadManager.Instance.PlusCoin(_score);
        SaveLoadManager.Instance.SetHighScore(_score);
        SaveLoadManager.Instance.SaveData();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(OpenGameOverUI(2));
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
            _player.TimeDamage();
            _player.ChargeEnergy();

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator OpenGameOverUI(float time)
    {
        yield return new WaitForSeconds(time);

        StageController.Instance.EndGame();
        _player.DestroyPlayer();

        _mainCamera.clearFlags = CameraClearFlags.SolidColor;
        CanvasController.Instance.OpenDamgePanel(false);
        CanvasController.Instance.OpenGameOverPanel(true);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_score);
    }
}
