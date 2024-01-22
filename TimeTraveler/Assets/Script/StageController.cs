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
    private float _speed = 10f;
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
    private float _accSpeed = 2f;
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
    private float _maxSpeed = 100f;
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

    string[] _concept =
    {
        "TestStage",
        "PipeMap",
        "LabMap"
    };

    static GameObject _stageController;
    GameObject[][] _stagePrefab;
    GameObject _stage, _nextStage, _parent;
    Queue<StageInfo> _queue; //생성될 스테이지들을 저장(컨셉 2개, 즉 스테이지는 6개)
    Coroutine _coroutine;

    private void OnEnable()
    {

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
        _stageController = GameObject.Find("StageController");
        if(!_stageController)
        {
            _stageController = new GameObject();
            _stageController.name = "StageController";
            _stageController.AddComponent<StageController>();
        }

        DontDestroyOnLoad(_stageController);
        _instance = _stageController.GetComponent<StageController>();
    }

    void Init()
    {
        _score = 0;
        _stagePrefab = new GameObject[_concept.Length][];
        for(int i = 0; i < _concept.Length; i++) _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);
        Reset_ConceptObject();
        _queue = new Queue<StageInfo>();

        InsertStageToQueue();
        InsertStageToQueue();
        InstantiateConcept(99.5f);
        InstantiateConcept(-0.5f); 
        
        _coroutine = StartCoroutine(ScoreTime());
    }

    void InsertStageToQueue()
    {
        int next_concept_num = Random.Range(0, _concept.Length);

        //각 컨셉당 Stage의 개수가 3개로 정해져 있기 때문에 {0, 1, 2}로 List를 만듦
        List<int> stageIndex = new List<int>(new int[]{ 0, 1, 2 });
        
        //랜덤으로 stageIndex의 index를 받아서 큐에 concept index와 stageIndex[index]를 넣어주고,
        //stageIndex[index]를 List에서 빼줘서 중복이 되지 않게 한다.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(next_concept_num, stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }
    }

    void InstantiateConcept(float y)
    {
        StageInfo next_stage_info = _queue.Dequeue(); //큐에서 나올 원소가 새로 생성될 스테이지
        if (_queue.Count <= 3) InsertStageToQueue(); //큐에 크기가 3 이하면 새로운 컨셉 큐에 삽입

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;

        //두번째 스테이지가 있다면 두번째 스테이지를 다음 스테이지로 설정
        if (_nextStage)
        {
            _stage = _nextStage;
            _stage.transform.position = new Vector3(0, 99.5f, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정 
        }

        Vector3 pos = new Vector3(0, y, 0);
        _nextStage = Instantiate(_stagePrefab[concept_index][stage_index], pos, Quaternion.identity);
        _nextStage.transform.SetParent(_parent.transform);

        int nextAngle = Random.Range(0, 360);
        _nextStage.transform.rotation = Quaternion.Euler(0, nextAngle, 0);

        /*
        if(_stage)
        {
            Debug.Log("cur pos : " + _stage.transform.position.y);
            Debug.Log("next pos : " + y);
        }
        */
    }

    public void DestroyStage()
    {
        UnsetAcceleration();
        Destroy(_stage);
        InstantiateConcept(-0.5f);
    }

    public void EndGame()
    {
        Destroy(_parent);
        StopCoroutine(_coroutine);
    }

    void Reset_ConceptObject()
    {
        _parent = new GameObject();
        _parent.name = "Concept";
    }

    public void SetAcceleration()
    {
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    public void UnsetAcceleration()
    {
        _nextStage.GetComponent<GateMovement>().UnsetAcceleration();
    }

    public void ScoreUp(int value)
    {
        _score += value;
    }
    IEnumerator ScoreTime()
    {
        while(true)
        {
            _score += 1;
            //CanvasController.Instance.ChangeScoreText(_score);
            Debug.Log("Score : " + _score);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
