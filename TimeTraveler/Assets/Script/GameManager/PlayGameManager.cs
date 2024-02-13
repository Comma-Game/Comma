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

    bool _coinBuff;
    Coroutine _coroutine;
    Player _player;
    StageController _stageController;
    GameObject _stage, _fog;
    int _scoreUp;
    int _coin;
    
    private void Awake()
    {
        Init_Instance();

        Application.targetFrameRate = 30;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _fog = GameObject.Find("Fog");

        _score = 0;
        _coinBuff = false;
    }

    private void OnEnable()
    {
        _stageController = StageController.Instance;

        _fog.SetActive(true);
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
                gameManager.AddComponent<ParticleFooling>();
            }

            _instance = gameManager.GetComponent<PlayGameManager>();
        }
    }

    void Init()
    {
        _stage = null;

        ResumeGame();
        GetComponent<ParticleFooling>().SetParticle(Resources.Load<GameObject>("Particle/JellyParticle"));

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
    }

    //젤리 파티클 활성화
    public void EnableParticle(Vector3 pos)
    {
        GetComponent<ParticleFooling>().EnableParticle(pos, _stage.transform);
    }

    public void EndGame()
    {
        //스테이지 속도 0으로 설정
        StageController.Instance.SetVelocity(0);

        SaveLoadManager.Instance.PlusCoin(_score);

        _coin = _score;
        //버프 받으면 코인 1.2배
        if (_coinBuff) _coin += (int)(_score * 0.2f);
        SaveLoadManager.Instance.SetHighScore(_coin);
        
        SaveLoadManager.Instance.SaveData();

        _player.DestroyPlayer();
        _fog.SetActive(false);

        SetGameOverUIText();
    }

    void SetGameOverUIText()
    {
        CanvasController.Instance.OpenDamgePanel(false);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_coin);
        CanvasController.Instance.ChangeResultStageText(StageController.Instance.GetStageCount());

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(OpenGameOverUI(2));
    }

    public void ScoreUp(int value)
    {
        _score += value;
        CanvasController.Instance.ChangeScoreText(_score);
    }

    public void SetCoinBuff()
    {
        _coinBuff = true;
    }

    public int ScorePerTime()
    {
        _scoreUp = StageController.Instance.GetPassThroughCount() >= 5 ? 25 : StageController.Instance.GetPassThroughCount() * 5;
        return 10 + _scoreUp;
    }

    public void SetStage(GameObject stage) { _stage = stage; }

    //게임 일시정지
    public void PauseGame() 
    { 
        Time.timeScale = 0f;
        _player.transform.GetComponent<MovePlayer>().SetPause();
    }

    //게임 재개
    public void ResumeGame() 
    { 
        Time.timeScale = 1f;
        _player.transform.GetComponent<MovePlayer>().ResetPause();
    } 

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
        CanvasController.Instance.OpenGameOverPanel(true);
    }
}
