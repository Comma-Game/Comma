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
        Time.timeScale = 1;
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
                gameManager.AddComponent<BigJellyPoolManager>();
            }

            _instance = gameManager.GetComponent<PlayGameManager>();
        }
    }

    void Init()
    {
        _stage = null;

        ResumeGame();
        GetComponent<ParticleFooling>().SetParticle(Resources.Load<GameObject>("Particle/JellyParticle"));
        GetComponent<BigJellyPoolManager>().SetObject(Resources.Load<GameObject>("Jelly/BigJelly"));

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

        SaveLoadManager.Instance.PlusCoin(_score);

        _coin = _score;
        //���� ������ ���� 1.2��
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
        _scoreUp = StageController.Instance.GetPassThroughCount() >= 5 ? 25 : StageController.Instance.GetPassThroughCount() * 5;
        return 10 + _scoreUp;
    }

    public void SetStage(GameObject stage) { _stage = stage; }

    //���� �Ͻ�����
    public void PauseGame() 
    { 
        Time.timeScale = 0f;
        _player.transform.GetComponent<MovePlayer>().SetPause();
    }

    //���� �簳
    public void ResumeGame() 
    { 
        Time.timeScale = 1f;
        _player.transform.GetComponent<MovePlayer>().ResetPause();
    } 

    public void SetBonusTime() { _isBonus = true; }

    public void ResetBonusTime() { _isBonus = false; }

    IEnumerator ScoreTime()
    {
        while (true)
        {
            _score += ScorePerTime();

            CanvasController.Instance.ChangeScoreText(_score);
            
            if(!_isBonus) 
            {
                _player.TimeDamage();
                _player.ChargeEnergy();
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
