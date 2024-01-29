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
    private float _basicSpeed = 30f;
    
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
    private float _accSpeed = 0.3f;
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
    private float _maxSpeed = 80f;
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

    [SerializeField]
    private float _maxFirstSpeed = 40f;
    public float MaxFirstSpeed
    {
        get
        {
            return _maxFirstSpeed;
        }
        set
        {
            _maxFirstSpeed = value;
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
        "ToyMap",
        "ForestMap",
        "DinoMap",
        "HouseMap",
        "LabMap",
        "PlanetMap",
        "PlaygroundMap",
        "SchoolMap",
        "SeaMap"
    };

    static GameObject _stageController;
    GameObject[][] _stagePrefab;
    GameObject _stage, _nextStage, _parent;
    Queue<StageInfo> _queue; //생성될 스테이지들을 저장(컨셉 2개, 즉 스테이지는 6개)
    List<int> _conceptIndex;
    int _prevConcept, _stageCount;
    List<GameObject> _disabled, _explode;
    bool[,] _iniStage;

    private void Awake()
    {
        _basicSpeed = 30f;
        _speed = _basicSpeed;
        _accSpeed = 0.3f;
        _maxSpeed = 80f;
        _maxFirstSpeed = 40f;

        _stageCount = 1;
        _prevConcept = -1;

        _stage = null;
        _nextStage = null;
        _iniStage = new bool[_concept.Length, 3];

        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        SetConceptIndex();
        ResetConceptObject();
        InstantiateStage();

        _queue = new Queue<StageInfo>();
    }

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
        InsertStageToQueue();
        InsertStageToQueue();

        _nextStage = SetNextStage();
        SetCurrentStage();
        SetStagesVelocity();
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

            int concept = _conceptIndex[conceptIndex], stage = stageIndex[next_stage_num];

            if (!_iniStage[concept, stage])
            {
                _iniStage[concept, stage] = true;

                _stagePrefab[concept][stage] = Instantiate(_stagePrefab[concept][stage]);
                _stagePrefab[concept][stage].transform.SetParent(_parent.transform);
                _stagePrefab[concept][stage].SetActive(false);
            }

            stageIndex.RemoveAt(next_stage_num);
        }

        //큐에 컨셉을 삽입했으면 List에서 제거
        _conceptIndex.RemoveAt(conceptIndex);
    }


    //현재 스테이지 설정
    void SetCurrentStage()
    {
        _stage = _nextStage;

        _stage.transform.GetComponent<GateMovement>().StopMove();
        _stage.transform.position = new Vector3(0, 400, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정
        //_stage.SetActive(true);

        _nextStage = SetNextStage();
    }

    //다음 스테이지 설정
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //큐에서 나올 원소가 새로 생성될 스테이지
        if (_queue.Count <= 3) InsertStageToQueue(); //큐에 크기가 3 이하면 새로운 컨셉 큐에 삽입

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        ret = _stagePrefab[concept_index][stage_index];

        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    //비활성화된 모든 오브젝트 활성화
    void ReturnStage()
    {
        _stage.SetActive(false);
        EnableObject();
        DeleteExploder();

        _stage.transform.position = new Vector3(0, 0, 0);
    }

    //Scene에 모든 스테이지를 생성 후 비활성화
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_concept.Length][];
        for (int i = 0; i < _concept.Length; i++) _stagePrefab[i] = Resources.LoadAll<GameObject>(_concept[i]);
    }

    //스테이지 지나면 호출
    public void DisableStage()
    {
        ReturnStage();

        int conceptCount = _stageCount++ / 3;

        _speed = _basicSpeed + conceptCount * 2;
        _speed = _speed > _maxFirstSpeed ? _maxFirstSpeed : _speed;

        SetCurrentStage();
        SetStagesVelocity();
    }

    //Scene에 있는 Stage 속도 설정
    public void SetStagesVelocity()
    {
        _stage.GetComponent<GateMovement>().Move();
        _nextStage.GetComponent<GateMovement>().Move();
    }

    //현재 스피드 반환
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    public void SetVelocity(float speed)
    {
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    public void ResetVelocity()
    {
        _stage.GetComponent<GateMovement>().SetVelocity(_speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(_speed);
    }

    //HP가 0 이하 일때 호출
    public void EndGame()
    {
        Destroy(_parent);
    }

    //Stage가 들어갈 부모 설정
    void ResetConceptObject()
    {
        _parent = new GameObject();
        _parent.name = "Concept";
    }

    //가속도 설정
    public void SetAcceleration()
    {
        Debug.Log("현재 Stage : " + _stage.name);
        Debug.Log("다음 Stage : " + _nextStage.name);
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
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

    public void MakeExploder(Transform parent, GameObject explode)
    {
        explode.transform.SetParent(parent);
        _explode.Add(explode);
    }

    void DeleteExploder()
    {
        foreach (GameObject explode in _explode)
        {
            if (explode != null) Debug.Log("Explode 깨짐 : " + explode.gameObject.name);
                Destroy(explode);
        }
        _explode.Clear();
    }

    void EnableObject()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();
    }

    public int GetStageCount() { return _stageCount; }
}
