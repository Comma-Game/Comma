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
    SaveLoadManager _saveLoadManager;
    StageController _stageController;
    int _scoreUp;
    int[] _needScore = { 0, 0, 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000 };

    private void Awake()
    {
        Init_Instance();

        Application.targetFrameRate = 30;

        _player = GameObject.Find("Player").GetComponent<Player>();

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
                gameManager.AddComponent<ParticleFooling>();
            }

            _instance = gameManager.GetComponent<PlayGameManager>();
        }
    }

    void Init()
    {
        ResumeGame();
        GetComponent<ParticleFooling>().SetParticle(Resources.LoadAll<GameObject>("Particle"));

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
    }

    //젤리 파티클 활성화
    public void EnableParticle(int index, Vector3 pos)
    {
        GetComponent<ParticleFooling>().EnableParticle(index, pos);
    }

    //업적 오픈 확인
    public void CheckUnlockScore()
    {
        int unlock = SaveLoadManager.Instance.GetUnlockedConcept() + 1;
        if (_score >= _needScore[unlock]) SaveLoadManager.Instance.SetUnlockedConcept(unlock);
    }

    public void EndGame()
    {
        if (_scoreBuff) _score += (int)(_score * 0.2f);

        //스테이지 속도 0으로 설정
        StageController.Instance.SetVelocity(0);

        CheckUnlockScore();
        SaveLoadManager.Instance.PlusCoin(_score);
        SaveLoadManager.Instance.SetHighScore(_score);
        SaveLoadManager.Instance.SaveData();

        _player.DestroyPlayer();
        SetGameOverUIText();
    }

    void SetGameOverUIText()
    {
        CanvasController.Instance.OpenDamgePanel(false);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_score);
        CanvasController.Instance.ChangeResultStageText(StageController.Instance.GetStageCount());

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
        _scoreUp = StageController.Instance.GetPassThroughCount() >= 5 ? 25 : StageController.Instance.GetPassThroughCount() * 5;
        return 10 + _scoreUp;
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
        CanvasController.Instance.OpenGameOverPanel(true);
    }
}
