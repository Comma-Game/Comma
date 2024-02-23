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

    bool _coinBuff, _isBonus;
    Coroutine _gameOverCoroutine, _timeCoroutine;
    GameObject _player, _camera;
    GameObject _stage, _fog;
    int _scoreUp, _coin;
    StageInfoUI _stageInfoUI;
    
    private void Awake()
    {
        Init_Instance();

        Application.targetFrameRate = 30;

        _player = GameObject.Find("Player");
        _fog = GameObject.Find("Fog");
        _camera = GameObject.Find("Main Camera");

        _score = 0;
        _coinBuff = false;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;

        _fog.SetActive(true);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public static void Init_Instance()
    {
        if (!_instance)
        {
            GameObject gameManager = GameObject.Find("PlayGameManager");
            if (!gameManager)
            {
                gameManager = new GameObject();
                gameManager.name = "PlayGameManager";
                gameManager.AddComponent<PlayGameManager>();
                gameManager.AddComponent<UseItem>();
                gameManager.AddComponent<ParticleFooling>();
                gameManager.AddComponent<BigJellyPoolManager>();
            }

            _instance = gameManager.GetComponent<PlayGameManager>();
        }
    }

    void Init()
    {
        SaveLoadManager.Instance.PlusPlayCount();

        _stage = null;

        ResumeGame();
        GetComponent<ParticleFooling>().SetParticle(Resources.Load<GameObject>("Particle/JellyParticle"));
        if(GameManager.Instance.GetGameMode()) GetComponent<BigJellyPoolManager>().SetObject(Resources.Load<GameObject>("Jelly/BigJelly"));

        if (GameManager.Instance.GetGameMode()) _stageInfoUI = GameObject.Find("StageInfo").GetComponent<StageInfoUI>();

        StopAllCoroutines();
        _timeCoroutine = StartCoroutine(ScoreTime());
    }

    //젤리 파티클 활성화
    public void EnableParticle(Vector3 pos)
    {
        GetComponent<ParticleFooling>().EnableParticle(pos, _stage.transform);
    }

    public void EndGame()
    {
        if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);

        //스테이지 속도 0으로 설정
        StageController.Instance.SetVelocity(0);
        
        //StageInfo UI 비활성화
        DisableStageInfoUI();
        
        _coin = _score / 2;

        //버프 받으면 코인 1.2배
        if (_coinBuff) _coin += (int)(_coin * 0.2f);

        _player.GetComponent<Player>().DestroyPlayer();
        _fog.SetActive(false);

        SetGameOverUIText();
    }

    void SetGameOverUIText()
    {
        CanvasController.Instance.OpenDamgePanel(false);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_coin);
        CanvasController.Instance.ChangeResultStageText(StageController.Instance.GetStageCount());

        if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);
        _gameOverCoroutine = StartCoroutine(OpenGameOverUI(2));
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
        _scoreUp = StageController.Instance.GetPassThroughCount() >= 10 ? 20 : StageController.Instance.GetPassThroughCount() * 2;
        return 10 + _scoreUp;
    }

    public void SetStage(GameObject stage) { _stage = stage; }

    //게임 일시정지
    public void PauseGame() 
    { 
        Time.timeScale = 0f;
        _player.GetComponent<MovePlayer>().SetPause();
    }

    //게임 재개
    public void ResumeGame() 
    { 
        Time.timeScale = 1f;
        _player.GetComponent<MovePlayer>().ResetPause();
    } 

    //초월 공간에 들어갈 때
    public void SetBonusTime() 
    { 
        _isBonus = true;

        DisableStageInfoUI();
    }

    public void ResetBonusTime() { _isBonus = false; }

    //StageInfo에 넣어줄 값
    public void AddStageInfoForUI(int concept, int secondStage) 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.SetInfo(concept, secondStage); 
    }

    //포탈 통과하면 StageInfo 최신화
    public void PlusStageInfoIndex() 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.PlusStageIndex(); 
    }

    //StageInfo UI 비활성화
    void DisableStageInfoUI() 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.DisableUI();
    }

    //StageInfo의 Memory 해금 유무 UI
    public void UnlockMemoryForStageInfoUI()
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.UnlockMemery();
    }


    //투명화 된 오브젝트 원상태로 설정
    public void SetOpaque() { _camera.GetComponent<ShowPlayer>().SetOpaque(); }

    //카메라 진동
    public void SetStress() { _camera.GetComponent<StressReceiver>().InduceStress(0.2f); }

        IEnumerator ScoreTime()
    {
        while (true)
        {
            if(!_isBonus)
            {
                _score += ScorePerTime();

                CanvasController.Instance.ChangeScoreText(_score);

                _player.GetComponent<Player>().TimeDamage();
                _player.GetComponent<Player>().ChargeEnergy();
            }

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
