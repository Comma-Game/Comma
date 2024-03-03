using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameManager : MonoBehaviour
{
    static PlayGameManager _instance; //���ϼ� ����
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
    Material[] _trail;
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
        
        //if (GameManager.Instance.GetGameMode()) _trail = Resources.LoadAll<Material>("TrailItem");
        //_player.GetComponent<TrailRenderer>().material = _trail[SaveLoadManager.Instance.GetCurrentSkinItem()];
        
        if (GameManager.Instance.GetGameMode()) _stageInfoUI = GameObject.Find("StageInfo").GetComponent<StageInfoUI>();

        StopAllCoroutines();
        _timeCoroutine = StartCoroutine(ScoreTime());
    }

    //���� ��ƼŬ Ȱ��ȭ
    public void EnableParticle(Vector3 pos)
    {
        GetComponent<ParticleFooling>().EnableParticle(pos, _stage.transform);
    }

    public void EndGame()
    {
        if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);

        //�������� �ӵ� 0���� ����
        StageController.Instance.SetVelocity(0);
        
        //StageInfo UI ��Ȱ��ȭ
        DisableStageInfoUI();
        
        _coin = _score / 2;

        //���� ������ ���� 1.2��
        if (_coinBuff) _coin += (int)(_coin * 0.2f);

        _player.GetComponent<Player>().DestroyPlayer();

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

    //���� �Ͻ�����
    public void PauseGame() 
    { 
        Time.timeScale = 0f;
        _player.GetComponent<MovePlayer>().SetPause();
    }

    //���� �簳
    public void ResumeGame() 
    { 
        Time.timeScale = 1f;
        _player.GetComponent<MovePlayer>().ResetPause();
    } 

    //�ʿ� ������ �� ��
    public void SetBonusTime() 
    { 
        _isBonus = true;

        DisableStageInfoUI();
    }

    public void ResetBonusTime() { _isBonus = false; }

    //StageInfo�� �־��� ��
    public void AddStageInfoForUI(int concept, int secondStage) 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.SetInfo(concept, secondStage); 
    }

    //��Ż ����ϸ� StageInfo �ֽ�ȭ
    public void PlusStageInfoIndex() 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.PlusStageIndex(); 
    }

    //StageInfo UI ��Ȱ��ȭ
    void DisableStageInfoUI() 
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.DisableUI();
    }

    //StageInfo�� Memory �ر� ���� UI
    public void UnlockMemoryForStageInfoUI()
    {
        if (GameManager.Instance.GetGameMode()) _stageInfoUI.UnlockMemery();
    }


    //����ȭ �� ������Ʈ �����·� ����
    public void SetOpaque() { _camera.GetComponent<ShowPlayer>().SetOpaque(); }

    //ī�޶� ����
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

        _fog.SetActive(false);
        StageController.Instance.EndGame();
        CanvasController.Instance.OpenGameOverPanel(true);
    }
}
