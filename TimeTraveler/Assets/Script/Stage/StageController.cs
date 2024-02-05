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
    private float _basicSpeed; //현재 진행하고 있는 스테이지의 기본 속도
    public float BasicSpeed
    {
        get { return _basicSpeed; }
    }

    private float _speed; //실시간 속도
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
    private float _accSpeed; //가속도
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
    private float _maxSpeed; //최대 속도
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
    private float _maxBasicSpeed; //기본 속도의 최대치
    public float MaxBasicSpeed
    {
        get
        {
            return _maxBasicSpeed;
        }
        set
        {
            _maxBasicSpeed = value;
        }
    }

    //큐에 들어갈 자료구조
    struct StageInfo
    {
        public int concept_index, stage_index;

        public StageInfo(int concept_index, int stage_index)
        {
            this.concept_index = concept_index;
            this.stage_index = stage_index;
        }
    }

    static GameObject _stageController;

    static void Init_Instance()
    {
        _stageController = GameObject.Find("StageController");
        if (!_stageController)
        {
            _stageController = new GameObject();
            _stageController.name = "StageController";
            _stageController.AddComponent<StageController>();
        }

        _instance = _stageController.GetComponent<StageController>();
    }

    GameObject[][] _stagePrefab; //리소스 파일에서 가져올 Stage 프리팹
    Queue<StageInfo> _queue; //생성될 스테이지들을 저장(컨셉 2개, 즉 스테이지는 6개)
    GameObject _stage, _nextStage, _prevStage, _stageParent; //_stageParent : 스테이지들의 부모 오브젝트
    List<int> _conceptIndex; //랜덤으로 가져올 컨셉을 저장할 리스트
    int _prevConcept, _stageCount; //_prevConcept : 바로 전에 사용한 컨셉, _conceptIndex에 빠진 concept을 넣어주기 위한 변수
    List<GameObject> _disabled, _explode; //비활성화된 오브젝트들과 파괴된 파편들을 저장할 리스트
    GameObject _stageEnd;
    GetStage _conceptInfo;
    int _passThroughCount, _conceptCount; //_passThroughCount : 장애물에 부딪히지 않고 계속 전진하는 횟수, _conceptCount : 플레이 가능한 concept 개수
    bool[,] _isInstantiateStage;

    private void Awake()
    {
        //스피드 초기화
        _basicSpeed = 7.5f;
        _speed = _basicSpeed;

        //테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _accSpeed = 0.5f;

        _maxSpeed = 25f;
        _maxBasicSpeed = 12.5f;

        //stage 정보 초기화
        _stageCount = 1;
        _prevConcept = -1;
        _stage = null;
        _nextStage = null;
        _prevStage = null;
        _stageParent = null;

        //저장할 Obstacle 리스트 초기화
        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        _queue = new Queue<StageInfo>();

        _stageEnd = GameObject.Find("StageEnd");
        _conceptInfo = GameObject.Find("Concept").GetComponent<GetStage>();
    }

    void Start()
    {
        Init_Instance();
        Init();
    }

    void Init()
    {
        _conceptCount = SaveLoadManager.Instance.GetUnlockedConcept() + 1;
        _isInstantiateStage = new bool[_conceptCount, 3];
        
        SetConceptIndex();
        SetStageParent();
        InstantiateStage();

        InsertStageToQueue();
        InsertStageToQueue();

        _nextStage = SetNextStage();
        SetCurrentStage();
        MoveStage();
    }

    //초기 Concept Index 설정
    void SetConceptIndex()
    {
        Debug.Log((_conceptCount - 1) + " 컨셉까지 활성화");

        _conceptIndex = new List<int>();
        for (int i = 0; i < _conceptCount; i++) _conceptIndex.Add(i);
    }

    //맵에 나타날 스테이지를 큐에 넣어주는 과정
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        //테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //while (_conceptIndex[conceptIndex] == 5 || _conceptIndex[conceptIndex] == 7 || _conceptIndex[conceptIndex] == 8) conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        
        //컨셉을 뽑고, 이미 통과한 컨셉을 큐에 넣어줌
        if (_prevConcept != -1) _conceptIndex.Add(_prevConcept);
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

    void TestStage(int index)
    {
        //각 컨셉당 Stage의 개수가 3개로 정해져 있기 때문에 {0, 1, 2}로 List를 만듦
        List<int> stageIndex = new List<int>(new int[] { 0, 1, 2 });

        //랜덤으로 stageIndex의 index를 받아서 큐에 concept index와 stageIndex[index]를 넣어주고,
        //stageIndex[index]를 List에서 빼줘서 중복이 되지 않게 한다.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[index], stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }

        //큐에 컨셉을 삽입했으면 List에서 제거
        _conceptIndex.RemoveAt(index);
    }

    //현재 스테이지 설정
    void SetCurrentStage()
    {
        _stage = _nextStage;
        PlayGameManager.Instance.SetStage(_stage);

        _stage.transform.GetComponent<GateMovement>().StopMove();
        _stage.transform.position = new Vector3(0, 95.1f, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정

        _stageEnd.transform.SetParent(null);
        _stageEnd.transform.position = new Vector3(-0.42f, 52.48f, -0.13f);
        _stageEnd.transform.SetParent(_stage.transform);

        _nextStage = SetNextStage();
    }

    //다음 스테이지 설정
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //큐에서 나올 원소가 새로 생성될 스테이지
        if (_queue.Count <= 3) InsertStageToQueue(); //큐에 크기가 3 이하면 새로운 컨셉 큐에 삽입

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        
        if(!_isInstantiateStage[concept_index, stage_index])
        {
            _isInstantiateStage[concept_index, stage_index] = true;
            _stagePrefab[concept_index][stage_index] = Instantiate(_stagePrefab[concept_index][stage_index]);
            _stagePrefab[concept_index][stage_index].transform.SetParent(_stageParent.transform);
        }
        
        ret = _stagePrefab[concept_index][stage_index];
        ret.transform.position = new Vector3(0, 0, 0);

        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    //비활성화된 모든 오브젝트 활성화
    void ReturnStage()
    {
        _prevStage = _stage;
        _prevStage.GetComponent<GateMovement>().StopMove();
        _prevStage.transform.position = new Vector3(0, 191.1f, 0);

        EnableObject();
        DeleteExploder();
    }

    public void DisablePrevStage() 
    {
        if (_prevStage == null) return;

        _prevStage.SetActive(false);
        //_prevStage.transform.position = new Vector3(0, 0, 0);
    }

    //Resources 파일에 있는 Prefab들 가져옴
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_conceptCount][];
       
        for (int i = 0; i < _conceptCount; i++)
        {
            _stagePrefab[i] = _conceptInfo.GetStagePrefab(i);
            for (int j = 0; j < 3; j++) _stagePrefab[i][j].SetActive(false);
        }
    }

    void SetStageParent()
    {
        _stageParent = new GameObject();
        _stageParent.name = "StageParent";
    }

    //스테이지 지나면 호출
    public void PrepareToStage()
    {
        if (_stageCount++ % 3 == 0) _passThroughCount = _passThroughCount >= 5 ? 5 : _passThroughCount + 1;

        _speed = _basicSpeed + _passThroughCount;
        _speed = _speed > _maxBasicSpeed ? _maxBasicSpeed : _speed;

        //점수 관련 UI 변경
        CanvasController.Instance.ChangeState(_stageCount);
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);

        ReturnStage();
        SetCurrentStage();
        MoveStage();
    }

    public void MinusPassThroughCount() 
    { 
        _passThroughCount = _passThroughCount > 0 ? --_passThroughCount : 0;
        
        //계수 UI 변경
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);
    }
    public int GetPassThroughCount() { return _passThroughCount; }


    //Scene에 있는 Stage 속도 설정
    public void MoveStage()
    {
        if(_prevStage != null) _prevStage.GetComponent<GateMovement>().Move();
        _stage.GetComponent<GateMovement>().Move();
        _nextStage.GetComponent<GateMovement>().Move();
    }

    //현재 스피드 반환
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //스테이지 속도 설정
    public void SetVelocity(float speed)
    {
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    //스테이지 속도 리셋
    public void ResetVelocity()
    {
        _speed = _basicSpeed;
        _stage.GetComponent<GateMovement>().SetVelocity(_speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(_speed);
    }

    public void EndGame() 
    { 
        _stageEnd.SetActive(false);
        _stageParent.SetActive(false);    
    }

    //가속도 설정
    public void SetAcceleration()
    {
        Debug.Log("현재 Stage : " + _stage.name);
        Debug.Log("다음 Stage : " + _nextStage.name);
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    //부딪힌 장애물 리스트에 넣어주기
    public void AddDisabled(GameObject gameObject)
    {
        _disabled.Add(gameObject);
    }

    //가속도 빨라지기
    public void SetAccDeBuff()
    {
        _accSpeed *= 1.2f;
    }

    //기본 속도 빨라지는 버프
    public void SetBasicSpeedDeBuff()
    {
        _basicSpeed *= 1.2f;
        _speed = _basicSpeed;
    }
    
    //터진 파편들 리스트에 넣어주기
    public void MakeExploder(Transform parent, GameObject explode)
    {
        explode.transform.SetParent(parent);
        _explode.Add(explode);
    }

    //리스트에 있는 터진 파편들 삭제
    void DeleteExploder()
    {
        foreach (GameObject explode in _explode) Destroy(explode);
        _explode.Clear();
    }

    //리스트에 들어있는 장애물 활성화
    void EnableObject()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();
    }

    //현재 스테이지 반환
    public int GetStageCount() { return _stageCount; }
}
