using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    static StageController _instance;
    public static StageController Instance
    {
        get
        {
            Init_Instance();
            return _instance;
        }
    }

    [SerializeField]
    private float _basicSpeed;
    
    private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    [SerializeField]
    private float _accSpeed;
    public float AccSpeed
    {
        get
        {
            return _accSpeed;
        }
        set
        {
            _accSpeed = value;
        }
    }

    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed
    {
        get
        {
            return _maxSpeed;
        }
        set
        {
            _maxSpeed = value;
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

    struct StageInfo
    {
        public int concept_index, stage_index;

        public StageInfo(int concept_index, int stage_index)
        {
            this.concept_index = concept_index;
            this.stage_index = stage_index;
        }
    }

    //Resources에 있는 파일 이름
    string[] _concept =
    {
        //"TestStage",
        "PipeMap",
        //"LabMap"
        "ToyMap"
    };

    static GameObject _stageController;
    GameObject[][] _stagePrefab;
    GameObject _stage, _nextStage, _prevStage, _parent;
    Queue<StageInfo> _queue; //생성될 스테이지들을 저장(컨셉 2개, 즉 스테이지는 6개)
    Coroutine _coroutine;
    List<int> _conceptIndex;
    Player _player;
    int _prevConcept, _stageCount;
    List<GameObject> _disabled;
    bool _scoreBuff;

    void Start()
    {
        Init_Instance();
        Init();
    }

    static void Init_Instance()
    {
        _stageController = GameObject.Find("StageController");
        if(!_stageController)
        {
            _stageController = new GameObject();
            _stageController.name = "StageController";
            _stageController.AddComponent<StageController>();
        }

        _instance = _stageController.GetComponent<StageController>();
    }

    void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _basicSpeed = 30f;
        _speed = _basicSpeed;
        _accSpeed = 0.15f;
        _maxSpeed = 100f;
        _scoreBuff = false;

        _stageCount = 1;
        _score = 0;
        _prevConcept = -1;

        _stage = null;
        _nextStage = null;
        _prevStage = null;

        _disabled = new List<GameObject>();

        SetConceptIndex();
        Reset_ConceptObject();
        InstantiateStage();

        _queue = new Queue<StageInfo>();
        InsertStageToQueue();
        InsertStageToQueue();

        SetNextStage();
        SetCurrentStage();
        SetStagesVelocity();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ScoreTime());
    }

    //초기 Concept Index 설정
    void SetConceptIndex()
    {
        _conceptIndex = new List<int>();
        for (int i = 0; i < _concept.Length; i++) _conceptIndex.Add(i);
    }

    //맵에 나타날 스테이지를 큐에 넣어주는 과정
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        //컨셉을 뽑고, 이미 통과한 컨셉을 큐에 넣어줌
        if(_prevConcept != -1) _conceptIndex.Add(_prevConcept);
        _prevConcept = _conceptIndex[conceptIndex];

        //각 컨셉당 Stage의 개수가 3개로 정해져 있기 때문에 {0, 1, 2}로 List를 만듦
        List<int> stageIndex = new List<int>(new int[]{ 0, 1, 2 });
        
        //랜덤으로 stageIndex의 index를 받아서 큐에 concept index와 stageIndex[index]를 넣어주고,
        //stageIndex[index]를 List에서 빼줘서 중복이 되지 않게 한다.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[conceptIndex], stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }

        //큐에 컨셉을 삽입했으면 List에서 제거
        _conceptIndex.RemoveAt(conceptIndex);
    }


    //현재 스테이지 설정
    void SetCurrentStage()
    {
        _prevStage = _stage;
        _stage = _nextStage;

        int nextAngle = Random.Range(0, 360);
        _stage.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        _stage.transform.position = new Vector3(0, 400, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정
        _stage.SetActive(true);

        SetNextStage();
    }

    //다음 스테이지 설정
    void SetNextStage()
    {
        StageInfo next_stage_info = _queue.Dequeue(); //큐에서 나올 원소가 새로 생성될 스테이지
        if (_queue.Count <= 3) InsertStageToQueue(); //큐에 크기가 3 이하면 새로운 컨셉 큐에 삽입

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        _nextStage = _stagePrefab[concept_index][stage_index];
    }

    //비활성화된 모든 오브젝트 활성화
    void ReturnStage()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();

        Debug.Log(_prevStage.name);
        _prevStage.transform.position = new Vector3(0, 0, 0);
    }

    //Scene에 모든 스테이지를 생성 후 비활성화
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_concept.Length][];
        for (int i = 0; i < _concept.Length; i++)
        {
            _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);

            for (int j = 0; j < 3; j++)
            {
                _stagePrefab[i][j] = Instantiate(_stagePrefab[i][j]);
                _stagePrefab[i][j].transform.SetParent(_parent.transform);
                _stagePrefab[i][j].SetActive(false);
            }
        }
    }

    //스테이지 지나면 호출
    public void DisableStage()
    {
        _stage.SetActive(false);

        _speed = _basicSpeed + _stageCount++;

        SetCurrentStage();
        SetStagesVelocity();
    }

    //Scene에 있는 Stage 속도 설정
    public void SetStagesVelocity()
    {
        _stage.GetComponent<GateMovement>().Move();
    }

    //현재 스피드 반환
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //HP가 0 이하 일때 호출
    public void EndGame()
    {
        Destroy(_parent);
        StopCoroutine(_coroutine);
        EnableGameOverUI();

        if (_scoreBuff) _score *= 2;

        SaveLoadManager.Instance.PlusCoin(_score);
        SaveLoadManager.Instance.SetHighScore(_score);
    }

    //Stage가 들어갈 부모 설정
    void Reset_ConceptObject()
    {
        _parent = new GameObject();
        _parent.name = "Concept";
    }

    //가속도 설정
    public void SetAcceleration()
    {
        _stage.GetComponent<GateMovement>().SetAcceleration();
        if(_prevStage) ReturnStage();
    }

    public void ScoreUp(int value)
    {
        _score += value;
        CanvasController.Instance.ChangeScoreText(_score);
    }

    IEnumerator ScoreTime()
    {
        while(true)
        {
            int conceptCount = (_stageCount - 1) / 3;
            _score += 10 + (conceptCount >= 5 ? 10 : conceptCount * 2);
            
            CanvasController.Instance.ChangeScoreText(_score);
            CanvasController.Instance.ChangeState(_stageCount);
            _player.TimeDamage();
            _player.ChargeEnergy();

            yield return new WaitForSeconds(1f);
        }
    }

    void EnableGameOverUI()
    {
        CanvasController.Instance.OpenGameOverPanel(true);
        CanvasController.Instance.ChangeResultScoreText(_score);
        CanvasController.Instance.ChangeResultCoinText(_score);
    }

    public void AddDisabled(GameObject gameObject)
    {
        _disabled.Add(gameObject);
    }

    public void SetAccDeBuff()
    {
        _accSpeed *= 1.2f;
    }

    public void SetBasicSpeedDeBuff()
    {
        _basicSpeed *= 1.2f;
        _speed = _basicSpeed;
    }

    public void SetScoreBuff()
    {
        _scoreBuff = true;
    }
}
