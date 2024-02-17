using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    float[] _speed = { 10, 13, 15, 17, 19 }; //계수 별 스피드
    float[] _maxSpeed = { 13, 15, 17, 19, 20 }; //계수 별 최대 스피드
    float _curSpeed;

    GameObject[][] _stagePrefab; //리소스 파일에서 가져올 Stage 프리팹
    Queue<StageInfo> _queue; //생성될 스테이지들을 저장(컨셉 2개, 즉 스테이지는 6개)
    
    GameObject _stage, _nextStage, _prevStage, _stageParent; //_stageParent : 스테이지들의 부모 오브젝트
    GameObject _stageEnd; 
    bool[,] _isInstantiateStage; //해당 스테이지가 생성 됐는지 확인하는 변수

    int _totalStageCount, _curStageCount; //총 스테이지 수와 현재 카운트 되고 있는 스테이지 수

    List<int> _conceptIndex; //랜덤으로 가져올 컨셉을 저장할 리스트
    int _prevConcept; //_prevConcept : 바로 전에 사용한 컨셉, _conceptIndex에 빠진 concept을 넣어주기 위한 변수
    GetStage _conceptInfo;

    List<GameObject> _disabled, _explode; //비활성화된 오브젝트들과 파괴된 파편들을 저장할 리스트
    int _passThroughCount, _conceptCount; //_passThroughCount : 장애물에 부딪히지 않고 계속 전진하는 횟수, _conceptCount : 플레이 가능한 concept 개수
    
    int _stageBonusJellyIndex, _nextStageBonusJellyIndex;
    List<int> _remainJelly;

    int _stageBigJellyCount, _nextStageBigJellyCount;
    List<GameObject> _stageDisabledJellys; //현재 스테이지에 보너스 젤리 자리의 조각
    List<GameObject> _nextStageDisabledJellys; //다음 스테이지에 보너스 젤리 자리의 조각

    Player _player;

    float _bounsStageSpeed;

    const int MaxConceptIndex = 10;

    bool _isSkill;

    private void Awake()
    {
        _curSpeed = _speed[0];

        //테스트용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _accSpeed = 0.7f;
        //_accSpeed = 0.5f;

        _bounsStageSpeed = 15f;

        //계수
        _passThroughCount = 0;

        //stage 정보 초기화
        _totalStageCount = 1;
        _curStageCount = 1;
        _prevConcept = -1;
        _stage = null;
        _nextStage = null;
        _prevStage = null;
        _stageParent = null;

        //보너스 관련 변수
        _stageBonusJellyIndex = -1;
        _nextStageBonusJellyIndex = -1;

        //곰젤리 관련 변수
        _stageBigJellyCount = -1;
        _nextStageBigJellyCount = -1;
        _stageDisabledJellys = new List<GameObject>();
        _nextStageDisabledJellys = new List<GameObject>();

        //저장할 Obstacle 리스트 초기화
        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        _queue = new Queue<StageInfo>();

        _stageEnd = GameObject.Find("StageEnd");
        _conceptInfo = GameObject.Find("Concept").GetComponent<GetStage>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _isSkill = false;
    }

    void Start()
    {
        Init_Instance();
        Init();
    }

    void Init()
    {
        _conceptCount = SaveLoadManager.Instance.GetUnlockedConcept() + 1;
        _isInstantiateStage = new bool[MaxConceptIndex, 3];

        SetStageParent();

        //TestStage(7);

        SetConceptIndex();
        InstantiateStage();

        TestStage();

        ResetQueue();
        SetStageForStart();

        SetVelocity(_speed[_passThroughCount]);
    }

    void TestStage(int index)
    {
        _conceptCount = index + 1 > _conceptCount ? index + 1 : _conceptCount;
        List<int> stageIndex = new List<int>(new int[] { 0, 1, 2 });

        for (int j = 3; j > 0; j--)
        {
            int next_stage_num = Random.Range(0, j);
            _queue.Enqueue(new StageInfo(index, stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }
    }

    void TestStage()
    {
        List<int> testConceptList = TestConceptButton.Instance.GetTestConcept();
        int index = -1;

        for (int i = 0; i < testConceptList.Count; i++)
        {
            List<int> stageIndex = new List<int>(new int[] { 0, 1, 2 });

            for (int j = 3; j > 0; j--)
            {
                int next_stage_num = Random.Range(0, j);
                _queue.Enqueue(new StageInfo(testConceptList[i], stageIndex[next_stage_num]));
                stageIndex.RemoveAt(next_stage_num);
            }

            index = testConceptList[i];
        }

        if (index != -1) _conceptIndex.Remove(index); //큐에 컨셉을 삽입했으면 List에서 제거
        _prevConcept = index;
    }

    //초기 Concept Index 설정
    void SetConceptIndex()
    {
        _conceptIndex = new List<int>();
        for (int i = 0; i < _conceptCount; i++) _conceptIndex.Add(i);
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

    //큐에 있는 모든 내용을 지우고 새로운 컨셉을 넣는다.
    void ResetQueue()
    {
        InsertStageToQueue();
        InsertStageToQueue();
    }

    //스테이지를 새로 시작한다.
    void SetStageForStart()
    {
        _nextStage = SetNextStage();
        CheckJelly();
        SetCurrentStage();
    }

    //맵에 나타날 스테이지를 큐에 넣어주는 과정
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
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

    //현재 스테이지 설정
    void SetCurrentStage()
    {
        _stage = _nextStage;
        PlayGameManager.Instance.SetStage(_stage);

        //위치 재설정
        _stage.transform.position = new Vector3(0, 96.1f, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정

        //포탈 위치 재설정
        _stageEnd.transform.SetParent(null);
        _stageEnd.transform.position = new Vector3(0, 78f, 0);
        _stageEnd.transform.SetParent(_stage.transform);

        _nextStage = SetNextStage();
        CheckJelly();
        CheckMemoryStage();
    }

    //다음 스테이지 설정
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //큐에서 나올 원소가 새로 생성될 스테이지
        if (_queue.Count <= 3) InsertStageToQueue(); //큐에 크기가 3 이하면 새로운 컨셉 큐에 삽입

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        
        //stage가 생성되어 있지 않다면 생성
        if(!_isInstantiateStage[concept_index, stage_index])
        {
            _isInstantiateStage[concept_index, stage_index] = true;
            _stagePrefab[concept_index][stage_index] = Instantiate(_stagePrefab[concept_index][stage_index]);
            _stagePrefab[concept_index][stage_index].transform.SetParent(_stageParent.transform);
        }
        
        //시작 위치 값
        ret = _stagePrefab[concept_index][stage_index];
        ret.transform.position = new Vector3(0, 0, 0);

        //시작 회전 값
        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    public void SetBonusJellyIndex(int index) { _nextStageBonusJellyIndex = index; }

    //보너스 젤리
    void CheckJelly()
    {
        if (_stageDisabledJellys.Count > 0)
        {
            foreach (GameObject obj in _stageDisabledJellys) obj.SetActive(true);
        }
        _stageDisabledJellys.Clear();

        foreach (GameObject obj in _nextStageDisabledJellys) _stageDisabledJellys.Add(obj);
        _nextStageDisabledJellys.Clear();

        if (_stageBonusJellyIndex != -1) BonusJelly.Instance.ReturnBonusJelly(_stageBonusJellyIndex);
        _stageBonusJellyIndex = _nextStageBonusJellyIndex;

        foreach (Transform child in _nextStage.transform)
        {
            if(child.CompareTag("Jelly"))
            {
                int childsCount = child.childCount;
                _remainJelly = new List<int>();
                for (int i = 0; i < childsCount; i++) _remainJelly.Add(i);
                
                GameObject obj = CheckMemory(child);
                obj.SetActive(false);
                _nextStageDisabledJellys.Add(obj);

                GameObject bonus = BonusJelly.Instance.GetBonusJelly();
                bonus.SetActive(true);
                bonus.transform.SetParent(child);
                bonus.transform.position = obj.transform.position;
                bonus.transform.localScale = new Vector3(300, 300, 300);

                CheckBigJelly(child);

                break;
            }
        }
    }

    void CheckBigJelly(Transform child)
    {
        if (_stageBigJellyCount != -1) { PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().ReturnObject(_stageBigJellyCount); }
        _stageBigJellyCount = _nextStageBigJellyCount;
        _nextStageBigJellyCount = 0;

        for(int i = 0; i < _remainJelly.Count; i++)
        {
            GameObject obj = child.GetChild(_remainJelly[i]).gameObject;

            if (!GetPercent(15) || obj.transform.GetChild(0).GetComponent<Jelly>().CheckMemory()) continue;

            GameObject bigJelly = PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().GetObject();
            bigJelly.SetActive(true);
            bigJelly.transform.SetParent(child);
            bigJelly.transform.position = obj.transform.position;
            bigJelly.transform.localScale = new Vector3(3, 3, 3);

            obj.SetActive(false);
            _nextStageDisabledJellys.Add(obj);
            _nextStageBigJellyCount++;
        }
    }

    //비활성화된 모든 오브젝트 활성화
    void SetPrevStage()
    {
        _prevStage = _stage;
        _prevStage.transform.position = new Vector3(0, 192.5f, 0);
    }

    public void DisablePrevStage()
    {
        if (_prevStage == null) return;
        _prevStage.SetActive(false);
        //_prevStage.transform.position = new Vector3(0, 0, 0);
    }

    //스테이지 지나면 스테이지 1 더해주고 다음 스테이지 설정
    public void PrepareToStage()
    {
        _curSpeed = GetStageVelocity();
        if (_curStageCount == 0)
        {
            if(!_isSkill) _curSpeed = _speed[_passThroughCount];
        }
        else if (_curStageCount % 3 == 0)
        {
            _passThroughCount = _passThroughCount >= 4 ? 4 : _passThroughCount + 1;
            if (!_isSkill) _curSpeed = _speed[_passThroughCount];
        }

        _curStageCount++;
        _totalStageCount++;

        //점수 관련 UI 변경
        CanvasController.Instance.ChangeState(_totalStageCount);
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);
        
        ReturnStage();
        StopMoveAllStage();
        SetPrevStage();
        SetCurrentStage();
        SetVelocity(_curSpeed);
    }

    void SetStageParent()
    {
        _stageParent = new GameObject();
        _stageParent.name = "StageParent";
    }

    public void MinusPassThroughCount() 
    { 
        if(_passThroughCount == 0) SetVelocity(_speed[_passThroughCount]);
        else SetVelocity(_maxSpeed[--_passThroughCount]);

        //계수 UI 변경
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);
    }
    public int GetPassThroughCount() { return _passThroughCount; }

    //현재 스피드 반환
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //스테이지 속도 설정
    public void SetVelocity(float speed)
    {
        _curSpeed = speed;

        if (_prevStage != null) _prevStage.GetComponent<GateMovement>().SetVelocity(speed);
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    public void SetBonusStageVelocity(float speed)
    {
        _curSpeed = speed;

        if (_prevStage != null) _prevStage.GetComponent<GateMovement>().SetBonusVelocity(speed);
        _stage.GetComponent<GateMovement>().SetBonusVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetBonusVelocity(speed);
    }

    public void EndGame() 
    { 
        _stageEnd.SetActive(false);
        _stageParent.SetActive(false);    
    }

    //가속도 설정
    public void SetAcceleration()
    {
        DisablePrevStage();

        Debug.Log("현재 Stage : " + _stage.name);
        Debug.Log("다음 Stage : " + _nextStage.name);
        //_stage.GetComponent<GateMovement>().SetAcceleration();
        //_nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    //1번째 스테이지일 때, 2번째 스테이지 메모리 활성화
    void CheckMemoryStage()
    {
        if (_curStageCount % 3 == 1) _nextStage.GetComponent<GateMovement>().EnableMemory();
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
        for (int i = 0; i < 5; i++) _speed[i] *= 1.2f;
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

    //스테이지를 원상태로 되돌린다.
    void ReturnStage()
    {
        EnableObject();
        DeleteExploder();
    }

    void StopMoveAllStage()
    {
        if (_stage != null) _stage.transform.GetComponent<GateMovement>().StopMove();
        if (_nextStage != null) _nextStage.transform.GetComponent<GateMovement>().StopMove();
        if (_prevStage != null) _prevStage.transform.GetComponent<GateMovement>().StopMove();
    }

    //현재 모든 스테이지를 초기화 시킨다.
    void DisableAllStage()
    {
        if (_stage != null)
        {
            _stage.SetActive(false);
            _stage = null;
        }

        if (_nextStage != null)
        {
            _nextStage.SetActive(false);
            _nextStage = null;
        }

        if (_prevStage != null)
        {
            _prevStage.SetActive(false);
            _prevStage = null;
        }

        ReturnStage();
    }

    //보너스 스테이지에 입장하면 현재 있는 모든 스테이지의 젤리들 초기화
    void ReturnNextStageJellys()
    {
        if (_nextStageBonusJellyIndex != -1)
        {
            BonusJelly.Instance.ReturnBonusJelly(_nextStageBonusJellyIndex);
            _nextStageBonusJellyIndex = -1;
        }

        if (_nextStageDisabledJellys.Count > 0)
        {
            foreach (GameObject obj in _nextStageDisabledJellys) obj.SetActive(true);
            _nextStageDisabledJellys.Clear();
        }

        if (_nextStageBigJellyCount != -1)
        {
            PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().ReturnObject(_nextStageBigJellyCount);
            _nextStageBigJellyCount = -1;
        }
    }

    void SetBonusStage()
    {
        //보너스 위치 설정
        _stage = BonusJelly.Instance.GetBonusStage();
        _stage.transform.position = new Vector3(0, 96.1f, 0); //첫번째 스테이지와 두번째 스테이지 위치 차이 고정
        _stage.SetActive(true);

        //포탈 위치 재설정
        _stageEnd.transform.SetParent(null);
        _stageEnd.transform.position = new Vector3(0, 78f, 0);
        _stageEnd.transform.SetParent(_stage.transform);

        foreach (Transform child in _stage.transform)
        {
            if (child.CompareTag("Jelly"))
            {
                int childsCount = child.childCount;
                _remainJelly = new List<int>();
                for (int i = 0; i < childsCount; i++) _remainJelly.Add(i);
                
                CheckBigJelly(child);

                break;
            }
        }
    }

    //보너스 스테이지 들어가기 전 효과를 위한 준비
    public void PrepareForBonusStage()
    {
        //스킬 사용 못하게 설정
        _player.EnterBonusStage();

        _curStageCount = 0;

        //모든 스테이지 움직임 멈춤
        StopMoveAllStage();

        //틱 데미지와 에너지 차징 되지 않게 설정
        PlayGameManager.Instance.SetBonusTime();

        //스킬 및 무적 중지
        _player.StopMyCoroutine();

        //플레이어 움직이지 못하게 설정
        _player.DisablePlayerMove();

        //보너스 스테이지 입장 시 효과 시작
        MyPostProcess.Instance.SetExposure();
    }

    //보너스 스테이지 시작
    public void EnableBonusStage()
    {
        //모든 스테이지 제거
        DisableAllStage();

        //플레이어 위치 중앙으로 이동
        _player.transform.position = new Vector3(0, 175, 0);

        //보너스 스테이지 입장 시 효과 끝
        MyPostProcess.Instance.ResetExposure();

        //큐에 있는 컨셉 재배치
        _queue.Clear();
        ResetQueue();

        //보너스 스테이지 활성화 및 다음 스테이지 설정
        ReturnNextStageJellys();
        SetBonusStage();
        _nextStage = SetNextStage();
        CheckJelly();

        //보너스 스테이지 속도 일정하게 설정
        SetBonusStageVelocity(_bounsStageSpeed);

        //플레이어 다시 움직임
        _player.EnablePlayerMove();
    }

    public void TriggerBonusJelly()
    {
        BonusJelly.Instance.TriggerBonusJelly(_stageBonusJellyIndex);
    }

    GameObject CheckMemory(Transform child)
    {
        GameObject obj;
        
        while (true)
        {
            int randomChild = GetRandomNumber(_remainJelly.Count);
            obj = child.GetChild(_remainJelly[randomChild]).gameObject;
            _remainJelly.RemoveAt(randomChild);

            if (!obj.transform.GetChild(0).GetComponent<Jelly>().CheckMemory()) break;
        }

        return obj;
    }

    //확률을 가져옴
    bool GetPercent(int p)
    {
        int random = Random.Range(0, 100);
        
        if (random > p) return false;
        else return true;
    }

    //일정한 개수 중에서 하나를 랜덤으로 가져옴
    int GetRandomNumber(int p)
    {
        int random = Random.Range(0, p);

        return random;
    }

    public void SetSkill()
    {
        _isSkill = true;

        if (_stage != null) _stage.transform.GetComponent<GateMovement>().SetSkill();
        _nextStage.transform.GetComponent<GateMovement>().SetSkill();
        _prevStage.transform.GetComponent<GateMovement>().SetSkill();
    }

    public void ResetSkill()
    {
        _isSkill = false;

        if (_stage != null) _stage.transform.GetComponent<GateMovement>().ResetSkill();
        _nextStage.transform.GetComponent<GateMovement>().ResetSkill();
        _prevStage.transform.GetComponent<GateMovement>().ResetSkill();
    }

    //현재 스테이지 반환
    public int GetStageCount() { return _totalStageCount; }
    public float GetSkillSpeed() { return 30; }
    public float GetSpeed() { return _speed[_passThroughCount]; }
    public float GetMaxSpeed() { return _maxSpeed[_passThroughCount]; }
}
