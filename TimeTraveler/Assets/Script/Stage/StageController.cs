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
    private float _basicSpeed; //���� �����ϰ� �ִ� ���������� �⺻ �ӵ�
    public float BasicSpeed
    {
        get { return _basicSpeed; }
    }

    private float _speed; //�ǽð� �ӵ�
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
    private float _accSpeed; //���ӵ�
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
    private float _maxSpeed; //�ִ� �ӵ�
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
    private float _maxBasicSpeed; //�⺻ �ӵ��� �ִ�ġ
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

    //ť�� �� �ڷᱸ��
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

    GameObject[][] _stagePrefab; //���ҽ� ���Ͽ��� ������ Stage ������
    Queue<StageInfo> _queue; //������ ������������ ����(���� 2��, �� ���������� 6��)
    
    GameObject _stage, _nextStage, _prevStage, _stageParent; //_stageParent : ������������ �θ� ������Ʈ
    GameObject _stageEnd; 
    bool[,] _isInstantiateStage; //�ش� ���������� ���� �ƴ��� Ȯ���ϴ� ����

    int _totalStageCount, _curStageCount; //�� �������� ���� ���� ī��Ʈ �ǰ� �ִ� �������� ��

    List<int> _conceptIndex; //�������� ������ ������ ������ ����Ʈ
    int _prevConcept; //_prevConcept : �ٷ� ���� ����� ����, _conceptIndex�� ���� concept�� �־��ֱ� ���� ����
    GetStage _conceptInfo;

    List<GameObject> _disabled, _explode; //��Ȱ��ȭ�� ������Ʈ��� �ı��� ������� ������ ����Ʈ
    int _passThroughCount, _conceptCount; //_passThroughCount : ��ֹ��� �ε����� �ʰ� ��� �����ϴ� Ƚ��, _conceptCount : �÷��� ������ concept ����
    
    int _stageBonusJellyIndex, _nextStageBonusJellyIndex;
    GameObject _stageDisabledJelly; //���� ���������� ���ʽ� ���� �ڸ��� ����
    GameObject _nextStageDisabledJelly; //���� ���������� ���ʽ� ���� �ڸ��� ����
    List<int> _remainJelly;

    int _stageBigJellyCount, _nextStageBigJellyCount;
    List<GameObject> _stageDisabledJellys;
    List<GameObject> _nextStageDisabledJellys;

    Player _player;

    float _bounsStageSpeed;

    const int MaxConceptIndex = 10;

    private void Awake()
    {
        //���ǵ� �ʱ�ȭ
        _basicSpeed = 7.5f;
        _speed = _basicSpeed;

        //�׽�Ʈ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _accSpeed = 0.5f;

        _maxSpeed = 25f;
        _maxBasicSpeed = 12.5f;

        _bounsStageSpeed = 15f;

        //stage ���� �ʱ�ȭ
        _totalStageCount = 1;
        _curStageCount = 1;
        _prevConcept = -1;
        _stage = null;
        _nextStage = null;
        _prevStage = null;
        _stageParent = null;

        //���ʽ� ���� ����
        _stageDisabledJelly = null;
        _nextStageDisabledJelly = null;
        _stageBonusJellyIndex = -1;
        _nextStageBonusJellyIndex = -1;

        //������ ���� ����
        _stageBigJellyCount = -1;
        _nextStageBigJellyCount = -1;
        _stageDisabledJellys = new List<GameObject>();
        _nextStageDisabledJellys = new List<GameObject>();

        //������ Obstacle ����Ʈ �ʱ�ȭ
        _disabled = new List<GameObject>();
        _explode = new List<GameObject>();

        _queue = new Queue<StageInfo>();

        _stageEnd = GameObject.Find("StageEnd");
        _conceptInfo = GameObject.Find("Concept").GetComponent<GetStage>();

        _player = GameObject.Find("Player").GetComponent<Player>();
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
        
        MoveStage();
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

        if (index != -1) _conceptIndex.Remove(index); //ť�� ������ ���������� List���� ����
        _prevConcept = index;
    }

    //�ʱ� Concept Index ����
    void SetConceptIndex()
    {
        _conceptIndex = new List<int>();
        for (int i = 0; i < _conceptCount; i++) _conceptIndex.Add(i);
    }

    //Resources ���Ͽ� �ִ� Prefab�� ������
    void InstantiateStage()
    {
        _stagePrefab = new GameObject[_conceptCount][];

        for (int i = 0; i < _conceptCount; i++)
        {
            _stagePrefab[i] = _conceptInfo.GetStagePrefab(i);
            for (int j = 0; j < 3; j++) _stagePrefab[i][j].SetActive(false);
        }
    }

    //ť�� �ִ� ��� ������ ����� ���ο� ������ �ִ´�.
    void ResetQueue()
    {
        _queue.Clear();
        InsertStageToQueue();
        InsertStageToQueue();
    }

    //���������� ���� �����Ѵ�.
    void SetStageForStart()
    {
        _nextStage = SetNextStage();
        CheckJelly();
        SetCurrentStage();
    }

    //�ʿ� ��Ÿ�� ���������� ť�� �־��ִ� ����
    void InsertStageToQueue()
    {
        int conceptIndex = Random.Range(0, _conceptIndex.Count);
        
        //������ �̰�, �̹� ����� ������ ť�� �־���
        if (_prevConcept != -1) _conceptIndex.Add(_prevConcept);
        _prevConcept = _conceptIndex[conceptIndex];

        //�� ������ Stage�� ������ 3���� ������ �ֱ� ������ {0, 1, 2}�� List�� ����
        List<int> stageIndex = new List<int>(new int[]{ 0, 1, 2 });
        
        //�������� stageIndex�� index�� �޾Ƽ� ť�� concept index�� stageIndex[index]�� �־��ְ�,
        //stageIndex[index]�� List���� ���༭ �ߺ��� ���� �ʰ� �Ѵ�.
        for (int i = 3; i > 0; i--)
        {
            int next_stage_num = Random.Range(0, i);
            _queue.Enqueue(new StageInfo(_conceptIndex[conceptIndex], stageIndex[next_stage_num]));
            stageIndex.RemoveAt(next_stage_num);
        }

        //ť�� ������ ���������� List���� ����
        _conceptIndex.RemoveAt(conceptIndex);
    }

    //���� �������� ����
    void SetCurrentStage()
    {
        _stage = _nextStage;
        PlayGameManager.Instance.SetStage(_stage);

        //�ö󰡰� �ִ� stage ���� �� ��ġ �缳��
        _stage.transform.GetComponent<GateMovement>().StopMove();
        _stage.transform.position = new Vector3(0, 96.1f, 0); //ù��° ���������� �ι�° �������� ��ġ ���� ����

        //��Ż ��ġ �缳��
        _stageEnd.transform.SetParent(null);
        _stageEnd.transform.position = new Vector3(0, 78f, 0);
        _stageEnd.transform.SetParent(_stage.transform);

        _nextStage = SetNextStage();
        CheckJelly();
        CheckMemoryStage();
    }

    //���� �������� ����
    GameObject SetNextStage()
    {
        GameObject ret;

        StageInfo next_stage_info = _queue.Dequeue(); //ť���� ���� ���Ұ� ���� ������ ��������
        if (_queue.Count <= 3) InsertStageToQueue(); //ť�� ũ�Ⱑ 3 ���ϸ� ���ο� ���� ť�� ����

        int concept_index = next_stage_info.concept_index, stage_index = next_stage_info.stage_index;
        
        //stage�� �����Ǿ� ���� �ʴٸ� ����
        if(!_isInstantiateStage[concept_index, stage_index])
        {
            _isInstantiateStage[concept_index, stage_index] = true;
            _stagePrefab[concept_index][stage_index] = Instantiate(_stagePrefab[concept_index][stage_index]);
            _stagePrefab[concept_index][stage_index].transform.SetParent(_stageParent.transform);
        }
        
        //���� ��ġ ��
        ret = _stagePrefab[concept_index][stage_index];
        ret.transform.position = new Vector3(0, 0, 0);

        //���� ȸ�� ��
        int nextAngle = Random.Range(0, 360);
        ret.transform.rotation = Quaternion.Euler(0, nextAngle, 0);
        ret.SetActive(true);

        return ret;
    }

    public void SetBonusJellyIndex(int index) { _nextStageBonusJellyIndex = index; }

    //���ʽ� ����
    void CheckJelly()
    {
        if (_stageDisabledJelly != null) _stageDisabledJelly.SetActive(true);
        _stageDisabledJelly = _nextStageDisabledJelly;

        if(_stageBonusJellyIndex != -1) BonusJelly.Instance.ReturnBonusJelly(_stageBonusJellyIndex);
        _stageBonusJellyIndex = _nextStageBonusJellyIndex;

        foreach (Transform child in _nextStage.transform)
        {
            if(child.CompareTag("Jelly"))
            {
                GameObject bonus = BonusJelly.Instance.GetBonusJelly();

                int childsCount = child.childCount;
                _remainJelly = new List<int>();
                for (int i = 0; i < childsCount; i++) _remainJelly.Add(i);
                
                GameObject obj = CheckMemory(child);
                obj.SetActive(false);
                _nextStageDisabledJelly = obj;

                bonus.SetActive(true);
                bonus.transform.SetParent(child);
                bonus.transform.position = obj.transform.position;

                CheckBigJelly(child);

                break;
            }
        }
    }

    void CheckBigJelly(Transform child)
    {
        if(_stageDisabledJellys.Count > 0)
        {
            foreach (GameObject obj in _stageDisabledJellys) obj.SetActive(true);
        }
        _stageDisabledJellys = new List<GameObject>();
        foreach (GameObject obj in _nextStageDisabledJellys) _stageDisabledJellys.Add(obj);
        _nextStageDisabledJellys = new List<GameObject>();

        if (_stageBigJellyCount != -1) { PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().ReturnObject(_stageBigJellyCount); }
        _stageBigJellyCount = _nextStageBigJellyCount;
        _nextStageBigJellyCount = 0;

        for(int i = 0; i < _remainJelly.Count; i++)
        {
            GameObject obj = child.GetChild(_remainJelly[i]).gameObject;

            if (!GetPercent(20) || obj.transform.GetChild(0).GetComponent<Jelly>().CheckMemory()) continue;

            GameObject bigJelly = PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().GetObject();
            bigJelly.SetActive(true);
            bigJelly.transform.SetParent(child);
            bigJelly.transform.position = obj.transform.position;
            bigJelly.transform.localScale = new Vector3(3, 3, 3);

            obj.SetActive(false);
            _nextStageDisabledJellys.Add(bigJelly);
            _nextStageBigJellyCount++;
        }
    }

    //��Ȱ��ȭ�� ��� ������Ʈ Ȱ��ȭ
    void SetPrevStage()
    {
        _prevStage = _stage;
        _prevStage.GetComponent<GateMovement>().StopMove();
        _prevStage.transform.position = new Vector3(0, 192.5f, 0);
    }

    public void DisablePrevStage()
    {
        if (_prevStage == null) return;
        _prevStage.SetActive(false);
        //_prevStage.transform.position = new Vector3(0, 0, 0);
    }

    //�������� ������ �������� 1 �����ְ� ���� �������� ����
    public void PrepareToStage()
    {
        if (_curStageCount++ % 3 == 0) _passThroughCount = _passThroughCount >= 5 ? 5 : _passThroughCount + 1;
        _totalStageCount++;

        _speed = _basicSpeed + _passThroughCount;
        _speed = _speed > _maxBasicSpeed ? _maxBasicSpeed : _speed;

        //���� ���� UI ����
        CanvasController.Instance.ChangeState(_totalStageCount);
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);

        ReturnStage();
        SetPrevStage();
        SetCurrentStage();
        MoveStage();
    }

    void SetStageParent()
    {
        _stageParent = new GameObject();
        _stageParent.name = "StageParent";
    }

    public void MinusPassThroughCount() 
    { 
        _passThroughCount = _passThroughCount > 0 ? --_passThroughCount : 0;
        
        //��� UI ����
        CanvasController.Instance.ChangeScoreUpText((float)PlayGameManager.Instance.ScorePerTime() / 10);
    }
    public int GetPassThroughCount() { return _passThroughCount; }


    //Scene�� �ִ� Stage �ӵ� ����
    public void MoveStage()
    {
        if(_prevStage != null) _prevStage.GetComponent<GateMovement>().Move();
        _stage.GetComponent<GateMovement>().Move();
        _nextStage.GetComponent<GateMovement>().Move();
    }

    //���� ���ǵ� ��ȯ
    public float GetStageVelocity()
    {
        return _stage.GetComponent<GateMovement>().GetVelocity();
    }

    //�������� �ӵ� ����
    public void SetVelocity(float speed)
    {
        _stage.GetComponent<GateMovement>().SetVelocity(speed);
        _nextStage.GetComponent<GateMovement>().SetVelocity(speed);
    }

    //�������� �ӵ� ����
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

    //���ӵ� ����
    public void SetAcceleration()
    {
        DisablePrevStage();

        Debug.Log("���� Stage : " + _stage.name);
        Debug.Log("���� Stage : " + _nextStage.name);
        _stage.GetComponent<GateMovement>().SetAcceleration();
        _nextStage.GetComponent<GateMovement>().SetAcceleration();
    }

    //1��° ���������� ��, 2��° �������� �޸� Ȱ��ȭ
    void CheckMemoryStage()
    {
        if (_curStageCount % 3 == 1) _nextStage.GetComponent<GateMovement>().EnableMemory();
    }

    //�ε��� ��ֹ� ����Ʈ�� �־��ֱ�
    public void AddDisabled(GameObject gameObject)
    {
        _disabled.Add(gameObject);
    }

    //���ӵ� ��������
    public void SetAccDeBuff()
    {
        _accSpeed *= 1.2f;
    }

    //�⺻ �ӵ� �������� ����
    public void SetBasicSpeedDeBuff()
    {
        _basicSpeed *= 1.2f;
        _speed = _basicSpeed;
    }
    
    //���� ����� ����Ʈ�� �־��ֱ�
    public void MakeExploder(Transform parent, GameObject explode)
    {
        explode.transform.SetParent(parent);
        _explode.Add(explode);
    }

    //����Ʈ�� �ִ� ���� ����� ����
    void DeleteExploder()
    {
        foreach (GameObject explode in _explode) Destroy(explode);
        _explode.Clear();
    }

    //����Ʈ�� ����ִ� ��ֹ� Ȱ��ȭ
    void EnableObject()
    {
        foreach (GameObject obj in _disabled) obj.SetActive(true);
        _disabled.Clear();
    }

    //���������� �����·� �ǵ�����.
    void ReturnStage()
    {
        EnableObject();
        DeleteExploder();
    }

    //���� ��� ���������� �ʱ�ȭ ��Ų��.
    void DisableAllStage()
    {
        if (_stage != null)
        {
            _stage.transform.GetComponent<GateMovement>().StopMove();
            _stage.SetActive(false);
            _stage = null;
        }

        if (_nextStage != null)
        {
            _nextStage.transform.GetComponent<GateMovement>().StopMove();
            _nextStage.SetActive(false);
            _nextStage = null;
        }

        if (_prevStage != null)
        {
            _prevStage.transform.GetComponent<GateMovement>().StopMove();
            _prevStage.SetActive(false);
            _prevStage = null;
        }

        ReturnStage();
    }

    //���ʽ� ���������� �����ϸ� ���� �ִ� ��� ���������� ������ �ʱ�ȭ
    void ReturnJellys()
    {
        if (_nextStageDisabledJelly != null)
        {
            _nextStageDisabledJelly.SetActive(true);
            _nextStageDisabledJelly = null;
        }

        if (_nextStageBonusJellyIndex != -1)
        {
            BonusJelly.Instance.ReturnBonusJelly(_nextStageBonusJellyIndex);
            _nextStageBonusJellyIndex = -1;
        }

        if (_nextStageDisabledJellys.Count > 0)
        {
            foreach (GameObject obj in _nextStageDisabledJellys) obj.SetActive(true);
            _nextStageDisabledJellys = new List<GameObject>();
        }

        if (_nextStageBigJellyCount != -1)
        {
            PlayGameManager.Instance.GetComponent<BigJellyPoolManager>().ReturnObject(_nextStageBigJellyCount);
            _nextStageBigJellyCount = -1;
        }
    }

    void SetBonusStage()
    {
        //���ʽ� ��ġ ����
        _stage = BonusJelly.Instance.GetBonusStage();
        _stage.transform.position = new Vector3(0, 96.1f, 0); //ù��° ���������� �ι�° �������� ��ġ ���� ����
        _stage.SetActive(true);

        //��Ż ��ġ �缳��
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

    //���ʽ� �������� ����
    public void EnableBonusStage()
    {
        _curStageCount = 0;
        
        //��ų �� ���� ����
        _player.StopMyCoroutine();

        //ƽ �������� ������ ��¡ ���� �ʰ� ����
        PlayGameManager.Instance.SetBonusTime();

        //��� �������� ����
        DisableAllStage();

        //ť�� �ִ� ���� ���ġ
        ResetQueue();

        //���ʽ� �������� Ȱ��ȭ �� ���� �������� ����
        ReturnJellys();
        SetBonusStage();
        _nextStage = SetNextStage();
        CheckJelly();

        //���ʽ� �������� �ӵ� �����ϰ� ����
        _speed = _bounsStageSpeed;
        MoveStage();
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

    //Ȯ���� ������
    bool GetPercent(int p)
    {
        int random = Random.Range(0, 100);
        
        if (random > p) return false;
        else return true;
    }

    //������ ���� �߿��� �ϳ��� �������� ������
    int GetRandomNumber(int p)
    {
        int random = Random.Range(0, p);

        return random;
    }

    //���� �������� ��ȯ
    public int GetStageCount() { return _totalStageCount; }
}
